using API_XCM.Code;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace API_XCM
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new AuthFilter());
        }


        public class AuthFilter : ActionFilterAttribute, IAuthenticationFilter
        {
            public void OnAuthentication(AuthenticationContext filterContext)
            {
                if (string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session["isAuthenticated"]))
                    || Convert.ToString(filterContext.HttpContext.Session["isAuthenticated"]) != "xwHD14") 
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }

            public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
            {
                if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
                {
                    //Redirecting the user to the Login View of Account Controller  
                    filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                         { "controller", "Account" },
                         { "action", "Login" }
                    });
                }
            }
        }

        public class AuthFilterNino : ActionFilterAttribute, IAuthenticationFilter
        {
            public void OnAuthentication(AuthenticationContext filterContext)
            {
                if (string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session["nino"])))
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }

            public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
            {
                if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
                {
                    //Redirecting the user to the Login View of Account Controller  
                    filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                         { "controller", "Home" },
                         { "action", "Index" }
                    });
                }
            }
        }

        public class ViewAuthFilter : ActionFilterAttribute, IAuthenticationFilter
        {
            public void OnAuthentication(AuthenticationContext filterContext)
            {
                if (!AuthHelper.IsAuthenticated())
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }

            public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
            {
                if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
                {
                    //Redirecting the user to the Login View of Account Controller  
                    filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                         { "controller", "Home" },
                         { "action", "Index" }
                    });
                }
            }
        }
    }
}
