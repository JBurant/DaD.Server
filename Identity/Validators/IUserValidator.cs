namespace Identity.Validators
{
    public interface IUserValidator
    {
        bool ValidateUsername(string username);
    }
}