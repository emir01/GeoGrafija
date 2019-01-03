using System;
using System.Linq;
using System.Web.Mvc;
using Services.Interfaces;

namespace GeoGrafija.CustomFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class RoleOrganizer:ActionFilterAttribute
    {
        private IRolesService RolesService { get; set; }

        public RoleOrganizer()
            : this(new SmDependencyResolver(IoC.Initialize()).GetService<IRolesService>())
        {

        }

        public RoleOrganizer(IRolesService roleService)
        {
            this.RolesService = roleService;
        }
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RolesService = new SmDependencyResolver(IoC.Initialize()).GetService<IRolesService>();
            
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var userName = filterContext.HttpContext.User.Identity.Name;
                
                //must have a name
                if (String.IsNullOrWhiteSpace(userName))
                    return;

                var userRoles = RolesService.GetUserRoles(userName);
                var lowerCaseRoles = userRoles.Select(x => x.ToLower()).ToList();

                if (lowerCaseRoles.Contains("admin"))
                {
                    filterContext.Controller.ViewBag.NavigationType = "_AdminNavigation";
                    return;
                }

                if (lowerCaseRoles.Contains("teacher"))
                {
                    filterContext.Controller.ViewBag.NavigationType = "_TeacherNavigation";
                    return;
                }

                if (lowerCaseRoles.Contains("student"))
                {
                    filterContext.Controller.ViewBag.NavigationType = "_StudentNavigation";
                    return;
                }

                filterContext.Controller.ViewBag.NavigationType = "_NoRolesUserNavigation";

            }
            else
            {
                filterContext.Controller.ViewBag.UserNavigation = null;
            }
        }
    }
}