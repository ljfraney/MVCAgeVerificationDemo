using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCAgeVerificationDemo
{
    public class VerifyAgeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // If the age has been verified, a session variable should hold the result.
            var ageVerified = httpContext.Session["AgeVerified"];
            
            // If there is no session variable set, or the user did not meet the age requirement, do not authorize.
            if (ageVerified == null || (bool)ageVerified == false)
                return false;

            // If we reach here, there was a session variable, and the value was true, so the user meets the age requirement.
            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // If the user has not verified their age, or they did not meet the age requirement, send them to VerifyAge/Index.
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "action", "Index" },
                    { "controller", "VerifyAge" },
                    { "redirectUrl", filterContext.HttpContext.Request.Url.AbsoluteUri }
                });
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (AuthorizeCore(filterContext.HttpContext))
                base.OnAuthorization(filterContext);
            else
                HandleUnauthorizedRequest(filterContext);
        }
    }
}