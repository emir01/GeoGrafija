using System.Collections.Generic;
using System.Linq;
using Model.Interfaces;

namespace Model.Repositories
{
    public class LocationsRepository:ILocationRepository
    {
        private GeoGrafijaEntities context;

        public LocationsRepository(IDbContext context)
        {
            this.context = (GeoGrafijaEntities)context;
        }

        public void addLocation(Location location)
        {
            context.Locations.AddObject(location);
            SaveChanges();
        }

        public void addLocationType(LocationType locationType)
        {
            context.LocationTypes.AddObject(locationType);
            SaveChanges();
        }

        public IEnumerable<Location> getAllLocations()
        {
            return context.Locations;
        }

        public IEnumerable<LocationType> getAllLocationTypes()
        {
            return context.LocationTypes;
        }

        public Location getLocation(int locationId)
        {
            return (from l in context.Locations
                    where l.ID == locationId
                    select l).FirstOrDefault();
        }

        public LocationType getLocationType(int locationTypeId)
        {
            return (from lt in context.LocationTypes
                    where lt.ID == locationTypeId
                    select lt).FirstOrDefault();
        }

        public void updateLocation(Location location)
        {
            var oldLocation = getLocation(location.ID);
            
            //TODO : Look into Changing this
            oldLocation.Name = location.Name;
            oldLocation.Description = location.Description;
           
            oldLocation.Lat = location.Lat;
            oldLocation.Lng = location.Lng;
          
            oldLocation.DisplaySettings = location.DisplaySettings;
            oldLocation.LocationType = location.LocationType;

            oldLocation.ParentId = location.ParentId;
        }

        public void updateLocationType(LocationType locationType)
        {
            var oldLocationType = getLocationType(locationType.ID);

            oldLocationType.TypeName = locationType.TypeName;
            oldLocationType.TypeDescription = locationType.TypeDescription;
            oldLocationType.Icon = locationType.Icon;
            oldLocationType.Color = locationType.Color;
            oldLocationType.DisplaySettings = locationType.DisplaySettings;
        }

        public void deleteLocation(Location location)
        {
            context.Locations.DeleteObject(location);
            SaveChanges();
        }

        public void deleteLocation(int locationId)
        {
            context.Locations.DeleteObject((from l in context.Locations where l.ID == locationId select l).FirstOrDefault());
            SaveChanges();
        }

        public void deleteLocationType(LocationType locationType)
        {
            context.LocationTypes.DeleteObject(locationType);
            SaveChanges();
        }

        public void deleteLocationType(int locationTypeId)
        {
            context.LocationTypes.DeleteObject((from lt in context.LocationTypes where lt.ID == locationTypeId select lt).FirstOrDefault());
            SaveChanges();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
        
        public void addDisplaySetting(LocationDisplaySetting setting)
        {
            context.LocationDisplaySettings.AddObject(setting);
            SaveChanges();
        }

        public IEnumerable<LocationDisplaySetting> getAllDisplaySettings()
        {
            return context.LocationDisplaySettings;
        }

        public LocationDisplaySetting getLocationDisplaySetting(int displatSettingId)
        {
            return  (from l in context.LocationDisplaySettings
                          where l.ID == displatSettingId
                          select l).FirstOrDefault();
        }
    }
}