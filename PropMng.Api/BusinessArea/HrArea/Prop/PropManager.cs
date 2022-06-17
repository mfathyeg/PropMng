using Gdnc.Services.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using PropMng.Api.BusinessArea.SystemArea;
using PropMng.Api.data;
using PropMng.Api.Models;
using PropMng.Api.Models.HrArea.Prop;
using PropMng.Api.Models.HrArea.PropUnit;
using PropMng.Api.Models.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PropMng.Api.BusinessArea.HrArea.Prop
{
    public class PropManager : IPropManager
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMyLogManager myLogManager;

        public PropManager(
            IUnitOfWork unitOfWork
           , IMyLogManager myLogManager
            )
        {

            this.unitOfWork = unitOfWork;
            this.myLogManager=myLogManager;
        }

        public async Task<PostResponseModel<ItemsListModel<PropModel>>> GetAll(UserInfo userInfo, string screenCode, PropFilterModel f)
        {
            unitOfWork.Logs.AddAsync(myLogManager.Browse(userInfo.LogId, screenCode, f.ToString()));
            await unitOfWork.CompleteAsync();
            var q = unitOfWork.Props
                .FindBy(a => !a.IsTrash)
                .Select(a => new PropModel
                {
                    Id=a.Id,
                    CmName=a.CmName,
                    Code=a.Code
                });

            if (!string.IsNullOrEmpty(f.SearchWord))
            {
                var lst = f.SearchWordsList;
                if (lst.Count==1&&long.TryParse(f.DcdSearchWord, out long _id))
                    q=q.Where(a => a.Code.Contains(_id.ToString()));
                else
                    foreach (var i in lst)
                        q=q.Where(a => a.CmName.Contains(i) || string.IsNullOrEmpty(i));
            }
            var itemsCount = await q.CountAsync();
            if (itemsCount == 0)
                return PostResponseModel<ItemsListModel<PropModel>>.GetSuccess(new ItemsListModel<PropModel>());
            f.PageNumber = f.PageNumber == 0 ? 0 : f.PageNumber - 1;
            f.PageSize = f.PageSize == 0 ? 10 : f.PageSize;

            return PostResponseModel<ItemsListModel<PropModel>>.GetSuccess(new ItemsListModel<PropModel>
            {
                ItemsCount = itemsCount,
                Items = await q.OrderBy(a => a.Id).Skip(f.PageNumber * f.PageSize).Take(f.PageSize).ToListAsync()
            });

        }

        public async Task<PostResponseModel<PropModel>> GetById(UserInfo userInfo, string screenCode, long id)
        {
            var o = await unitOfWork.Props.FindBy(a => !a.IsTrash && a.Id == id).SingleOrDefaultAsync();
            if (o == null) return PostResponseModel<PropModel>.GetError(1);
            o.PropsLogs.Add(new PropsLog { Log = myLogManager.Browse(userInfo.LogId, screenCode) });
            unitOfWork.Props.EditAsync(o);
            await unitOfWork.CompleteAsync();

            var obj = await unitOfWork.Props.FindBy(a => a.Id == id)
                .Select(a => new PropModel
                {
                    Id=a.Id,
                    CmName=a.CmName,
                    Code=a.Code,
                    Address=a.Address,
                    CrNum=a.CrNum,
                    TaxRegNum=a.TaxRegNum,
                }).SingleOrDefaultAsync();

            return PostResponseModel<PropModel>.GetSuccess(obj);
        }

        public async Task<PostResponseModel<PropModel>> Create(UserInfo userInfo, string screenCode, PropModel model)
        {
            if (await unitOfWork.Props.FindBy(a => a.CmName == model.CmName && !a.IsTrash).AnyAsync())
                return PostResponseModel<PropModel>.GetError(2);

            var lst = long.TryParse(await unitOfWork.Props
                .FindBy(a => !a.IsTrash)
                .OrderByDescending(a => a.Id)
                .Select(a => a.Code).FirstOrDefaultAsync(), out long l) ? l+1 : 1;

            var obj = new data.Prop
            {
                IsTrash = false,
                CreatedDate=DateTime.Now,
                Code= lst.ToString("0000") 
            };
            FillObj(obj, model);

            obj.PropsLogs.Add(new PropsLog { Log = myLogManager.Create(userInfo.LogId, screenCode, model.ToString()) });
            unitOfWork.Props.AddAsync(obj);
            await unitOfWork.CompleteAsync();
            model.Id=obj.Id;
            return PostResponseModel<PropModel>.GetSuccess(model);
        }

        public async Task<PostResponseModel<bool>> Update(UserInfo userInfo, string screenCode, PropModel model)
        {
            var obj = await unitOfWork.Props
                .FindBy(a => !a.IsTrash && a.Id == model.Id)
                .Include(a => a.PropsUnits)
                .SingleOrDefaultAsync();
            if (obj == null) return PostResponseModel<bool>.GetError(1);

            if (await unitOfWork.Props.FindBy(a => a.CmName == model.CmName && !a.IsTrash && a.Id != model.Id).AnyAsync())
                return PostResponseModel<bool>.GetError(2);

            obj.PropsLogs.Add(new PropsLog
            {
                Log = myLogManager.Update(userInfo.LogId, screenCode, model.ToString())
            });

            FillObj(obj, model);

            unitOfWork.Props.EditAsync(obj);
            await unitOfWork.CompleteAsync();
            return PostResponseModel<bool>.GetSuccess(true);
        }

        private void FillObj(data.Prop o, PropModel m)
        {
            o.Address=m.Address;
            o.CmName=m.CmName;
            o.CrNum=m.CrNum;
            o.TaxRegNum=m.TaxRegNum;
        }



        public async Task<PostResponseModel<bool>> Delete(UserInfo userInfo, string screenCode, long id)
        {
            if (await unitOfWork.Invoices.AnyAsync(a => a.Unit.PropId==id))
                return PostResponseModel<bool>.GetError(5);

            var obj = await unitOfWork.Props.FindAsync(id);
            if (obj == null) return PostResponseModel<bool>.GetError(1);

            obj.PropsLogs.Add(new PropsLog { Log = myLogManager.Remove(userInfo.LogId, screenCode, $"Remove:{id}") });

            obj.IsTrash = true;
            unitOfWork.Props.EditAsync(obj);
            await unitOfWork.CompleteAsync();
            return PostResponseModel<bool>.GetSuccess(true);
        }
    }
}