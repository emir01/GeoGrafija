using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using GeoGrafija.ViewModels;

using GeoGrafija.ViewModels.LocationTypeViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeoGrafija.Controllers;
using System.Web.Mvc;
using Model;
using Moq;
using GeoGrafija.ResultClasses;
using Services;
using Services.Interfaces;

namespace GeoGrafija.Tests.Controllers
{
    [TestClass]
    public class LocationTypesControllerTests
    {
        LocationTypesController controller;

        [TestInitialize]
        public void Setup()
        {
            var userName = "UserName1";
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(cc => cc.HttpContext.User.Identity.Name).Returns(userName);

            controller = new LocationTypesController(new UserService(new FakeRepositories.TestUserRepository()),new LocationService(new FakeRepositories.FakeLocationRepository()));
            controller.ControllerContext = controllerContext.Object;
        }

        [TestMethod]
        public void IndexShould_Return_View_With_Model_List_Of_Types_Created_By_User()
        {
            // Arrange
            var userName = "UserName1";
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(cc => cc.HttpContext.User.Identity.Name).Returns(userName);
    
            controller.ControllerContext = controllerContext.Object;
            
            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            var model = result.Model as IEnumerable<LocationType>;
            Assert.IsNotNull(model);
            Assert.IsTrue(model.Count() == 2);
        }
        
        [TestMethod]
        public void CreateGet_Should_Return_With_AddlocationTypeModel()
        {
            // Act
            var result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Model is AddLocationTypeViewModel);
        }

        [TestMethod]
        public void CreatPost_Should_redirect_if_ModelState_VAlid()
        {
            // Arrange
            var model = GetAddLocatioTypeViewModel();

            // Act
            var result = controller.Create(model) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.RouteValues["action"] == "Details");
            Assert.IsNotNull(result.RouteValues["id"]);
        }

        [TestMethod]
        public void CreatePost_Should_Redisplay_WithErrors_IfModelState_Not_Valid()
        {
            // Arrange
            controller.ModelState.AddModelError("", "TEST");
            var model = GetAddLocatioTypeViewModel();

            // Act
            var result = controller.Create(model) as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(result.ViewData.ModelState.Count > 0);
        }

        [TestMethod]
        public void CreatePost_Should_Call_LocationService_AddLocation()
        {
            // Arrange
            var service = new FakeServices.FakeLocationService();
            controller.LocationService = service;
            var model = GetAddLocatioTypeViewModel();

            // Act
            var result = controller.Create(model) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(service.AddLocationTypeWasCalled);
        }

        [TestMethod]
        public void CreatePost_Should_Pass_Correct_Data_To_Service_Call()
        {
            // Arrange 
            var userName = "UserName1";
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(cc => cc.HttpContext.User.Identity.Name).Returns(userName);

            var service = new FakeServices.FakeLocationService();
            controller.LocationService = service;
            var model = GetAddLocatioTypeViewModel();

            controller.ControllerContext = controllerContext.Object;

            // Act
            var result = controller.Create(model) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(service.passedLocationType);
            Assert.AreEqual(service.passedLocationType.TypeName, model.TypeName);
            Assert.AreEqual(service.passedLocationType.TypeDescription, model.TypeDescription);
            Assert.AreEqual(service.passedLocationType.CreatedBy,1);
        }

        [TestMethod]
        public void CreatePost_Should_Return_View_With_errors_if_Result_fail()
        {
            // Arragne
            var service = new Mock<ILocationService>();
            var message = "Message1";

            service.Setup(l => l.AddLocationType(It.IsAny<LocationType>())).Returns(new OperationResult()
            {
                IsOK = false,
                Messages = new List<string>() { message }
            });

            controller.LocationService = service.Object;
            var model  =GetAddLocatioTypeViewModel();

            // Act
            var result = controller.Create(model) as ViewResult;
            
            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.Count>0);
            Assert.AreEqual(result.ViewData.ModelState[""].Errors[0].ErrorMessage, message);
        }
        
        [TestMethod]
        public void Details_Shuld_Return_View_With_Proper_Display_ViewModel()
        {
            // Act
            var result = controller.Details(1) as  ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Model is DisplayLocationTypeViewModel);
        }

        [TestMethod]
        public void Details_Should_return_apropriate_data()
        {
            // Act
            var result = controller.Details(1) as ViewResult;

            // Assert
            DisplayLocationTypeViewModel model = (DisplayLocationTypeViewModel)result.Model;
            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.AreEqual(model.TypeName,"Continent");
            Assert.AreEqual(model.TypeDescription, "Continent Description");
            Assert.AreEqual(model.Icon, "Continent.png");
            Assert.AreEqual(model.CreatedBy, "UserName1");
            Assert.AreEqual(model.DefaultDisplayName, "Contitent Display");
        }

        [TestMethod]
        public void Details_Should_Redirect_To_index_For_Non_Valid_id()
        {
            // Act
            var result = controller.Details(55) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteValues["action"], "Index");
        }

        [TestMethod]
        public void Edit_Should_REturn_View_WithModel()
        {
            // Act
            var result = controller.Edit(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(result.Model is AddLocationTypeViewModel);
        }


        //TODO: Wrire the rest of the edit tests

        private AddLocationTypeViewModel GetAddLocatioTypeViewModel()
        {
            var ViewModel = new AddLocationTypeViewModel()
            {
                TypeDescription="New Type Description",
                TypeName="New Type Name",
                IconString="newtype.png",
                DefaultDisplaySetting=1
            };
            return ViewModel;
        }
    }
}
