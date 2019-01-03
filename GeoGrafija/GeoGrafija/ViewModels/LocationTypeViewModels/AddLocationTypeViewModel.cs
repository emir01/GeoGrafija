using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Model;

namespace GeoGrafija.ViewModels.LocationTypeViewModels
{
    [Bind(Exclude = "CreatedBy,DisplaySettingsSelect,DisplaySettings,DisplaySettingsSelect")]
    public class AddLocationTypeViewModel
    {
        public int ID { get; set; }

        [DisplayName("Име на тип")]
        [Required(ErrorMessage="Мора да внесете име на типот")]
        [MinLength(3, ErrorMessage = "Мора да внесете име со минимум 3 карактери")]
        public string TypeName { get; set; }
        
        [DisplayName("Опис на тип")]
        [Required(ErrorMessage = "Мора да внесете опис на типот")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [MinLength(3, ErrorMessage = "Мора да внесете опис со минимум 3 карактери")]
        public string TypeDescription { get; set; }

        [DisplayName("Стандарден поглед за локација")]
        [Required(ErrorMessage = "Мора да изберете поглед")]
        public int DefaultDisplaySetting { get; set; }

        [DisplayName("Боја на типот : ")]
        public string Color { get; set; }

        [DisplayName("Име на слика на маркер")]
        public string IconString { get; set; }
        
        public int CreatedBy { get; set; }

        public List<string> MarkerFileNames { get; set; }

        public List<LocationDisplaySetting> DisplaySettings { get; set; }

        public SelectList DisplaySettingsSelect
        {
            get
            {
                return new SelectList(DisplaySettings,"ID","Name");
            }
        }

        public List<string> UploadedMarkerIcons { get; set; }

        public Model.LocationType getLocationTypeFromViewModel()
        {
            var newLocationType = new Model.LocationType();

            newLocationType.TypeName = TypeName;
            newLocationType.TypeDescription= TypeDescription;
            newLocationType.Icon= IconString;
            newLocationType.CreatedBy = CreatedBy;
            newLocationType.Color = Color.Substring(1);
            newLocationType.DisplaySettings = DefaultDisplaySetting;
            newLocationType.ID = ID;

            return newLocationType;
        }

        public void AddLocationTypeToViewModel(Model.LocationType locationType)
        {
            ID = locationType.ID;
            TypeName = locationType.TypeName;
            TypeDescription = locationType.TypeDescription;
            IconString = locationType.Icon;
            Color = locationType.Color;
            DefaultDisplaySetting = locationType.DisplaySettings;

        }
    }
}