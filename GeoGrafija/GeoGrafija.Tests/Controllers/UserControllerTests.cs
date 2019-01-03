using GeoGrafija.ViewModels.UserViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeoGrafija.Controllers;
using System.Web.Mvc;
using Model;
using GeoGrafija.Tests.FakeServices;
using Moq;
using Services.Interfaces;
using Services.ResultClasses;

namespace GeoGrafija.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        public void LogOnAction_Should_ReturnViewIfNotAuthenticated()
        {
            // Arrange
            var controllerMOQ = new Mock<ControllerContext>();
            controllerMOQ.SetupGet(c=>c.HttpContext.User.Identity.IsAuthenticated).Returns(false);
            var controller = new UserController(null,null);
            controller.ControllerContext = controllerMOQ.Object;

            // Act
            var view = controller.LogOn() as ViewResult;

            // Assert
            Assert.IsNotNull(view,"View Expected");
            Assert.AreEqual(view.ViewName, string.Empty);
        }

        [TestMethod]
        public void LogOnAction_Should_Redirect_IfAuthenticated()
        {
            // Arrange
            var controllerMOQ = new Mock<ControllerContext>();
            controllerMOQ.SetupGet(c => c.HttpContext.User.Identity.IsAuthenticated).Returns(true);
            var controller = new UserController(null,null);
            controller.ControllerContext = controllerMOQ.Object;
            
            // Act
            var action= controller.LogOn() as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(action, "Redirect Expected");
            Assert.AreEqual(action.RouteValues["action"], "Home");
        }
        
        [TestMethod]
        public void LogOnPost_Should_ReturnErrorView_WithInvalidUserAndPass()
        {
            // Arrange
            var service = new AlwaysDenyCredentialsUserService();
            UserController controller = new UserController(service,null);

            // Act
            var view = controller.LogOn(null,null) as ViewResult;

            // Assert
            Assert.IsNotNull(view,"View Expected");
            Assert.IsTrue(view.ViewData.ModelState.Count > 0,"Expecting ModelState Count >0");
        }
        
        [TestMethod]
        public void LogOnPost_Should_RedirectToApropriatePage_On_OKLogin_Withouth_ReturnURL()
        {
            // Arange
            UserController controller = new UserController(new SignIn_Register_UserServiceMock(),null);
            User user = new User()
            {
                UserName = "TestUser",
                Password = "TestPassword"
            };

            // Act
            var result = controller.LogOn(user,null) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result, "Expecter Redirect Result");
            Assert.AreEqual(result.RouteValues["action"], "Home");
        }

        [TestMethod]
        public void LogOnPost_Should_RedirectToApropriatePage_On_OKLogin_With_ReturnURL()
        {
            // Arange
            UserController controller = new UserController(new AlwaysAcceptCredentialsUserService(),null);
            User user = new User()
            {
                UserName = "TestUser",
                Password = "TestPassword"
            };
            
            // Act
            var result = controller.LogOn(user, "~/User/Home") as RedirectResult;

            // Assert
            Assert.IsNotNull(result, "Expecter Redirect Result");
            Assert.AreEqual(result.Url, "~/User/Home");
        }

        [TestMethod]
        public void LogOnPost_ShouldCall_LogIn_FromUserService()
        {
            // Arrange
            IUserService service = new SignIn_Register_UserServiceMock();
            UserController controller = new UserController(new SignIn_Register_UserServiceMock(),null);

            User user = new User()
            {
                UserName = "TestUser", 
                Password = "TestPassword"
            };
            
            // Act
            var result = controller.LogOn(user, null) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(((SignIn_Register_UserServiceMock)controller.UserService).SignInWasCalled);
        }

        [TestMethod]
        public void LogOffAction_ShouldRedirect_To_HomeController_IndexAction()
        {
            // Arrange
            var service = new SignIn_Register_UserServiceMock();
            var controller = new UserController(service,null);
            
            // Act
            var result = controller.LogOff() as RedirectToRouteResult; 

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteValues["controller"], "Home");  
            Assert.AreEqual(result.RouteValues["action"], "Index");
            // LogOff Was Called : 
            Assert.IsTrue(((SignIn_Register_UserServiceMock)controller.UserService).SignOutWasCalled);
        }

        [TestMethod]
        public void HomeAction_ShouldReturn_View()
        {
            // Assert
            UserController controller = new UserController(new AlwaysAcceptCredentialsUserService(),null);

            // Act
            var result = controller.Home() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewName, string.Empty);
        }

        [TestMethod]
        public void RegisterActionGet_Should_ReturnView()
        {
            // Arrange 
            var controller = new UserController(new SignIn_Register_UserServiceMock(),null);    

            // Act
            var result = controller.Register() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewName,string.Empty);
        }

        [TestMethod] 
        public void RegisterPOST_Action_Should_RedirectOnSucess()
        {
            // Arrange 
            var service = new SignIn_Register_UserServiceMock();
            var controller = new UserController(service,null);
            
            var model = new RegisterViewModel(){
                UserName="newUserName",
                Password="password",
                PasswordAgain="password",
                Email="email@com.com"
            };

            // Act
            var result = controller.Register(model) as RedirectToRouteResult;

            // Assert: 
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteValues["controller"], "Home");
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.IsTrue(((SignIn_Register_UserServiceMock)controller.UserService).RegisterWasCalled);
        }

        [TestMethod]
        public void RegisterPOST_Action_Should_RedisplayOnModelInvalid()
        {
            // Arrange 
            var service = new SignIn_Register_UserServiceMock();
            var controller = new UserController(service,null);

            var model = new RegisterViewModel()
            {
                UserName = "newUserName",
                Password = "password",
                PasswordAgain = "password",
                Email = "email@com.com"
            };

            controller.ModelState.AddModelError("No need to test the Model Binder", "Only Test Controller Logic");

            // Act
            var result = controller.Register(model) as ViewResult;

            // Assert: 
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.Count > 0);
        }
        
        [TestMethod]
        public void RegisterPost_Action_ShouldDisplay_ErrorMessage_ForTaken_Username()
        {
            // Arrange
            var model = new RegisterViewModel()
            {
                UserName = "duplicate",
                Password = "password",
                PasswordAgain = "password",
                Email = "email@com.com"
            };

            var service = new SignIn_Register_UserServiceMock();
            var controller = new UserController(service,null);
          
            // Act
            var result = controller.Register(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result,"Expected View as Result");
            Assert.IsTrue(result.ViewData.ModelState.Count > 0,"Expecter Erros in Model");
            string errorEx = result.ViewData.ModelState[""].Errors[0].ErrorMessage;
            Assert.AreEqual(errorEx, RegisterResult.GetErrorMessage(RegisterErrorCodes.UsernameAlreadyTaken),"Expecter Correct Error Message");
        }

        [TestMethod]
        public void RegisterPOST_Should_Call_SignIn_ForValid_Data()
        {
            // Arrange
            var service = new SignIn_Register_UserServiceMock();
            var controller = new UserController(service,null);

            var model = new RegisterViewModel()
            {
                UserName = "newUserName",
                Password = "password",
                PasswordAgain = "password",
                Email = "email@com.com"
            };
            
            // Act
            var result = controller.Register(model)as RedirectToRouteResult;

            // Assert 
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteValues["controller"], "Home");
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.IsTrue(((SignIn_Register_UserServiceMock)controller.UserService).SignInWasCalled);
        }
    }
}
