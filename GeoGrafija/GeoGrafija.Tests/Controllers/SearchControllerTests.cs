using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using GeoGrafija.ViewModels;

using GeoGrafija.ViewModels.LocationViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeoGrafija.Controllers;
using GeoGrafija.Tests.FakeRepositories;
using Model;
using Moq;
using System.Web.Mvc;
using Services;

namespace GeoGrafija.Tests.Controllers
{
    [TestClass]
    public class SearchControllerTests
    {
        SearchController controller;

        [TestInitialize]
        public void Setup()
        {
            controller = new SearchController(new LocationService(new FakeLocationRepository()), 
                                              new UserService(new TestUserRepository()),
                                              new SearchService(new FakeLocationRepository()),
                                              null
                                              );
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(cc => cc.HttpContext.User.Identity.Name).Returns("UserName1");
        }

        [TestMethod]
        public void Index_Returns_View()
        {
            // Act
            var view = controller.Index() as ViewResult;
            
            // Assert
            Assert.IsNotNull(view);
        }

        [TestMethod]
        public void AjaxFindLocations_ReturnsJsonList()
        {
            // Arrange
            int? parentId = 1;
            string name = String.Empty;
            string type = String.Empty;
            
            // Act
            var jsonResult = controller.AjaxFindLocations(parentId, type, name) as JsonResult;

            // Assert
            Assert.IsNotNull(jsonResult);
        }

        [TestMethod]
        public void AjaxFindLocations_Should_Return_ListOf_JsonLocations()
        {
            // Arrange
            int? parentId = 1;
            string name = String.Empty;
            string type = String.Empty;

            // Act
            var jsonResult = controller.AjaxFindLocations(parentId, type, name) as JsonResult;
            
            // Assert
            Assert.IsTrue(jsonResult.Data is List<JsonLocation>);
        }

        [TestMethod]
        public void AjaxFindLocations_Returns_All_Locations_ForOnlyEmpty_ParentId()
        {
            // Arrange
            int? parentId = null;
            string name = String.Empty;
            string type = String.Empty;

            // Act
            var jsonResult = controller.AjaxFindLocations(parentId, type, name) as JsonResult;

            // Assert
            Assert.AreEqual((jsonResult.Data as List<JsonLocation>).Count, 7);
        }

        [TestMethod]
        public void AjaxFindLocations_Returns_SubsetLocations_ForValid_ParentId()
        {
            // Arrange
            int? parentId = 1;
            string name = String.Empty;
            string type = String.Empty;

            // Act
            var jsonResult = controller.AjaxFindLocations(parentId, type, name) as JsonResult;

            // Assert
            Assert.AreEqual((jsonResult.Data as List<JsonLocation>).Count, 3);
        }
    }
}
