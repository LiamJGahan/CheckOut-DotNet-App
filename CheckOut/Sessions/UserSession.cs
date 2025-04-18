using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CheckOut.Sessions
{
    public class UserSession : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            if (session.GetInt32("UserId") == null)
            {
                context.Result = new RedirectToActionResult("Login", "Users", null);
            }
        }
    }
}