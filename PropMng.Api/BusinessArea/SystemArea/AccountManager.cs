using Gdnc.Services.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PropMng.Api.data;
using PropMng.Api.DataArea.Identity;
using PropMng.Api.Models;
using PropMng.Api.Models.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace PropMng.Api.BusinessArea.SystemArea
{
    public class AccountManager : IAccountManager
    {
        private readonly UserManager<CustomUser> userManager;
        private readonly SignInManager<CustomUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        public AccountManager(UserManager<CustomUser> userManager
            , SignInManager<CustomUser> signInManager
            , RoleManager<IdentityRole> roleManager
            , IUnitOfWork unitOfWork
            , IConfiguration configuration
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager=roleManager;
            this.unitOfWork = unitOfWork;
            this.configuration=configuration;
        }

        public async Task<LoginBaseDto> Login(LoginBaseModel model)
        {
            var user = await GetAuthorizedUser(model.Username, model.Password);

            if (user == null)
            {
                await AddErrorLog(model.Username, model.Password, model.IpAddress);
                return new LoginBaseDto { StatusId = (int)EnmLoginStatus.Failed };
            }
            else
            {
                var check = await signInManager.CheckPasswordSignInAsync(user, model.Password, true);
                if (check.IsLockedOut)
                {
                    return await SuccessLog(user, model.IpAddress, EnmLoginStatus.LockedOut);
                }
                else if (check.Succeeded)
                {
                    return await SuccessLog(user, model.IpAddress, EnmLoginStatus.Success);
                }
                else
                {
                    await AddErrorLog(model.Username, model.Password, model.IpAddress);
                    return new LoginBaseDto { StatusId = (int)EnmLoginStatus.Failed };
                }
            }
        }

        public async Task<LoginBaseDto> RefreshToken(RefreshTokenModel model)
        {
            var refreshLifetime = Convert.ToInt32(configuration["JwtTokenSetting:RefreshLifetime"]);
            var expiredToken = JwtTokenManager.GetExpiredToken(model.AccessToken, configuration["JwtTokenSetting:Key"]);
            if (expiredToken == null)
                return new LoginBaseDto { StatusId = (int)EnmLoginStatus.Failed };
            var logId = Convert.ToInt64(expiredToken.Claims.Where(a => a.Type == "logId").Single().Value);
            var userName = expiredToken.Identity.Name;
            var user = await userManager.FindByNameAsync(userName);
            var token = await unitOfWork.LogsTokens.FindBy(a => a.Id == logId && a.Token == model.RefreshToken).SingleOrDefaultAsync();

            if (token == null || token.CreatedDate < DateTime.Now.AddMinutes(-1 * refreshLifetime))
                return new LoginBaseDto { StatusId = (int)EnmLoginStatus.Failed };

            var obj = new LoginBaseDto { StatusId = (int)EnmLoginStatus.Success, RefreshToken = JwtTokenManager.GenerateRefreshToken(), AccessToken = await GetToken(user, logId) };

            var newToken = new LogsToken
            {
                Id = token.Id,
                Token = obj.RefreshToken,
                CreatedDate = DateTime.Now
            };


            unitOfWork.LogsTokens.RemoveAsync(token);
            unitOfWork.LogsTokens.AddAsync(newToken);
            await unitOfWork.CompleteAsync();
            return obj;
        }

        public async Task<ChangePasswordDto> ChangePassword(ChangeMyPasswordModel model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var sign = await signInManager.CheckPasswordSignInAsync(user, model.OldPassword, true);
            if (!sign.Succeeded)
                return new ChangePasswordDto(EnmLoginStatus.Failed);
            var change = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            return new ChangePasswordDto(change.Succeeded ? EnmLoginStatus.Success : EnmLoginStatus.Failed);
        }

        public async Task<PostResponseModel<RoleModel>> CreateRole(RoleModel model)
        {
            var r = await roleManager.FindByNameAsync(model.Name.ToUpper());
            if (r!=null) return PostResponseModel<RoleModel>.GetError(2);
            var rst = await roleManager.CreateAsync(new IdentityRole { Name=model.Name.ToUpper() });
            if (rst.Succeeded)
            {
                r=await roleManager.FindByNameAsync(model.Name.ToUpper());
                model.Id=r.Id;
                return PostResponseModel<RoleModel>.GetSuccess(model);
            }
            return PostResponseModel<RoleModel>.GetError(3);
        }

        public async Task<PostResponseModel<LinkRoleModel>> LinkRole(string userName, LinkRoleModel model)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user==null) return PostResponseModel<LinkRoleModel>.GetError(1);

            var rst = await userManager.IsInRoleAsync(user, model.RoleName);
            if (rst) return PostResponseModel<LinkRoleModel>.GetError(2);
            var r = await roleManager.FindByNameAsync(model.RoleName);
            if (r==null) return PostResponseModel<LinkRoleModel>.GetError(1);
            var lnk = await userManager.AddToRoleAsync(user, model.RoleName);
            if (lnk.Succeeded) return PostResponseModel<LinkRoleModel>.GetSuccess(model);
            return PostResponseModel<LinkRoleModel>.GetError(3);
        }


        public async Task<PostResponseModel<UserRoleModel>> GetRoles(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user==null) return PostResponseModel<UserRoleModel>.GetError(1);


            var roles = await userManager.GetRolesAsync(user);
            return PostResponseModel<UserRoleModel>.GetSuccess(new UserRoleModel
            {
                Roles=roles.Any() ? roles.ToList() : new List<string>()
            });

        }

        private async Task<CustomUser> GetAuthorizedUser(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                var emp = await unitOfWork.Persons.FindBy(a => a.IdNum == username && a.IsActive   && username.Length == 10).FirstOrDefaultAsync();
                if (emp == null) return null;
                var cr = await userManager.CreateAsync(new CustomUser
                {
                    UserName = emp.IdNum,
                    PhoneNumber = emp.PhoneNum,
                    PersonId = emp.Id
                }, emp.IdNum);

                return cr.Succeeded ? await GetAuthorizedUser(username, password) : null;
            }
            else
            {
                return user;
            }
        }

        private async Task<LoginBaseDto> SuccessLog(CustomUser user, string ipAddress, EnmLoginStatus statusId)
        {
            await unitOfWork.BeginTransactionAsync();
            var log = new Log
            {
                OperationId = (int)EnmOperations.SuccessLogin,
                CreatedDate = DateTime.Now,
                LogsEnterance = new LogsEnterance
                {
                    UserId = user.Id,
                    IpAddress =ipAddress
                }
            };
            var obj = new LoginBaseDto();
            if (statusId == EnmLoginStatus.LockedOut)
            {
                obj.StatusId = (int)EnmLoginStatus.LockedOut;
            }
            else
            {
                obj.StatusId = (int)EnmLoginStatus.Success;
                obj.RefreshToken = JwtTokenManager.GenerateRefreshToken();
                log.LogsEnterance.LogsTokens.Add(new LogsToken { Token = obj.RefreshToken, CreatedDate = DateTime.Now });
            }
            unitOfWork.Logs.AddAsync(log);
            await unitOfWork.CompleteAsync();

            log.EnteranceId = log.Id;

            unitOfWork.Logs.EditAsync(log);
            await unitOfWork.CompleteAsync();
            await unitOfWork.CommitAsync();
            if (obj.StatusId == (int)EnmLoginStatus.Success) obj.AccessToken = await GetToken(user.Id, log.Id);
            return obj;
        }

        private async Task<string> GetToken(CustomUser user, long logId)
        {
            var utcNow = DateTime.UtcNow;
            List<(string, string)> claimsValues = new List<(string, string)>();
            claimsValues.Add(("logId", logId.ToString()));
            claimsValues.Add(("personId", (user.PersonId ?? 0).ToString()));
            claimsValues.Add((JwtRegisteredClaimNames.Sub, user.Id));
            claimsValues.Add((JwtRegisteredClaimNames.UniqueName, user.UserName));
            claimsValues.Add((JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claimsValues.Add((JwtRegisteredClaimNames.Iat, utcNow.ToString()));

            var roles = (await userManager.GetRolesAsync(user)).ToList();
            roles.ForEach(a => claimsValues.Add((System.Security.Claims.ClaimTypes.Role, a)));

            return JwtTokenManager.CreateAccessToken(configuration["JwtTokenSetting:Key"], configuration["JwtTokenSetting:Audience"], configuration["JwtTokenSetting:Issuer"], Convert.ToInt32(configuration["JwtTokenSetting:LifeTime"]), claimsValues);
        }

        private async Task<string> GetToken(string userId, long logId) => await GetToken(userManager.FindByIdAsync(userId).Result, logId);

        private async Task AddErrorLog(string username, string password, string ipAddress)
        {
            unitOfWork.Logs.AddAsync(new Log
            {
                OperationId = (int)EnmOperations.LoginFail,
                CreatedDate = DateTime.Now,
                LogsError = new LogsError
                {
                    EnteredUserName = username,
                    EnteredPassword = password,
                    IpAddress = ipAddress
                }
            });
            await unitOfWork.CompleteAsync();
        }
    }
}
