using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Model
{
    [MetadataType(typeof(LocationType_Metadata))]
    public partial class LocationType
    {
    }
    
    public class LocationType_Metadata
    {
        [DisplayName("Име на Тип")]
        public string TypeName { get; set; }

        [DisplayName("Тип")]
        public string TypeDescription { get; set; }
    }
}