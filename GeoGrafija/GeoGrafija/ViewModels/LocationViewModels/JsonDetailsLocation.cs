using System.Collections.Generic;

namespace GeoGrafija.ViewModels.LocationViewModels
{
    public class JsonDetailsLocation
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public int Id { get; set; }
        public List<LocationBreadCrumb> Path { get; set; }
        public int NumberOfResources { get; set; }
    }

    public class LocationBreadCrumb
    {
        public string Name  { get; set; }
        public int Id       { get; set; }
        public string Color { get; set; }
    }

}