using PropMng.Api.data;

namespace PropMng.Api.BusinessArea.SystemArea
{
    public interface IMyLogManager
    {
        Log Browse(long enteranceId, string screenCode) => Browse(enteranceId, screenCode, string.Empty);
        Log Browse(long enteranceId, string screenCode, string details);
        Log Create(long enteranceId, string screenCode, string details);
        Log Remove(long enteranceId, string screenCode, string details);
        Log Update(long enteranceId, string screenCode, string details);
    }
}