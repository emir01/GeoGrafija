using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Services;
using Services.Interfaces;

namespace GeoGrafija.Tests.Service_Tests
{
    [TestClass]
    public class LocationServiceTests
    {
        ILocationService service;

        [TestInitialize]
        public void Setup()
        {
            service = new LocationService(new FakeRepositories.FakeLocationRepository());
        }
        
        [TestMethod]
        public void getAllLocations_Should_Return_AllLocations()
        {
            // Act
            var results = service.GetAllLocations();

            //asser
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count == 10);
        }

        [TestMethod]
        public void getAllLocationTypes_Should_Return_AllLocationTypes()
        {
            // Act
            var results = service.GetAllLocationTypes();

            // Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count == 4);
        }

        [TestMethod]
        public void getLocation_id_Should_ReturnLocation()
        {
            // Act
            var result = service.GetLocation(1) as Location;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ID == 1);
        }
        
        [TestMethod]
        public void getLocationType_id_Should_Return_LocationType()
        {
            // Act 
            var result = service.GetLocationType(1) as LocationType;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ID == 1);
        }

        [TestMethod]
        public void getLocationType_string_should_return_LocationType()
        {
            // Arrange
            var locationTypeName = "City";

            // Act
            var result = service.GetLocationType(locationTypeName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.TypeName.Equals(locationTypeName, StringComparison.InvariantCultureIgnoreCase));
        }
        
        [TestMethod]
        public void getLocationType_string_Should_Return_Null_For_False_String()
        {
            // Arrange
            var typeName = "BOGUS";

            // Act
            var result = service.GetLocationType(typeName);

            // Assert
            Assert.IsNull(result);
        }
        
        [TestMethod]
        public void AddLocation_Should_AddLocation()
        {
            // Arrange
            var loc = getLocation();

            // Act
            var result = service.AddLocation(loc);

            // Assert
            Assert.IsTrue(result.IsOK);
            Assert.IsNotNull(service.GetLocation(99));
            Assert.AreEqual(service.GetLocation(99).Name, "New York");
        }

        [TestMethod]
        public void AddLocation_Should_Fail_ForNullOjbectPassed()
        {
            // Arrange
            Location loc = null;

            // Act
            var result = service.AddLocation(loc);

            // Assert
            Assert.IsFalse(result.IsOK);
            Assert.IsTrue(result.Messages.Count>0);
        }
        
        [TestMethod]
        public void AddLocationType_Should_Add_Location_Type()
        {
            // Arrange
            var locationType = getLocationType();

            // Act
            var result = service.AddLocationType(locationType);

            // Assert
            Assert.IsTrue(result.IsOK);
            Assert.IsNotNull(service.GetLocationType(100));
            Assert.AreEqual(service.GetLocationType(100).TypeName, "Natural Landmark");
        }

        [TestMethod]
        public void AddLocationType_Should_Fail_ForNullObject()
        {
            // Arrange
            LocationType locationType = null;

            // Act
            var result = service.AddLocationType(locationType);

            // Assert
            Assert.IsFalse(result.IsOK);
            Assert.IsTrue(result.Messages.Count > 0);
        }

        [TestMethod]
        public void RemoveLocation_object_Should_RemoveLocation()
        {
            // Arrange
            var locationToRemove = service.GetLocation(1);

            // Act
            var result = service.RemoveLocation(locationToRemove);

            // Assert
            Assert.IsTrue(result.IsOK);
            Assert.IsTrue(service.GetLocation(1) == null);
            Assert.IsTrue(service.GetAllLocations().Count == 9);
        }
        
        [TestMethod]
        public void removeLocation_Should_Remove_Location()
        {
            // Arrange
            var location = service.GetLocation(1);

            // Act
            var result = service.RemoveLocation(location);

            // Assert
            Assert.IsTrue(result.IsOK);
            Assert.IsNull(service.GetLocation(1));
            Assert.AreEqual(service.GetAllLocations().Count, 9);
        }

        [TestMethod]
        public void removeLocation_Should_Return_Fail_For_NotExcisting_Location()
        {
            // Arrange
            Location location = null;

            // Act
            var result = service.RemoveLocation(location);

            // Assert
            Assert.IsFalse(result.IsOK);
            Assert.IsTrue(result.Messages.Count > 0);
        }

        [TestMethod]
        public void removeLocation_Should_Return_Fail_Exception_On_Exception()
        {
            // Arrange
            var location = service.GetLocation(1);
            var newService = new LocationService(null);

            // Act
            var result = newService.RemoveLocation(location);

            // Assert
            Assert.IsFalse(result.IsOK);
            Assert.IsTrue(result.ExceptionThrown);
            Assert.IsTrue(result.ExceptionMessages.Count > 0);
        }
        
        [TestMethod]
        public void removeLocationType_Should_Remove_LocationType()
        {
            // Arrange
            var locationType = service.GetLocationType(1);

            // Act
            var result = service.RemoveLocationType(locationType);

            // Assert
            Assert.IsTrue(result.IsOK);
            Assert.IsNull(service.GetLocationType(1));
            Assert.AreEqual(service.GetAllLocationTypes().Count, 3);
        }

        [TestMethod]
        public void removeLocationType_Should_Return_fail_for_Null_location()
        {
            // Arrange
            LocationType locationType = null;

            // Act
            var result = service.RemoveLocationType(locationType);

            // Assert
            Assert.IsFalse(result.IsOK);
            Assert.IsTrue(result.Messages.Count > 0);
        }

        [TestMethod]
        public void removeLocationType_Should_Return_Fail_And_Exception_For_Exception()
        {
            // Arrange
            var locationType = service.GetLocationType(1);
            var serviceOther = new LocationService(null);
            
            // Act
            var result = serviceOther.RemoveLocationType(locationType);

            // Assert
            Assert.IsFalse(result.IsOK);
            Assert.IsTrue(result.ExceptionThrown);
            Assert.IsTrue(result.ExceptionMessages.Count > 0);
        }
        
        [TestMethod]
        public void updateLocation_Should_Update_Location()
        {
            // Arrange
            var location = service.GetLocation(1);
            var modifiedName=  "ModifiedName";
            var modifiedDecription="ModifiedDescription";
            location.Name = modifiedName;
            location.Description = modifiedDecription;

            // Act
            var result = service.UpdateLocation(location);

            // Assert
            Assert.IsTrue(result.IsOK);
            Assert.AreEqual(service.GetLocation(1).Name, modifiedName);
            Assert.AreEqual(service.GetLocation(1).Description, modifiedDecription);
        }

        [TestMethod]
        public void updateLocation_Should_Fail_ForNull_Location()
        {
            // Arrange
            Location location = null;

            // Act
            var result = service.UpdateLocation(location);
            
            // Assert
            Assert.IsFalse(result.IsOK);
            Assert.IsTrue(result.Messages.Count > 0);
        }

        [TestMethod]
        public void UpdateLocation_Should_Fail_And_Exception_For_Faulty_RepositoryCall()
        {
            // Arrange
            var location = service.GetLocation(1);
            var newService = new LocationService(null);
            
            // Act 
            var result = newService.UpdateLocation(location);

            // Assert
            Assert.IsFalse(result.IsOK);
            Assert.IsTrue(result.ExceptionThrown);
            Assert.IsTrue(result.ExceptionMessages.Count > 0);
        }

        [TestMethod]
        public void updateLocationType_Should_Update_LocationType()
        {
            // Arrange
            var locationType = service.GetLocationType(1);
            var modifiedName = "ModifiedName";
            var modifiedDecription = "ModifiedDescription";
            locationType.TypeName = modifiedName;
            locationType.TypeDescription = modifiedDecription;

            // Act
            var result = service.UpdateLocationType(locationType);

            // Assert
            Assert.IsTrue(result.IsOK);
            Assert.AreEqual(service.GetLocationType(1).TypeName, modifiedName);
            Assert.AreEqual(service.GetLocationType(1).TypeDescription, modifiedDecription);
        }

        [TestMethod]
        public void updateLocationType_Should_Fail_ForNull_LocationType()
        {
            // Arrange
            LocationType locationType = null;

            // Act
            var result = service.UpdateLocationType(locationType);

            // Assert
            Assert.IsFalse(result.IsOK);
            Assert.IsTrue(result.Messages.Count > 0);
        }

        [TestMethod]
        public void UpdateLocationType_Should_Fail_And_Exception_For_Faulty_RepositoryCall()
        {
            // Arrange
            var locationType = service.GetLocationType(1);
            var newService = new LocationService(null);

            // Act 
            var result = newService.UpdateLocationType(locationType);

            // Assert
            Assert.IsFalse(result.IsOK);
            Assert.IsTrue(result.ExceptionThrown);
            Assert.IsTrue(result.ExceptionMessages.Count > 0);
        }

        [TestMethod]
        public void GetLocationsOfUser_object_Should_Return_Locations_Of_ObjectUser()
        {
            // Arrange
            var user = new User()
            {
                UserName="User1",
                UserID=1
            };

            // Act
            var results = service.GetLocationsOfUser(user) as List<Location>;

            // Assert
            Assert.IsNotNull(results);
            foreach (var loc in results)
            {
                Assert.IsTrue(loc.User.UserName.Equals("UserName1", StringComparison.InvariantCultureIgnoreCase));
                Assert.AreEqual(loc.User.UserID, 1);
            }
        }

        [TestMethod]
        public void getLocationsOfUser_object_Should_Retrun_Null_For_Null_Object()
        {
            // Arrange
            User user = null;

            // Act
            var results = service.GetLocationsOfUser(user);

            // Assert
            Assert.IsNull(results);
        }

        [TestMethod]
        public void GetLocationsOfUser_object_Should_Return_Null_For_User_Not_Excist()
        {
            // Arrange
            var user = new User()
            {
                UserName = "BOGUS",
                UserID = 22
            };

            // Act
            var results = service.GetLocationsOfUser(user) as List<Location>;

            // Assert
            Assert.IsNull(results);
        }
        
        [TestMethod]
        public void GetLocationsTyleOfUser_object_Should_Return_Locations_Of_ObjectUser()
        {
            // Arrange
            var user = new User()
            {
                UserName = "User1",
                UserID = 1
            };

            // Act
            var results = service.GetLocationTypesForUser(user) as List<LocationType>;

            // Assert
            Assert.IsNotNull(results);
            foreach (var locType in results)
            {
                Assert.IsTrue(locType.User.UserName.Equals("UserName1", StringComparison.InvariantCultureIgnoreCase));
                Assert.AreEqual(locType.User.UserID, 1);
            }
        }

        [TestMethod]
        public void getLocationsTypeOfUser_object_Should_Retrun_Null_For_Null_Object()
        {
            // Arrange
            User user = null;

            // Act
            var results = service.GetLocationTypesForUser(user);

            // Assert
            Assert.IsNull(results);
        }

        [TestMethod]
        public void GetLocationTypesOfUser_object_Should_Return_Null_For_User_Not_Excist()
        {
            // Arrange
            var user = new User()
            {
                UserName = "BOGUS",
                UserID = 22
            };

            // Act
            var results = service.GetLocationTypesForUser(user) as List<LocationType>;

            // Assert
            Assert.IsNull(results);
        }
        
        [TestMethod]
        public void GetAllLocationSettings_Should_Return_Display_Location_Settings()
        {
            // Act
            var results = service.GetAllLocationDisplaySettings();

            // Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count == 4);
        }

        [TestMethod]
        public void getLocationDisplaSetting_Should_Return_locationDisplaySetting()
        {
            // Act
            var result = service.GetLocationDisplaySetting(1) as LocationDisplaySetting;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Name, "Contitent Display");
        }

        [TestMethod]
        public void getLocationDisplaySetting_id_Should_Return_null_for_false_id()
        {
            // Act
            var result = service.GetLocationDisplaySetting(99) as LocationDisplaySetting;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void getLocationDisplaySetting_string_should_return_LocationDisplaySetting()
        {
            // Act
            var result = service.GetLocationDisplaySetting("Contitent Display") as LocationDisplaySetting;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ID, 1);
        }

        [TestMethod]
        public void getLocationDisplaySetting_string_Should_return_Null_for_False_name()
        {
            // Act
            var result = service.GetLocationDisplaySetting("BOGUS") as LocationDisplaySetting;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void getLocationDisplaySetting_Should_return_null_for_Null_Name()
        {
            // Act
            var result = service.GetLocationDisplaySetting(null) as LocationDisplaySetting;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void addLocationDisplaySetting_Should_AddLocationDisplaySetting()
        {
            // Arrange
            var displaySetting = getLocationDisplaySetting();

            // Act
            var result = service.AddLocationDisplaySetting(displaySetting);

            // Assert
            Assert.IsTrue(result.IsOK);
        }

        #region Helpers

        private Location getLocation()
        {
            var location = new Location()
            {
                ID = 99,
                Name = "New York",
                LocationTypeObject = service.GetLocationType("City"),
                Lat = 244.234,
                Lng = 23.2342
            };
            return location;
        }

        private LocationType getLocationType()
        {
            var locationType =  new LocationType(){
                TypeName="Natural Landmark",
                ID=100,
            };
            
            locationType.Locations.Add(service.GetLocation(1));
            locationType.Locations.Add(service.GetLocation(2));
            return locationType;
        }
        
        private LocationDisplaySetting getLocationDisplaySetting()
        {
            var locationDisplaySetting = new LocationDisplaySetting()
            {
                ID=99,
                Zoom="5",
                MapType="ROADMAP",
                Name="display"
            };

            return locationDisplaySetting;
        }

        #endregion
    }
}
