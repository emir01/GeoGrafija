using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace Services.Interfaces
{
    public interface ISearchService
    {
        /// <summary>
        /// General Search function for locations. Returns Locations based on the supplied parameters.
        /// </summary>
        /// <param name="parentLocationId">Retrieve locations that have parent with given parentLocationId. If set to non zero value, returns location with the supplied parentId.</param>
        /// <param name="term">Return locations that have a name or type string match given term.If set to non empty returns location that match the supplied name.</param> 
        /// <param name="type">If set to non empty value returns locations that have the supplied type name</param>
        /// <param name="zoom">Returns locations that have given zoom value in their display settings</param>
        /// <param name="zoomTolerance">Returns locations that have zoom value in display settings matching passed in zoom parameter +- tolerance</param>
        /// <param name="locationId">Return a single location with the given id.</param>
        /// <returns>List of locations filtered with the parameters</returns>
        List<Location> FindLocations(int? parentLocationId=0, string term="", string type="",int? zoom=null,int? zoomTolerance = null, int? locationId=null);
    }
}
