using System;
using System.Linq;
using GeoGrafija.Tests.FakeRepositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Model.Interfaces;

namespace GeoGrafija.Tests.FakeModelTests
{
    [TestClass]
    public class UserRepositoryTests
    {
        [TestMethod]
        public void UserRepository_Should_Return_Users()
        {
            // Arange
            IUserRepository repository = new TestUserRepository();

            // Act
            var users = repository.GetAllUsers();

            // Assert
            Assert.IsNotNull(users);
        }

        [TestMethod]
        public void TestUserRepository_Should_Return_15Users()
        {
            // Arange
            IUserRepository repository = new TestUserRepository();

            // Act
            var users = repository.GetAllUsers();

            // Assert
            Assert.AreEqual(15,users.Count());
        }

        [TestMethod]
        public void GetUser_ShouldReturn_UserObject_ForTrue_UserName()
        {
            // Arrange
            IUserRepository repository = new TestUserRepository();

            // Act
            User user = repository.GetUser("UserName1");

            // Assert 
            Assert.IsNotNull(user);
            Assert.AreEqual("UserName1", user.UserName);
        }

        [TestMethod]
        public void GetUser_ShouldReturn_Null_ForFalse_UserName()
        {
            // Arrange
            IUserRepository repository = new TestUserRepository();

            // Act
            User user = repository.GetUser("BOGUS");

            // Assert 
            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserWithID_Should_Return_Null_For_FalseID()
        {
            // Arrange
            var repository = new TestUserRepository();
            
            // Act
            var user = repository.GetUser(20);

            // Assert
            Assert.IsNull(user);
        }
        
        [TestMethod]
        public void GetUserWithID_Should_Return_User_For_TrueID()
        {
            // Arrange
            var repository = new TestUserRepository();

            // Act
            var user = repository.GetUser(1);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(user.UserName, "UserName1");
            Assert.AreEqual(user.Password, "Password1");
        }
        
        [TestMethod]
        public void AddUser_Should_AddUserToData()
        {
            // Arrange
            var repository = new TestUserRepository();
            var user = new User()
            {
                UserID = 20,
                UserName = "TestUserName",
                Password = "TestPassword",
                Email = "TestEmail"
            };

            // Act   
            repository.AddUser(user);
            var theNewUserViaID = repository.GetUser(20);
            var theNewUserViaUserName = repository.GetUser("TestUserName");
            
            //Assert
            Assert.AreEqual(repository.GetAllUsers().Count(), 16);
            Assert.IsNotNull(theNewUserViaID);
            Assert.IsNotNull(theNewUserViaUserName);
        }

        [TestMethod]
        public void DeteUser_Should_Delete_ApropriateUser()
        {
            // Arrange 
            var repository = new TestUserRepository();
            var userToDelete = repository.GetUser(1);
            
            // Act
            repository.DeleteUser(userToDelete);
            var userDeleted = repository.GetUser(1);
            
            // Assert
            Assert.AreEqual(repository.GetAllUsers().Count(), 14);
            Assert.IsNull(userDeleted);
        }

        [TestMethod]
        public void DeteUser_Should_ThrowExceptionFor_InvalidUser()
        {
            // Arrange 
            var repository = new TestUserRepository();
            var userToDelete = new User() { UserID = 22 };

            // Act
            try
            {
                repository.DeleteUser(userToDelete);
                Assert.Fail();
            }
            catch (Exception ex) {}
        }

        [TestMethod]
        public void UpdateUser_Should_Change_Common_UserData()
        {
            // Arrange 
            var repository = new TestUserRepository();
            var user = repository.GetUser(1);
            //Change the data
            user.UserName = "NewUserName";
            user.Password = "NewPassword";
            user.Email = "NewEmail@email.com";
            
            //Act
            repository.UpdateUser(user);
            var updatedUser = repository.GetUser(1);
            
            //Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(user.UserName, "NewUserName");
            Assert.AreEqual(user.Password, "NewPassword");
            Assert.AreEqual(user.Email, "NewEmail@email.com");
        }
    }
} 