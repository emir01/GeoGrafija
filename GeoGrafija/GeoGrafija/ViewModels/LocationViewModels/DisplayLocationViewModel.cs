using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GeoGrafija.ViewModels.ResourcesViewModels;
using Model;

namespace GeoGrafija.ViewModels.LocationViewModels
{
    public class DisplayLocationViewModel
    {
        public int ID { get; set; }

        [DisplayName("Име ")]
        public string Name { get; set; }
        
        [DisplayName("Опис")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }
        
        public float Lat { get; set; }
        public float Lng { get; set; }


        [DisplayName("Тип на преглед")]
        public string DisplayName { get; set; }
        
        public string Zoom { get; set; }
        public string MapType { get; set; }
        public bool RenderControls { get; set; }
        public string Icon { get; set; }

        [DisplayName("Тип на локација")]
        public string TypeName { get; set; }
        public string TypeDescription { get; set; }
        public string TypeColor { get; set; }

        public List<ResourceTypeViewModel> AllResourceTypes { get; set; }
        
        public static DisplayLocationViewModel  BindToLocation(Model.Location location, List<ResourceType> allResourceTypes)
        {
            if (location == null)
            {
                return null;
            }
           
            var model = new DisplayLocationViewModel();
            
            model.ID = location.ID;
            model.Name = location.Name;
            model.Description = location.Description;

            model.Lat = (float)location.Lat;
            model.Lng = (float)location.Lng;
            
            if (location.LocationDisplaySetting != null)
            {
                model.DisplayName = location.LocationDisplaySetting.Name;
                model.Zoom = location.LocationDisplaySetting.Zoom;
                model.MapType = location.LocationDisplaySetting.MapType;
                model.RenderControls = location.LocationDisplaySetting.RenderControls;
            }
            else
            {
                model.DisplayName = location.LocationTypeObject.LocationDisplaySetting.Name;
                model.Zoom = location.LocationTypeObject.LocationDisplaySetting.Zoom;
                model.MapType = location.LocationTypeObject.LocationDisplaySetting.MapType;
                model.RenderControls = location.LocationTypeObject.LocationDisplaySetting.RenderControls;
            }

            model.Icon = location.LocationTypeObject.Icon;
            model.TypeName = location.LocationTypeObject.TypeName;
            model.TypeDescription = location.LocationTypeObject.TypeDescription;
            model.TypeColor = location.LocationTypeObject.Color;

            // Bind the resources to the View Model 
            model.AllResourceTypes = BindResources(allResourceTypes,location.ID);

            return model;
       }

        private static List<ResourceTypeViewModel> BindResources(List<ResourceType> allResourceTypes,int locationId)
        {
            if (allResourceTypes == null)
            {
                return new List<ResourceTypeViewModel>();
            }

            List<ResourceTypeViewModel> allResourceTypeViewModels = new List<ResourceTypeViewModel>();

            foreach (var resourceType in allResourceTypes)
            {
                var resourceTypeViewModel = new ResourceTypeViewModel(resourceType, locationId);
                allResourceTypeViewModels.Add(resourceTypeViewModel);
            }
            return allResourceTypeViewModels;
        }
    }
}