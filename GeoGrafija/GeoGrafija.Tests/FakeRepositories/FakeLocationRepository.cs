using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Interfaces;

namespace GeoGrafija.Tests.FakeRepositories
{
    public class FakeLocationRepository : ILocationRepository
    {
        public List<Location> Locations{ get; set; }
        public List<LocationType> LocationTypes { get; set; }
        public List<LocationDisplaySetting> LocationDisplaySettings { get; set; }

        public FakeLocationRepository()
        {
            Locations = new List<Location>();
            LocationTypes = new List<LocationType>();
            LocationDisplaySettings =  new List<LocationDisplaySetting>();

            LocationDisplaySettings.Add(new LocationDisplaySetting() { Name = "Contitent Display",ID=1 });
            LocationDisplaySettings.Add(new LocationDisplaySetting() { Name = "City Display", ID = 2 });
            LocationDisplaySettings.Add(new LocationDisplaySetting() { Name = "River Display", ID = 3 });
            LocationDisplaySettings.Add(new LocationDisplaySetting() { Name = "Mountaint Display", ID = 4 });
            
            LocationTypes.Add(new LocationType() { LocationDisplaySetting=LocationDisplaySettings[0], Icon ="Continent.png", TypeDescription="Continent Description", TypeName = "Continent", ID = 1, CreatedBy=1, User = new User() { UserName="UserName1", UserID=1 } });
            LocationTypes.Add(new LocationType() { TypeName = "City", ID = 2, User = new User() { UserName = "UserName1", UserID = 1 } });
            LocationTypes.Add(new LocationType() { TypeName = "Landmark", ID = 3, User = new User() { UserName = "UserName2", UserID = 2 } });
            LocationTypes.Add(new LocationType() { TypeName = "EmptyLocation", ID = 4, User = new User() { UserName = "UserName2", UserID = 2 } });
 
            for (int i = 0; i < 10; i++)
            {
                var loc = new Location() { LocationDisplaySetting = LocationDisplaySettings[i % 4], 
                                           User = new User() { UserName = "UserName" + ((i + 1) % 3), 
                                           UserID = (i + 1) % 3 }, 
                                           ID = i + 1, 
                                           Name = "Location" + (i + 1), 
                                           Lat = i * 20.2345, 
                                           Lng = i * 54.3432, 
                                           Description = "Description" + i, 
                                           LocationTypeObject = LocationTypes[i % 3], 
                                           LocationType = LocationTypes[i % 3].ID ,
                                           ParentId = ((i+1)%3==0?1:(int?)null)
                                           };
                Locations.Add(loc);
            }
            
            foreach (var loc in Locations)
            {
                if (loc.LocationTypeObject.TypeName == "Continent")
                {
                    LocationTypes[0].Locations.Add(loc);
                }
                if (loc.LocationTypeObject.TypeName == "City")
                {
                    LocationTypes[1].Locations.Add(loc);
                }
                if (loc.LocationTypeObject.TypeName == "Landmark")
                {
                    LocationTypes[2].Locations.Add(loc);
                }
            }
        }

        public void addLocation(Location location)
        {
            Locations.Add(location);
        }

        public void addLocationType(LocationType locationType)
        {
            LocationTypes.Add(locationType);
        }

        public IEnumerable<Location> getAllLocations()
        {
            return Locations;
        }

        public IEnumerable<LocationType> getAllLocationTypes()
        {
            return LocationTypes;
        }

        public Location getLocation(int locationId)
        {
            var result = (from location in Locations
                          where location.ID == locationId
                          select location).FirstOrDefault();

            return result;
        }

        public LocationType getLocationType(int locationTypeId)
        {
            var result = (from locationType in LocationTypes
                          where locationType.ID == locationTypeId
                          select locationType).FirstOrDefault();

            return result;
        }

        public void updateLocation(Location location)
        {
            var oldLocation = (from l in Locations
                               where l.ID == location.ID
                               select l).FirstOrDefault();
            oldLocation = location;
        }

        public void updateLocationType(LocationType locationType)
        {
            var oldLocationType = (from lt in LocationTypes
                                   where lt.ID == locationType.ID
                                   select lt).FirstOrDefault();
            oldLocationType = locationType;
        }

        public void deleteLocation(Location location)
        {
            Locations.Remove(location);
        }

        public void deleteLocation(int locationId)
        {
            Locations.Remove((from l in Locations where l.ID == locationId select l).FirstOrDefault());
        }

        public void deleteLocationType(LocationType locationType)
        {
            LocationTypes.Remove(locationType);
        }

        public void deleteLocationType(int locationTypeId)
        {
            LocationTypes.Remove((from l in LocationTypes where l.ID == locationTypeId select l).FirstOrDefault());
        }

        public void SaveChanges() { }
        
        public void addDisplaySetting(LocationDisplaySetting setting)
        {
            LocationDisplaySettings.Add(setting);
        }

        public IEnumerable<LocationDisplaySetting> getAllDisplaySettings()
        {
            return LocationDisplaySettings;
        }

        public LocationDisplaySetting getLocationDisplaySetting(int displatSettingId)
        {
            var result = (from l in LocationDisplaySettings
                          where l.ID == displatSettingId
                          select l).FirstOrDefault();
            return result;
        }
    }
}
