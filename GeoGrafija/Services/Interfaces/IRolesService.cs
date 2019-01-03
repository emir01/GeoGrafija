using System.Collections.Generic;
using GeoGrafija.ResultClasses;
using Model;

namespace Services.Interfaces
{
    public interface IRolesService
    {
        bool UserHasRole(string UserName,string Role);
        bool UserHasPrivilige(string UserName,string Privilige);
        string[] GetUserRoles(string UserName);

        OperationResult AddRole(Role Role);
        OperationResult AddPrivilege(Privilege Privilege);
     
        /// <summary>
        /// Adds the Supplied Multiple Priviliges via the Privielge Names to The Supplied Role. Both role and privilege  must already be added to the system.
        /// </summary>
        /// <param name="RoleName">String reprsenting the role name</param>
        /// <param name="PrivilegeNames">List of Strings representing the Privilege Names of the privileges to be added.</param>
        /// <returns>Returns and OperationResult object with the result and Messages.</returns>
        OperationResult SetNewPrivilegesToRole(string RoleName, List<string> PrivilegeNames);
        OperationResult SetNewRolesToUser(User User, List<string> RoleNames);

        /// <summary>
        /// Add The Single Privilege to The Role
        /// </summary>
        /// <param name="Role">Role Entity Object</param>
        /// <param name="Privilege">Privilege Entity Object</param>
        /// <returns></returns>
        OperationResult AddPrivilegeToRole(Role Role, Privilege Privilege);

        /// <summary>
        /// Remove a Single Privilege from a Role
        /// </summary>
        /// <param name="Role"></param>
        /// <param name="Privilege"></param>
        /// <returns></returns>
        OperationResult RemovePrivilegeFromRole(Role Role, Privilege Privilege);

        /// <summary>
        /// Remove all privileges from the Role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        OperationResult RemoveAllPrivilegesFromRole(Role role);
     
        /// <summary>
        /// Returns all The Roles in The System
        /// </summary>
        /// <returns></returns>
        List<Role> GetAllRoles();
        
        /// <summary>
        /// Returns all the Roles in The System
        /// </summary>
        /// <returns></returns>
        List<Privilege> GetAllPrivileges();

        /// <summary>
         /// Returns a single role Based on the role name.
        /// </summary>
        /// <returns></returns>
        Role GetRole(string RoleName);
        /// <summary>
        /// Returns a single privilege based on the privilege name.
        /// </summary>
        /// <returns></returns>
        Privilege GetPrivilege(string PrivilegeName);
        
        #region ExtraMethods
        GenericOperationResult<List<Role>> GenerateRolesFromStrings(List<string> ListOfRoleNames);
        GenericOperationResult<List<Privilege>> GeneratePrivilegesFromStrings(List<string> ListOfPrivilegeNames);
        void ResetUserRoles(User user, List<Role> newRoles);
        void ResetRolePrivileges(Role role, List<Privilege> newPrivileges);
        #endregion
    }
}
