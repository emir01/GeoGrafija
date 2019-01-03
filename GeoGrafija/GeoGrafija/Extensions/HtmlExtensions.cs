using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Common.Static_Dictionary;
using GeoGrafija.ViewModels.Profiles;
using GeoGrafija.ViewModels.ResourcesViewModels;
using Model;

namespace GeoGrafija.Helpers
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString Navigation(this HtmlHelper helper, object partialViewName)
        {
            if (partialViewName != null)
            {
                return helper.Partial((string) partialViewName);
            }
            else
            {
                return new MvcHtmlString("");
            }
        }

        public static MvcHtmlString Nl2Br(this HtmlHelper htmlHelper, string text)
        {
            StringBuilder builder = new StringBuilder();
            string[] lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                if (i > 0)
                    builder.Append("<br/>");
                builder.Append(HttpUtility.HtmlEncode(lines[i]));
            }
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString PrintValueOrDefault(this HtmlHelper helper, object value, object defaultValue = null)
        {
            if (value != null)
            {
                return new MvcHtmlString(value.ToString());
            }
            else
            {
                var def = defaultValue == null ? "": defaultValue.ToString();
                return new MvcHtmlString(def);
            }
        }
        
        public static MvcHtmlString CheckObjectAndPrintProperty(this HtmlHelper helper, object objectToCheck, string propertyName,string defaultValue)
        {
            if (objectToCheck == null)
            {
                return new MvcHtmlString(defaultValue);
            }
            else
            {
                //check if object has property 
                var type = objectToCheck.GetType();
                var property = type.GetProperty(propertyName);
                if  (property != null)
                {
                    var propValue = property.GetValue(objectToCheck,null);

                    if (propValue !=null && !String.IsNullOrWhiteSpace(propValue.ToString()))
                    {
                        return new MvcHtmlString(propValue.ToString());
                    }
                    else
                    {
                        return new MvcHtmlString(defaultValue);      
                    }
                }
                else
                {
                    return new MvcHtmlString(defaultValue);
                }
            }
        }

        public static MvcHtmlString DisplayResourcesOfType(this HtmlHelper helper,  ResourceTypeViewModel resourceType, int locationId)
        {
            //figure out the current resources. for the locaiton
            var resources = resourceType.Resources.Where(x => x.LocaitonId == locationId).ToList();
            return helper.Partial(@"/Views/Partials/Resources/ResourcesTable.cshtml",resources);
        }

        public static MvcHtmlString DisplayResourcesOfTypeForLocationType(this HtmlHelper helper, ResourceTypeViewModel resourceType, int locationTypeId)
        {
            //figure out the current resources. for the locaiton
            var resources = resourceType.Resources.Where(x=>x.LocationTypeId == locationTypeId);
            return helper.Partial(@"/Views/Partials/Resources/ResourceTableForLocType.cshtml", resources);
        }

        #region Profiles
        
        public static MvcHtmlString RenderUserProfile(this HtmlHelper htmlHelper, ProfileViewModel model)
        {
            if (model.GeneralViewModel.Roles.Where(x => x.RoleName.Equals(RoleNames.ADMIN, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
            {
                // Admin Role
                return htmlHelper.Partial(@"/Views/Partials/Profiles/AdminHome.cshtml", model);
            }

            if (model.GeneralViewModel.Roles.Where(x => x.RoleName.Equals(RoleNames.TEACHER, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
            {
                // Teacher Role
                return htmlHelper.Partial(@"/Views/Partials/Profiles/TeacherHome.cshtml", model);
            }

            if (model.GeneralViewModel.Roles.Where(x => x.RoleName.Equals(RoleNames.STUDENT, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
            {
                // Student Role
                return htmlHelper.Partial(@"/Views/Partials/Profiles/StudentHome.cshtml", model);
            }

            return htmlHelper.Partial(@"/Views/Partials/Profiles/MissingProfile.cshtml");
        }
    
        public static MvcHtmlString Profile_RenderName(this HtmlHelper htmlHelper, ProfileViewModel model)
        {
            return htmlHelper.Partial(@"/Views/Partials/Profiles/_ProfileName.cshtml",model);
        }

        public static MvcHtmlString Profile_RenderActionBoxes(this HtmlHelper htmlHelper, ProfileViewModel model, string userType)
        {
            if (userType.Equals(RoleNames.ADMIN, StringComparison.InvariantCultureIgnoreCase))
            {
                var actionBoxes = GetAdminActionBoxes(model);
                return htmlHelper.Partial(@"/Views/Partials/Profiles/_ProfileActionBoxes.cshtml",actionBoxes);
            }

            if (userType.Equals(RoleNames.TEACHER, StringComparison.InvariantCultureIgnoreCase))
            {
                var actionBoxes = GetTeacherActionBoxes(model);
                return htmlHelper.Partial(@"/Views/Partials/Profiles/_ProfileActionBoxes.cshtml", actionBoxes);
            }

            if (userType.Equals(RoleNames.STUDENT, StringComparison.InvariantCultureIgnoreCase))
            {
                var actionBoxes = GetStudentActionBoxes(model);
                return htmlHelper.Partial(@"/Views/Partials/Profiles/_ProfileActionBoxes.cshtml", actionBoxes);
            }

            return htmlHelper.Partial(@"/Views/Partials/Profiles/MissingProfile.cshtml");
        }

        public static MvcHtmlString Profile_RenderCanvases(this HtmlHelper htmlHelper, ProfileViewModel model, string userType)
        {
            if (userType.Equals(RoleNames.ADMIN, StringComparison.InvariantCultureIgnoreCase))
            {
                return htmlHelper.Partial(@"/Views/Partials/Profiles/_AdminCanvases.cshtml", model);
            }

            if (userType.Equals(RoleNames.TEACHER, StringComparison.InvariantCultureIgnoreCase))
            {
                return htmlHelper.Partial(@"/Views/Partials/Profiles/_TeacherCanvases.cshtml", model);
            }

            if (userType.Equals(RoleNames.STUDENT, StringComparison.InvariantCultureIgnoreCase))
            {
                
                return htmlHelper.Partial(@"/Views/Partials/Profiles/_StudentCanvases.cshtml", model);
            }

            return htmlHelper.Partial(@"/Views/Partials/Profiles/MissingProfile.cshtml");
        }

      
        #region Helpers

        private static ActionBoxesListViewModel GetAdminActionBoxes(ProfileViewModel model)
        {
            var actionBoxesList = new ActionBoxesListViewModel(model);

            actionBoxesList.ActionBoxes.Add(new ActionBoxViewModel("Статистики!", 1, 1, "stat.png"));

            return actionBoxesList;
        }

        private static ActionBoxesListViewModel GetTeacherActionBoxes(ProfileViewModel model)
        {
            var actionBoxesList = new ActionBoxesListViewModel(model);
            actionBoxesList.ActionBoxes.Add(new ActionBoxViewModel("Помош!", 0, 3, "help.png"));
            actionBoxesList.ActionBoxes.Add(new ActionBoxViewModel("Moja Училница!", 1, 1, "book.png"));
            actionBoxesList.ActionBoxes.Add(new ActionBoxViewModel("Мои Студенти!", 2, 2, "students.png"));
            
            return actionBoxesList;
        }

        private static ActionBoxesListViewModel GetStudentActionBoxes(ProfileViewModel model)
        {
            var actionBoxesList = new ActionBoxesListViewModel(model);

            actionBoxesList.ActionBoxes.Add(new ActionBoxViewModel("Помош!", 0, 5, "help.png"));
            actionBoxesList.ActionBoxes.Add(new ActionBoxViewModel("Мојот Ранк!", 1, 1,"rank.png"));
            actionBoxesList.ActionBoxes.Add(new ActionBoxViewModel("Резултати од Квизови", 2, 2, "qResults.png"));
            actionBoxesList.ActionBoxes.Add(new ActionBoxViewModel("Избери Професор!", 3, 3,"action.png"));
            actionBoxesList.ActionBoxes.Add(new ActionBoxViewModel("Училница!", 4, 4, "book.png"));
            
            return actionBoxesList;
        }

        #endregion

        #endregion
}
}