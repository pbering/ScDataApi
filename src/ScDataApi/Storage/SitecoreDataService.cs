﻿using System;
using ScDataApi.Security;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Security.Accounts;

namespace ScDataApi.Storage
{
    public class SitecoreDataService
    {
        private readonly IAuthenticationService _authenticationService;

        public SitecoreDataService(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public bool ItemExists(string databaseName, string path)
        {
            var database = GetDatabase(databaseName);
            var language = GetLanguage("en");
            var item = GetItem(database, language, path);

            if (item != null)
            {
                return true;
            }

            return false;
        }

        public DataItem GetItem(string databaseName, string languageName, string path, string payload, string fields)
        {
            var database = GetDatabase(databaseName);
            var language = GetLanguage(languageName);

            return GetItem(database, language, path, payload, fields);
        }

        private DataItem GetItem(Database database, Language language, string path, string payload, string fields)
        {
            var item = GetItem(database, language, path);

            if (item == null)
            {
                return null;
            }

            return new DataItem(item, payload, fields);
        }

        private Item GetItem(Database database, Language language, string path)
        {
            // TODO: Check user has read access?

            using (new UserSwitcher(_authenticationService.GetUserName(), true))
            {
                return database.GetItem(path, language);
            }
        }

        public void CreateItem(string database, string language, string path, DataItem value)
        {
            using (new UserSwitcher(_authenticationService.GetUserName(), true))
            {
                throw new NotImplementedException();
            }
        }

        public void UpdateItem(string database, string language, string path, DataItem value)
        {
            using (new UserSwitcher(_authenticationService.GetUserName(), true))
            {
                throw new NotImplementedException();
            }
        }

        public void DeleteItem(string database, string path)
        {
            using (new UserSwitcher(_authenticationService.GetUserName(), true))
            {
                throw new NotImplementedException();
            }
        }

        private Database GetDatabase(string databaseName)
        {
            return Factory.GetDatabase(databaseName);
        }

        private Language GetLanguage(string languageName)
        {
            return Language.Parse(languageName);
        }
    }
}