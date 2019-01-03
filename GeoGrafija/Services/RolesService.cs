using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using GeoGrafija.ResultClasses;
using Model;
using Model.Interfaces;

using Services.Interfaces;

namespace Services
{
    public class RolesService:IRolesService
    {
        private IUserRepository UserRepository { get; set; }
        private IRolesRepository RolesRepository { get; set; }
        
        public RolesService(IUserRepository userRepository,IRolesRepository rolesRepository )
        {
            UserRepository = userRepository;
            RolesRepository= rolesRepository;
        }

        public bool UserHasRole(string UserName,string Role)
        {
            var user = UserRepository.GetUser(UserName);
            
            if (user == null)
                return false;

            var roles = from role in user.Roles
                        where role.RoleName == Role
                        select role;

            if (roles.Count() > 0)
                return true;
            else
                return false;
        }

        public bool UserHasPrivilige(string UserName, string Privilige)
        {
            User user;

            try
            {
                user = UserRepository.GetUser(UserName);
            }
            catch (ObjectNotFoundException ex)
            {
                throw new ObjectNotFoundException("Could not retrieve privileges for User. User not found");
            }

            var privileges =  new List<Privilege>();

            foreach (var role in user.Roles)
            {
                foreach (var priv in role.Privileges)
                {
                    if (!privileges.Contains(priv))
                    {
                        privileges.Add(priv);
                    }
                }
            }

            var priviligeNames = (from p in privileges
                                  select p.PrivilegeName.ToLower()).ToList();
            
            if(priviligeNames.Contains(Privilige.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public string[] GetUserRoles(string UserName)
        {
            User user;
        
            try
            {
              user = UserRepository.GetUser(UserName);
            }
            catch (ObjectNotFoundException ex)
            {
                throw new ObjectNotFoundException("Could not retrieve roles for User. User not found");
            }

            if (user != null) { 
            
                var roles = from r in user.Roles
                            select r.RoleName.ToLower();
                return roles.ToArray();
            }
            else
            {
                return new string[0];
            }
            
        }
        
        public OperationResult AddRole(Role Role)
        {
            var result =  OperationResult.GetResultObject();
            var role = RolesRepository.GetRole(Role.RoleName);

            if (role == null)
            {
                RolesRepository.AddRole(Role);
                result.SetSuccess();
            }
            else
            {
                result.AddMessage("Веќе постои улога со име :" + Role.RoleName);
            }

            return result;
        }

        public OperationResult AddPrivilege(Privilege Privilege)
        {
            var result = OperationResult.GetResultObject();
            var privilege = RolesRepository.GetPrivilege(Privilege.PrivilegeName);

            if (privilege == null)
            {
                RolesRepository.AddPrivilige(Privilege);
                result.SetSuccess();
            }
            else
            {
                result.AddMessage("Веќе постои привилегија со име :" + Privilege.PrivilegeName);
            }

            return result;
        }

        public OperationResult SetNewPrivilegesToRole(string RoleName, List<string> PrivilegeNames)
        {
            var result = OperationResult.GetResultObject();
            var role = GetRole(RoleName);

            if (PrivilegeNames == null)
            {
                result.AddMessage("Мора да се избере барем една привилегија");
                return result;
            }
            
            List<Privilege> newPrivileges;
            var generatePrivResult = GeneratePrivilegesFromStrings(PrivilegeNames);

            if (generatePrivResult.IsOK)
            {
                  newPrivileges = generatePrivResult.GetData();
            }
            else
            {
                result.Messages.AddRange(generatePrivResult.Messages);
                return result;
            }
          
            ResetRolePrivileges(role, newPrivileges);

            try
            {
                RolesRepository.SaveChanges();
                result.SetSuccess();
            }
            catch (Exception ex)
            {
                result.SetException();
                result.AddExceptionMessage(ex.Message);
            }

            return result;
        }
        
        public  void ResetRolePrivileges(Role role, List<Privilege> newPrivileges)
        {
            role.Privileges.Clear();

            foreach (var newpriv in newPrivileges)
            {
                role.Privileges.Add(newpriv);
            }
        }
        
        public GenericOperationResult<List<Privilege>> GeneratePrivilegesFromStrings(List<string> PrivilegeNames)
        {
            var result = OperationResult.GetGenericResultObject<List<Privilege>>();
            var newPrivilegesList = new List<Privilege>();

            foreach (var privName in PrivilegeNames)
            {
                var newPrivilege = GetPrivilege(privName);
                
                if (newPrivilege == null)
                {
                    result.AddMessage("Привилегијата " + privName + " не постои!");
                    return result;
                }

                //Double Check
                if (!newPrivilegesList.Contains(newPrivilege))
                    newPrivilegesList.Add(newPrivilege);
            }

            if (newPrivilegesList.Count == 0)
            {
                result.AddMessage("Не најдовме привилегии за додавање");
                return result;
            }

            result.SetSuccess();
            result.SetData(newPrivilegesList);
            return result;
        }


        public OperationResult AddPrivilegeToRole(Role Role, Privilege Privilege)
        {
            var result = OperationResult.GetResultObject();

            try
            {
               if (Role.Privileges.Contains(Privilege))
                {
                    result.AddMessage("Привилегијата :  " + Privilege.PrivilegeName + " веќе е додадена на Улогата" + Role.RoleName);
                    return result;
                }

                RolesRepository.AddPriviligeToRole(Role, Privilege);
                result.SetSuccess();
            }
            catch (Exception ex)
            {
                result.SetException();
                result.AddExceptionMessage(ex.Message);
                result.AddMessage("Грешка при доделувањето на привилегија на улога.");
            }
           
            return result;   
        }
        
        public OperationResult RemovePrivilegeFromRole(Role Role, Privilege Privilege)
        {
            var result = OperationResult.GetResultObject();

            if (!Role.Privileges.Contains(Privilege))
            {
                result.AddMessage("Улогата " + Role.RoleName + " ја нема привилегијата " + Privilege.PrivilegeName);
                return result;
            }
            else
            {
                try
                {
                    RolesRepository.RemovePrivilegeFromRole(Privilege, Role);
                    result.SetSuccess();
                    return result;
                }
                catch (Exception ex)
                {
                    result.SetException();
                    result.AddExceptionMessage(ex.Message);
                }
            }

            return result; 
        }

        public OperationResult RemoveAllPrivilegesFromRole(Role role)
        {
            var result = OperationResult.GetResultObject();

            try
            {
                if (role.Privileges.Count == 0)
                {
                    result.AddMessage("Улогата " + role.RoleName + " нема привилегии");
                }
                else
                {
                    RolesRepository.RemoveAllPrivilegesFromRole(role);
                    result.SetSuccess();
                }
            }
            catch (Exception ex)
            {
                result.AddMessage("Моментално не може да се одстранат привилегиите. Обидете се подоцна");
                result.SetException();
                result.AddExceptionMessage(ex.Message);
            }

            return result;
        }
        
        public List<Role> GetAllRoles()
        {
            return RolesRepository.GetAllRoles().ToList();
        }

        public List<Privilege> GetAllPrivileges()
        {
            return RolesRepository.GetAllPrivileges().ToList();
        }

        public Role GetRole(string RoleName)
        {
            return RolesRepository.GetRole(RoleName);
        }

        public Privilege GetPrivilege(string PrivilegeName)
        {
            return RolesRepository.GetPrivilege(PrivilegeName);
        }

        public OperationResult SetNewRolesToUser(User user, List<string> RoleNames)
        {
            var result = OperationResult.GetResultObject();
            
            if (RoleNames == null)
            {
                result.AddMessage("Мора да се избере барем една улога");
                return result;
            }

            List<Role> newRoles;
            var generateRolesResult = GenerateRolesFromStrings(RoleNames);

            if (generateRolesResult.IsOK)
            {
                newRoles = generateRolesResult.GetData();
            }
            else
            {
                result.Messages.AddRange(generateRolesResult.Messages);
                return result;
            }

            ResetUserRoles(user, newRoles);

            try
            {
                UserRepository.SaveChanges();
                result.SetSuccess();
            }
            catch (Exception ex)
            {
                result.SetException();
                result.AddExceptionMessage(ex.Message);
            }

            return result;
        }

        public  GenericOperationResult<List<Role>> GenerateRolesFromStrings(List<string> RoleNames)
        {
            var result = OperationResult.GetGenericResultObject<List<Role>>();
            var newRolesList = new List<Role>();

            foreach (var roleName in RoleNames)
            {
                var newRole = GetRole(roleName);
                if (newRole == null)
                {
                    result.AddMessage("Улогата " + roleName + " не постои!");
                    return result;
                }

                if (!newRolesList.Contains(newRole))
                    newRolesList.Add(newRole);
            }

            if (newRolesList.Count == 0)
            {
                result.AddMessage("Не најдовме привилегии за додавање");
                return result;
            }

            result.SetSuccess();
            result.SetData(newRolesList);
            return result;
        }

        public  void ResetUserRoles(User user, List<Role> newRoles)
        {
            user.Roles.Clear();

            foreach (var newRole in newRoles)
            {
                user.Roles.Add(newRole);
            }
        }
    }
}