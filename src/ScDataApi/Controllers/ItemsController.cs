using System.Net;
using System.Net.Http;
using System.Web.Http;
using ScDataApi.Configuration;
using ScDataApi.Security;
using ScDataApi.Storage;

namespace ScDataApi.Controllers
{
    public class ItemsController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly SitecoreDataService _data;

        public ItemsController()
        {
            _authenticationService = ServiceLocator.GetAuthenticationService();
            _data = new SitecoreDataService(_authenticationService);
        }

        public HttpResponseMessage Get(string database, string language, string query, string payload, string fields = "")
        {
            var items = _data.GetItems(database, language, query, payload, fields);

            return Request.CreateResponse(HttpStatusCode.OK, items);
        }
    }
}