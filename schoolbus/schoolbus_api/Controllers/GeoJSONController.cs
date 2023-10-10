using Microsoft.AspNetCore.Mvc;
using schoolbus_api.Models;

namespace schoolbus_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeoJSONController : ControllerBase
    {
        [HttpGet]
        [Route("bus_location")]
        public _c_geo_json bus_location()
        {
            return new _c_geo_json();
        }
    }
}