using Common.DTO;
using System.Collections.Generic;

namespace Common.Services
{
    public class ErrorListProvider : IErrorListProvider
    {
        public List<Error> ErrorList { get; }

        public ErrorListProvider()
        {
            ErrorList = new List<Error>()
            {
                new Error() { ErrorCode = ErrorCode.IE0000, ErrorMessage = "Invalid file format" },
                new Error() { ErrorCode = ErrorCode.IE0001, ErrorMessage = "File already exists" },
                new Error() { ErrorCode = ErrorCode.IE0002, ErrorMessage = "File doe not exist" },
                new Error() { ErrorCode = ErrorCode.IE0003, ErrorMessage = "User creation failed" },
                new Error() { ErrorCode = ErrorCode.IE0004, ErrorMessage = "Login password incorrect" },
                new Error() { ErrorCode = ErrorCode.IE0005, ErrorMessage = "User does not exist" },
                new Error() { ErrorCode = ErrorCode.IE0006, ErrorMessage = "User already exists" },
                new Error() { ErrorCode = ErrorCode.IE0007, ErrorMessage = "Invalid form of username" },
                new Error() { ErrorCode = ErrorCode.IE0008, ErrorMessage = "Invalid form of password" },
                new Error() { ErrorCode = ErrorCode.IE0009, ErrorMessage = "Invalid form of email" },
                new Error() { ErrorCode = ErrorCode.IE0010, ErrorMessage = "Unable to delete article" },
                new Error() { ErrorCode = ErrorCode.IE0011, ErrorMessage = "Unable to write article" }
            };
        }

        public Error GetError(ErrorCode code)
        {
            return ErrorList.Find(x => x.ErrorCode == code);
        }
    }
}
