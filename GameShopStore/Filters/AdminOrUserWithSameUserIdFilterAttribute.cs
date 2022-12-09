using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace GameShopStore.Filters
{
    public class AdminOrUserWithSameUserIdFilterAttribute : Attribute, IActionFilter
    {
        private readonly string _userIdArgName;

        public AdminOrUserWithSameUserIdFilterAttribute(string userIdArgName = "Id")
        {
            _userIdArgName = userIdArgName;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            int requestId = (int)context.ActionArguments[_userIdArgName];
            int currentUserId;
            int.TryParse((context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value), out currentUserId);
            if (currentUserId != requestId && context.HttpContext.User.FindAll(ClaimTypes.Role).Where(x => x.Value == "Admin").FirstOrDefault() == null)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
