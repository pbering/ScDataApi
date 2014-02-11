using System.Net.Http.Headers;
using System.Web.Http;
using Sitecore.Pipelines;

namespace ScDataApi.WebApp
{
    public class ConfigureWebApiForDevelopment
    {
        public void Process(PipelineArgs args)
        {
            var config = GlobalConfiguration.Configuration;

            config.EnableSystemDiagnosticsTracing();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}