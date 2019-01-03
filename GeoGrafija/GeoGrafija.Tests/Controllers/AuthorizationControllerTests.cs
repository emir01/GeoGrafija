using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GeoGrafija.Controllers;



using GeoGrafija.Tests.FakeRepositories;

using GeoGrafija.ViewModels.UserViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Moq;
using System.Collections;

using GeoGrafija.ResultClasses;
using Services;
using Services.Interfaces;


namespace GeoGrafija.Tests.Controllers
{
    [TestClass]
    public class AuthorizationControllerTests
    {
        #region Role Tests
        [TestMethod]
        public void Index_Should_ReturnView()
        {
            // Arrange
            var controller = GetMockRepositoryController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewName, String.Empty);
        }

        [TestMethod]
        public void RolesGet_Should_Return_List_Of_AllRoles()
        {
            // Arrange
            var controller = GetMockRepositoryController();

            // Act
            var result = controller.Roles() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ViewData.Model);
            Assert.IsTrue(result.ViewData.Model is List<Role>);    
        }
        
        [TestMethod]
        public void CreateRole_GET_Should_ReturnView()
        {
            //Arrange
            var controller = GetMockRepositoryController();

            //Act
            var result = controller.CreateRole() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewName, String.Empty);
            Assert.IsTrue(result.ViewData.Model is Role);
        }

        [TestMethod]
        public void CreateRole_POST_Should_Return_View_WithErrors_ForInvalid_ModelState()
        {
            // Arrange
            var controller = GetMockRepositoryController();
            var newRole = new Role()
            {
                RoleName = "",
                RoleDescription="NewRole Description"
            };

            controller.ViewData.ModelState.AddModelError("", "TestModelError");

            //Act
            var result = controller.CreateRole(newRole) as ViewResult;
            
            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.Count > 0);
        }

        [TestMethod]
        public void CreateRole_POST_ShouldRedirectTo_RoleDetails_On_OK_Create()
        {
            // Arrange
            var controller = GetMockRepositoryController();
            var role = new Role()
            {
                RoleName="MyNewRole",
                RoleDescription = "MyNew RoleDescription"
            };
            
            // Act 
            var result = controller.CreateRole(role) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteValues["action"], "RoleDetails");
            Assert.AreEqual(result.RouteValues["RoleName"], "MyNewRole");
        }

        [TestMethod]
        public void CreateRole_POST_ShouldReturnError_On_FailCreateCall_ToService()
        {
            // Arrange
            var controller = GetMockRepositoryController();
            var mockRoleService = new Mock<IRolesService>();

            var role = new Role()
            {
                RoleName = "MyNewRole",
                RoleDescription = "MyNew RoleDescription"
            };
            var operationResult = new OperationResult();
            operationResult.SetFail();
            operationResult.AddMessage("ErrorMessage");

            mockRoleService.Setup(ms => ms.AddRole(role)).Returns(operationResult);
            controller.RolesService = mockRoleService.Object;
            
            // Act 
            var result = controller.CreateRole(role) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.Count > 0);
            Assert.AreEqual(result.ViewData.ModelState[""].Errors[0].ErrorMessage, "ErrorMessage");
        }

        [TestMethod]
        public void RoleDetails_Should_Return_View()
        {
            // Arrange
            var controller = GetMockRepositoryController();
            var roleName = "Admin";
            
            // Act
            var result = controller.RoleDetails(roleName) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewName, String.Empty);
            Assert.IsTrue(result.Model is Role);
        }
        
        #endregion

        #region Privilege Tests

        [TestMethod]
        public void PrivilegesGet_Should_Return_List_Of_AllPrivileges()
        {
            // Arrange
            var controller = GetMockRepositoryController();

            // Act
            var result = controller.Privileges() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ViewData.Model);
            Assert.IsTrue(result.ViewData.Model is List<Privilege>);
        }

        [TestMethod]
        public void PrivilegeCreateGet_Should_Return_View()
        {
            // Arrange
            var controller = GetMockRepositoryController();

            // Act
            var result = controller.CreatePrivilege() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewName, String.Empty); 
        }

        [TestMethod]
        public  void PrivilegeCreatePOST_Should_ReturnView_For_Invalid_ModelState()
        {
            // Arrange
            var controller = GetMockRepositoryController();
            var privilege = new Privilege()
            {
                PrivilegeName="MyPrivilege",
                PrivilegeDescription="MyPrivilege Description"
            
            };

            // Act
            controller.ModelState.AddModelError("","CrazyModelError");
            var result = controller.CreatePrivilege(privilege) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.Count > 0);
            Assert.AreEqual(result.ViewData.ModelState[""].Errors[0].ErrorMessage, "CrazyModelError");
        }

        [TestMethod]
        public void PrivilegeCreatePOST_Should_Redirect_To_Details_After_Ok_Create()
        {
            // Arrange
            var controller = GetMockRepositoryController();
            var privilege = new Privilege()
            {
                PrivilegeName="MyPrivilege",
                PrivilegeDescription="MyPrivilege Description"
            };

            // Act
            var result = controller.CreatePrivilege(privilege) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteValues["action"], "PrivilegeDetails");
            Assert.AreEqual(result.RouteValues["PrivilegeName"], "MyPrivilege");

        }

        [TestMethod]
        public void PrivilegeCreatePOST_Should_Redisplay_With_Errors_After_Fail_ServiceCall()
        {
            // Arrange
            var controller = GetMockRepositoryController();
            var serviceMock = new Mock<IRolesService>();

            var privilege = new Privilege()
            {
                PrivilegeName = "MyPrivilege",
                PrivilegeDescription = "MyPrivilege Description"
            };
            
            var mockedResult = new OperationResult();
            mockedResult.SetFail();
            var errorMessage = "ErrorMessage";
            mockedResult.AddMessage(errorMessage);

            serviceMock.Setup(sm => sm.AddPrivilege(privilege)).Returns(mockedResult);
            controller.RolesService = serviceMock.Object;

            // Act
            var result = controller.CreatePrivilege(privilege) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.Count > 0);
            Assert.AreEqual(errorMessage, result.ViewData.ModelState[""].Errors[0].ErrorMessage);
        }
        
        [TestMethod]
        public  void PrivilegeDetails_Should_Return_View()
        {
            // Arrange
            var controller = GetMockRepositoryController();
            var privilege = new Privilege(){
                PrivilegeName = "CreateUsers",
                PrivilegeDescription="My Privilege Description"
            };

            // Act
            var result = controller.PrivilegeDetails(privilege.PrivilegeName)as ViewResult;
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewName,String.Empty);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(result.Model is Privilege);
        }

        #endregion

        #region RolePrivilegeTests

        [TestMethod]
        public void AddPrivilegeToRoleGET_Should_Return_View()
        {
            // Arrange
            var controller = GetMockRepositoryController();
            var role = new Role()
            {
                RoleName="Admin",
                RoleDescription="AdminDescription"
            };
            
            // Act
            var result = controller.AddPrivilegeToRole(role.RoleName) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewName, String.Empty);
        }

        [TestMethod]
        public void AddPrivilegeToRole_POST_Should_Return_Model_Errors_ON_FAIL()
        {
            // Arrange
            var controller = GetMockRepositoryController();
            var roleService = new Mock<IRolesService>();

            roleService.Setup(rs => rs.SetNewPrivilegesToRole(It.IsAny<string>(), It.IsAny<List<string>>())).Returns(new OperationResult()
            {
                IsOK = false,
                Messages = new List<string>()
                {
                    "ModelMessage1"
                }
            });

            controller.RolesService = roleService.Object;
            var formCollection = new FormCollection()
            {
                {"privileges","Privilege1"},
                {"privileges","Privilege2"}
            };
            
            // Act
            var result = controller.AddPrivilegeToRole("Role1",formCollection) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewName, String.Empty);
            Assert.IsTrue(result.ViewData.ModelState.Count>0);
            Assert.AreEqual(result.ViewData.ModelState[""].Errors[0].ErrorMessage,"ModelMessage1");
        }


        [TestMethod]
        public void AddPrivilegeToRole_POST_Should_Return_Redirect_ON_Success()
        {
            // Arrange
            var controller = GetMockRepositoryController();
            var roleService = new Mock<IRolesService>();

            roleService.Setup(rs => rs.SetNewPrivilegesToRole(It.IsAny<string>(), It.IsAny<List<string>>())).Returns(new OperationResult()
            {
                IsOK = true,
               
            });

            controller.RolesService = roleService.Object;
            var formCollection = new FormCollection()
            {
                {"privileges","Privilege1"},
                {"privileges","Privilege2"}
            };

            // Act
            var result = controller.AddPrivilegeToRole("Role1", formCollection) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteValues["action"], "RoleDetails");
            Assert.AreEqual(result.RouteValues["RoleName"], "Role1");
        }
        
        #endregion 

        #region User Tests

        [TestMethod]
        public void UsersGET_Should_Return_Users()
        {
            // Arrange
            var controller = GetMockRepositoryController();

            // Act
            var result = controller.Users() as ViewResult;
            
            //Assert
            Assert.IsNotNull(controller);
            Assert.AreEqual(result.ViewName, string.Empty);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(result.Model is IEnumerable);
        }

        [TestMethod]
        public void UserDetailsGET_Should_Return_Details()
        {
            // Arrange
            var controller = GetMockRepositoryController();
            var userID = 1;
            
            // Act
            var result = controller.UserDetails(userID) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(result.Model is User);
        }
        
        [TestMethod]
        public void AddRoleToUser_Should_ReturnView_With_AddRolesToUser_ViewModel()
        {
            // Arrange
            var controller = GetMockRepositoryController();
            var userID = 1;

            // Act
            var result = controller.AddRolesToUser(userID) as ViewResult;
            
            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(result.Model is AddRolesToUserViewModel);
        }

        [TestMethod]
        public void AddRoleToUserPOST_Should_Return_ModelError_Fail()
        {
            // Arrange   
            var controller = GetMockRepositoryController();
            var mockRoleService = new Mock<IRolesService>();
            
            var formCollection = new FormCollection(){
                    {"roles","Role1,Role2"}
            };
            
            mockRoleService.Setup(rs => rs.SetNewRolesToUser(It.IsAny<User>(), It.IsAny<List<string>>())).Returns(new OperationResult()
            {
                IsOK = false,
                Messages = new List<string>() { "Message1" }
            });
            
            controller.RolesService = mockRoleService.Object;

            // Act
            
            var result = controller.AddRolesToUser(1 ,formCollection) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewData.ModelState.Count > 0);
            Assert.IsTrue(result.ViewData.ModelState[""].Errors[0].ErrorMessage == "Message1");

        }

        [TestMethod]
        public void AddRoleToUserPOST_Should_Redirect_OnSuccess()
        {
            // Arrange 
            var controller = GetMockRepositoryController();
            var mockRoleService = new Mock<IRolesService>();

            var formCollection = new FormCollection(){
                {"roles","Role1,Role2"}
            };

            mockRoleService.Setup(rs => rs.SetNewRolesToUser(It.IsAny<User>(), It.IsAny<List<string>>())).Returns(new OperationResult()
            {
                IsOK=true
            });

            controller.RolesService = mockRoleService.Object;

            //Act
            var result = controller.AddRolesToUser(1, formCollection) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteValues["action"], "UserDetails");
        }
        
        #endregion

        #region GetCalls

        private AuthorizationController GetMockRepositoryController()
        {
            var userRepository = new TestUserRepository();
            var rolesRepository = new TestRolesRepository();

            var userService = new UserService(userRepository);
            var rolesService = new RolesService(userRepository, rolesRepository);
            
            var controller = new AuthorizationController(userService,rolesService);

            return controller;
        }

        #endregion
    }
}
