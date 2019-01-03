using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Model
{
    [MetadataType(typeof(LocationDisplaySetting_Metadata))]
    public  partial class  LocationDisplaySetting
    {
    }

    public class LocationDisplaySetting_Metadata
    {
        [DisplayName("Име на Преглед")]
        public string Name { get; set; }
    }
}