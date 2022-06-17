using PropMng.Api.data;
using PropMng.Api.Models;
using System;

namespace PropMng.Api.BusinessArea.SystemArea
{
    public class MyLogManager : IMyLogManager
    {

        private Log MainOperation(long enteranceId, string screenCode, string details, EnmOperations operation)
        {
            var o = new Log
            {
                OperationId = (int)operation,
                CreatedDate = DateTime.Now,
                ScreenCode = screenCode.ToUpper(),
                EnteranceId = enteranceId
            };
            if (!string.IsNullOrEmpty(details))
                o.LogsOperation = new LogsOperation { Details = details };
            return o;
        }

        public Log Browse(long enteranceId, string screenCode, string details) => MainOperation(enteranceId, screenCode, details, EnmOperations.Browse);
        public Log Create(long enteranceId, string screenCode, string details) => MainOperation(enteranceId, screenCode, details, EnmOperations.Add);
        public Log Update(long enteranceId, string screenCode, string details) => MainOperation(enteranceId, screenCode, details, EnmOperations.Edit);
        public Log Remove(long enteranceId, string screenCode, string details) => MainOperation(enteranceId, screenCode, details, EnmOperations.Delete);
    }
}
