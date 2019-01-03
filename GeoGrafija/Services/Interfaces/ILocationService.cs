using System.Collections.Generic;
using GeoGrafija.ResultClasses;
using Model;

namespace Services.Interfaces
{
    public interface ILocationService
    {
        //Getting Data
        List<Location> GetAllLocations();
        List<LocationType> GetAllLocationTypes();
        List<LocationDisplaySetting> GetAllLocationDisplaySettings();
        
        List<Location> GetLocationsOfUser(User user);
        
        List<LocationType> GetLocationTypesForUser(User user);
        
        Location GetLocation(int locationId);

        LocationType GetLocationType(string locationTypeName);
        LocationType GetLocationType(int locationTypeId);

        LocationDisplaySetting GetLocationDisplaySetting(int id);
        LocationDisplaySetting GetLocationDisplaySetting(string name);
        
        //Creating Data
        OperationResult AddLocation(Location location);
        OperationResult AddLocationType(LocationType locationType);
        OperationResult AddLocationDisplaySetting(LocationDisplaySetting setting);
        
        //removing data
        OperationResult RemoveLocation(Location location);
        OperationResult RemoveLocationType(LocationType locationType);
        
        //update data
        OperationResult UpdateLocation(Location location);
        OperationResult UpdateLocationType(LocationType locationType);
    }
}