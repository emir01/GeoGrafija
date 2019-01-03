using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Model.Interfaces;

namespace GeoGrafija.Tests.FakeRepositories
{
    class TestRolesRepository : IRolesRepository
    {
        public List<Role> Roles { get; set; }
        public List<Privilege> Privileges { get; set; }

        public TestRolesRepository()
        {
            Roles = new List<Role>()
            {
                new Role() { RoleID=1, RoleName="Admin", RoleDescription="Admins can Do Everything" },
                new Role() { RoleID=2, RoleName="Student",RoleDescription="Students Can Learn and Study" },
                new Role() {RoleID=3, RoleName="Teacher", RoleDescription ="Teachers can Learn and Teach" },
                new Role() {RoleID=4, RoleName= "Assistant", RoleDescription="Assistans can Learn and Assist" },
                new Role() {RoleID=5, RoleName= "NeparnaUloga", RoleDescription="Uloga Dodelena vo testUserRepository" },
                new Role() {RoleID=6, RoleName= "ParnaUloga", RoleDescription="Uloga Dodelena vo testUserRepository" }
            };

            Privileges = new List<Privilege> ()
            {
                new Privilege(){ PrivilegeID=1,PrivilegeName="CreateUsers"},
                new Privilege(){PrivilegeID=2,PrivilegeName="CreateResource"},
                new Privilege(){PrivilegeID=3,PrivilegeName="UseResource"},
                new Privilege(){PrivilegeID=4,PrivilegeName="DeleteResource"},
                new Privilege(){PrivilegeID=5,PrivilegeName="BanUsers"},
                new Privilege(){PrivilegeID=6,PrivilegeName="ViewLogs"}
            };

            //Add roles to Admin
            var role = Roles.Find(r=>r.RoleName=="Admin");
            role.Privileges.Add(Privileges[0]);
            role.Privileges.Add(Privileges[4]);
            role.Privileges.Add(Privileges[5]);

            //Add roles to Student
            role = Roles.Find(r => r.RoleName == "Student");
            role.Privileges.Add(Privileges[2]);
            
            //Add roles to Teacher
            role = Roles.Find(r => r.RoleName == "Teacher");
            role.Privileges.Add(Privileges[2]);
            role.Privileges.Add(Privileges[1]);
            role.Privileges.Add(Privileges[3]);

            //Add roles to Assistant
            role = Roles.Find(r => r.RoleName == "Assistant");
            role.Privileges.Add(Privileges[1]);
            role.Privileges.Add(Privileges[2]);
        }

        //Create
        public void AddRole(Role role)
        {
            Roles.Add(role);
        }

        public void AddPrivilige(Privilege privilege)
        {
            Privileges.Add(privilege);
        }

        public void AddPriviligeToRole(Role role, Privilege privilege)
        {
            role.Privileges.Add(privilege);
        }
        
        // Retrieve
        public Role GetRole(string RoleName)
        {
            return (from r in Roles where r.RoleName == RoleName select r).FirstOrDefault();
        }

        public Privilege GetPrivilege(string PrivilegeName)
        {
            return (from p in Privileges where p.PrivilegeName == PrivilegeName select p).FirstOrDefault();
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return Roles;
        }

        public IEnumerable<Privilege> GetAllPrivileges()
        {
            return Privileges;
        }

        //UPDATE
        public void UpdateRole(Role Role)
        {
            var role = (from r in Roles
                        where r.RoleID == Role.RoleID
                        select r).FirstOrDefault();
            role.RoleName = Role.RoleName;
            role.RoleDescription = Role.RoleDescription;
            role.Privileges = Role.Privileges;
        }

        public void UpdatePrivilege(Privilege privilege)
        {
            throw new NotImplementedException();
        }
        
        //DELETES
        public void RemovePrivilegeFromRole(Privilege privilege, Role role)
        {
            role.Privileges.Remove(privilege);
        }

        public void RemovePrivilege(Privilege privilege)
        {
            Privileges.Remove(privilege);
        }

        public void RemoveRole(Role role)
        {
            Roles.Remove(role);
        }

        public void RemoveAllPrivilegesFromRole(Role role)
        {
            role.Privileges.Clear();
        }

        public void SaveChanges() { }
    }
}
