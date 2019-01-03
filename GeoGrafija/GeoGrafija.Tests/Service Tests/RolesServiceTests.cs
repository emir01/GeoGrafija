using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using GeoGrafija.Tests.FakeRepositories;
using GeoGrafija.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Model;
using Model.Interfaces;
using Services;
using Services.Interfaces;

namespace GeoGrafija.Tests.Service_Tests
{
    [TestClass]
    public class RolesServiceTests
    {
        [TestMethod]
        public void GetUserRoles_Should_Return_All_User_Roles()
        {
            // Arrange 
            var repository = new TestUserRepository();
            var user = new User() { UserName = "TestUser" };
            user.Roles = new EntityCollection<Role> { new Role(){RoleName="Role1"}, new Role(){RoleName="Role2"}};
            repository.AddUser(user);
            var service = GetRoleService(repository,null);

            // Act 
            var userRoles = service.GetUserRoles("TestUser");

            // Assert
            Assert.IsTrue(userRoles.Length == 2,"Expected to return 2 roles");
            Assert.IsTrue(userRoles[0] == "role1");
            Assert.IsTrue(userRoles[1] == "role2");
        }

        [TestMethod]
        public void UserHasRole_Should_Return_True_IfUserHasRole()
        {
            // Arrange
            var repository = new TestUserRepository();
            var user = new User() { UserName = "TestUser" };
            user.Roles = new EntityCollection<Role> { new Role() { RoleName = "Role1" }, new Role() { RoleName = "Role2" } };
            repository.AddUser(user);
            var service = GetRoleService(repository ,null);

            // Act
            bool userHasRole = service.UserHasRole("TestUser", "Role1");
            
            // Assert 
            Assert.IsTrue(userHasRole,"Expected True.User has Role1");
        }

        [TestMethod]
        public void UserHasRole_Should_Return_False_IfUserDoes_NotHaveRole()
        {
            // Arrange
            var repository = new TestUserRepository();
            var user = new User() { UserName = "TestUser" };
            user.Roles = new EntityCollection<Role> { new Role() { RoleName = "Role1" }, new Role() { RoleName = "Role2" } };
            repository.AddUser(user);
            var service = GetRoleService(repository,null);

            // Act
            bool userHasRole = service.UserHasRole("TestUser", "Admin");

            // Assert 
            Assert.IsFalse(userHasRole,"Expected False. User Does not have Admin Role");
        }

        [TestMethod]
        public void UserHasPrivilige_Should_Return_False_IsUser_DoesNot_Have_Privilige()
        {
            // Arrange
            var repository = new TestUserRepository();
            var user = new User() { UserName = "TestUser" };
            user.Roles = new EntityCollection<Role> { new Role() { RoleName = "Role1", Privileges=new EntityCollection<Privilege>{new Privilege(){PrivilegeName="Privilige1"},new Privilege(){PrivilegeName="Privilige1"}}},
                                                      new Role() { RoleName = "Role2", Privileges=new EntityCollection<Privilege>{new Privilege(){PrivilegeName="Privilige3"}}}};
            repository.AddUser(user);
            var service = GetRoleService(repository,null);
            
            // Act
            bool userHasPrivilige = service.UserHasPrivilige("TestUser", "Privilige4");

            // Assert
            Assert.IsFalse(userHasPrivilige);
        }

        [TestMethod]
        public void UserHasPrivilige_Should_Return_True_IsUser_Doe_Have_Privilige()
        {
            // Arrange
            var repository = new TestUserRepository();
            var user = new User() { UserName = "TestUser" };
            user.Roles = new EntityCollection<Role> { new Role() { RoleName = "Role1", Privileges=new EntityCollection<Privilege>{new Privilege(){PrivilegeName="Privilige1"},new Privilege(){PrivilegeName="Privilige2"}}},
                                                      new Role() { RoleName = "Role2", Privileges=new EntityCollection<Privilege>{new Privilege(){PrivilegeName="Privilige3"}}}};
            repository.AddUser(user);
            var service = GetRoleService(repository,null);
            
            // Act
            bool userHasPrivilige = service.UserHasPrivilige("TestUser", "Privilige2");

            // Assert
            Assert.IsTrue(userHasPrivilige);
        }

        [TestMethod]
        public void AddRole_ShouldAdd_Role_To_Roles_If_NotAdded()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var role = new Role()
            {
                RoleName = "MyRole"
            };

            // Act
            var result = service.AddRole(role);
            var RoleList = service.GetAllRoles();
           
            // Assert
            Assert.IsTrue(result.IsOK);
            Assert.IsTrue(service.GetAllRoles().Contains(role));
        }

