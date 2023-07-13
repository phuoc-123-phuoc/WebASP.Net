using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopmeProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
             name: "OrderReport3",
             url: "reports/sales_by_date/{period}",
             defaults: new { controller = "ReportRest", action = "GetReportDataByDatePeriod" }

         );

            routes.MapRoute(
              name: "OrderReport2",
              url: "reports/sales_by_date/{startDate}/{endDate}",
              defaults: new { controller = "ReportRest", action = "GgetReportDataByDatePeriod" }

          );

            routes.MapRoute(
                name: "OrderReport1",
                url: "reports/{groupBy}/{period}",
                defaults: new { controller = "ReportRest", action = "getReportDataByCategoryOrProduct" }

            );

            routes.MapRoute(
                name: "OrderReport",
                url: "reports/{groupBy}/{startDate}/{endDate}",
                defaults: new { controller = "ReportRest", action = "getReportDataByCategoryOrProductDateRange" }
               
            );

            routes.MapRoute(
              name: "Admin",
              url: "admin",
              defaults: new { controller = "Home", action = "Index" }

          );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "HomeSite", action = "Index", id = UrlParameter.Optional }
               
            );

          
        }
    }
}
