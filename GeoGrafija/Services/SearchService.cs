using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Interfaces;
using Services.Extensions;
using Services.Interfaces;

namespace Services
{
    public class SearchService : ISearchService
    {
        private ILocationRepository _locationRepository;

        public SearchService(ILocationRepository locationReporistory)
        {
            _locationRepository = locationReporistory;
        }

        public List<Location> FindLocations(int? parentLocationId = 0, string term = "", string typeString = "", int? zoom = null, int? zoomTolerance = null, int? locationId = null)
        {
            // Check if single location based search
            if (locationId != null)
            {
                var location = _locationRepository.getLocation((int)locationId);

                if (location == null)
                {
                    return new List<Location>();
                }
                else
                {
                    return  new List<Location>()
                                {
                                    location
                                };
                }
            }

            // Check for zoom based search first.
            if (zoom != null && zoomTolerance != null)
            {
                return GetByZoomTolerance((int) zoom, (int)zoomTolerance);
            }

            var locations = _locationRepository.getAllLocations();
            if (locations == null)
                return null;

            bool filterTop = false;
            int parent = 0 ;
            int type = 0;
            
            if (parentLocationId != null)
            {
                parent = (int)parentLocationId;
            }
            else if (String.IsNullOrWhiteSpace(term) && String.IsNullOrWhiteSpace(typeString))
            {
                filterTop = true;
            }

            if (!String.IsNullOrWhiteSpace(typeString))
            {
                Int32.TryParse(typeString, out type);
            }

            if (String.IsNullOrWhiteSpace(term))
            {
                term = "";
            }
            
            locations = locations.FilterTopLocations(filterTop);
            locations = locations.FilterType(type);
            locations = locations.FilterTerm(term);
            locations = locations.FilterParentId(parent);

            return locations.ToList();
        }

        private List<Location> GetByZoomTolerance(int currentZoom, int zoomTolerance)
        {
            var locations = _locationRepository.getAllLocations();

            var filteredLocations = locations.Where(x=>CheckZoomTolerance(x,currentZoom,zoomTolerance));

            return filteredLocations.ToList();
        }

        private bool CheckZoomTolerance(Location loc, int currentZoom, int zoomTolerance)
        {
            var displaySetting = loc.LocationDisplaySetting ?? loc.LocationTypeObject.LocationDisplaySetting;

            var minZoom = currentZoom - zoomTolerance;
            var maxZoom = currentZoom + zoomTolerance;
            int dsZoom;
            Int32.TryParse(displaySetting.Zoom, out dsZoom);

            if (dsZoom >= minZoom && dsZoom <= maxZoom)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}