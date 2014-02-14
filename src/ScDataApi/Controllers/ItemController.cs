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

        public HttpResponseMessage Get(string database, string language, string path, string payload, string fields = "")
        {
            if (!_authenticationService.IsAuthenticated())
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            if (!_data.ItemExists(database, path))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var item = _data.GetItem(database, language, path, payload, fields);

            return Request.CreateResponse(HttpStatusCode.OK, item);
        }

        public HttpResponseMessage Post(string database, string language, string path, [FromBody] DataItem value)
        {
            if (!_authenticationService.IsAuthenticated())
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            if (!_data.ItemExists(database, path))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            _data.CreateItem(database, language, path, value);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage Put(string database, string language, string path, [FromBody] DataItem value)
        {
            if (!_authenticationService.IsAuthenticated())
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            if (!_data.ItemExists(database, path))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            _data.UpdateItem(database, language, path, value);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(string database, string path)
        {
            if (!_authenticationService.IsAuthenticated())
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            if (!_data.ItemExists(database, path))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            _data.DeleteItem(database, path);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}