using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShipmentManagement.Filters
{
    public class AuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sessionFeature = context.HttpContext.Features.Get<Microsoft.AspNetCore.Http.Features.ISessionFeature>();
            if (sessionFeature?.Session == null || !sessionFeature.Session.IsAvailable)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
            else
            {
                var email = context.HttpContext.Session.GetString("UserEmail");
                var controller = context.RouteData.Values["controller"]?.ToString();
                var action = context.RouteData.Values["action"]?.ToString();

                // Skip auth check for Login page
                if (controller == "Account" && (action == "Login" || action == "LogoutGet"))
                {
                    base.OnActionExecuting(context);
                    return;
                }

                // Not logged in → redirect to Login
                if (string.IsNullOrEmpty(email))
                {
                    context.Result = new RedirectToActionResult("Login", "Account", null);
                }
            }

            base.OnActionExecuting(context);
        }
    }
}