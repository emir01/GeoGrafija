using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Model
{
    [Bind(Exclude="ID,CreatedBy,LocationType,DisplaySettings")]
    [MetadataType(typeof(Location_Metadata))]
    public partial class Location
    {
    }

    public class Location_Metadata
    {
        [DisplayName("Име")]
        public string Name { get; set; }

        [DisplayName("Опис")]
        [AllowHtml]
        public string Description { get; set; }
    }
}