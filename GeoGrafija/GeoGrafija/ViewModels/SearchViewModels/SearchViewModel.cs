using System.Collections.Generic;
using GeoGrafija.ViewModels.LocationViewModels;
using Model;

namespace GeoGrafija.ViewModels.SearchViewModels
{
    public class SearchViewModel
    {
        public string SearchTerm { get; set; }
        public List<LocationViewModel> Results { get; set; }
        public List<LocationType> LocationTypes { get; set; }
        public bool Result { get; set; }
    }
}