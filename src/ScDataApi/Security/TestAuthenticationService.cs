namespace ScDataApi.Security
{
    public class TestAuthenticationService : IAuthenticationService
    {
        public string GetUserName()
        {
            return "sitecore\\pmb";
        }

        public bool IsAuthenticated()
        {
            return true;
        }
    }
}