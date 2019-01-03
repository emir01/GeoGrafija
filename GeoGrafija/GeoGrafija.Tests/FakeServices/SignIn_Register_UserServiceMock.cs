using System;
using System.Collections.Generic;
using GeoGrafija.ResultClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Services.Interfaces;
using Services.ResultClasses;

namespace GeoGrafija.Tests.FakeServices
{
    class SignIn_Register_UserServiceMock : IUserService
    {
        public bool SignInWasCalled { get; set; }
        public bool SignOutWasCalled { get; set; }
        public bool RegisterWasCalled { get; set; }

        public bool CheckCredentials(User User)
        {
            return true;
        }

        public void SignIn(string UserName, bool persistance)
        {
            SignInWasCalled = true;
        }

        public void SignOut()
        {
            SignOutWasCalled = true;
        }
        
        public RegisterResult RegisterUser(User model)
        {
            RegisterWasCalled = true;
         
            if(model.UserName.Equals("duplicate",StringComparison.InvariantCultureIgnoreCase))
            {
                return new RegisterResult(){
                    Status=false,
                    ErrorCodes=new List<RegisterErrorCodes>(){
                        RegisterErrorCodes.UsernameAlreadyTaken
                    }
                };
            }

            Assert.AreEqual("newUserName", model.UserName);
            Assert.AreEqual("password", model.Password);
            Assert.AreEqual("email@com.com", model.Email);

            return new RegisterResult() { Status = true };
        }
        
        public List<User> GetFilteredUsers(string FilterOutRole)
        {
            throw new NotImplementedException();
        }

        public List<User> GetUsersByRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public List<User> GetUsers()
        {
            throw new NotImplementedException();
        }

        public List<User> GetTeacherStudents(string teacherName)
        {
            throw new NotImplementedException();
        }

        public User GetUser(int UserID)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string UserName)
        {
            throw new NotImplementedException();
        }

        public GenericOperationResult<User> SetStudentTeacher(string studentName, int teacherId)
        {
            throw new NotImplementedException();
        }

        public int CalculateStudentRank(string studentName)
        {
            throw new NotImplementedException();
        }

        public int CalculateStudentTotalPoints(string studentName)
        {
            throw new NotImplementedException();
        }

        public int CalculateStudentTotalQuizPoints(string studentName)
        {
            throw new NotImplementedException();
        }

        public string CalculateStudentPercentProgress(string studentName)
        {
            throw new NotImplementedException();
        }

        public GenericOperationResult<StudentQuizResult> GetStudentQuizResult(int resultId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUserViewedLocation(string userName)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUserViewedResource(string userName)
        {
            throw new NotImplementedException();
        }

        public bool GeneralUpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
