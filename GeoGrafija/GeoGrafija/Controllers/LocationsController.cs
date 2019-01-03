using System;
using System.Linq;
using System.Web.Mvc;
using GeoGrafija.CustomFilters;
using GeoGrafija.ViewModels;
using GeoGrafija.ViewModels.DisplaySettingViewModels;
using GeoGrafija.ViewModels.LocationViewModels;
using Model;
using Services.Interfaces;

namespace GeoGrafija.Controllers
{
    [RoleOrganizer]
    public class LocationsController : Controller
    {
        public ILocationService  LocationService { get; set; }
        public IUserService UserService { get; set; }
        public IRolesService RoleService { get; set; }

        public LocationsController(ILocationService locationService,IUserService userService ,IRolesService roleService)
        {   
            LocationService = locationService;
            UserService= userService;
            RoleService = roleService;
        }

        //GET : Locations/index
        [Authorize]
        [RolesActionFilter(RequiredRoles = "Teacher|Admin")]
        public ActionResult Index()
        {
            var user = UserService.GetUser(User.Identity.Name);
            var locationsForUser = LocationService.GetLocationsOfUser(user);

            if (locationsForUser == null && user!=null)
            {
                ModelState.AddModelError("", "Вие моментално немате креирано ниту една локација");
            }

            return View(locationsForUser);
        }

        //GET /Locations/Create
        [Authorize]
        [RolesActionFilter(RequiredRoles = "Teacher|Admin")]
        public ActionResult Create()
        {
            var model = new LocationFormViewModel();

            PopulateAddLocationViewModel(model);
            return View(model);
        }
        
        //POST /Locations/Craete
        [HttpPost]
        [Authorize]
        [RolesActionFilter(RequiredRoles = "Teacher|Admin")]
        [ValidateInput(false)]
        public ActionResult Create(LocationFormViewModel model)
        {
            LocationDisplaySetting locationDisplaySetting =  new LocationDisplaySetting();
            if (ViewData.ModelState.IsValid)
            {
                model.UserId = UserService.GetUser(User.Identity.Name).UserID;
                var location = model.getLocationFromViewModel();
                
                if (model.CurrentDisplaySetting)
                {
                    if (string.IsNullOrWhiteSpace(model.DisplayName))
                    {
                        ModelState.AddModelError("", "Мора да внесете име на новиот поглед");
                        PopulateAddLocationViewModel(model);
                        return View(model);
                    }

                    locationDisplaySetting = model.getLocationDisplaySettingFromViewModel();
                    var resultDisplay= LocationService.AddLocationDisplaySetting(locationDisplaySetting);

                    if (!resultDisplay.IsOK)
                    {
                        PopulateAddLocationViewModel(model);

                        foreach (var message in resultDisplay.Messages.Union(resultDisplay.ExceptionMessages))
                        {
                            ModelState.AddModelError("", message);
                        }

                        return View(model);
                    }

                    location.DisplaySettings = locationDisplaySetting.ID;
                }

                var result  =  LocationService.AddLocation(location);

                if (result.IsOK)
                {
                    return RedirectToAction("Details", new {id=location.ID});
                }
                else
                {
                    PopulateAddLocationViewModel(model);

                    foreach (var message in result.Messages.Union(result.ExceptionMessages))
                    {
                        ModelState.AddModelError("", message);
                    }

                    return View(model);
                }
            }
            else
            {
                PopulateAddLocationViewModel(model);
                return View(model);
            }
        }

        //GET: /Locations/Edit/id
        [Authorize]
        [RolesActionFilter(RequiredRoles = "Teacher|Admin")]
        public ActionResult Edit(int id)
        {
            var editModel = new LocationFormViewModel();
            var location= LocationService.GetLocation(id);
            
            editModel.setLocationToViewModel(location);

            PopulateAddLocationViewModel(editModel);
            return View(editModel);
        }

        //POST /Locations/Edit/id
        [HttpPost]
        [Authorize]
        [RolesActionFilter(RequiredRoles = "Teacher|Admin")]
        [ValidateInput(false)]
        public ActionResult Edit(int id,LocationFormViewModel model)
        {
            LocationDisplaySetting locationDisplaySetting = new LocationDisplaySetting();
            if (ViewData.ModelState.IsValid)
            {
                var location = model.getLocationFromViewModel();

                if (model.CurrentDisplaySetting)
                {
                    //Check the name
                    //TODO : This needs to go somepalce else. The Check needs to be done some other way. not in the controller
                    //TODO : again use the damn ViewModel to validate. usint the supplied attributes for data validation
                    if (string.IsNullOrWhiteSpace(model.DisplayName))
                    {
                        ModelState.AddModelError("", "Мора да внесете име на новиот поглед");
                        PopulateAddLocationViewModel(model);
                        return View(model);
                    }

                    locationDisplaySetting = model.getLocationDisplaySettingFromViewModel();
                    var resultDisplay = LocationService.AddLocationDisplaySetting(locationDisplaySetting);

                    if (!resultDisplay.IsOK)
                    {
                        PopulateAddLocationViewModel(model);
                        foreach (var message in resultDisplay.Messages.Union(resultDisplay.ExceptionMessages))
                        {
                            ModelState.AddModelError("", message);
                        }
                        return View(model);
                    }
                    
                    location.DisplaySettings = locationDisplaySetting.ID;
                }

                var result = LocationService.UpdateLocation(location);

                if (result.IsOK)
                {
                    return RedirectToAction("Details", new { id = location.ID });
                }
                else
                {
                    PopulateAddLocationViewModel(model);

                    foreach (var message in result.Messages.Union(result.ExceptionMessages))
                    {
                        ModelState.AddModelError("", message);
                    }

                    return View(model);
                }
            }
            else
            {
                PopulateAddLocationViewModel(model);
                return View(model);
            }
        }

        //GET : /Locations/Details/id
        public ActionResult Details(int id)
        {
            var location = LocationService.GetLocation(id);
            var locationViewModel =  DisplayLocationViewModel.BindToLocation(location,null);
            if (locationViewModel != null)
            {
                return View(locationViewModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        private void PopulateAddLocationViewModel(LocationFormViewModel model)
        {
            model.DisplaySettings = LocationService.GetAllLocationDisplaySettings();
            model.LocationTypes = LocationService.GetAllLocationTypes();
        }

        //AJAX : /Locations/GetDisplaySettings
        [HttpPost]
        public ActionResult LocationDisplaySettingsAjax(int id)
        {
            var result = LocationService.GetLocationDisplaySetting(id);
            var resultJson= new JsonDisplaySetting();
            if (result != null)
            {   
                resultJson.Result= true;
                resultJson.Zoom = result.Zoom;
                resultJson.MapType = result.MapType;
                resultJson.RenderControlls = result.RenderControls;
            }
            return Json(resultJson);
        }

        public ActionResult Delete(int id)
        {
            var location = LocationService.GetLocation(id);

            var viewModel = DisplayLocationViewModel.BindToLocation(location, null);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Delete(DisplayLocationViewModel viewModel, int id)
        {
            var location = LocationService.GetLocation(id);

            if (location == null)
            {
                ModelState.AddModelError("","Локацијата не постои или е веќе избришана");
                return View(viewModel);
            }
            else
            {
                var result = LocationService.RemoveLocation(location);

                if (result.IsOK) 
                { 
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Грешка  при бришење на локација. Обиди се повторно");
                    return View(viewModel);
                }
            }
        }
    }
}
