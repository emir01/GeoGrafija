using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace Services.Extensions
{
    public static class  FilterExtensions
    {
        public static IEnumerable<Location> FilterTopLocations(this IEnumerable<Location> locations,bool filterTop)
        {
            locations = locations.Where(x =>
                                        filterTop != true || (x.ParentId == null || x.ParentId==0));
            return locations;
        }

        public static IEnumerable<Location> FilterType(this IEnumerable<Location> locations, int type)
        {
            locations = locations.Where(x =>
                                        type==0 || x.LocationTypeObject.ID.Equals(type));
            return locations;
        }

        public static IEnumerable<Location> FilterTerm(this IEnumerable<Location> locations, string term)
        {
            // needs more complex search logic by term.
            
            locations = locations.Where(x =>
                                        term == "" 
                                        || term.ToLower().Equals("се")
                                        || x.Name.ToLower().Contains(term.ToLower()) 
                                        || x.LocationTypeObject.TypeName.ToLower().Contains(term.ToLower()));
            return locations;
        }
        
        public static IEnumerable<Location> FilterParentId(this IEnumerable<Location> locations, int parentId)
        {
            locations = locations.Where(x =>
                                        parentId == 0 || x.ParentId==parentId);
            return locations;
        }

        
    }
}
