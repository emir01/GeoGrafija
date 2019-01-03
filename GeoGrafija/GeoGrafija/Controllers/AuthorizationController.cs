using System.Web.Mvc;
using GeoGrafija.CustomFilters;
using GeoGrafija.Extensions;
using GeoGrafija.ModelResolver;
using Model;

using Services.Interfaces;

namespace GeoGrafija.Controllers
{
    [Authorize]
    [RolesActionFilter(RequiredRoles="Admin")]
    [RoleOrganizer]
    public class AuthorizationController : Controller
    {
        public IUserService UserService { get; set; }
        public IRolesService RolesService { get; set; }

        public AuthorizationController(IUserService userService, IRolesService rolesService)
        {
            UserService = userService;
            RolesService = rolesService;
        }

        //GET : INDEX
        public ActionResult Index()
        {
            return View();
        }

        //GET : ROLES
        public ActionResult Roles()
        {
            var allRoles = RolesService.GetAllRoles();
            return View(allRoles);
        }

        //GET : CreateRole
        public ActionResult CreateRole()
        {
            var role = new Role();
            return View(role);
        }

        // POST: /Authorization/CreateRole
        [HttpPost]
        public ActionResult CreateRole(Role role)
        {
            if (ModelState.IsValid)
            {
                var result =  RolesService.AddRole(role);

                if (result.IsOK)
                {
                    return RedirectToAction("RoleDetails", new { RoleName = role.RoleName });
                }
                else
                {
                    foreach (var message in result.Messages)
                    {
                        ModelState.AddModelError("", message);
                    }
                }
            }
            return View(role);
        }

        //GET: /Authorization/RoleDetails?RoleNAme=name
        public ActionResult RoleDetails(string RoleName)
        {
            var role = RolesService.GetRole(RoleName);
            return View(role);
        }

        //GET : /Authorization/Privileges
        public ActionResult Privileges()
        {
            var allPrivileges = RolesService.GetAllPrivileges();
            return View(allPrivileges);
        }

        //Get: /Authorization/CreatePrivilege
        public ActionResult CreatePrivilege()
        {
            var privilege = new Privilege();
            return View(privilege);
        }

        //POST: /Authorization/CreatePrivilege
        [HttpPost]
        public ActionResult CreatePrivilege(Privilege privilege)
        {
            if(ModelState.IsValid)
            {
                var result = RolesService.AddPrivilege(privilege);

                if (result.IsOK)
                {
                    return RedirectToAction("PrivilegeDetails", new { PrivilegeName = privilege.PrivilegeName });
                }
                else
                {
                    //Add The messages to be displayed
                    foreach (var message in result.Messages)
                    {
                        ModelState.AddModelError("", message);
                    }
                }
            }
            return View(privilege);
        }

        //GET : /Authorization/PrivilegeDetails?PrivilegeName=name
        public ActionResult PrivilegeDetails(string PrivilegeName)
        {
            var privilege = RolesService.GetPrivilege(PrivilegeName);
            return View(privilege);
        }

        //GET: /Authorization/AddPrivilegeToRole?RoleName=name
        public ActionResult AddPrivilegeToRole(string RoleName)
        {
            var role = RolesService.GetRole(RoleName);
            var privileges = RolesService.GetAllPrivileges();

            var viewModel = SimpleFactories.BuildAddPrivilegesToRoleViewModel(role, privileges);
            return View(viewModel);
        }

        //POST: /Authorization/AddPrivilegeToRole?RoleName=name
        [HttpPost]
        public ActionResult AddPrivilegeToRole(string RoleName,FormCollection collection)
        {
            var selectedPrivileges = StringHelpers.SplitStringToList(collection["privileges"], ',');
            var result = RolesService.SetNewPrivilegesToRole(RoleName, selectedPrivileges);

            if (result.IsOK)
            {
                return RedirectToAction("RoleDetails", new { RoleName=RoleName });
            }
            else
            {
                var role = RolesService.GetRole(RoleName);
                var privileges = RolesService.GetAllPrivileges();

                var viewModel = SimpleFactories.BuildAddPrivilegesToRoleViewModel(role, privileges);

                foreach (var message in result.Messages)
                {
                    ModelState.AddModelError("", message);
                }

                return View(viewModel);
            }
        }

        //Get : /Authorization/Users
        public ActionResult Users()
        {
            var usersToDipslay = UserService.GetUsers();

            return View(usersToDipslay);
        }

        //Get : /Authorization/UserDetails/id
        public ActionResult UserDetails(int id)
        {
            var user = UserService.GetUser(id);
            return View(user);
        }

        //GET : /Authorization/AddRolesToUser/id{UserID}
        public ActionResult AddRolesToUser(int id)
        {
            var user = UserService.GetUser(id);
            var roles = RolesService.GetAllRoles();

            var ViewModel = SimpleFactories.BuildAddRolesToUserviewModel(user, roles);
            return View(ViewModel);
        }

        //POST : /Authorization/AddRolesToUser/id{UserID}
        [HttpPost]
        public ActionResult AddRolesToUser(int id,FormCollection collection)
        {
            var user = UserService.GetUser(id);
            var selectedRoles = StringHelpers.SplitStringToList(collection["roles"], ',');
            var result = RolesService.SetNewRolesToUser(user, selectedRoles);

            if (result.IsOK)
            {
                return RedirectToAction("UserDetails", new { id = id});
            }
            else
            {
                var allRoles = RolesService.GetAllRoles();
                var viewModel = SimpleFactories.BuildAddRolesToUserviewModel(user, allRoles);

                foreach (var message in result.Messages)
                {
                    ModelState.AddModelError("", message);
                }

                return View(viewModel);
            }

        }

        public ActionResult Edit(int id)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult EditRole(int id)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult Create()
        {
            throw new System.NotImplementedException();
        }
    }
}
