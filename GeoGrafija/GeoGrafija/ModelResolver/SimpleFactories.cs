using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeoGrafija.ViewModels;

using GeoGrafija.ViewModels.UserViewModels;
using Model;

namespace GeoGrafija.ModelResolver
{
    public static class SimpleFactories
    {
        public static AddPrivilegesToRoleViewModel  BuildAddPrivilegesToRoleViewModel(Role role,List<Privilege> privileges)
        {
            var viewModel = new AddPrivilegesToRoleViewModel();

            if (role == null || privileges == null) return viewModel;
            viewModel.Role = role;
            viewModel.GenerateList(privileges);

            return viewModel;
        }

        public static AddRolesToUserViewModel BuildAddRolesToUserviewModel(User user,List<Role> allRoles)
        {
            var viewModel = new AddRolesToUserViewModel();

            if (user == null || allRoles == null) return viewModel;
            viewModel.User = user;
            viewModel.GenerateList(allRoles);
            return viewModel;
        }

        public static User GetUserFromRegisterViewModel(RegisterViewModel viewModel)
        {
            var user = new User();
            
            user.UserName = viewModel.UserName;
            user.Email = viewModel.Email;
            user.Password = viewModel.Password;
            
            return user;
        }

        
    }
}