using System.Collections.Generic;

namespace Model.Interfaces
{
    public interface IUserRepository : IDatabaseRepository
    {
        IEnumerable<User> GetAllUsers();
        void AddUser(User user);
        void DeleteUser(User user);
        void UpdateUser(User user);
        User GetUser(int UserID);
        User GetUser(string UserName);
    }
}
