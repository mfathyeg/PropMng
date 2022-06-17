using Gdnc.Services.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using PropMng.Api.BusinessArea.SystemArea;
using PropMng.Api.data;
using PropMng.Api.Models;
using PropMng.Api.Models.HrArea.Inc;
using PropMng.Api.Models.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PropMng.Api.BusinessArea.HrArea.Inc
{
    public class IncManager : IIncManager
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMyLogManager myLogManager;

        public IncManager(
            IUnitOfWork unitOfWork
           , IMyLogManager myLogManager
            )
        {

            this.unitOfWork = unitOfWork;
            this.myLogManager=myLogManager;
        }

        public async Task<PostResponseModel<ItemsListModel<IncModel>>> GetAll(UserInfo userInfo, string screenCode, IncFilterModel f)
        {
            unitOfWork.Logs.AddAsync(myLogManager.Browse(userInfo.LogId, screenCode, f.ToString()));
            await unitOfWork.CompleteAsync();
            var q = unitOfWork.Incomes
                .FindBy(a => !a.IsTrash)
                .Select(a => new IncModel
                {
                    Id=a.Id,
                    Amount=a.Amount,
                    CreatedDate=a.CreatedDate,
                    InvoiceId=a.InvoiceId,
                    InvoiceNum=a.Invoice.Num,
                    PersonName=a.Invoice.Person.FullName,
                    PropName=a.Invoice.Unit.Prop.CmName,
                    ReceiptNum=a.ReceiptNum,
                    TypeId=a.TypeId,
                    UnitName=a.Invoice.Unit.Name
                });


            if (!string.IsNullOrEmpty(f.SearchWord))
            {
                var lst = f.SearchWordsList;
                foreach (var i in lst)
                    q=q.Where(a => a.PersonName.Contains(i) || string.IsNullOrEmpty(i));
            }
            var itemsCount = await q.CountAsync();
            if (itemsCount == 0)
                return PostResponseModel<ItemsListModel<IncModel>>.GetSuccess(new ItemsListModel<IncModel>());
            f.PageNumber = f.PageNumber == 0 ? 0 : f.PageNumber - 1;
            f.PageSize = f.PageSize == 0 ? 10 : f.PageSize;

            return PostResponseModel<ItemsListModel<IncModel>>.GetSuccess(new ItemsListModel<IncModel>
            {
                ItemsCount = itemsCount,
                Items = await q.OrderBy(a => a.Id).Skip(f.PageNumber * f.PageSize).Take(f.PageSize).ToListAsync()
            });

        }

        public async Task<PostResponseModel<IncModel>> GetById(UserInfo userInfo, string screenCode, long id)
        {
            var o = await unitOfWork.Incomes.FindBy(a => !a.IsTrash && a.Id == id).SingleOrDefaultAsync();
            if (o == null) return PostResponseModel<IncModel>.GetError(1);

            o.IncomesLogs.Add(new IncomesLog { Log = myLogManager.Browse(userInfo.LogId, screenCode) });
            unitOfWork.Incomes.EditAsync(o);
            await unitOfWork.CompleteAsync();

            var obj = await unitOfWork.Incomes.FindBy(a => a.Id == id)
                .Select(a => new IncModel
                {
                    Id=a.Id,
                    CreatedDate=a.CreatedDate,
                    Amount=a.Amount,
                    InvoiceId=a.InvoiceId,
                    TypeId=a.TypeId,
                    ReceiptNum=a.ReceiptNum
                }).SingleOrDefaultAsync();

            return PostResponseModel<IncModel>.GetSuccess(obj);
        }

        public async Task<PostResponseModel<IncModel>> Create(UserInfo userInfo, string screenCode, IncModel model)
        {
            var obj = new Income
            {
                IsTrash = false,
                CreatedDate=DateTime.Now,
                InvoiceId = model.InvoiceId
            };
            FillObj(obj, model);

            obj.IncomesLogs.Add(new IncomesLog { Log = myLogManager.Create(userInfo.LogId, screenCode, model.ToString()) });
            unitOfWork.Incomes.AddAsync(obj);
            await unitOfWork.CompleteAsync();
            model.Id=obj.Id;
            return PostResponseModel<IncModel>.GetSuccess(model);
        }

        public async Task<PostResponseModel<bool>> Update(UserInfo userInfo, string screenCode, IncModel model)
        {
            var obj = await unitOfWork.Incomes.FindBy(a => !a.IsTrash && a.Id == model.Id)
                .SingleOrDefaultAsync();
            if (obj == null) return PostResponseModel<bool>.GetError(1);

            obj.IncomesLogs.Add(new IncomesLog
            {
                Log = myLogManager.Update(userInfo.LogId, screenCode, model.ToString())
            });

            FillObj(obj, model);

            unitOfWork.Incomes.EditAsync(obj);
            await unitOfWork.CompleteAsync();
            return PostResponseModel<bool>.GetSuccess(true);
        }

        private void FillObj(Income o, IncModel m)
        {
            o.TypeId=m.TypeId;
            o.ReceiptNum=m.ReceiptNum;
            o.Amount=m.Amount;
        } 

        public async Task<PostResponseModel<bool>> Delete(UserInfo userInfo, string screenCode, long id)
        {
            var obj = await unitOfWork.Incomes.FindAsync(id);
            if (obj == null) return PostResponseModel<bool>.GetError(1);

            obj.IncomesLogs.Add(new IncomesLog { Log = myLogManager.Remove(userInfo.LogId, screenCode, $"Remove:{id}") });

            obj.IsTrash = true;
            unitOfWork.Incomes.EditAsync(obj);
            await unitOfWork.CompleteAsync();
            return PostResponseModel<bool>.GetSuccess(true);
        }
    }
}