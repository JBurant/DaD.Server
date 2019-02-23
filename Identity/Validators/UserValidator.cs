using System.Text.RegularExpressions;

namespace Identity.Validators
{
    public class UserValidator : IUserValidator
    {
        public bool ValidateUsername(string username)
        {
            var regExpr = new Regex("^[a-zA-Z0-9]*$");

            if (username.Length < 5 ||
                username.Length > 20 ||
                !regExpr.IsMatch(username))
            {
                return false;
            }
            return true;
        }
    }
}