using System;
using System.Collections.Generic;
using GeoGrafija.ViewModels.SharedViewModels;
using Model;

namespace GeoGrafija.ViewModels.UserViewModels
{
    public class AddPrivilegesToRoleViewModel
    {
        public Role Role { get; set; }
        public List<CheckBoxModel> AllPrivileges { get; set; }
        
        public void GenerateList(List<Privilege> allPrivileges)
        {
            AllPrivileges = new List<CheckBoxModel>();

            if (Role == null)
            {
                throw new NullReferenceException("Role must be not null to generate List for View Model");
            }

            foreach (var priv in allPrivileges)
            {
                var roleHasPrivilege = new CheckBoxModel();
                roleHasPrivilege.DisplayText = priv.PrivilegeName;
                roleHasPrivilege.CheckBoxNames = "privileges";
                roleHasPrivilege.TableDisplayName = "Име на Привилегија";

                if (priv.PrivilegeID != null)
                    roleHasPrivilege.ItemID = priv.PrivilegeID;

                roleHasPrivilege.CheckBoxChecked = Role.RoleHasPrivilege(priv.PrivilegeName);

                AllPrivileges.Add(roleHasPrivilege);
            }
        }
    }
}