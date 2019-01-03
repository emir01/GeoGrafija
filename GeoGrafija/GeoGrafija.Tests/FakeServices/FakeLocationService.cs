using System;
using System.Collections.Generic;
using GeoGrafija.ResultClasses;
using Model;
using Services.Interfaces;

namespace GeoGrafija.Tests.FakeServices
{
    class FakeLocationService:ILocationService
    {
        public bool addLocationWasCalled { get; set; }
        public bool addNewDisplaySetingWasCalled { get; set; }
        public bool AddLocationTypeWasCalled { get; set; }
        public LocationType passedLocationType { get; set; }
        public Location passedParameter { get; set; }
        public LocationDisplaySetting passedDisplaySetting { get; set; }
        public string CalledFirst { get; set; }

        public FakeLocationService()
        {
            CalledFirst = "";
        }

        public List<Location> GetAllLocations()
        {
            return new List<Location>();
        }

        public List<LocationType> GetAllLocationTypes()
        {
            return new List<LocationType>();
        }

        public List<LocationDisplaySetting> GetAllLocationDisplaySettings()
        {
            return new List<LocationDisplaySetting>();
        }

        public List<Location> GetLocationsOfType(LocationType type)
        {
            throw new NotImplementedException();
        }

        public List<Location> GetLocationsOfType(string typeName)
        {
            throw new NotImplementedException();
        }

        public List<Location> GetLocationsOfUser(User user)
        {
            throw new NotImplementedException();
        }

        public List<Location> GetLocationsOfUser(string UserName)
        {
            throw new NotImplementedException();
        }

        public List<LocationType> GetLocationTypesForUser(User user)
        {
            throw new NotImplementedException();
        }

        public List<LocationType> GetLocationTypesForUser(string UserName)
        {
            throw new NotImplementedException();
        }

        public Location GetLocation(string locationName)
        {
            throw new NotImplementedException();
        }

        public Location GetLocation(int locationId)
        {
            throw new NotImplementedException();
        }

        public LocationType GetLocationType(string locationTypeName)
        {
            throw new NotImplementedException();
        }

        public LocationType GetLocationType(int locationTypeId)
        {
            throw new NotImplementedException();
        }

        public ResultClasses.OperationResult AddLocation(Location location)
        {
            if (CalledFirst == "")
            {
                CalledFirst = "location";
            }

            addLocationWasCalled = true;
            passedParameter = location;
            return new OperationResult()
            {
                IsOK=true
            };
        }

        public ResultClasses.OperationResult AddLocationType(LocationType locationType)
        {
            if (CalledFirst == "")
            {
                CalledFirst = "type";
            }
            
            passedLocationType = locationType;
            AddLocationTypeWasCalled = true;

            return new OperationResult() { 
                IsOK=true
            };
        }

        public ResultClasses.OperationResult RemoveLocation(Location location)
        {
            throw new NotImplementedException();
        }

        public ResultClasses.OperationResult RemoveLocationType(LocationType locationType)
        {
            throw new NotImplementedException();
        }

        public ResultClasses.OperationResult UpdateLocation(Location location)
        {
            throw new NotImplementedException();
        }

        public ResultClasses.OperationResult UpdateLocationType(LocationType locationType)
        {
            throw new NotImplementedException();
        }

        public LocationDisplaySetting GetLocationDisplaySetting(int id)
        {
            throw new NotImplementedException();
        }

        public LocationDisplaySetting GetLocationDisplaySetting(string name)
        {
            throw new NotImplementedException();
        }

        public OperationResult AddLocationDisplaySetting(LocationDisplaySetting setting)
        {
            if (CalledFirst == "")
            {
                CalledFirst = "display";
            }
            addNewDisplaySetingWasCalled = true;
            passedDisplaySetting = setting;
            
            return new OperationResult()
            {
                IsOK = true
            };
        }
    }
}
