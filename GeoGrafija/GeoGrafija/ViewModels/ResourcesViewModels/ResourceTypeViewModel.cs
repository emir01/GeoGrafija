using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace GeoGrafija.ViewModels.ResourcesViewModels
{
    public class ResourceTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }

        public List<ResourceViewModel> Resources { get; set; }

        // Constructor with optional parameter to filter resources for a given location. 
        // Does not fiter if value is  0
        public ResourceTypeViewModel(ResourceType resourceType,int filterLocation = 0)
        {
            Id = resourceType.ID;
            Name = resourceType.Name;
            Description = resourceType.Description;
            Color = resourceType.Color;

            // Setup the resources view model list
            Resources = new List<ResourceViewModel>();
           
            foreach (var resource in resourceType.Resources)
            {
                var resourceViewModel = new ResourceViewModel(resource);
                if (filterLocation == 0)
                {
                    Resources.Add(resourceViewModel);
                }
                else
                {
                    if (resource.LocationId == filterLocation)
                    {
                        Resources.Add(resourceViewModel);
                    }
                }
            }
        }
    }
}