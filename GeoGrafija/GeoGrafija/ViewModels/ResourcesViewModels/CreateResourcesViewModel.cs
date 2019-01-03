using System.Collections.Generic;
using System.Web.Mvc;
using GeoGrafija.ViewModels.LocationTypeViewModels;
using Model;

namespace GeoGrafija.ViewModels.ResourcesViewModels
{
    public class CreateResourcesViewModel
    {
        public bool IsOk { get; set; }
        public string Message { get; set; }

        public int LocationTypeId { get; set; }
        
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationTypeName { get; set; }

        public string Text { get; set; }
        public string ResourceName { get; set; }

        public int ResourceTypeId { get; set; }

        public List<ResourceTypeViewModel> AvailableResourceTypes { get; set; }
        public SelectList ResourceTypesSelectList { get; set; }

        public List<JsonLocationType> LocationTypes { get; set; }
        public SelectList LocationTypesSelectList { get; set; }

        #region Constructor 

        public CreateResourcesViewModel(Location location, IEnumerable<ResourceType> availableResourceTypes, IEnumerable<LocationType> locationTypes = null)
        {
            if (location != null)
            {
                LocationId = location.ID;
                LocationName = location.Name;

                LocationTypeId = location.LocationTypeObject.ID;
                LocationTypeName = location.LocationTypeObject.TypeName;
            }

            CreateListItems(availableResourceTypes,locationTypes);
        }

        public CreateResourcesViewModel()
        {
        }

        private void CreateListItems(IEnumerable<ResourceType> availableResourceTypes,
                                                     IEnumerable<LocationType> locationTypes)
        {
            AvailableResourceTypes = new List<ResourceTypeViewModel>();
            LocationTypes = new List<JsonLocationType>();

            foreach (var availableResourceType in availableResourceTypes)
            {
                var resourceTypeViewModel = new ResourceTypeViewModel(availableResourceType);

                AvailableResourceTypes.Add(resourceTypeViewModel);
            }

            // create the select list for resouce type
            ResourceTypesSelectList = new SelectList(AvailableResourceTypes, "Id", "Name");

            if (locationTypes != null)
            {
                foreach (var locationType in locationTypes)
                {
                    var locaitonTypeViewModle = new JsonLocationType(locationType);
                    LocationTypes.Add(locaitonTypeViewModle);
                }

                LocationTypesSelectList = new SelectList(LocationTypes, "Id", " LocationTypeName");
            }
        }

        #endregion

        public Resource GetResourceFromModel()
        {
            var resource = new Resource();

            if (LocationId != null && LocationId != 0) { 
                resource.LocationId = LocationId;
            }

            if (LocationTypeId != null && LocationTypeId != 0)
            {
                resource.LocationTypeId = LocationTypeId;
            }

            resource.Name = ResourceName;
            resource.Text = Text;
            resource.TypeId = ResourceTypeId;

            return resource;
        }
    }
}