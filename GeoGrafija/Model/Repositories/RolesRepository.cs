using System.Collections.Generic;
using System.Linq;
using Model.Interfaces;

namespace Model.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private GeoGrafijaEntities context;

        public RolesRepository(IDbContext context)
        {
            this.context = (GeoGrafijaEntities)context;
        }

        // CREATE
        public void AddRole(Role Role)
        {
            context.Roles.AddObject(Role);
            context.SaveChanges();
            
        }

        public void AddPrivilige(Privilege Privilege)
        {
            context.Privileges.AddObject(Privilege);
            context.SaveChanges();
        }

        public void AddPriviligeToRole(Role Role, Privilege Privilege)
        {
            var role = (from r in context.Roles
                       where r.RoleName == Role.RoleName
                       select r).FirstOrDefault();
            
            role.Privileges.Add(Privilege);
            context.SaveChanges();
        }
        
        // RETRIEVE
        public Role GetRole(string RoleName)
        {
            var role = (from r in context.Roles
                        where r.RoleName == RoleName
                        select r).FirstOrDefault();
            return role;
        }

        public Privilege GetPrivilege(string PrivilegeName)
        {
            var privilege = (from p in context.Privileges
                             where p.PrivilegeName == PrivilegeName
                             select p).FirstOrDefault();
            return privilege;
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return context.Roles;
        }

        public IEnumerable<Privilege> GetAllPrivileges()
        {
            return context.Privileges;
        }

        //UPDATE
        public void UpdateRole(Role Role)
        {
            var role = (from r in context.Roles
                        where r.RoleID == Role.RoleID
                        select r).SingleOrDefault();

            role.RoleDescription = Role.RoleDescription;
            role.RoleName = Role.RoleName;
            role.Privileges = Role.Privileges;

            context.SaveChanges();
        }

        public void UpdatePrivilege(Privilege Privilege)
        {
            var priv = (from p in context.Privileges
                        where p.PrivilegeID== Privilege.PrivilegeID
                        select p).SingleOrDefault();
            
            priv.PrivilegeDescription = Privilege.PrivilegeDescription;
            priv.PrivilegeName = Privilege.PrivilegeName;

            context.SaveChanges();
        }

        //DELETE
        public void RemovePrivilegeFromRole(Privilege Privilege, Role Role)
        {
            var role = (from r in context.Roles
                        where r.RoleName == Role.RoleName
                        select r).FirstOrDefault();
            
            role.Privileges.Remove(Privilege);
            context.SaveChanges();
        }

        public void RemoveAllPrivilegesFromRole(Role Role)
        {
            var role = (from r in context.Roles
                        where r.RoleName == Role.RoleName
                        select r).FirstOrDefault();
            
            role.Privileges.Clear();
            context.SaveChanges();
        }

        public void RemovePrivilege(Privilege privilege)
        {
            context.Privileges.DeleteObject(privilege);
            context.SaveChanges();
        }

        public void RemoveRole(Role role)
        {
            context.Roles.DeleteObject(role);
            context.SaveChanges();
        }
        
        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}