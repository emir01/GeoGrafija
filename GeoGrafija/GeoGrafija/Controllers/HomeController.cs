using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeoGrafija.CustomFilters;
using Services.Interfaces;

namespace GeoGrafija.Controllers
{
    [RoleOrganizer]
    public class HomeController : Controller
    {
        
        public HomeController(IRolesService roleService, IUserService userService)
        {
            
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Добредојдовте на ГеоГрафија!";
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult ErrorTesting()
        {
            throw new NotImplementedException();
        }


        public ActionResult Error()
        {
            return View("Error");
        }

        public ActionResult NotFound()
        {
            return View("NotFound");
        }
    }
}
