namespace Common.DTO
{
    public class Error
    {
        public Error(ErrorCode errorCode)
        {
            ErrorCode = errorCode;
        }

        public ErrorCode ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}