using System.Collections.Generic;
using System.Text;
using System.Linq;
using GeoGrafija.Tests.FakeRepositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Services;
using Services.Interfaces;

namespace GeoGrafija.Tests.Service_Tests
{
    [TestClass]
    public class SearchServiceTests
    {
        private ISearchService service;
        private int            parentId;
        private string         name;
        private string         type;
        
        [TestInitialize]
        public void Setup()
        {
            service      = new SearchService(new FakeLocationRepository());
            parentId     = 0;
            name         = "";
            type         = "";
        }

        [TestMethod]
        public void FindLocations_Should_Return_List_Of_Locations()
        {
            // Act
            var locations = service.FindLocations(parentId, name, type) as List<Location>;

            // Assert
            Assert.IsNotNull(locations,"Find Locations fails to return List of Locations");
        }

        [TestMethod]
        public void FindLocations_Should_Return_All_Locations_For_Default_Values()
        {
            // Act
            var locations = service.FindLocations();
            
            // Assert
            Assert.AreEqual(locations.Count,10,"Find location fails to return all locations for default filter values");
        }

        [TestMethod]
        public void FindLocations_Should_Filter_Top_Locations_For_Apropriate_Sent_Params()
        {
            // Act
            var locations = service.FindLocations(null, "", "");

            // Assert
            Assert.AreEqual(locations.Count,7);
            foreach (var location in locations)
            {
                Assert.IsTrue(location.ParentId==null,"Find Locations fails to filter only top locations for topFilter set to true");
            }
        }

        [TestMethod]
        public void FindLocations_Should_NotFilter_Top_Locations_For_FilterValue_False()
        {
            // Act
            var locations = service.FindLocations(0, "", "");

            // Assert
            Assert.AreEqual(locations.Count, 10,"Find Locations failed to return all locations for no filters set");
        }
            
        [TestMethod]
        public void FindLocations_Should_FilterByType_If_TypeId_Set_ToValue()
        {
            // Act
            var locations = service.FindLocations(0, "", "3");

            // Assert
            Assert.AreEqual(locations.Count,3,"Find Locations Failed to Filter properly for only Type Filter");
        }

        [TestMethod]
        public void FindLocations_Should_FilterByName_If_Name_Set_To_Value()
        {
            // Arrange
            var name = "Location2";

            // Act
            var locations = service.FindLocations(0, name);

            // Assert
            Assert.AreEqual(locations.Count, 1);
            Assert.AreEqual(locations[0].Name,name);
        }

        [TestMethod]
        public void FindLocations_Should_FilterByParentId_ForSet_Value()
        {
            // Arrange
            var parentId = 1;
            // Act
            var locations = service.FindLocations(parentId);

            // Assert
            Assert.AreEqual(locations.Count,3);
            foreach (var location in locations)
            {
                Assert.AreEqual(parentId,location.ParentId);
            }
        }
    }
}
