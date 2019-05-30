using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ImagineBrownBag
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Permissions.GetRolesCsvForCurrentUser = () =>
            {
                string rolesCsv = "";
                // Don't actually use this custom cookie auth method in a production system. This is not secure.
                if (HttpContext.Current.Request.QueryString.AllKeys.Any(x => x == "Roles"))
                {
                    string[] roles = HttpContext.Current.Request.QueryString.GetValues("Roles");
                    rolesCsv = string.Join(",", string.Join(",", roles).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                    HttpContext.Current.Response.SetCookie(new HttpCookie("Roles", rolesCsv)
                    {
                        HttpOnly = true,
                        Expires = DateTime.MaxValue,
                    });
                }

                if (HttpContext.Current.Request.Cookies.AllKeys.Any(x => x == "Roles"))
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("Roles");
                    rolesCsv = cookie.Value;
                }

                return rolesCsv;
            };
        }
    }
}
