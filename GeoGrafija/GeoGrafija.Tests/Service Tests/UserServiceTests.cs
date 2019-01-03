using System;
using GeoGrafija.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;


using GeoGrafija.Tests.FakeRepositories;
using Model;
using Services;
using Services.ResultClasses;

namespace GeoGrafija.Tests.TestServices
{
    [TestClass]
    public class UserServiceTests
    {

        [TestMethod]
        public void CheckCredentials_Should_Return_True_If_FoundUser()
        {
            //Arrange
            UserService service = new UserService(new FakeRepositories.TestUserRepository(), null);
            User user = new User() { UserName = "UserName1", Password = "Password1" };

            //Act
            bool foundUser = service.CheckCredentials(user);

            //Assert
            Assert.IsTrue(foundUser);

        }

        [TestMethod]
        public void CheckCredentials_Should_Return_False_If_NotFoundUser()
        {
            //Arrange
            UserService service = new UserService(new FakeRepositories.TestUserRepository(),null);
            User user = new User() { UserName = "BOGUS", Password = "Bogus" };

            //Act
            bool foundUser = service.CheckCredentials(user);

            //Assert
            Assert.IsFalse(foundUser);

        }

        [TestMethod]
        public void CheckCredentials_Should_Return_False_If_FoundUser_FalsePassword()
        {
            //Arrange
            UserService service = new UserService(new FakeRepositories.TestUserRepository(),null);
            User user = new User() { UserName = "UserName1", Password = "Bogus" };

            //Act
            bool foundUser = service.CheckCredentials(user);

            //Assert
            Assert.IsFalse(foundUser);

        }

        [TestMethod]
        public void LogIn_ThrowsException_With_EmptyUserName()
        {
            //Arrange
            UserService service = new UserService(new TestUserRepository(), null);
            User user = new User() { UserName = "", Password = "Bogus" };
            try
            {
                //Act
                service.SignIn(user.UserName, false);

                //Assert
                Assert.Fail();

            }
            catch (Exception ex) { }


        }
        [TestMethod]
        public void LogIn_ThrowsException_With_NullUserName()
        {
            //Arrange
            UserService service = new UserService(new TestUserRepository());
            try
            {
                //Act
                service.SignIn(null, false);

                //Assert
                Assert.Fail();

            }
            catch (Exception ex) { }

        }

        [TestMethod]
        public void Register_Should_Add_User_To_Repository_And_Return_Success()
        {
            //Arrange
            var repository = new TestUserRepository();
            var service = new UserService(repository, null);

            var model = new User()
            {
                UserName = "NewUserName",
                Password = "password",
                
                Email = "email@email.com"
            };
            //Act

            var result = service.RegisterUser(model);
            var registeredUser = repository.GetUser("NewUserName");

            //Assert
            Assert.IsTrue(result.Status);
            Assert.IsTrue(result.ErrorCodes.Count == 0);
            Assert.IsNotNull(registeredUser);

        }


        [TestMethod]
        public void Register_Should_Return_ErrorCodes_For_Duplicate_UserName()
        {

            //Arrange
            var repository = new TestUserRepository();
            var service = new UserService(repository);

            var model = new User()
            {
                UserName = "UserName1",
                Password = "password",
                
                Email = "email@email.com"
            };
            
            //Act
            var result = service.RegisterUser(model);
            
            //Assert
            Assert.IsFalse(result.Status);
            Assert.IsTrue(result.ErrorCodes.Count == 1);
            Assert.AreEqual((RegisterResult.GetErrorMessage(result.ErrorCodes[0])),RegisterResult.GetErrorMessage(RegisterErrorCodes.UsernameAlreadyTaken));
            
        }

        
        [TestMethod]
        public void Register_Should_Return_ErrorCodes_For_Duplicate_Email()
        {

            //Arrange
            var repository = new TestUserRepository();
            var service = new UserService(repository);

            var model = new User()
            {
                UserName = "newUserNAme",
                Password = "password",
                
                Email = "Email1@test.com"
            };
            
            //Act
            var result = service.RegisterUser(model);
            
            //Assert
            Assert.IsFalse(result.Status);
            Assert.IsTrue(result.ErrorCodes.Count == 1);
            Assert.AreEqual((RegisterResult.GetErrorMessage(result.ErrorCodes[0])),RegisterResult.GetErrorMessage(RegisterErrorCodes.EmailAlreadyTaken));
            
        }

        [TestMethod]
        public void GetFilteredUsers_Should_Return_Users_Without_SuppliedRole()
        {
            //arrange
            var service = new UserService(new TestUserRepository());

            //act
            var allUsers = service.GetFilteredUsers("ParnaUloga");

            //assert
            foreach (var user in allUsers)
            {
                Assert.IsFalse(user.UserHasRole("ParnaUloga"));
            }
        }


    }


}
