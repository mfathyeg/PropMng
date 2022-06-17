using System.Collections.Generic;

namespace PropMng.Api.Models
{
    public class PostResponseModel<T>
    {
        public T GetT { get; set; }
        public bool IsSucceed { get; set; }
        public List<int> Errors { get; set; }

        public static PostResponseModel<T> GetError(int errorId)
           => new PostResponseModel<T> { IsSucceed = false, Errors = new List<int> { errorId } };

        public static PostResponseModel<T> GetSuccess(T obj)
            => GetSuccess(obj, true, 0);
        public static PostResponseModel<T> GetSuccess(T obj, bool isSucceed)
            => GetSuccess(obj, isSucceed, 0);
        public static PostResponseModel<T> GetSuccess(T obj, bool isSucceed, int errorId)
           => new PostResponseModel<T> { GetT = obj, IsSucceed = isSucceed, Errors = errorId == 0 ? null : new List<int> { errorId } };
    }
}
