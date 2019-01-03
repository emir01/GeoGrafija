using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GeoGrafija.CustomFilters;
using GeoGrafija.ViewModels.Exploration;
using GeoGrafija.ViewModels.LocationTypeViewModels;

using Services.Interfaces;

namespace GeoGrafija.Controllers
{
    [RoleOrganizer]
    public class ExplorationController : Controller
    {
        private ISearchService _searchService;
        private ILocationService _locationService;

        public ExplorationController(ISearchService searchService, ILocationService locationService)
        {
            _searchService = searchService;
            _locationService = locationService;
        }

        //
        // GET: /Exploration/
        public ActionResult Index()
        {
            var viewModel = _locationService.GetAllLocationTypes().Select(x => new ExploreLocaitonTypeViewModel()
                                                                                   {
                                                                                       Name = x.TypeName,
                                                                                       Icon = x.Icon
                                                                                   }).OrderBy(x=>x.Name);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AjaxFindLocations(int? parentId, string locationType, string locationName,int? zoom, int? zoomTolerance, int? locationId)
        {
            var locations = _searchService.FindLocations(parentId, locationName, locationType,zoom,zoomTolerance,locationId);

            var jsonLocations = (locations == null
                                     ? new List<ExplorationJsonLocation>()
                                     : (from l in locations
                                        select new ExplorationJsonLocation()
                                        {
                                            Id = l.ID,
                                            Name = l.Name,
                                            ShortDescription = l.Description,
                                            Lat = l.Lat,
                                            Lng = l.Lng,
                                            LocationZoomLevel = Int32.Parse(l.LocationDisplaySetting.Zoom),
                                            NumberOfChildren = l.Children.Count,
                                            NumberOfResources = l.Resources.Count,
                                            HasChildren = (l.Children.Count > 0),
                                            TypeName = l.LocationTypeObject.TypeName,
                                            TypeId = l.LocationTypeObject.ID,
                                            Color = l.LocationTypeObject.Color,
                                            Icon = l.LocationTypeObject.Icon,
                                            TypeZoomLevel = Int32.Parse(l.LocationTypeObject.LocationDisplaySetting.Zoom),
                                        }
                                       ).ToList()
                                );
            return Json(jsonLocations);
        }

    }
}
