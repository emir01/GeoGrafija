using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoGrafija.ViewModels.Exploration
{
    public class ExplorationJsonLocation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }

        
        public double Lat { get; set; }
        public double Lng { get; set; }
        public int LocationZoomLevel { get; set; }
        
        public bool HasChildren { get; set; }

        public int NumberOfChildren { get; set; }
        public int NumberOfResources { get; set; }

        public string TypeName { get; set; }
        public int TypeId { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        public int TypeZoomLevel { get; set; }
    }
}