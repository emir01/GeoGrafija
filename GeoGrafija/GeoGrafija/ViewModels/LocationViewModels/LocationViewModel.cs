using System;
using Model;

namespace GeoGrafija.ViewModels.LocationViewModels
{
    public class LocationViewModel
    {
        public int ID { get; set;}
        
        public String Name { get; set; }
        public String Description { get; set; }

        public double Lat { get; set; }
        public double Lng { get; set; }

        public string TypeName { get; set; }
        public int TypeID { get; set; }
        public string TypeColor { get; set; }

        public LocationViewModel(){}

        public LocationViewModel(Location location)
        {
            ID = location.ID;
            Name = location.Name;
            Description = location.Description;

            Lat = location.Lat;
            Lng = location.Lng;

            TypeName = location.LocationTypeObject.TypeName;
            TypeID = location.LocationTypeObject.ID;
            TypeColor = location.LocationTypeObject.Color;
        }
    }
}