
namespace Quotes.Core.Models
{
    public class Result
    {
        private readonly static Result sucess = new(true, null);
        public bool IsSuccess { get; set; }
        public Error? Error { get; set; }


        private Result(bool isSucess, Error? error)
        {
            IsSuccess = isSucess;
            Error = error;  
        }

        internal static Result Sucess()
        {
            return sucess;
        }

        internal static Result Fail(Error error)
        {
            return new Result(false, error);
        }
    }
}
