using Gdnc.Services.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PropMng.Api.BusinessArea.SystemArea;
using PropMng.Api.data;
using PropMng.Api.Models;
using PropMng.Api.Models.HrArea.Inv;
using PropMng.Api.Models.HrArea.PropUnit;
using PropMng.Api.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropMng.Api.BusinessArea.HrArea.Inv
{
    public class InvManager : IInvManager
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMyLogManager myLogManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public InvManager(
            IUnitOfWork unitOfWork
           , IMyLogManager myLogManager
            , RoleManager<IdentityRole> roleManager
            )
        {

            this.unitOfWork = unitOfWork;
            this.myLogManager=myLogManager;
            this.roleManager=roleManager;
        }

        public async Task<PostResponseModel<ItemsListModel<InvModel>>> GetAll(UserInfo userInfo, string screenCode, InvFilterModel f)
        {
            unitOfWork.Logs.AddAsync(myLogManager.Browse(userInfo.LogId, screenCode, f.ToString()));
            await unitOfWork.CompleteAsync();
            var q = unitOfWork.Invoices
                .FindBy(a => !a.IsTrash)
                .Select(a => new InvModel
                {
                    Id=a.Id,
                    Num=a.Num,
                    MonthlyRent= a.MonthlyRent,
                    StartMonth=a.StartMonth,
                    StartYear=a.StartYear,
                    EndMonth=a.EndMonth,
                    EndYear=a.EndYear,
                    PersonName=a.Person.FullName,
                    UnitName=a.Unit.Name,
                    PropName=a.Unit.Prop.CmName
                });

            if (!string.IsNullOrEmpty(f.SearchWord))
            {
                var lst = f.SearchWordsList;
                foreach (var i in lst)
                    q=q.Where(a => a.Num.ToString().Contains(i) || string.IsNullOrEmpty(i));
            }
            var itemsCount = await q.CountAsync();
            if (itemsCount == 0)
                return PostResponseModel<ItemsListModel<InvModel>>.GetSuccess(new ItemsListModel<InvModel>());
            f.PageNumber = f.PageNumber == 0 ? 0 : f.PageNumber - 1;
            f.PageSize = f.PageSize == 0 ? 10 : f.PageSize;

            return PostResponseModel<ItemsListModel<InvModel>>.GetSuccess(new ItemsListModel<InvModel>
            {
                ItemsCount = itemsCount,
                Items = await q.OrderBy(a => a.Id).Skip(f.PageNumber * f.PageSize).Take(f.PageSize).ToListAsync()
            });

        }

        public async Task<PostResponseModel<InvModel>> GetById(UserInfo userInfo, string screenCode, long id)
        {
            var o = await unitOfWork.Invoices.FindBy(a => !a.IsTrash && a.Id == id).SingleOrDefaultAsync();
            if (o == null) return PostResponseModel<InvModel>.GetError(1);
            o.InvoicesLogs.Add(new InvoicesLog { Log = myLogManager.Browse(userInfo.LogId, screenCode) });
            unitOfWork.Invoices.EditAsync(o);
            await unitOfWork.CompleteAsync();

            var obj = await unitOfWork.Invoices.FindBy(a => a.Id == id)
                .Select(a => new InvModel
                {
                    Id=a.Id,
                    PersonId=a.PersonId,
                    UnitId= a.UnitId,
                    PropId=a.Unit.PropId,
                    Num=a.Num,
                    PersonName=a.Person.FullName,
                    PropName=a.Unit.Prop.CmName + " " + a.Unit.Prop.Code,
                    UnitName=a.Unit.Name +  " " + a.Unit.Code +  " " + a.Unit.MonthlyRent,
                    StartYear=a.StartYear,
                    StartMonth=a.StartMonth,
                    EndMonth=a.EndMonth,
                    EndYear=a.EndYear,
                    Utilities=a.Utilities,
                    MonthlyRent=a.MonthlyRent,
                    Details=a.Details,
                    Utlities=a.Utilities
                }).SingleOrDefaultAsync();

            return PostResponseModel<InvModel>.GetSuccess(obj);
        }

        public async Task<PostResponseModel<InvModel>> Create(UserInfo userInfo, string screenCode, InvModel model)
        {
            if (await unitOfWork.Invoices.FindBy(a => a.PersonId==model.PersonId && a.UnitId==model.UnitId && !a.IsTrash).AnyAsync())
                return PostResponseModel<InvModel>.GetError(2);

            var lst = await unitOfWork.Invoices
                .FindBy(a => !a.IsTrash)
                .OrderByDescending(a => a.Id)
                .Select(a => a.Num).FirstOrDefaultAsync()+1;

            var unit = await unitOfWork.PropsUnits.FindBy(a => a.Id==model.UnitId && !a.IsTrash).SingleOrDefaultAsync();

            var obj = new Invoice
            {
                IsTrash = false,
                CreatedDate=DateTime.Now,
                Num =lst,
                MonthlyRent=unit.MonthlyRent,
                Utilities = unit.Utilities,
                UnitId = unit.Id
            };
            FillObj(obj, model);

            obj.InvoicesLogs.Add(new InvoicesLog { Log = myLogManager.Create(userInfo.LogId, screenCode, model.ToString()) });
            unitOfWork.Invoices.AddAsync(obj);
            await unitOfWork.CompleteAsync();
            model.Id=obj.Id;
            return PostResponseModel<InvModel>.GetSuccess(model);
        }

        public async Task<PostResponseModel<bool>> Update(UserInfo userInfo, string screenCode, InvModel model)
        {
            var obj = await unitOfWork.Invoices
                .FindBy(a => !a.IsTrash && a.Id == model.Id)
                .SingleOrDefaultAsync();
            if (obj == null) return PostResponseModel<bool>.GetError(1);

            if (await unitOfWork.Invoices.FindBy(a => a.PersonId == model.PersonId && a.UnitId==model.UnitId && !a.IsTrash && a.Id != model.Id).AnyAsync())
                return PostResponseModel<bool>.GetError(2);

            obj.InvoicesLogs.Add(new InvoicesLog
            {
                Log = myLogManager.Update(userInfo.LogId, screenCode, model.ToString())
            });

            FillObj(obj, model);

            unitOfWork.Invoices.EditAsync(obj);
            await unitOfWork.CompleteAsync();
            return PostResponseModel<bool>.GetSuccess(true);
        }

        private void FillObj(Invoice o, InvModel m)
        {
            o.PersonId = m.PersonId;
            o.StartYear=m.StartYear;
            o.StartMonth=m.StartMonth;
            o.EndYear=m.EndYear;
            o.EndMonth=m.EndMonth;
            o.Months=m.Months;
            o.Details = m.Details;
        }

        public async Task<PostResponseModel<bool>> Delete(UserInfo userInfo, string screenCode, long id)
        {
            if (await unitOfWork.Incomes.AnyAsync(a => a.InvoiceId==id))
                return PostResponseModel<bool>.GetError(5);

            var obj = await unitOfWork.Invoices.FindAsync(id);
            if (obj == null) return PostResponseModel<bool>.GetError(1);

            obj.InvoicesLogs.Add(new InvoicesLog { Log = myLogManager.Remove(userInfo.LogId, screenCode, $"Remove:{id}") });

            obj.IsTrash = true;
            unitOfWork.Invoices.EditAsync(obj);
            await unitOfWork.CompleteAsync();
            return PostResponseModel<bool>.GetSuccess(true);
        }

        public async Task<PostResponseModel<List<LookupModel>>> GetCustomers()
        {

            var q = unitOfWork.Persons.FindBy(a => !a.IsTrash && a.IsActive && a.AspNetUsers.Any(b => b.AspNetUserRoles.Any(c => c.Role.Name.ToLower()=="customer")))
                .Select(a => new LookupModel
                {
                    Id=a.Id,
                    Name=a.FullName,
                    Details=a.IdNum
                });
            return PostResponseModel<List<LookupModel>>.GetSuccess(await q.ToListAsync());
        }

        public async Task<PostResponseModel<List<LookupModel>>> GetProps()
        {
            var q = unitOfWork.Props.FindBy(a => !a.IsTrash)
                .Select(a => new LookupModel
                {
                    Id=a.Id,
                    Name=a.CmName,
                    Details=a.Code
                });
            return PostResponseModel<List<LookupModel>>.GetSuccess(await q.ToListAsync());
        }

        public async Task<PostResponseModel<List<LookupModel>>> GetUnits(long propId)
        {
            var q = await unitOfWork.PropsUnits
                .FindBy(a => !a.IsTrash&& a.PropId==propId).Select(a => new LookupModel
                {
                    Id=a.Id,
                    Name= a.Name + " " + a.Code,
                    Details = a.MonthlyRent.ToString()
                }).ToListAsync();
            return PostResponseModel<List<LookupModel>>.GetSuccess(q);
        }

        private LookupModel GetUnit(PropsUnit u) => new LookupModel
        {
            Id=u.Id,
            Name=$"{u.Name} {u.Code}",
            Details=$"Rent:{u.MonthlyRent}, Utl{u.Utilities}"
        };

        public async Task<PostResponseModel<LookupModel>> GetUnitInfo(long unitId)
        {
            return PostResponseModel<LookupModel>.GetSuccess(await unitOfWork.PropsUnits.FindBy(a => a.Id==unitId).Select(a => new LookupModel
            {
                Id=unitId,
                Name=a.MonthlyRent.ToString(),
                Details=a.Utilities
            }).SingleOrDefaultAsync());
        }
    }
}