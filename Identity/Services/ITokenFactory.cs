namespace Identity.Services
{
    public interface ITokenFactory
    {
        string GenerateToken(int size);
    }
}