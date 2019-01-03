using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeoGrafija.Tests.FakeRepositories;
using Model;
using Model.Interfaces;

namespace GeoGrafija.Tests.FakeModelTests
{
    [TestClass]
    public class LocationRepositoryTests
    {
        public ILocationRepository Repository{ get; set; }

        [TestInitialize]
        public void Arrange()
        {
            Repository = new FakeLocationRepository();
        }

        [TestMethod]
        public void getAllLocations_returns_10_Locations()
        {
            // Act
            var results = Repository.getAllLocations().ToList();

            // Assert 
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 10);
            Assert.IsTrue(results[0] is Location);
        }

        [TestMethod]
        public void getAllLocationTypes_should_return_4_types()
        {
            // Act
            var results = Repository.getAllLocationTypes().ToList();

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 4);
            Assert.IsTrue(results[0] is LocationType);
        }
        
        [TestMethod]
        public void getAllLocations_Returns_Locations_with_LocationType_NotNull()
        {
            // Act
            var results = Repository.getAllLocations().ToList();

            // Assert
            foreach (var loc in results)
            {
                Assert.IsNotNull(loc.LocationType);
            }
        }

        [TestMethod]
        public void getAllLocationTypes_Returns_LocationTypes_with_AtLeast_One_Location()
        {
            // Act
            var results = Repository.getAllLocationTypes();

            // Assert
            foreach (var locType in results)
            {
                Assert.IsNotNull(locType.Locations);
            }
        }

        [TestMethod]
        public void getLocation_id_Should_ReturnLocation()
        {
            // Act
            var result = Repository.getLocation(1) as Location;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ID == 1);
            Assert.IsTrue(result.Name.Equals("Location1", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void getLocationType_id_Should_Return_LocationType()
        {
            // Act
            var result = Repository.getLocationType(1) as LocationType;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ID == 1);
            Assert.IsTrue(result.TypeName.Equals("Continent", StringComparison.InvariantCultureIgnoreCase));
        }

        [TestMethod]
        public void addLocation_Should_AddLocation()
        {
            // Arrange
            var newLocation = new Location() { 
                Name = "NewLocation",
                ID=99,
                Description="Description"
            };

            // Act
            Repository.addLocation(newLocation);

            // Assert
            var addedLocation = Repository.getLocation(99) as Location;
            Assert.IsNotNull(addedLocation);
            Assert.AreEqual(addedLocation.Name,"NewLocation");
            Assert.AreEqual(addedLocation.Description ,"Description");
            Assert.IsTrue(Repository.getAllLocations().Count() == 11);
        }

        [TestMethod]
        public void addLocationType_Should_Add_LocationType()
        {
            // Arrange
            var locationType = new LocationType() { 
                ID=99,
                TypeName="NewType",
                TypeDescription="NewType Decription"
            };

            // Act
            Repository.addLocationType(locationType);

            // Assert
            var addedLType= Repository.getLocationType(99) as LocationType;
            Assert.IsNotNull(addedLType);
            Assert.AreEqual(addedLType.ID, 99);
            Assert.AreEqual(addedLType.TypeName, "NewType");
            Assert.AreEqual(addedLType.TypeDescription, "NewType Decription");
            Assert.IsTrue(Repository.getAllLocationTypes().Count() == 5);   
        }

        [TestMethod]
        public void DeleteLocation_id_should_remove_location()
        {
            // Act
            Repository.deleteLocation(1);

            // Assert
            Assert.IsNull(Repository.getLocation(1));
            Assert.AreEqual(Repository.getAllLocations().Count(), 9);
        }

        [TestMethod]
        public void DeleteLocation_object_shoul_dRemove_Location()
        {
            // Arrange
            var location = Repository.getLocation(1);

            // Act
            Repository.deleteLocation(location);

            // Assert
            Assert.IsNull(Repository.getLocation(1));
            Assert.AreEqual(Repository.getAllLocations().Count(), 9);
        }

        [TestMethod]
        public void DeleteLocationType_id_should_remove_locationType()
        {
            // Act 
            Repository.deleteLocationType(1);

            // Assert
            Assert.IsNull(Repository.getLocationType(1));
            Assert.AreEqual(Repository.getAllLocationTypes().Count(), 3);
        }

        [TestMethod]
        public void DeleteLocationType_object_should_delete_locationtype()
        {
            // Arrange
            var locationtype = Repository.getLocationType(1);

            // Act
            Repository.deleteLocationType(locationtype);
            
            // Assert
            Assert.IsNull(Repository.getLocationType(1));
            Assert.AreEqual(Repository.getAllLocationTypes().Count(), 3);
        }

        [TestMethod]
        public void updateLocation_Should_Update_Location()
        {
            // Arrage
            var location = Repository.getLocation(1);
            location.Name = "New Name";

            // Act
            Repository.updateLocation(location);

            // Assert
            Assert.IsNotNull(Repository.getLocation(1));
            Assert.AreEqual(Repository.getLocation(1).Name, "New Name");
        }
        
        [TestMethod]
        public void updateLocationType_Should_Update_LocationType()
        {
            // Arrange
            var locationType = Repository.getLocationType(1);
            locationType.TypeName = "New Name";

            // Act
            Repository.updateLocationType(locationType);

            // Assert
            Assert.IsNotNull(Repository.getLocation(1));
            Assert.AreEqual(Repository.getLocationType(1).TypeName,"New Name");

        }

        [TestMethod]
        public void GetAllDisplaySettings_Should_Return_AllDisplaySettings()
        {   
            // Act
            var results = Repository.getAllDisplaySettings();

            // Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count() == 4);
            Assert.IsTrue(results.ToList()[0] is LocationDisplaySetting);
        }

        [TestMethod]
        public void getDisplySetting_id_Should_Return_DisplaySetting()
        {
            // Act
            var result = Repository.getLocationDisplaySetting(1) as LocationDisplaySetting;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Name, "Contitent Display");
        }

        [TestMethod]
        public void addLocationDisplaySetting_Should_AddLocationDisplaySetting()
        {
            // Arrange
            var locationDisplaySetting = new LocationDisplaySetting() { 
                Name="Name",
                ID=99,
                Zoom="5",
                MapType="ROADMAP"
            };

            // Act
            Repository.addDisplaySetting(locationDisplaySetting);

            // Assert
            Assert.IsTrue(Repository.getAllDisplaySettings().Count() == 5);
            Assert.IsTrue(Repository.getLocationDisplaySetting(99).Name == "Name");
        }
    }
}
