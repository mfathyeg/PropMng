using Gdnc.Services.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using PropMng.Api.BusinessArea.SystemArea;
using PropMng.Api.data;
using PropMng.Api.Models;
using PropMng.Api.Models.HrArea.Persons;
using PropMng.Api.Models.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PropMng.Api.BusinessArea.HrArea.Persons
{
    public class PersonsManager : IPersonsManager
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMyLogManager myLogManager;

        public PersonsManager(
            IUnitOfWork unitOfWork
           , IMyLogManager myLogManager
            )
        {

            this.unitOfWork = unitOfWork;
            this.myLogManager=myLogManager;
        }

        public async Task<PostResponseModel<ItemsListModel<PersonsModel>>> GetAll(UserInfo userInfo, string screenCode, PersonsFilterModel f)
        {
            unitOfWork.Logs.AddAsync(myLogManager.Browse(userInfo.LogId, screenCode, f.ToString()));
            await unitOfWork.CompleteAsync();
            var q = unitOfWork.Persons
                .FindBy(a => !a.IsTrash)
                .Select(a => new PersonsModel
                {
                    Id=a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    FullName = a.FullName,
                    BirthDate=a.BirthDate,
                    IdNum=a.IdNum,
                    IsActive = a.IsActive,
                    PhoneNum=a.PhoneNum,
                    IsMale = a.IsMale,
                    IsCorp = a.IsCorp
                });


            if (!string.IsNullOrEmpty(f.SearchWord))
            {
                var lst = f.SearchWordsList;
                if (lst.Count==1&&long.TryParse(f.DcdSearchWord, out long _id))
                    q=q.Where(a => a.IdNum.Contains(_id.ToString()));
                else
                    foreach (var i in lst)
                        q=q.Where(a => a.FullName.Contains(i) || string.IsNullOrEmpty(i));
            }
            var itemsCount = await q.CountAsync();
            if (itemsCount == 0)
                return PostResponseModel<ItemsListModel<PersonsModel>>.GetSuccess(new ItemsListModel<PersonsModel>());
            f.PageNumber = f.PageNumber == 0 ? 0 : f.PageNumber - 1;
            f.PageSize = f.PageSize == 0 ? 10 : f.PageSize;

            return PostResponseModel<ItemsListModel<PersonsModel>>.GetSuccess(new ItemsListModel<PersonsModel>
            {
                ItemsCount = itemsCount,
                Items = await q.OrderBy(a => a.Id).Skip(f.PageNumber * f.PageSize).Take(f.PageSize).ToListAsync()
            });

        }

        public async Task<PostResponseModel<PersonsModel>> GetById(UserInfo userInfo, string screenCode, long id)
        {
            var o = await unitOfWork.Persons.FindBy(a => !a.IsTrash && a.Id == id).SingleOrDefaultAsync();
            if (o == null) return PostResponseModel<PersonsModel>.GetError(1);

            o.PersonsLogs.Add(new PersonsLog { Log = myLogManager.Browse(userInfo.LogId, screenCode) });
            unitOfWork.Persons.EditAsync(o);
            await unitOfWork.CompleteAsync();

            var obj = await unitOfWork.Persons.FindBy(a => a.Id == id)
                .Select(a => new PersonsModel
                {
                    Id=a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    FullName = a.FullName,
                    BirthDate=a.BirthDate,
                    IdNum=a.IdNum,
                    IsActive = a.IsActive,
                    IsMale = a.IsMale,
                    IsCorp = a.IsCorp,
                    PhoneNum=a.PhoneNum,
                    CrName = a.IsCorp && a.PersonsCorp!=null ? a.PersonsCorp.CrName : String.Empty,
                    CrNum = a.IsCorp && a.PersonsCorp!=null ? a.PersonsCorp.CrNum : String.Empty,
                    Details = a.Details
                }).SingleOrDefaultAsync();

            return PostResponseModel<PersonsModel>.GetSuccess(obj);
        }

        public async Task<PostResponseModel<PersonsModel>> Create(UserInfo userInfo, string screenCode, PersonsModel model)
        {
            if (await unitOfWork.Persons.FindBy(a => a.IdNum == model.IdNum    && !a.IsTrash).AnyAsync())
                return PostResponseModel<PersonsModel>.GetError(2);

            var obj = new Person
            {
                IsTrash = false,
                CreatedDate=DateTime.Now
            };
            FillObj(obj, model);

            obj.PersonsLogs.Add(new PersonsLog { Log = myLogManager.Create(userInfo.LogId, screenCode, model.ToString()) });
            unitOfWork.Persons.AddAsync(obj);
            await unitOfWork.CompleteAsync();
            model.Id=obj.Id;
            return PostResponseModel<PersonsModel>.GetSuccess(model);
        }

        public async Task<PostResponseModel<bool>> Update(UserInfo userInfo, string screenCode, PersonsModel model)
        {
            var obj = await unitOfWork.Persons.FindBy(a => !a.IsTrash && a.Id == model.Id)
                .Include(a => a.PersonsCorp)
                .SingleOrDefaultAsync();
            if (obj == null) return PostResponseModel<bool>.GetError(1);

            if (await unitOfWork.Persons.FindBy(a => a.IdNum == model.IdNum && !a.IsTrash && a.Id != model.Id).AnyAsync())
                return PostResponseModel<bool>.GetError(2);

            obj.PersonsLogs.Add(new PersonsLog
            {
                Log = myLogManager.Update(userInfo.LogId, screenCode, model.ToString())
            });

            FillObj(obj, model);

            unitOfWork.Persons.EditAsync(obj);
            await unitOfWork.CompleteAsync();
            return PostResponseModel<bool>.GetSuccess(true);
        }

        private void FillObj(Person o, PersonsModel m)
        {
            o.IdNum=m.IdNum;
            o.FirstName=m.FirstName;
            o.LastName=m.LastName;
            o.FullName=$"{m.FirstName} {m.LastName}";
            o.IsMale=m.IsMale;
            o.IsCorp=m.IsCorp;
            o.BirthDate=m.BirthDate;
            o.IsActive=m.IsActive;
            o.PhoneNum = m.PhoneNum;
            if (o.IsCorp)
            {
                if (o.PersonsCorp==null)
                    o.PersonsCorp = new PersonsCorp();
                FillSub(o.PersonsCorp, m);
            }
        }

        private void FillSub(PersonsCorp o, PersonsModel m)
        {
            o.CrNum=m.CrNum;
            o.CrName = m.CrName;
        }

        public async Task<PostResponseModel<bool>> Delete(UserInfo userInfo, string screenCode, long id)
        {
            if (await unitOfWork.Invoices.AnyAsync(a => a.PersonId==id))
                return PostResponseModel<bool>.GetError(5);
            var obj = await unitOfWork.Persons.FindAsync(id);
            if (obj == null) return PostResponseModel<bool>.GetError(1);

            obj.PersonsLogs.Add(new PersonsLog { Log = myLogManager.Remove(userInfo.LogId, screenCode, $"Remove:{id}") });

            obj.IsTrash = true;
            unitOfWork.Persons.EditAsync(obj);
            await unitOfWork.CompleteAsync();
            return PostResponseModel<bool>.GetSuccess(true);
        }
    }
}