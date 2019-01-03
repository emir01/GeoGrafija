using Model;

namespace GeoGrafija.ViewModels.Profiles
{
    /// <summary>
    /// View Model for the user profile page.
    /// </summary>
    public class ProfileViewModel
    {
        public  ProfileViewModel (User user)
        {
            GeneralViewModel = user;
        }
        
        // General View Model ( should be ) available for all users
        public User GeneralViewModel { get; set; }

        // View Model only for the Student Profile Pages
        public StudentProfileViewModel StudentViewModel { get; set; }

        //View Model only for the Admin Profile Pages
        public AdminProfileViewModel AdminViewModel { get; set; }

        // View Model only for the Teacher pfofile Pages
        public TeacherProfileViewModel TeacherViewModel { get; set; }
    }
}