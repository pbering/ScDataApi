using System;
using System.Web;
using System.Web.Routing;
using Sitecore.Pipelines.HttpRequest;

namespace ScDataApi.Pipelines
{
    public class AbortSitecoreOnApiRequest : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(args.Context));

            if (routeData == null)
            {
                return;
            }

            var route = routeData.Route as Route;

            if (route == null)
            {
                return;
            }

            if (route.Url.StartsWith("/api/data/", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            args.Context.RemapHandler(routeData.RouteHandler.GetHttpHandler(args.Context.Request.RequestContext));
            args.AbortPipeline();
        }
    }
}