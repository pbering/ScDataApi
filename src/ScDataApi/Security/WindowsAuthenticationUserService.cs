﻿namespace ScDataApi.Security
{
    public class WindowsAuthenticationUserService : IAuthenticationService
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