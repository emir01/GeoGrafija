using System;
using System.Text;
using System.Collections.Generic;
using GeoGrafija.ViewModels;
using GeoGrafija.ViewModels.DisplaySettingViewModels;

using GeoGrafija.ViewModels.LocationViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeoGrafija.Controllers;
using Model;
using Moq;
using System.Web.Mvc;
using GeoGrafija.Tests.FakeServices;
using Services;
using Services.Interfaces;


namespace GeoGrafija.Tests.Controllers
{
    [TestClass]
    public class LocationsControllerTests
    {
        private LocationsController controller { get; set; }

        [TestInitialize]
        public void Setup()
        {
            controller =  new LocationsController(new LocationService(new FakeRepositories.FakeLocationRepository()),
                                                 new UserService(new FakeRepositories.TestUserRepository()), 
                                                 new RolesService(new FakeRepositories.TestUserRepository(),new FakeRepositories.TestRolesRepository()));
        }

        [TestMethod]
        public void Index_Should_Return_IdnexView_List_OfLocations_ForUser()
        {
            // Arrange
            var moqControllerContext = new Mock<ControllerContext>();
            moqControllerContext.SetupGet(cc => cc.HttpContext.User.Identity.Name).Returns("UserName1");
            controller.ControllerContext = moqControllerContext.Object;
            
            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ViewData.Model);
            Assert.AreEqual(((List<Location>)result.ViewData.Model).Count, 4);
        }

