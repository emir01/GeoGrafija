using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Model;

namespace GeoGrafija.ViewModels.LocationViewModels
{
    [Bind(Exclude = "LocationTypes,DisplaySettings")]
    public class LocationFormViewModel
    {
        public int ID { get; set; }

        [DisplayName("Име на локација :")]
        [Required(ErrorMessage = "Мора да внесете име за локацијата")]
        [MinLength(3,ErrorMessage="Мора да внесете име со минимум 3 карактери")]
        public string Name { get; set; }

        [Required(ErrorMessage="Мора да внесете краток опис на локацијата")]
        [DisplayName("Краток Опис :")]
        [MinLength(3, ErrorMessage = "Мора да внесете опис со минимум 3 карактери")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Мора да изберете локација на мапата")]
        public float Lat { get; set; }

        [Required(ErrorMessage = "Мора да изберете локација на мапата")]
        public float Lng { get; set; }

        [Required(ErrorMessage = "Мора да го изберете типот на локацијата")]
        [DisplayName("Тип на локација :")]
        public int ChosenType { get; set; }
        
        [DisplayName("Тип на преглед :")]
        public int? ChosenDisplaySetting { get; set; }

        [DisplayName("Искористи Тековен Поглед :")]
        public bool CurrentDisplaySetting { get; set; }

        [DisplayName("Име на родител локација :")]
        public string  ParentLocationName { get; set; }


        public int? ParentLocationId { get; set; }

        public int UserId { get; set; }
        
        //VAlues for Possibly Creating new Display Settings
        public string  Zoom { get; set; }
        public string MapType { get; set; }

        
        [DisplayName("Име на нов Поглед :")]
        public string DisplayName { get; set; }

        //For RENDER
        public List<LocationType> LocationTypes { get; set; }

        //For Render
        public List<LocationDisplaySetting> DisplaySettings { get; set; }
        
        public Location getLocationFromViewModel()
        {
            var location = new Location();
            
            location.CreatedBy = UserId;
            location.ID = ID;
            location.Description = Description;
            location.Name = Name;
            location.LocationType = ChosenType;
            location.DisplaySettings = ChosenDisplaySetting;
            location.Lat = Lat;
            location.Lng = Lng;
            location.ParentId = ParentLocationId;

            return location;
        }

        public void  setLocationToViewModel(Location location)
        {
            ID = location.ID;

            Name = location.Name;
            Description = location.Description;
            Lat = (float)location.Lat;
            Lng = (float)location.Lng;
            ChosenType = location.LocationTypeObject.ID;

            var locationDisplaySettingObject = location.LocationDisplaySetting == null ? location.LocationTypeObject.LocationDisplaySetting : location.LocationDisplaySetting;
            ChosenDisplaySetting = locationDisplaySettingObject.ID;
            
            UserId = location.CreatedBy;

            Zoom = locationDisplaySettingObject.Zoom;
            MapType = locationDisplaySettingObject.MapType;
            DisplayName = locationDisplaySettingObject.Name;

            ParentLocationId = location.ParentId ?? 0;
            ParentLocationName = location.Parent != null ? location.Parent.Name ?? "" : "";
        }

        public LocationDisplaySetting getLocationDisplaySettingFromViewModel()
        {
            if (!CurrentDisplaySetting)
            {
                return null;
            }

            var displaysetting = new LocationDisplaySetting();
            displaysetting.Name = DisplayName;
            displaysetting.Zoom = Zoom;
            displaysetting.MapType = MapType;
            displaysetting.RenderControls = false;

            return displaysetting;
        }

        #region Select Lists
        public SelectList LocationTypesSelectList
        {
            get
            {
                SelectList list = new SelectList(LocationTypes, "ID", "TypeName");
                return list;
            }
        }

        public SelectList LocationDisplaySettingsSelectList
        {
            get
            {
                SelectList list = new SelectList(DisplaySettings, "ID", "Name");
                return list;
            }
        }
        #endregion
    }
}