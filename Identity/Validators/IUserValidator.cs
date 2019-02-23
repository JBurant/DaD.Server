using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Validators
{
    public interface IUserValidator
    {
        bool ValidateUsername(string username);
    }
}
