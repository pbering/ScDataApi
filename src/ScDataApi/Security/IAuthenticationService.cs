namespace ScDataApi.Security
{
    public interface IAuthenticationService
    {
        string GetUserName();

        bool IsAuthenticated();
    }
}