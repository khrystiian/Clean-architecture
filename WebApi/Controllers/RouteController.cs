using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using TrainApp.Core.ApplicationService.Interfaces;
using TrainApp.Core.Entity;

namespace UI.Controllers
{
    //public class RouteController : ApiController
    //{
    //    private readonly IRouteService _routeService;

    //    public RouteController(IRouteService routeService)
    //    {
    //        this._routeService = routeService ?? throw new ArgumentNullException(nameof(_routeService));
    //    }

        // GET: api/Route
        //[HttpGet]
        //public async Task<IList<RouteModel>> Get()
        //{
        //    return await _routeService.ReadAllRoutes();
        //}

        //[HttpGet]
        //// GET: api/Route/5
        //public RouteModel Get(int id)
        //{
        //    if (id == default(int))
        //    {
        //        throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
        //    }
        //    return _routeService.FindRouteById(id); ;
        //}

        //[HttpPost]
        //// POST: api/Route
        //public void Post([FromBody]RouteModel route)
        //{
        //    _routeService.CreateRoute(route);

        //    if (route == null)
        //    {
        //        throw new HttpResponseException(System.Net.HttpStatusCode.NoContent);
        //    }
        //}

        //[HttpPut]
        //// PUT: api/Route/5
        //public void Put(int id, [FromBody]RouteModel route)
        //{
        //    route.ID = id;
        //    if (!_routeService.EditRoute(route))
        //    {
        //        throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
        //    }
        //}

        //[HttpDelete]
        //// DELETE: api/Route/5
        //public void Delete(int id, [FromBody]RouteModel route)  //?????????
        //{
        //    route.ID = id;
        //    if (route == null)
        //    {
        //        throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
        //    }
        //    _routeService.RemoveRoute(route);
        //}
    //}
}
