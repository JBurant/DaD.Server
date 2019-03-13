using Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Services
{
    public interface IErrorListProvider
    {
        List<Error> ErrorList { get; }

        Error GetError(ErrorCode code);
    }
}
