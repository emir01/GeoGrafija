using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace GeoGrafija.ViewModels.LocationTypeViewModels
{
    public class JsonLocationType
    {
        public int Id { get; set; }
        public string LocationTypeName { get; set; }
        public string Color { get; set; }

        public JsonLocationType(LocationType type)
        {
            Id = type.ID;
            LocationTypeName = type.TypeName;
            Color = type.Color;
        }

        public JsonLocationType()
        {
        }
    }
}