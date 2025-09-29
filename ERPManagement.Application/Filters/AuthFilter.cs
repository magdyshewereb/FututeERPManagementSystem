using ERPManagement.Application.Contracts.Infrastructure;
using ERPManagement.Application.Routing;
using ERPManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;


namespace ERPManagement.Application.Filters
{
    public class AuthFilter : IAsyncActionFilter
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        public AuthFilter(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            if (context.HttpContext.User.Identity?.IsAuthenticated == true)
            {
                //var action1 = context.HttpContext.Request.RouteValues["action"]?.ToString();
                //var controller = context.HttpContext.Request.RouteValues["controller"]?.ToString();
                var action = context.RouteData.Values["action"]?.ToString();
                var controller = context.RouteData.Values["controller"]?.ToString();
                var formRoles = await _currentUserService.GetCurrentUserFormRolesAsync(controller);

                var saveNew = action?.ToLower() == ApiRoutes.Main.Create.ToLower();
                var updateData = action?.ToLower() == ApiRoutes.Main.Edit.ToLower();
                var deleteData = action?.ToLower() == ApiRoutes.Main.Delete.ToLower();
                var user = await _currentUserService.GetUserAsync();

                var hasPermission = formRoles;
                if (hasPermission != null)
                {

                    if (action?.ToLower() == ApiRoutes.Main.Create.ToLower())
                    {
                        if (hasPermission.AddData == null || !hasPermission.AddData.Value)
                        {
                            //context.Result = new ObjectResult("No Permmision")
                            //{
                            //    StatusCode = StatusCodes.Status403Forbidden
                            //};
                        }
                        else
                        {
                            await next();
                        }
                    }

                    else if (action?.ToLower() == ApiRoutes.Main.Edit.ToLower())
                    {
                        if (hasPermission.UpdateData == null && !hasPermission.UpdateData.Value)
                        {
                            //context.Result = new ObjectResult("No Permmision")
                            //{
                            //    StatusCode = StatusCodes.Status403Forbidden
                            //};
                        }
                        else
                        {
                            await next();
                        }
                    }
                    else if (action?.ToLower() == "EditData".ToLower())
                    {
                        if (hasPermission.UpdateData == null && !hasPermission.UpdateData.Value)
                        {
                            //context.Result = new ObjectResult("No Permmision")
                            //{
                            //    StatusCode = StatusCodes.Status403Forbidden
                            //};
                        }
                        else
                        {
                            await next();
                        }
                    }
                    else if (action?.ToLower() == ApiRoutes.Main.Delete.ToLower())
                    {
                        if (hasPermission.DeleteData == null && !hasPermission.DeleteData.Value)
                        {
                            //context.Result = new ObjectResult("No Permmision")
                            //{
                            //    StatusCode = StatusCodes.Status403Forbidden
                            //};
                        }
                        else
                        {
                            await next();
                        }
                    }
                }
                else
                {
                    //context.Result = new ObjectResult("No Permmision")
                    //{
                    //    StatusCode = StatusCodes.Status403Forbidden
                    //};
                }
            }
        }
    }
}

