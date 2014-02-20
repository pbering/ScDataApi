using System.Net;
using System.Net.Http;
using System.Web.Http;
using ScDataApi.Configuration;
using ScDataApi.Security;
using ScDataApi.Storage;

namespace ScDataApi.Controllers
{
    public class ItemController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly SitecoreDataService _data;

        public ItemController()
        {
            _authenticationService = ServiceLocator.GetAuthenticationService();
            _data = new SitecoreDataService(_authenticationService);
        }

        public HttpResponseMessage Get(string database, string language, string query, string payload, string fields = "")
        {
            if (!_authenticationService.IsAuthenticated())
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            var items = _data.GetItems(database, language, query, payload, fields);

            if (items.Length == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, items);
        }
    }
}