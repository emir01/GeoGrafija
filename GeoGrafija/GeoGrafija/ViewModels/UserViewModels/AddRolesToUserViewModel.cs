using System;
using System.Collections.Generic;
using GeoGrafija.ViewModels.SharedViewModels;
using Model;

namespace GeoGrafija.ViewModels.UserViewModels
{
    public class AddRolesToUserViewModel
    {
        public User User { get; set; }
        public List<CheckBoxModel> UserRoles { get; set; }

        public void GenerateList(List<Role> allRoles)
        {
            UserRoles = new List<CheckBoxModel>();

            if (User == null)
            {
                throw new NullReferenceException("User must be not null to generate List for View Model");
            }

            foreach (var role in allRoles)
            {
                var userHasRole = new CheckBoxModel();
                userHasRole.TableDisplayName = "Име на Улога";
                userHasRole.CheckBoxNames = "roles";
                userHasRole.DisplayText = role.RoleName;

                if (role.RoleID != null)
                    userHasRole.ItemID = role.RoleID;
                
                userHasRole.CheckBoxChecked = User.UserHasRole(role.RoleName);
                UserRoles.Add(userHasRole);
            }
        }
    }
}