        [TestMethod]
        public void AddRole_ShouldReturn_Fail_If_Role_Already_Added()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var role = new Role()
            {
              RoleName = "Admin"
            };
            
            // Act
            var result = service.AddRole(role);
            var RoleList = service.GetAllRoles();

            // Assert
            Assert.IsFalse(RoleList.Contains(role));
            Assert.IsFalse(result.IsOK);
            Assert.AreEqual(result.Messages.Count, 1);
        }

        [TestMethod]
        public void AddPrivilege_ShouldAdd_Privilege_To_Privileges_If_NotAdded()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var privilege= new Privilege()
            {
              PrivilegeName= "MyPriv"
            };

            // Act
            var result = service.AddPrivilege(privilege);
            var PrivilegeList = service.GetAllPrivileges();

            // Assert
            Assert.IsTrue(result.IsOK);
            Assert.IsTrue(PrivilegeList.Contains(privilege));
        }

        [TestMethod]
        public void AddPrivilege_ShouldReturn_Fail_If_Privilege_Already_Added()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var privilege = new Privilege()
            {
               PrivilegeName= "CreateUsers"
            };

            // Act
            var result = service.AddPrivilege(privilege);
            var PrivilegeList= service.GetAllPrivileges();

            // Assert
            Assert.IsFalse(PrivilegeList.Contains(privilege));
            Assert.IsFalse(result.IsOK);
            Assert.AreEqual(result.Messages.Count, 1);
        }


        [TestMethod]
        public void AddPrivilegeToRole_Should_Add_PrivilegeToRole()
        {
            // Arrange
            var service = GetRoleService(null,null);
            var roleName = "Teacher";
            var privilegeName = "CreateUsers";
            var role = service.GetRole(roleName);
            var privilege = service.GetPrivilege(privilegeName);

            // Act
            var result = service.AddPrivilegeToRole(role, privilege);
   
            // Assert
            Assert.IsTrue(role.Privileges.Contains(privilege));
            Assert.IsTrue(result.IsOK);
        }

        [TestMethod]
        public void AddPrivilegeToRole_Should_Fail_If_Role_Already_Has_Priviege()
        {
            // Arrange
            var service = GetRoleService(null,null);
            var roleName = "Admin";
            var privilegeName = "CreateUsers";
            var role = service.GetRole(roleName);
            var privilege = service.GetPrivilege(privilegeName);

            // Act
            var result = service.AddPrivilegeToRole(role, privilege);
   
            // Assert
            Assert.IsTrue(role.Privileges.Contains(privilege));
            Assert.IsFalse(result.IsOK);
            Assert.IsTrue(result.Messages.Count > 0);
        }


        [TestMethod]
        public void AddPrivilegeToRole_Should_Fail_Sending_Null_Objects()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var roleName = "NoRoleLikeThis";
            var privilegeName = "NoPrivilegeLikeThis";
            var role = service.GetRole(roleName);
            var privilege = service.GetPrivilege(privilegeName);

            // Act
            var result = service.AddPrivilegeToRole(role, privilege);

            // Assert
            Assert.IsFalse(result.IsOK);
            Assert.IsTrue(result.ExceptionThrown);
            Assert.IsTrue(result.Messages.Count > 0);
            Assert.IsTrue(result.ExceptionMessages.Count > 0);
        }

        [TestMethod]
        public void SetNewPrivilegesToRole_Should_AddNewSelected_Priviliges_ForRole()
        {
            // Arrange
            var service = GetRoleService(null,null);
            var roleName = "Student";
            var privileges = new List<string>()
            {
                "CreateResource",
                "UseResource",
                "DeleteResource"
            };

            // Act
            var result = service.SetNewPrivilegesToRole(roleName, privileges);

            // Asset
            Assert.IsTrue(result.IsOK);
            Assert.AreEqual(service.GetRole(roleName).Privileges.Count, 3);
            Assert.IsTrue(service.GetRole(roleName).RoleHasPrivilege("CreateResource"));
            Assert.IsTrue(service.GetRole(roleName).RoleHasPrivilege("UseResource"));
            Assert.IsTrue(service.GetRole(roleName).RoleHasPrivilege("DeleteResource"));
        }

        [TestMethod]
        public void SetNewPrivilegesToRole_Should_Remove_DeselectedPrivileges_ForRole()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var roleName = "Student";
            var privileges = new List<string>()
            {
                "CreateResource",
                "DeleteResource"
            };

            //A ct
            var result = service.SetNewPrivilegesToRole(roleName, privileges);

            // Asset
            Assert.IsTrue(result.IsOK);
            Assert.AreEqual(service.GetRole(roleName).Privileges.Count, 2);
            Assert.IsTrue(service.GetRole(roleName).RoleHasPrivilege("CreateResource"));
            Assert.IsTrue(service.GetRole(roleName).RoleHasPrivilege("DeleteResource"));
            Assert.IsFalse(service.GetRole(roleName).RoleHasPrivilege("UseResource"));
        }

        [TestMethod]
        public void SetNewPrivilegesToRole_Should_Ignore_DoublePrivileges_ForRole()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var roleName = "Student";
            var privileges = new List<string>()
            {
                "UseResource",
                "UseResource"
            };

            // Act
            var result = service.SetNewPrivilegesToRole(roleName, privileges);

            // Asset
            Assert.IsTrue(result.IsOK);
            Assert.AreEqual(service.GetRole(roleName).Privileges.Count, 1);
            Assert.IsTrue(service.GetRole(roleName).RoleHasPrivilege("UseResource"));
        }

        [TestMethod]
        public void SetNewPrivilegesToRole_Should_Return_FailFor_NotAddedPrivilege_ForRole()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var roleName = "Student";
            var privileges = new List<string>()
            {
             "BogusPrivilege"
            };

            // Act
            var result = service.SetNewPrivilegesToRole(roleName, privileges);

            // Asset
            Assert.IsFalse(result.IsOK);
        }
        
        [TestMethod]
        public void SetNewPrivilegesToRole_Should_Keep_OldPrivileges_ForFail_ForRole()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var roleName = "Student";
            var privileges = new List<string>()
            {
              "BogusPrivilege"
            };

            // Act
            var result = service.SetNewPrivilegesToRole(roleName, privileges);

            // Asset
            Assert.IsFalse(result.IsOK);
            Assert.AreEqual(service.GetRole(roleName).Privileges.Count, 1);
            Assert.IsTrue(service.GetRole(roleName).RoleHasPrivilege("UseResource"));
        }

        [TestMethod]
        public void AddPrivilegeToRole_Should_AddPrivilegeToRole()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var role = service.GetRole("Student");
            var privilege = service.GetPrivilege("CreateUsers");

            // Act
            var result = service.AddPrivilegeToRole(role, privilege);

            // Assert
            Assert.IsTrue(result.IsOK);
            Assert.IsTrue(service.GetRole("Student").RoleHasPrivilege("CreateUsers"));
            Assert.AreEqual(service.GetRole("Student").Privileges.Count, 2);
        }

        [TestMethod]
        public void AddPrivilegeToRole_Should_ReturnFail_For_DoublePrivilege()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var role = service.GetRole("Student");
            var privilege = service.GetPrivilege("UseResource");

            // Act
            var result = service.AddPrivilegeToRole(role, privilege);

            // Assert
            Assert.IsFalse(result.IsOK);
            Assert.IsTrue(service.GetRole("Student").RoleHasPrivilege("UseResource"));
            Assert.AreEqual(service.GetRole("Student").Privileges.Count, 1);
        }

        [TestMethod]
        public void AddPrivilegeToRole_Should_ReturnFail_For_NullPrivilege()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var role = service.GetRole("Student");
            Privilege privilege = null;

            // Act
            var result = service.AddPrivilegeToRole(role, privilege);

            // Assert
            Assert.IsFalse(result.IsOK);
            Assert.IsTrue(service.GetRole("Student").RoleHasPrivilege("UseResource"));
            Assert.AreEqual(service.GetRole("Student").Privileges.Count, 1);
        }

        [TestMethod]
        public void RemovePrivilegeFromRole_Should_Remove_PrivilegeFromRole()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var role = service.GetRole("Admin");
            var privilege = service.GetPrivilege("CreateUsers");

            // Act 
            var result = service.RemovePrivilegeFromRole(role, privilege);  

            // Assert
            Assert.IsTrue(result.IsOK);
            Assert.IsFalse(service.GetRole("Admin").RoleHasPrivilege("CreateUsers"));
            Assert.AreEqual(service.GetRole("Admin").Privileges.Count, 2);
        }

        [TestMethod]
        public void RemovePrivilegeFromRole_Should_Fail_ForRole_NotHavingPrivilege()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var role = service.GetRole("Admin");
            var privilege = service.GetPrivilege("CreateResource");

            // Act 
            var result = service.RemovePrivilegeFromRole(role, privilege);

            // Assert
            Assert.IsFalse(result.IsOK);
            Assert.IsFalse(service.GetRole("Admin").RoleHasPrivilege("CreateResource"));
            Assert.AreEqual(service.GetRole("Admin").Privileges.Count, 3);
        }


        [TestMethod]
        public void RemoveAllPrivilegesFromRole_Should_Remove_AllPrivileges()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var role = service.GetRole("Admin");
            
            // Act
            var result = service.RemoveAllPrivilegesFromRole(role);

            // Assert        
            Assert.IsTrue(result.IsOK);
            Assert.IsTrue(service.GetRole("Admin").Privileges.Count==0);
        }

        [TestMethod]
        public void RemoveAllPrivilegesFromRole_Should_Fail_ForNullRole()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var role = service.GetRole("BOGUS");

            // Act
            var result = service.RemoveAllPrivilegesFromRole(role);

            // Assert        
            Assert.IsFalse(result.IsOK);
            Assert.IsTrue(result.ExceptionThrown);
        }
        
        [TestMethod]
        public void SetNewRolesToUser_Should_SetNew_Roles_ToUser()
        {
            // Arrange
            var roleService = GetRoleService(null,null);
            var userService = GetUserService(null);
            var user =  userService.GetUser(1);
            
            var listOfRoleNames = new List<string>()
            {
                "Admin",
                "Assistant",
                "NeparnaUloga"
            };

            // Act
            var result = roleService.SetNewRolesToUser(user, listOfRoleNames);

            // Assert
            Assert.IsTrue(result.IsOK);
            Assert.IsTrue(user.UserHasRole("Admin"));
            Assert.IsTrue(user.UserHasRole("Assistant"));
            Assert.IsTrue(user.UserHasRole("NeparnaUloga"));
        }

        [TestMethod]
        public void SetNewRolesToUser_Should_RemoveNotSelected_Roles()
        {
            // Arrange
            var roleService = GetRoleService(null, null);
            var userService = GetUserService(null);
            var user = userService.GetUser(1);
            var listOfRoleNames = new List<string>(){
                "Admin",
                "Assistant",
            };

            // Act
            var result = roleService.SetNewRolesToUser(user, listOfRoleNames);

            // Assert
            Assert.IsTrue(result.IsOK);
            Assert.IsTrue(user.UserHasRole("Admin"));
            Assert.IsTrue(user.UserHasRole("Assistant"));
            Assert.IsFalse(user.UserHasRole("NeparnaUloga"));
        }

        [TestMethod]
        public void SetNewRolesToUser_Should_IgnoreDouble_Roles()
        {
            // Arrange
            var roleService = GetRoleService(null, null);
            var userService = GetUserService(null);
            var user = userService.GetUser(1);
            var listOfRoleNames = new List<string>()
            {
                "Admin",
                "Admin"
            };

            // Act
            var result = roleService.SetNewRolesToUser(user, listOfRoleNames);

            // Assert
            Assert.IsTrue(result.IsOK);
            Assert.IsTrue(user.UserHasRole("Admin"));
            Assert.IsFalse(user.UserHasRole("NeparnaUloga"));
            Assert.AreEqual(user.Roles.Count, 1);
        }

        [TestMethod]
        public void SetNewRolesToUser_Should_Return_FailFor_NotAddedRole_AndKeepOldRoles()
        {
            // Arrange
            var roleService = GetRoleService(null, null);
            var userService = GetUserService(null);
            var user = userService.GetUser(1);
            var listOfRoleNames = new List<string>(){
                "BOGUS",
            };

            // Act
            var result = roleService.SetNewRolesToUser(user, listOfRoleNames);

            // Assert
            Assert.IsFalse(result.IsOK);
            Assert.IsTrue(user.UserHasRole("NeparnaUloga"));
            Assert.AreEqual(user.Roles.Count, 1);
        }


        [TestMethod]
        public void SetNewRolesToUser_Should_Return_FailFor_NullRoleList()
        {
            // Arrange
            var roleService = GetRoleService(null, null);
            var userService = GetUserService(null);
            var user = userService.GetUser(1);
            List<string> listOfRoleNames = null;
       
            // Act
            var result = roleService.SetNewRolesToUser(user, listOfRoleNames);

            // Assert
            Assert.IsFalse(result.IsOK);
            Assert.IsTrue(user.UserHasRole("NeparnaUloga"));
            Assert.AreEqual(user.Roles.Count, 1);
        }

        [TestMethod]
        public void GetRole_Should_Return_Role()
        {
            // Arrange
            var service = GetRoleService(null, null);

            // Act
            var role = service.GetRole("Admin") as Role;

            // Assert
            Assert.IsNotNull(role);
        }

        [TestMethod]
        public void Get_Privilege_Should_Return_Privilege()
        {
            // Arrange
            var service = GetRoleService(null, null);

            // Act
            var privilege = service.GetPrivilege("CreateUsers") as Privilege;

            // Assert
            Assert.IsNotNull(privilege);
        }

        [TestMethod]
        public void GetAllPrivileges_Should_Return_AllPrivileges()
        {
            // Arrange
            var service = GetRoleService(null, null);

            // Act
            var allPrivileges= service.GetAllPrivileges() as IEnumerable<Privilege>;

            // Assert
            Assert.IsNotNull(allPrivileges);
            Assert.AreEqual(allPrivileges.Count(), 6);
        }

        [TestMethod]
        public void GetAllRoles_Should_Return_AllRoles()
        {
            // Arrange
            var service = GetRoleService(null, null);

            // Act
            var allRoles = service.GetAllRoles() as IEnumerable<Role>;

            // Assert
            Assert.IsNotNull(allRoles);
            Assert.AreEqual(allRoles.Count(), 6);
        }
        
        [TestMethod]
        public void GenerateRolesFromStrings_ShouldRetrun_ListOf_Roles_In_OperationResult_Data()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var listOfStringRoles = new List<string>()
            {
                "Admin",
                "Teacher",
                "Student"
            };

            // Act
            var result = service.GenerateRolesFromStrings(listOfStringRoles);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsOK);
            Assert.IsNotNull(result.GetData());
            Assert.IsTrue(result.GetData() is List<Role>);
            Assert.AreEqual(((List<Role>)result.GetData()).Count, 3);
        }


        [TestMethod]
        public void GenerateRolesFromStrings_Should_Fail_On_Incorrect_Role_Name()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var listOfStringRoles = new List<string>()
            {
                "Admin",
                "BOGUS ROLE",
                "Student"
            };

            // Act
            var result = service.GenerateRolesFromStrings(listOfStringRoles);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsOK);
        }

        [TestMethod]
        public void GeneratePrivilegesFromStrings_ShouldRetrun_ListOf_Privileges_In_OperationResult_Data()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var listOfStringRoles = new List<string>()
            {
                "CreateResource",
                "UseResource",
                "ViewLogs"
            };

            // Act
            var result = service.GeneratePrivilegesFromStrings(listOfStringRoles);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsOK);
            Assert.IsNotNull(result.GetData());
            Assert.AreEqual(((List<Privilege>)result.GetData()).Count, 3);
        }
        
        [TestMethod]
        public void GeneratePrivilegesFromStrings_Should_Fail_On_Incorrect_Privilege_Name()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var listOfStringRoles = new List<string>()
            {
                "BogusPrivilege",
                "BOGUS ",
                "Bogus"
            };

            // Act
            var result = service.GenerateRolesFromStrings(listOfStringRoles);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsOK);
        }


        [TestMethod]
        public void ResetUserRoles_Resets_TheRoles_ForTheUSer()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var userService = GetUserService(null);
            var user = userService.GetUser(1);
            var newRoles = new List<Role>()
            {
                service.GetRole("Admin"),
                service.GetRole("Teacher")
            };
            
            // Act
            service.ResetUserRoles(user, newRoles);

            // Assert
            Assert.IsTrue(user.Roles.Count == 2);
            Assert.IsTrue(user.UserHasRole("Admin"));
            Assert.IsTrue(user.UserHasRole("Teacher"));
        }
        
        [TestMethod]
        public void ResetRolePrivileges_Resets_ThePrivileges_ForTheRole()
        {
            // Arrange
            var service = GetRoleService(null, null);
            var role = service.GetRole("Admin");
            var newPrivileges = new List<Privilege>()
            {
                service.GetPrivilege("DeleteResource"),
                service.GetPrivilege("BanUsers"),
                service.GetPrivilege("CreateUsers")
            };
            
            // Act
            service.ResetRolePrivileges(role, newPrivileges);

            // Assert
            Assert.IsTrue(role.Privileges.Count == 3);
            Assert.IsTrue(role.RoleHasPrivilege("DeleteResource"));
            Assert.IsTrue(role.RoleHasPrivilege("BanUsers"));
            Assert.IsTrue(role.RoleHasPrivilege("CreateUsers"));
        }

        private IRolesService GetRoleService(IUserRepository  UserRepository,IRolesRepository  RolesRepository)
        {
           return new RolesService(UserRepository ?? new TestUserRepository(), RolesRepository?? new TestRolesRepository());
        }
        
        private IUserService GetUserService(IUserRepository UserRepository)
        {
           return new UserService(UserRepository??new TestUserRepository());
        }
    }
}
