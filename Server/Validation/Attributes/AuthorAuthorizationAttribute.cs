using Microsoft.AspNetCore.Authorization;

namespace Server.Attributes
{
    public class AuthorAuthorizationAttribute : AuthorizeAttribute
    {
        private const string POLICY_PREFIX = "AuthorAuthorization";
    }
}