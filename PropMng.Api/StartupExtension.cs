using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using PropMng.Api.DataArea.Identity;
using Gdnc.Services.UnitOfWorks;
using PropMng.Api.DataArea;
using PropMng.Api.data;
using PropMng.Api.BusinessArea.SystemArea;
using PropMng.Api.BusinessArea.HrArea.Persons;
using PropMng.Api.BusinessArea.HrArea.Prop;
using PropMng.Api.BusinessArea.HrArea.PropUnit;
using PropMng.Api.BusinessArea.HrArea.Inv;
using PropMng.Api.BusinessArea.HrArea.Inc;

namespace Gdnc.Midan.Api
{
    public static class StartupExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<PropsMngDbContext>();
            services.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAccountManager, AccountManager>();
            services.AddScoped<IPersonsManager, PersonsManager>();
            services.AddScoped<IMyLogManager, MyLogManager>();
            services.AddScoped<IPropManager, PropManager>();
            services.AddScoped<IPropUnitManager,  PropUnitManager>();
            services.AddScoped<IInvManager, InvManager>();
            services.AddScoped<IIncManager,  IncManager>();
        }

        public static void RegisterIdentityContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<CustomIdentityContext>(options =>
              options.UseSqlServer(config.GetConnectionString("PropsMngDbConnectionString")));

            services.AddIdentity<CustomUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<CustomIdentityContext>();
        }

        public static void RegisterJwtIdentityContext(this IServiceCollection services, string key, string audience, string issuer)
        {
            var signingKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    IssuerSigningKey = signingKey,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });
        }

        public static void AuthorizeSystem(this IServiceCollection services)
        {
            services.AddMvc(config =>
            {
                var policy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter(policy));
            });
        }
    }
}
