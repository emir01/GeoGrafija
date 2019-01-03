using System.Collections.Generic;

namespace Model.Interfaces
{
    public interface ILocationRepository:IDatabaseRepository
    {
        //Create
        void addLocation(Location location);
        void addLocationType(LocationType locationType);
        void addDisplaySetting(LocationDisplaySetting setting);

        //Read
        IEnumerable<Location> getAllLocations();
        IEnumerable<LocationType> getAllLocationTypes();
        IEnumerable<LocationDisplaySetting> getAllDisplaySettings();

        Location getLocation(int locationId);
        LocationType getLocationType(int locationTypeId);
        LocationDisplaySetting getLocationDisplaySetting(int displatSettingId);
        
        //Update
        void updateLocation(Location location);
        void updateLocationType(LocationType locationType);

        //Delete
        void deleteLocation(Location location);
        void deleteLocation(int locationId);
        
        void deleteLocationType(LocationType locationType);
        void deleteLocationType(int locationTypeId);
    }
}