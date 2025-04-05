using BLL.Services.Interface;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BLL.ActionFilter
{
    public class LogRequestActionFilter : IActionFilter
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        public LogRequestActionFilter(ITokenService tokenService, IUserService userService = null)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var email = _tokenService.GetEmail();
            if (email != null)
            {
                _userService.LogUserActivity(email);
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // No action needed
        }
    }
}
