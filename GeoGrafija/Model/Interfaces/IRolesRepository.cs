using System.Collections.Generic;

namespace Model.Interfaces
{
    public interface IRolesRepository:IDatabaseRepository
    {
        //Create
        void AddRole(Role Role);
        void AddPrivilige(Privilege privilege);
        void AddPriviligeToRole(Role Role,Privilege Privilege);

        //Read
        Role GetRole(string RoleName);
        Privilege GetPrivilege(string PrivilegeName);

        IEnumerable<Role> GetAllRoles();
        IEnumerable<Privilege> GetAllPrivileges();

        //Updates
        void UpdateRole(Role Role);
        void UpdatePrivilege(Privilege Privilege);

        //Delete
        void RemovePrivilegeFromRole(Privilege Privilege,Role Role);
        void RemoveAllPrivilegesFromRole(Role role);
        void RemovePrivilege(Privilege Privilege);
        void RemoveRole(Role Role);
    }
}
