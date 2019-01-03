using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using GeoGrafija.CustomFilters;
using System.IO;
using GeoGrafija.ViewModels;

using GeoGrafija.ViewModels.LocationTypeViewModels;
using Services.Interfaces;

namespace GeoGrafija.Controllers
{

    [Authorize]
    [RoleOrganizer]
    [RolesActionFilter(RequiredRoles="Teacher|Admin")]
    public class LocationTypesController : Controller
    {
        public IUserService UserService { get; set; }
        public ILocationService LocationService { get; set; }
        
        public LocationTypesController(IUserService userService, ILocationService locationService)
        {
            UserService = userService;
            LocationService = locationService;
        }

        // GET: /LocationTypes/
        public ActionResult Index()
        {
            var user = UserService.GetUser(User.Identity.Name);
            var result  =  LocationService.GetLocationTypesForUser(user);

            return View(result);
        }

        //GET /LocationTypes/Create
        public ActionResult Create ()
        {
            var model = GetAddLocationTypeViewModel();
            return View(model);
        }

        //POST /LocationTypes/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(AddLocationTypeViewModel model, string pickedMarker="")
        {
            bool isOk = false;
            string picName = "";
            if (!String.IsNullOrWhiteSpace(pickedMarker) && FileHelpers.FolderHasFile(Server.MapPath(@"\Content\MarkerIcons\"), pickedMarker))
            {
                isOk = true;
                picName = pickedMarker;
            }
            else
            {
                ImageUpload(ref isOk, ref picName);
            }

            //File  === 
            if (!isOk)
            {
                model.DisplaySettings = LocationService.GetAllLocationDisplaySettings();
                ModelState.AddModelError("", "Грешка при креирање на икона за маркер");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                //add the new location type
                var whatUser = UserService.GetUser(User.Identity.Name);
                model.IconString = picName;
                var locationType = model.getLocationTypeFromViewModel();
                locationType.CreatedBy = whatUser.UserID;
                
                var result = LocationService.AddLocationType(locationType);

                if (result.IsOK)
                {
                    return RedirectToAction("Details", new { id  = locationType.ID });
                }
                else 
                {
                    foreach (var message in result.Messages.Union(result.ExceptionMessages))
                    {
                        ModelState.AddModelError("",message);

                    }
                    model.DisplaySettings = LocationService.GetAllLocationDisplaySettings();
                    return View(model);
                 }
            }
            else
            {
                model.DisplaySettings = LocationService.GetAllLocationDisplaySettings();
                return View(model);
            }
        }

        //GET /LocationTypes/Edit/id
        public ActionResult Edit(int id)
        {
            var locationType = LocationService.GetLocationType(id);
            var model = GetAddLocationTypeViewModel(locationType.Icon);

            model.AddLocationTypeToViewModel(locationType);

            return View(model);
        }

        //POST /LocationType/EDIT/id
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id,AddLocationTypeViewModel model,string pickedMarker="")
        {
            //File
            
            bool isOk = false;
            string picName = "";
            if (!String.IsNullOrWhiteSpace(pickedMarker) && FileHelpers.FolderHasFile(Server.MapPath(@"\Content\MarkerIcons\"), pickedMarker))
            {
                isOk = true;
                picName = pickedMarker;
            }
            else
            {
                ImageUpload(ref isOk, ref picName);
            }

            //File  === 

            if (!isOk)
            {
                model.DisplaySettings = LocationService.GetAllLocationDisplaySettings();
                ModelState.AddModelError("", "Грешка при креирање на икона за маркер");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                //add the new location type
                 
                if(!picName.Equals("default.png",StringComparison.InvariantCultureIgnoreCase)){
                    model.IconString = picName;  
                }
                
                var locationType = model.getLocationTypeFromViewModel();
                var result = LocationService.UpdateLocationType(locationType);

                if (result.IsOK)
                {
                    return RedirectToAction("Details", new { id = locationType.ID });
                }
                else
                {
                    foreach (var message in result.Messages.Union(result.ExceptionMessages))
                    {
                        ModelState.AddModelError("", message);
                    }
                    
                    model.DisplaySettings = LocationService.GetAllLocationDisplaySettings();
                    return View(model);
                }

            }
            else
            {
                model.DisplaySettings = LocationService.GetAllLocationDisplaySettings();
                return View(model);
            }
        }
        
        private void ImageUpload(ref bool isOk, ref string picName)
        {
            try
            {
                if (Request != null && Request.Files != null && Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files["fileUpload"];

                    if (file.ContentLength > 0)
                    {
                        string path = Path.Combine(HttpContext.Server.MapPath("/Content/MarkerIcons"), Path.GetFileName(file.FileName));
                       
                        file.SaveAs(path);
                        picName = file.FileName;
                        isOk = true;
                    }
                    else
                    {
                        picName = "default.png";
                        isOk = true;
                    }
                }
                else
                {
                    picName = "default.png";
                    isOk = true;
                }
            }
            catch (Exception e) { }
        }

        //GET /LocationTypes/Details/id
        public ActionResult Details( int  id)
        {
            var model = new DisplayLocationTypeViewModel();
            var locationType = LocationService.GetLocationType(id);

            if (locationType == null)
            {
                return RedirectToAction("Index");
            }

            var createdBy = UserService.GetUser(locationType.CreatedBy);
            model.PopulateFields(locationType,createdBy);

            return View(model);
        }

        private AddLocationTypeViewModel GetAddLocationTypeViewModel(string currentIcon ="")
        {
            var model = new AddLocationTypeViewModel();
            model.DisplaySettings = LocationService.GetAllLocationDisplaySettings();

            var directory = Server.MapPath(@"\Content\MarkerIcons\");
            var files = Directory.GetFiles(directory).Select(Path.GetFileName).Where(x => !x.Equals(currentIcon)).ToList();
            model.MarkerFileNames = files;
            
            return model;
        }

        public ActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
