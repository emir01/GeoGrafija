using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GeoGrafija.CustomFilters;
using GeoGrafija.ViewModels.LocationTypeViewModels;
using GeoGrafija.ViewModels.LocationViewModels;
using GeoGrafija.ViewModels.ResourcesViewModels;
using Model;
using Services;
using Services.Interfaces;

namespace GeoGrafija.Controllers
{
    [RoleOrganizer]
    public class SearchController : Controller
    {
        private ILocationService _locationService;
        private IUserService _userService;
        private ISearchService _searchService;
        private IResourceService _resourceService;

        public SearchController(ILocationService locationService, IUserService userService, ISearchService searchService, IResourceService resourceService)
        {
            _locationService = locationService;
            _userService = userService;
            _searchService = searchService;
            _resourceService = resourceService;
        }

        public ActionResult Index()
        {
            return View();
        }

        // == 
        // GET : Search Location Details, available to public.
        public ActionResult Location(int id)
        {
            var location = _locationService.GetLocation(id);
            var allResourceTypes = _resourceService.GetAllResourceTypes().GetData().ToList();

            _userService.UpdateUserViewedLocation(User.Identity.Name);

            var viewModel = DisplayLocationViewModel.BindToLocation(location,allResourceTypes);


            if (viewModel != null)
            {
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public ActionResult AjaxFindLocations(int? parentId, string locationType, string locationName)
        {
            var locations = _searchService.FindLocations(parentId, locationName, locationType);

            var jsonLocations = (locations == null
                                     ? new List<JsonLocation>()
                                     : (from l in locations
                                        select new JsonLocation()
                                                   {
                                                       Name = l.Name,
                                                       Type = l.LocationTypeObject.TypeName,
                                                       Color = l.LocationTypeObject.Color,
                                                       Id = l.ID,
                                                       HasChildren = (l.Children.Count > 0)
                                                   }
                                       ).ToList()
                                );
            return Json(jsonLocations);
        }

        //== 
        // AJAX : GET Location extra sugar info 
        // id - For which location will we be building the LocationInfo Json Object
        [HttpPost]
        public ActionResult LocationInfo(int locationId)
        {
            var location = _locationService.GetLocation(locationId);

            // if there is no locaiton by that id return empty result, and pass procesing to the 
            // client modules.
            if (location == null)
            {
                return Json(new JsonDetailsLocation());
            }

            var result = new JsonDetailsLocation();

            result.Name = location.Name;
            result.Id = location.ID;
            result.Color = location.LocationTypeObject.Color;

            result.Path = GetPath(location);
            result.Path.Reverse();
            result.NumberOfResources = location.Resources.Count;
            return Json(result);
        }

        [HttpPost]
        public ActionResult GetResourceDetails(int resourceId)
        {
            var result = _resourceService.GetResource(resourceId);

            if (result.IsOK)
            {
                var viewModel = new ResourceViewModel(result.GetData());
                viewModel.IsOk = true;
                _userService.UpdateUserViewedResource(User.Identity.Name);
                return Json(viewModel);
            }
            else
            {
                var viewModel = new ResourceViewModel(result.GetData());
                viewModel.IsOk = false;
                viewModel.Message = result.Messages.FirstOrDefault();

                return Json(viewModel);
            }
        }


        /// <summary>
        /// Creates a List of location breadcrumbs for the passed in location.
        /// </summary>
        /// <param name="location">The locaiton for which the function will create the breadcrumb path</param>
        /// <returns>List of location breadcrumb elements that can be then displayed on the client as 
        /// the hirearchical path to the locaton. Each element in the path should be a link to a search under the context of the clicked breadcrumb location</returns>
        private List<LocationBreadCrumb> GetPath(Location location)
        {
            var path = new List<LocationBreadCrumb>();

            if (location.Parent == null)
            {
                path.Add(GetBreadCrumbLocaiton(location));
                return path;
            }
            else
            {
                // build path as long as you have parent locations
                path.Add(GetBreadCrumbLocaiton(location));
                var activeLocation = location;
                while (activeLocation.Parent != null)
                {
                    var parent = activeLocation.Parent;
                    path.Add(GetBreadCrumbLocaiton(parent));
                    activeLocation = parent;
                }
            }

            return path;
        }

        private LocationBreadCrumb GetBreadCrumbLocaiton(Location location)
        {
            if (location == null)
                throw new ArgumentNullException("location was null");
            else
                return new LocationBreadCrumb()
                           {
                               Id = location.ID,
                               Name = location.Name,
                               Color = location.LocationTypeObject.Color
                           };
        }

        //==
        // AJAX : GET ALL LOCATION TYPES 
        public ActionResult GetJsonLTypes()
        {
            var locationTypes = new List<JsonLocationType>();

            locationTypes = (from locType in _locationService.GetAllLocationTypes()
                             select new JsonLocationType()
                                        {
                                            LocationTypeName = locType.TypeName,
                                            Color = locType.Color,
                                            Id = locType.ID

                                        }).ToList();

            return Json(locationTypes);
        }
    }
}
