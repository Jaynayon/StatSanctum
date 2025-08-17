namespace StatSanctum.API.Helper
{
    public interface IJwtHelper
    {
        string GenerateToken(string sub, string name);
    }
}
