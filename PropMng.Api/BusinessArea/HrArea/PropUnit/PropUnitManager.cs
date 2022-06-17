using Gdnc.Services.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using PropMng.Api.BusinessArea.SystemArea;
using PropMng.Api.data;
using PropMng.Api.Models;
using PropMng.Api.Models.HrArea.PropUnit;
using PropMng.Api.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropMng.Api.BusinessArea.HrArea.PropUnit
{
    public class PropUnitManager : IPropUnitManager
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMyLogManager myLogManager;

        public PropUnitManager(
            IUnitOfWork unitOfWork
           , IMyLogManager myLogManager
            )
        {

            this.unitOfWork = unitOfWork;
            this.myLogManager=myLogManager;
        }

        public async Task<PostResponseModel<ItemsListModel<PropUnitModel>>> GetAll(UserInfo userInfo, string screenCode, PropUnitFilterModel f)
        {
            unitOfWork.Logs.AddAsync(myLogManager.Browse(userInfo.LogId, screenCode, f.ToString()));
            await unitOfWork.CompleteAsync();
            var q = unitOfWork.PropsUnits
                .FindBy(a => !a.IsTrash)
                .Select(a => new PropUnitModel
                {
                    Id=a.Id,
                    Name=a.Name,
                    PropName=a.Prop.CmName,
                    MonthlyRent=a.MonthlyRent,
                    Code=a.Code
                });


            if (!string.IsNullOrEmpty(f.SearchWord))
            {
                var lst = f.SearchWordsList;
                foreach (var i in lst)
                    q=q.Where(a => a.Address.Contains(i) || string.IsNullOrEmpty(i));
            }
            var itemsCount = await q.CountAsync();
            if (itemsCount == 0)
                return PostResponseModel<ItemsListModel<PropUnitModel>>.GetSuccess(new ItemsListModel<PropUnitModel>());
            f.PageNumber = f.PageNumber == 0 ? 0 : f.PageNumber - 1;
            f.PageSize = f.PageSize == 0 ? 10 : f.PageSize;

            return PostResponseModel<ItemsListModel<PropUnitModel>>.GetSuccess(new ItemsListModel<PropUnitModel>
            {
                ItemsCount = itemsCount,
                Items = await q.OrderBy(a => a.Id).Skip(f.PageNumber * f.PageSize).Take(f.PageSize).ToListAsync()
            });

        }

        public async Task<PostResponseModel<PropUnitModel>> GetById(UserInfo userInfo, string screenCode, long id)
        {
            var o = await unitOfWork.PropsUnits.FindBy(a => !a.IsTrash && a.Id == id).SingleOrDefaultAsync();
            if (o == null) return PostResponseModel<PropUnitModel>.GetError(1);

            o.PropsUnitsLogs.Add(new PropsUnitsLog { Log = myLogManager.Browse(userInfo.LogId, screenCode) });
            unitOfWork.PropsUnits.EditAsync(o);
            await unitOfWork.CompleteAsync();

            var obj = await unitOfWork.PropsUnits.FindBy(a => a.Id == id)
                .Select(a => new PropUnitModel
                {
                    Id=a.Id,
                    Address=a.Address,
                    MonthlyRent=a.MonthlyRent,
                    Name=a.Name,
                    PropId=a.PropId,
                    Code=a.Code,
                    Utilities = a.Utilities
                }).SingleOrDefaultAsync();

            return PostResponseModel<PropUnitModel>.GetSuccess(obj);
        }

        public async Task<PostResponseModel<PropUnitModel>> Create(UserInfo userInfo, string screenCode, PropUnitModel model)
        {
            var lst = await unitOfWork.PropsUnits
                .FindBy(a => a.PropId==model.PropId&&!a.IsTrash)
                .OrderByDescending(a => a.Num)
                .Select(a => a.Num).FirstOrDefaultAsync()+1;
            var prop = await unitOfWork.Props.FindBy(a => a.Id==model.PropId).Select(a => a.Code).SingleOrDefaultAsync();
            var obj = new PropsUnit
            {
                Num = lst,
                Code=$"{prop}-{lst.ToString("0000")}",
                IsTrash = false,
                CreatedDate=DateTime.Now
            };
            FillObj(obj, model);

            obj.PropsUnitsLogs.Add(new PropsUnitsLog { Log = myLogManager.Create(userInfo.LogId, screenCode, model.ToString()) });
            unitOfWork.PropsUnits.AddAsync(obj);
            await unitOfWork.CompleteAsync();
            model.Id=obj.Id;
            return PostResponseModel<PropUnitModel>.GetSuccess(model);
        }

        public async Task<PostResponseModel<bool>> Update(UserInfo userInfo, string screenCode, PropUnitModel model)
        {
            var obj = await unitOfWork.PropsUnits.FindBy(a => !a.IsTrash && a.Id == model.Id)
                .SingleOrDefaultAsync();
            if (obj == null) return PostResponseModel<bool>.GetError(1);

            obj.PropsUnitsLogs.Add(new PropsUnitsLog
            {
                Log = myLogManager.Update(userInfo.LogId, screenCode, model.ToString())
            });

            FillObj(obj, model);

            unitOfWork.PropsUnits.EditAsync(obj);
            await unitOfWork.CompleteAsync();
            return PostResponseModel<bool>.GetSuccess(true);
        }

        private void FillObj(PropsUnit o, PropUnitModel m)
        {
            o.Name = m.Name;
            o.Address=m.Address;
            o.PropId=m.PropId;
            o.MonthlyRent=m.MonthlyRent;
            o.Utilities=m.Utilities;
        }


        public async Task<PostResponseModel<bool>> Delete(UserInfo userInfo, string screenCode, long id)
        {
            if (await unitOfWork.Invoices.AnyAsync(a => a.UnitId==id))
                return PostResponseModel<bool>.GetError(5);
            var obj = await unitOfWork.PropsUnits.FindAsync(id);
            if (obj == null) return PostResponseModel<bool>.GetError(1);

            obj.PropsUnitsLogs.Add(new PropsUnitsLog { Log = myLogManager.Remove(userInfo.LogId, screenCode, $"Remove:{id}") });

            obj.IsTrash = true;
            unitOfWork.PropsUnits.EditAsync(obj);
            await unitOfWork.CompleteAsync();
            return PostResponseModel<bool>.GetSuccess(true);
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
    }
}