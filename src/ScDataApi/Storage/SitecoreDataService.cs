using System;
using System.Collections.Generic;
using System.Linq;
using ScDataApi.Security;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Query;
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

        public DataItem[] GetItems(string databaseName, string languageName, string query, string payload, string fields)
        {
            var database = GetDatabase(databaseName);
            var batches = query.Split(',').Select(q => q.Trim());

            // TODO: Validate query?...
            // TODO: Could we just return hashtables?

            var dataItems = new List<DataItem>();

            Func<Item, DataItem> itemTypeConstrutor;

            if ((payload.Equals("full", StringComparison.OrdinalIgnoreCase)))
            {
                itemTypeConstrutor = item => new FullDataItem(item, fields);
            }
            else if (payload.Equals("min", StringComparison.OrdinalIgnoreCase))
            {
                itemTypeConstrutor = item => new MinimalDataItem(item, fields);
            }
            else if (payload.Equals("custom", StringComparison.OrdinalIgnoreCase))
            {
                itemTypeConstrutor = item => new DataItem(item, fields);
            }
            else
            {
                throw new ArgumentException("Please use either 'min', 'full' or 'custom'", "payload");
            }

            using (new LanguageSwitcher(languageName))
            {
                foreach (var batchQuery in batches)
                {
                    var items = new List<Item>();

                    if (batchQuery.Contains("{") && batchQuery.Contains("}"))
                    {
                        ID id;

                        if (ID.TryParse(batchQuery, out id))
                        {
                            var item = database.Items.GetItem(id);

                            if (item != null)
                            {
                                items.Add(item);
                            }
                        }
                    }
                    else
                    {
                        var results = Query.SelectItems(batchQuery, database);

                        if (results == null)
                        {
                            continue;
                        }

                        items.AddRange(results);
                    }

                    foreach (var item in items)
                    {
                        dataItems.Add(itemTypeConstrutor.Invoke(item));
                    }
                }
            }

            return dataItems.ToArray();
        }

        private DataItem GetItem(Database database, Language language, string path, string payload, string fields)
        {
            var item = GetItem(database, language, path);

            if (item == null)
            {
                return null;
            }

            return new DataItem(item, fields);
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