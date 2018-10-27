using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HR_Staffing
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "FormQuestions",
                url: "{formTypeString}/Questions/{action}/{id}",
                defaults: new { controller = "FormQuestions", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ApplicantsQuestions",
                url: "Applicants/{applicantId}/Questions/{action}/{id}",
                defaults: new { controller = "ApplicantsQuestions", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
