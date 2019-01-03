using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using Model.Interfaces;

namespace Model.Repositories
{
    public class UserRepository: IUserRepository 
    {
        private GeoGrafijaEntities context;
        
        public UserRepository(IDbContext context)
        {
            this.context = (GeoGrafijaEntities)context;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return context.Users;
        }

        public void AddUser(User user)
        {
            context.Users.AddObject(user);
            context.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            context.Users.DeleteObject(user);
            context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            if (user.EntityState == EntityState.Modified)
            {
                SaveChanges();
                return;
            }

            var userTmp = context.Users.Single(u => u.UserID == user.UserID);

            userTmp.UserName = user.UserName;
            userTmp.Password = user.Password;
            userTmp.Email= user.Email;
            
            userTmp.CurrentRank = user.CurrentRank;
            userTmp.HiddenLocationsFound = user.HiddenLocationsFound;

            userTmp.OpenedLocationDetails = user.OpenedLocationDetails;
            userTmp.OpenedResources = user.OpenedResources;

            userTmp.TeacherClassroomDefinition = user.TeacherClassroomDefinition;
            
            userTmp.ParentUserId = user.ParentUserId;

            SaveChanges();
        }

        public User GetUser(int UserID)
        {
            return context.Users.Where(u => u.UserID == UserID).FirstOrDefault();
        }
        
        public User GetUser(string UserName)
        {
            context.Refresh(RefreshMode.StoreWins,context.Users);
            var userObject = from user in context.Users.ToList()
                             where user.UserName == UserName
                             select user;
            
            if (userObject.Count() > 0)
            {
                return userObject.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}