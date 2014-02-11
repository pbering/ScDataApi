using System.Net;
using System.Net.Http;
using System.Web.Http;
using ScDataApi.Configuration;
using ScDataApi.Storage;

namespace ScDataApi.Controllers
{
    public class ItemController : ApiController
    {
        private readonly DataService _data;

        public ItemController()
        {
            _data = new DataService(ServiceLocator.GetAuthenticationService());
        }

        public HttpResponseMessage Get(string database, string language, string path, string payload, string fields = "")
        {
            if (!_data.ItemExists(database, path))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var item = _data.GetItem(database, language, path, payload, fields);

            return Request.CreateResponse(HttpStatusCode.OK, item);
        }

        public HttpResponseMessage Post(string database, string language, string path, [FromBody] DataItem value)
        {
            if (!_data.ItemExists(database, path))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            _data.CreateItem(database, language, path, value);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage Put(string database, string language, string path, [FromBody] DataItem value)
        {
            if (!_data.ItemExists(database, path))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            _data.UpdateItem(database, language, path, value);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(string database, string path)
        {
            if (!_data.ItemExists(database, path))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            _data.DeleteItem(database, path);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}