        [TestMethod]
        public void Index_Should_Return_NullLocations_ForNonValidUser()
        {
            // Arrange
            var moqControllerContext = new Mock<ControllerContext>();
            
            moqControllerContext.SetupGet(cc => cc.HttpContext.User.Identity.Name).Returns("BIGUS");
            controller.ControllerContext = moqControllerContext.Object;
            
            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.ViewData.Model);
        }
        
        [TestMethod]
        public void Index_Should_Return_NullLocations_ForValidUser_Without_Locations()
        {
            // Arrange
            var moqControllerContext = new Mock<ControllerContext>();
            moqControllerContext.SetupGet(cc => cc.HttpContext.User.Identity.Name).Returns("UserName5");
            controller.ControllerContext = moqControllerContext.Object;
            
            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.ViewData.Model);
            Assert.IsTrue(result.ViewData.ModelState.Count > 0);
        }

        [TestMethod]
        public void CreateGET_Should_Return_View_With_AddLocationViewMode()
        {
            // Act
            var result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(result.Model is LocationFormViewModel);
        }

        [TestMethod]
        public void CreateGET_Should_Return_View_With_Non_Empty_LocationTypeList()
        {
            // Arrange
            var result = controller.Create() as ViewResult;

            // Act
            Assert.IsNotNull(result);
            var model = (LocationFormViewModel)result.Model;
            Assert.IsNotNull(model);
            Assert.IsNotNull(model.LocationTypes);
            Assert.IsTrue(model.LocationTypes.Count > 0);
            Assert.IsTrue(model.LocationTypes[0] is LocationType);
        }

        [TestMethod]
        public void CreateGET_Should_Return_View_With_Non_Empty_LocationDisplaySettings()
        {
            // Arrange
            var result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = (LocationFormViewModel)result.Model;
            Assert.IsNotNull(model);
            Assert.IsNotNull(model.DisplaySettings);
            Assert.IsTrue(model.DisplaySettings.Count > 0);
            Assert.IsTrue(model.DisplaySettings[0] is LocationDisplaySetting);
        }

        [TestMethod]
        public void CreatePost_Should_Redirect_To_Details_For_Ok_Create()
        {
            // Arrange
            var model = getViewModel();
            
            var userService = new Mock<IUserService>();
            userService.Setup(us => us.GetUser(It.IsAny<string>())).Returns(new User()
            {
                UserID = 1
            });
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(cc => cc.HttpContext.User.Identity.Name).Returns("UserName1");

            controller.UserService = userService.Object;
            controller.ControllerContext = controllerContext.Object;
            
            // Act
            var result = controller.Create(model) as RedirectToRouteResult;
            
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreatePost_Should_Redisplay_View_For_bogus_data()
        {
            // Arrange
            var model = getViewModel();
            model.Description = null;
            controller.ModelState.AddModelError("", "Error1");

            // Act
            var result = controller.Create(model) as ViewResult;
            
            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.Count > 0);
        }

        [TestMethod]
        public void CreatePost_Should_Redisplay_View_For_bogus_data_And_Values_Should_BeFine()
        {
            // Arrange
            var model = getViewModel();
            model.Description = null;
            controller.ModelState.AddModelError("", "Error1");
            
            // Act
            var result = controller.Create(model) as ViewResult;
            
            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.Count > 0);
            var modelReturned = (LocationFormViewModel)result.Model;
            Assert.IsNotNull(modelReturned.LocationTypes);
            Assert.IsNotNull(modelReturned.DisplaySettings);
            Assert.IsTrue(modelReturned.LocationTypes.Count > 0);
            Assert.IsTrue(modelReturned.DisplaySettings.Count > 0);
        }
        
        [TestMethod]
        public void CraetePOST_Should_Call_ServiceLayer_IfModelStateValid()
        {
            //arrange
            FakeLocationService service = new FakeLocationService();
            controller.LocationService = service;
            var model = getViewModel();
            
            var userService = new Mock<IUserService>();
            userService.Setup(us => us.GetUser(It.IsAny<string>())).Returns(new User()
            {
                UserID = 1
            });

            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(cc => cc.HttpContext.User.Identity.Name).Returns("UserName1");

            controller.UserService = userService.Object;
            controller.ControllerContext = controllerContext.Object;
            
            // Act
            var result = controller.Create(model) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.RouteValues["Action"].Equals("Details"));
            Assert.IsTrue(((FakeLocationService)controller.LocationService).addLocationWasCalled);
        }

        [TestMethod]
        public void CreatePOST_Should_Pass_Apropriate_Model_ToServiceLayer()
        {
            // Arrange
            FakeLocationService service = new FakeLocationService();
            controller.LocationService = service;
            var model = getViewModel();
            
            var userService = new Mock<IUserService>();
            userService.Setup(us => us.GetUser(It.IsAny<string>())).Returns(new User()
            {
                UserID = 1
            });

            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(cc => cc.HttpContext.User.Identity.Name).Returns("UserName1");

            controller.UserService = userService.Object;
            controller.ControllerContext = controllerContext.Object;
            
            // Act
            var result = controller.Create(model) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.RouteValues["Action"].Equals("Details"));
            Assert.IsTrue(((FakeLocationService)controller.LocationService).addLocationWasCalled);
            var location = ((FakeLocationService)controller.LocationService).passedParameter as Location;
            Assert.IsNotNull(location);
            Assert.IsTrue(location.CreatedBy==1);
        }

        [TestMethod]
        public void CreatePOST_Should_REdisplay_With_Errors_For_FAil_Result()
        {
            // Arrange
            var moqLocationService = new Mock<ILocationService>();
            var moqUserService = new Mock<IUserService>();

            var userService = new Mock<IUserService>();

            userService.Setup(us => us.GetUser(It.IsAny<string>())).Returns(new User()
            {
                UserID = 1
            });

            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(cc => cc.HttpContext.User.Identity.Name).Returns("UserName1");

            moqLocationService.Setup(ls => ls.AddLocation(It.IsAny<Location>())).Returns(new ResultClasses.OperationResult()
            {
                IsOK = false,
                Messages = new List<string>() { "FailMessage" }
            });

            controller.LocationService = moqLocationService.Object;
            controller.UserService = userService.Object;
            controller.ControllerContext = controllerContext.Object;
            
            // Act
            var result = controller.Create(getViewModel()) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.Count > 0);
            Assert.AreEqual(result.ViewData.ModelState[""].Errors[0].ErrorMessage, "FailMessage");
        }

        [TestMethod]
        public void CreatePOST_Should_REdisplay_With_Errors_For_FAil_Result_Exceptions()
        {
            //Arrange
            var moqLocationService = new Mock<ILocationService>();
            var moqUserService = new Mock<IUserService>();
            
            var userService = new Mock<IUserService>();
            userService.Setup(us => us.GetUser(It.IsAny<string>())).Returns(new User()
            {
                UserID = 1
            });

            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(cc => cc.HttpContext.User.Identity.Name).Returns("UserName1");

            moqLocationService.Setup(ls => ls.AddLocation(It.IsAny<Location>())).Returns(new ResultClasses.OperationResult()
            {
                IsOK = false,
                ExceptionThrown=true,
                ExceptionMessages=new List<string>(){"ExceptionMessage"}
            });

            controller.LocationService = moqLocationService.Object;
            controller.UserService = userService.Object;
            controller.ControllerContext = controllerContext.Object;

            // Act
            var result = controller.Create(getViewModel()) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.Count > 0);
            Assert.AreEqual(result.ViewData.ModelState[""].Errors[0].ErrorMessage, "ExceptionMessage");
        }

        [TestMethod]
        public void CreatePost_Should_Call_Service_Layer_ForNew_DisplaySetting_if_chosen_to_createNewDisplaySetting()
        {
            // Arrange
            FakeLocationService service = new FakeLocationService();
            controller.LocationService = service;
            var model = getViewModel();
            model.CurrentDisplaySetting = true;
            model.Zoom = "5";
            model.MapType = "ROADMAP";
            model.DisplayName = "New Display";
            
            var userService = new Mock<IUserService>();
            userService.Setup(us => us.GetUser(It.IsAny<string>())).Returns(new User()
            {
                UserID = 1
            });
            
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(cc => cc.HttpContext.User.Identity.Name).Returns("UserName1");

            controller.UserService = userService.Object;
            controller.ControllerContext = controllerContext.Object;

            // Act
            var result = controller.Create(model) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.RouteValues["Action"].Equals("Details"));
            Assert.IsTrue(((FakeLocationService)controller.LocationService).addNewDisplaySetingWasCalled);
            Assert.IsTrue(((FakeLocationService)controller.LocationService).CalledFirst=="display");
            var displaySetting = ((FakeLocationService)controller.LocationService).passedDisplaySetting as LocationDisplaySetting;
            Assert.IsNotNull(displaySetting);
            Assert.IsTrue(displaySetting.Zoom == "5");
        }

        [TestMethod]
        public void CreatePost_Should_Return_Error_If_No_Name_Is_Provided()
        {
            // Arrange
            FakeLocationService service = new FakeLocationService();
            controller.LocationService = service;
            var model = getViewModel();
            model.CurrentDisplaySetting = true;
            model.Zoom = "5";
            model.MapType = "ROADMAP";
            
            var userService = new Mock<IUserService>();
            userService.Setup(us => us.GetUser(It.IsAny<string>())).Returns(new User()
            {
                UserID = 1
            });
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(cc => cc.HttpContext.User.Identity.Name).Returns("UserName1");

            controller.UserService = userService.Object;
            controller.ControllerContext = controllerContext.Object;

            // Act
            var result = controller.Create(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.Count > 0);
            Assert.IsFalse(((FakeLocationService)controller.LocationService).addNewDisplaySetingWasCalled);
        }

        [TestMethod]
        public void CreatePost_Should_Return_Errors_If_Result_FromAdding_Display_Fails()
        {
            // Arrange
            var service = new Mock<ILocationService>();
            service.Setup(sr => sr.AddLocationDisplaySetting(It.IsAny<LocationDisplaySetting>())).Returns(new ResultClasses.OperationResult()
            {
                IsOK = false,
                Messages = new List<string>() {"Message1"}

            });

            controller.LocationService = service.Object;
            var model = getViewModel();
            model.CurrentDisplaySetting = true;
            model.DisplayName = "New DisplayName";
            model.Zoom = "5";
            model.MapType = "ROADMAP";

            var userService = new Mock<IUserService>();
            userService.Setup(us => us.GetUser(It.IsAny<string>())).Returns(new User()
            {
                UserID = 1
            });

            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(cc => cc.HttpContext.User.Identity.Name).Returns("UserName1");
            controller.UserService = userService.Object;
            controller.ControllerContext = controllerContext.Object;

            // Act
            var result = controller.Create(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.Count > 0);
            Assert.IsTrue(result.ViewData.ModelState[""].Errors[0].ErrorMessage=="Message1");
        }
        
        [TestMethod]
        public void LocationsDetails_Should_ReturnView_WithProperModel()
        {
            // Act 
            var result = controller.Details(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as DisplayLocationViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(model.Name, "Location1");
        }

        [TestMethod]
        public void LocationDetails_Should_RedirectTo_List_For_Invalid_ID()
        {
            // Act
            var result = controller.Details(32) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteValues["action"], "Index");
        }

        [TestMethod]
        public void LocationDisplaySettingsAjax_Should_Return_JsonResult()
        {
            // Act
            var result = controller.LocationDisplaySettingsAjax(1) as JsonResult;

            // Assert
            var data = result.Data as JsonDisplaySetting;
            Assert.IsNotNull(result);
            Assert.IsTrue(data.Result);
        }

        [TestMethod]
        public void LocationDisplaySettingAjax_Should_Return_Fail_Result_For_NonValid_id()
        {
            // Act
            var result = controller.LocationDisplaySettingsAjax(88) as JsonResult;

            // Assert 
            var data = result.Data as JsonDisplaySetting;
            Assert.IsNotNull(result);
            Assert.IsFalse(data.Result);
        }

        [TestMethod]
        public void Edit_Should_Return_View_With_ApropriateModel()
        {
            // Act
            var result = controller.Edit(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model;
            Assert.IsNotNull(model);
            Assert.IsTrue(model is LocationFormViewModel);
        }
        
        private LocationFormViewModel getViewModel()
        {
            var viewmodel = new LocationFormViewModel();

            viewmodel.Lat = 22.33f;
            viewmodel.Lng = 55.55f;
            
            viewmodel.Name = "Location Name";
            viewmodel.Description = "Location Description";

            viewmodel.ChosenType = 2;
            viewmodel.ChosenDisplaySetting = 1;
            viewmodel.CurrentDisplaySetting = false;
            
            return viewmodel;
        }
    }
}
