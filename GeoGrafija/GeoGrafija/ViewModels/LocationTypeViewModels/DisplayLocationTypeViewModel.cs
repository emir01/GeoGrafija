using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Model;

namespace GeoGrafija.ViewModels.LocationTypeViewModels
{
    public class DisplayLocationTypeViewModel
    {
        public int ID { get; set; }

        [DisplayName("Име на Тип : ")]
        public string TypeName { get; set; }
        
        [DisplayName("Опис на Тип : ")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public string TypeDescription { get; set; }

        [DisplayName("Креиран од : ")]
        public string CreatedBy { get; set; }
        
        [DisplayName("Слика на маркер : ")]
        public string Icon { get; set; }

        [DisplayName("Стандарден приказ за овој тип :")]
        public string DefaultDisplayName { get; set; }

        [DisplayName("Избрана боја за овој тип :")]
        public string Color { get; set; }

        public void PopulateFields(LocationType locationType,User createdBy)
        {
            ID = locationType.ID;
            TypeDescription = locationType.TypeDescription;
            TypeName = locationType.TypeName;

            CreatedBy = createdBy.UserName;
            Icon =  locationType.Icon;
            Color = locationType.Color;

            DefaultDisplayName = locationType.LocationDisplaySetting.Name;
        }
    }
}