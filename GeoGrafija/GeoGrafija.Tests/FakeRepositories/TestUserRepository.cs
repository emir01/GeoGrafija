using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Model.Interfaces;

namespace GeoGrafija.Tests.FakeRepositories
{
    class TestUserRepository : IUserRepository
    {
        private List<User> Users=new List<User>();

        public TestUserRepository()
        {
            for (int i = 1; i <= 15; i++)
            {
                User user = new User() { UserName = "UserName" + i, Password = "Password" + i, Email = "Email" + i + "@test.com", UserID=i };
                string  roleName = i%2==0? "ParnaUloga" : "NeparnaUloga";
                user.Roles.Add(new Role() { RoleName = roleName });
                Users.Add(user);
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            return Users;
        }

        public void AddUser(User user)
        {
            Users.Add(user);
        }

        public void DeleteUser(User user)
        {
            Users.Remove(user);
        }

        public void UpdateUser(User user)
        {
            var tmpUser = (from u in Users
                          where u.UserID == user.UserID
                          select u).Single();
            Users.Remove(tmpUser);
            
            //Do Changes
            tmpUser.UserName = user.UserName;
            tmpUser.Password = user.Password;
            tmpUser.Email = user.Email;
            
            Users.Add(tmpUser);
        }

        public User GetUser(int UserID)
        {
            var userResults = from user in Users
                             where user.UserID == UserID
                             select user;

            if (userResults.Count() > 0) return userResults.Single();
            else
                return null;
        }

        public User GetUser(string UserName)
        {
            var userObject = from user in Users
                             where user.UserName == UserName
                             select user;

            if(userObject.Count() > 0)
            {
                return userObject.Single();
            }
            else
            {
                return null;
            }
        }

        public void SaveChanges() { }
    }
}
