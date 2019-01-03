using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeoGrafija.CustomFilters;
using GeoGrafija.ViewModels.Learning;
using Services.Interfaces;

namespace GeoGrafija.Controllers
{
    [Authorize]
    [RoleOrganizer]
    public class LearningController : Controller
    {
        private ILocationService _locationService;
        private IResourceService _resourceService;

        public LearningController(ILocationService locationService, IResourceService resourceService)
        {
            _locationService = locationService;
            _resourceService = resourceService;
        }

        //
        // GET: /Learning/

        public ActionResult Index()
        {
            var allLocationTypes = _locationService.GetAllLocationTypes();
            var allResourceTypes = _resourceService.GetAllResourceTypes().GetData();

            var viewModel = new LearningGeneralViewModel(allResourceTypes, allLocationTypes);

            return View(viewModel);
        }

    }
}
