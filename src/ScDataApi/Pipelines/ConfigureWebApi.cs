using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using Sitecore.Pipelines;

namespace ScDataApi.Pipelines
{
    public class ConfigureWebApi
    {
        public void Process(PipelineArgs args)
        {
            var config = GlobalConfiguration.Configuration;

            config.Routes.MapHttpRoute("DefaultApi", "api/data/v1/item", new
            {
                controller = "item",
                payload = "min",
                language = "en",
                database = "master",
                fields = RouteParameter.Optional
            });

            config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

            var filters = GlobalFilters.Filters;
            var defaultErrorHandler = new HandleErrorAttribute();

            if (!filters.Contains(defaultErrorHandler))
            {
                filters.Add(new HandleErrorAttribute());
            }
        }
    }
}