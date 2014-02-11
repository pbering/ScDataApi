using System;
using ScDataApi.Security;
using Sitecore.Configuration;
using Sitecore.Xml;

namespace ScDataApi.Configuration
{
    public static class ServiceLocator
    {
        public static IAuthenticationService GetAuthenticationService()
        {
            // TODO: Config parameters: http://www.sitecore.net/unitedkingdom/Community/Technical-Blogs/John-West-Sitecore-Blog/Posts/2011/02/The-Sitecore-ASPNET-CMS-Configuration-Factory.aspx

            return CreateInstance<IAuthenticationService>("dataApiServices/authentication");
        }

        private static T CreateInstance<T>(string configurationPath) where T : class
        {
            if (string.IsNullOrEmpty(configurationPath))
            {
                throw new ArgumentException("Parameter value must be supplied.", "configurationPath");
            }

            var config = Factory.GetConfigNode(configurationPath);

            if (config == null)
            {
                throw new Exception(string.Format("Could not locate configuration in path '{0}'.", configurationPath));
            }

            var service = Factory.CreateObject<T>(config);

            if (service == null)
            {
                throw new Exception(string.Format("Could not create type '{0}' from configuration in path '{1}'.", config, XmlUtil.GetAttribute("type", config)));
            }

            return service;
        }
    }
}