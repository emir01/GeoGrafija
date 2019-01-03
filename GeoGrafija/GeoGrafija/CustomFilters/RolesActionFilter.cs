using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Services.Interfaces;


namespace GeoGrafija.CustomFilters
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method, Inherited=true, AllowMultiple=true)]
    public class RolesActionFilter : ActionFilterAttribute
    {
        private IRolesService RolesService { get; set; }
        public string RequiredRoles { get; set; }

        public RolesActionFilter()
            : this(new SmDependencyResolver(IoC.Initialize()).GetService<IRolesService>())
        {

        }

        public RolesActionFilter(IRolesService roleService)
        {
            this.RolesService = roleService;
        }
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool UserHasRequiredRole = false;
            var userName = filterContext.HttpContext.User.Identity.Name;

            var userRoles = RolesService.GetUserRoles(userName);

            if (string.IsNullOrWhiteSpace(RequiredRoles))
            {
                UserHasRequiredRole =  true;
            }
            else
            {
                var roles = RequiredRoles.Split('|');
                foreach (var role in roles)
                {
                    if(userRoles.Contains(role.ToLower())){
                        UserHasRequiredRole=true;
                        break;
                    }
                }
            }
            
            if (!UserHasRequiredRole)
            {
                var routeValues = new RouteValueDictionary();
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary ( new { controller ="User",   action= "Home", MissingRole=RequiredRoles}));
            }
        }
    }
}