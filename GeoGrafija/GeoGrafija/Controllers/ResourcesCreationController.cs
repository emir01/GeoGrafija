using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeoGrafija.CustomFilters;
using GeoGrafija.ViewModels.ResourcesViewModels;
using GeoGrafija.ViewModels.SharedViewModels;
using Model;
using Services;

namespace GeoGrafija.Controllers
{
    [RoleOrganizer]
    public class ResourcesCreationController : Controller
    {

        #region Services

        private readonly LocationService _locationService;
        private readonly ResourceService _resourcesService;
        private readonly UserService _userService;

        #endregion

        public ResourcesCreationController(LocationService locationService, ResourceService resourcesService,
                                           UserService userService)
        {
            _locationService = locationService;
            _resourcesService = resourcesService;
            _userService = userService;
        }

        //
        // GET: /Resources/Index/LocaitonId
        public ActionResult Index(int locationId)
        {
            var locaiton = _locationService.GetLocation(locationId);
            var resourceTypes = _resourcesService.GetAllResourceTypes();

            var viewModel = new CreateResourcesViewModel(locaiton, resourceTypes.GetData());

            return View(viewModel);
        }

        // View that allows you to create resources connected to locaiton types
        public ActionResult  GeneralResources()
        {
            var allLocationTypes = _locationService.GetAllLocationTypes();
            var allResourceTypes = _resourcesService.GetAllResourceTypes();

            var viewModel = new CreateResourcesViewModel(null, allResourceTypes.GetData(), allLocationTypes);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CreateResources(CreateResourcesViewModel model)
        {
            var resource = model.GetResourceFromModel();

            var result = _resourcesService.CreateResource(resource);
            var jsonMessage = new SimpleJsonMessageViewModel();
            if (result.IsOK)
            {
                jsonMessage.IsOk = true;
                jsonMessage.Message = "Успешно креиран ресурс!";
                jsonMessage.Data = new ResourceViewModel(result.GetData());
                return Json(jsonMessage);
            }
            else
            {
                jsonMessage.IsOk = false;
                jsonMessage.Message = "Неуспешно креран ресурс. Обиди се повторно!";
                return Json(jsonMessage);
            }
        }

        public ActionResult GetResourceDetails(int resourceId)
        {
            var result = _resourcesService.GetResource(resourceId);

            if (result.IsOK)
            {
                var viewModel = new ResourceViewModel(result.GetData());
                viewModel.IsOk = true;
                return Json(viewModel);
            }

            else
            {
                var viewModel = new ResourceViewModel(result.GetData());
                viewModel.IsOk = false;
                return Json(viewModel);
            }
        }

        public ActionResult UpdateResource(ResourceViewModel viewModel)
        {
            var simpleResource = viewModel.GetSimpleResource();
            var result = _resourcesService.UpdateResource(simpleResource);

            if (result.IsOK)
            {
                var successViewModel  = new ResourceViewModel(result.GetData());
                successViewModel.IsOk = true;
                return Json(successViewModel);
            }
            else
            {
                var failViewModel = new ResourceViewModel(result.GetData());
                failViewModel.IsOk = false;
                failViewModel.Message = result.Messages[0];
                return Json(failViewModel);
            }
        }

        public ActionResult DeleteResource(int resourceId)
    {
        var result = _resourcesService.DeleteResource(resourceId);

        if (result.IsOK)
        {
            var jsonREsult = new SimpleJsonMessageViewModel();
            jsonREsult.IsOk = true;
            jsonREsult.Message = "Успешно избришан ресурс!";
            jsonREsult.Data = resourceId;
            return Json(jsonREsult);
        }
        else
        {
            var jsonREsult = new SimpleJsonMessageViewModel();
            jsonREsult.IsOk = false;
            jsonREsult.Message = "Грешка при бришење. Обиди се повторно!";
            jsonREsult.Data = resourceId;
            return Json(jsonREsult);
        }
    }
    }
}