using System;
using System.Collections.Generic;
using System.Linq;
using GeoGrafija.ResultClasses;
using Model;
using Model.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class LocationService:ILocationService
    {
        private ILocationRepository locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            this.locationRepository = locationRepository;
        }
        
        public Location GetLocation(int locationId)
        {
            var result = locationRepository.getLocation(locationId);
            return result;
        }

        public LocationType GetLocationType(string locationTypeName)
        {
            var result = (from locationTypes in locationRepository.getAllLocationTypes()
                          where locationTypes.TypeName.Equals(locationTypeName, StringComparison.InvariantCultureIgnoreCase)
                          select locationTypes).FirstOrDefault();
            return result;
        }

        public LocationType GetLocationType(int locationTypeId)
        {
            var result = locationRepository.getLocationType(locationTypeId);
            return result;
        }

        public List<Location> GetAllLocations()
        {
            return locationRepository.getAllLocations().ToList();
        }

        public List<LocationType> GetAllLocationTypes()
        {
            return locationRepository.getAllLocationTypes().ToList();
        }
        
        public List<LocationDisplaySetting> GetAllLocationDisplaySettings()
        {
            var results = locationRepository.getAllDisplaySettings().ToList();
            return results;
        }
        
        public LocationDisplaySetting GetLocationDisplaySetting(int id)
        {
            return locationRepository.getLocationDisplaySetting(id);
        }

        public LocationDisplaySetting GetLocationDisplaySetting(string name)
        {
            var allResults = locationRepository.getAllDisplaySettings();

            var result = (from p in allResults
                          where p.Name == name
                          select p).FirstOrDefault();
            return result;
        }

        public OperationResult AddLocation(Location location)
        {
            var result = OperationResult.GetResultObject();

            if (location == null)
            {
                result.AddMessage("Не може да се додаде празна локација");
                return result;
            }

            try
            {
                locationRepository.addLocation(location);
                result.SetSuccess();
                return result;
            }
            catch(Exception ex)
            {
                result.SetException();
                result.AddExceptionMessage(ex.Message);
            }
            return result;
        }

        public OperationResult AddLocationType(LocationType locationType)
        {
            var result = OperationResult.GetResultObject();

            if (locationType == null)
            {
                result.AddMessage("Не може да се додаде празен тип на локација");
                return result;
            }
            
            try
            {
                locationRepository.addLocationType(locationType);
                result.SetSuccess();
                return result;
            }
            catch (Exception ex)
            {
                result.SetException();
                result.AddExceptionMessage(ex.Message);
            }
            return result;
        }


        public OperationResult AddLocationDisplaySetting(LocationDisplaySetting setting)
        {
            var result = OperationResult.GetResultObject();

            if (setting == null)
            {
                result.AddMessage("Не може да се додаде празно подесување за преглед!");
                return result;
            }

            try
            {
                locationRepository.addDisplaySetting(setting);
                result.SetSuccess();
            }
            catch (Exception e)
            {
                result.SetException();
                result.AddExceptionMessage(e.Message);
            }
            return result;
        }
   
        public OperationResult RemoveLocation(Location location)
        {
            var result = OperationResult.GetResultObject();

            if (location == null)
            {
                result.AddMessage("Не може да се избрише локација која не постои");
                return result;
            }

            try
            {
                locationRepository.deleteLocation(location);
                result.SetSuccess();
            }
            catch (Exception ex)
            {
                result.SetException();
                result.AddExceptionMessage(ex.Message);
            }
            return result;
        }
        
        public OperationResult RemoveLocationType(LocationType locationType)
        {
            var result = OperationResult.GetResultObject();

            if (locationType == null)
            {
                result.AddMessage("Не може да се избрише тип на локација која не постои");
                return result;
            }
            
            try
            {
                locationRepository.deleteLocationType(locationType);
                result.SetSuccess();
            }
            catch (Exception ex)
            {
                result.SetException();
                result.AddExceptionMessage(ex.Message);
            }
            return result;
        }

        public OperationResult UpdateLocation(Location location)
        {
            var result = OperationResult.GetResultObject();

            if (location == null)
            {
                result.AddMessage("Не можат да се сменат податоците за локација која не постои");
                return result;
            }

            try
            {
                locationRepository.updateLocation(location);
                locationRepository.SaveChanges();
                result.SetSuccess();
            }
            catch (Exception ex)
            {
                result.SetException();
                result.AddExceptionMessage(ex.Message);
            }

            return result;
        }

        public OperationResult UpdateLocationType(LocationType locationType)
        {
            var result = OperationResult.GetResultObject();

            if (locationType == null)
            {
                result.AddMessage("Не можат да се сменат податоците за локација која не постои");
                return result;
            }

            try
            {
                locationRepository.updateLocationType(locationType);
                locationRepository.SaveChanges();
                result.SetSuccess();
            }
            catch (Exception ex)
            {
                result.SetException();
                result.AddExceptionMessage(ex.Message);
            }

            return result;
        }


        public List<Location> GetLocationsOfUser(User user)
        {
            if (user == null)
                return null;

            var results = (from locations in locationRepository.getAllLocations()
                           where user.UserID == locations.User.UserID
                           select locations).ToList();
            return results.Count>0?results:null;
        }

        public List<LocationType> GetLocationTypesForUser(User user)
        {
            if (user == null)
                return null;

            var results = (from locations in locationRepository.getAllLocationTypes()
                           where user.UserID == locations.User.UserID
                           select locations).ToList();
            return results.Count > 0 ? results : null;
        }

        public List<LocationType> GetLocationTypesForUser(string UserName)
        {
            throw new NotImplementedException();
        }
    }
}