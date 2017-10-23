using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace gs
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Index",
                url: "oculos/{slug}/{id}",
                defaults: new
                {
                    controller = "oculos",
                    action = "Index",
                    slug = UrlParameter.Optional,
                    id = UrlParameter.Optional
                }
            );
            /*
            routes.MapRoute(
                name: "Filtro",
                url: "/{filtro}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    filtro = UrlParameter.Optional,
                }
            );
            */
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
