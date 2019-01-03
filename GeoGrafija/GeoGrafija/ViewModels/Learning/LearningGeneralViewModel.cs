using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeoGrafija.ViewModels.LocationTypeViewModels;
using GeoGrafija.ViewModels.ResourcesViewModels;
using Model;

namespace GeoGrafija.ViewModels.Learning
{
    public class LearningGeneralViewModel
    {
        public List<JsonLocationType> LocationTypes { get; set; }
        public SelectList LocationTypesSelectList { get; set; }

        public List<ResourceTypeViewModel> AllResourceTypes { get; set; }

        public LearningGeneralViewModel(IEnumerable<ResourceType> allResourceTypes, IEnumerable<LocationType> locationTypes = null)
        {
            LocationTypes = new List<JsonLocationType>();
            
            if (locationTypes == null)
            {
                return;
            }

            foreach (var locationType in locationTypes)
            {
                var locaitonTypeViewModle = new JsonLocationType(locationType);
                LocationTypes.Add(locaitonTypeViewModle);
            }

            LocationTypesSelectList = new SelectList(LocationTypes, "Id", " LocationTypeName");


            if (allResourceTypes == null)
            {
                AllResourceTypes = new List<ResourceTypeViewModel>();
            }
            else
            {
                AllResourceTypes = new List<ResourceTypeViewModel>();

                foreach (var resourceType in allResourceTypes)
                {
                    var resourceTypeViewModel = new ResourceTypeViewModel(resourceType);
                    AllResourceTypes.Add(resourceTypeViewModel);
                }
            }
        }
    }
}