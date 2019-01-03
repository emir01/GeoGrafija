using System.Web.Mvc;

namespace GeoGrafija.Helpers
{
    public static class UrlViewExtensions
    {
        #region Profile Views

        #region Student Partials

        public static string PartialsStudent(this UrlHelper helper, string view)
        {
            return "/Views/Partials/Profiles/StudentPartials/" + view;
        }

        public static string PartialStudentQuizes(this UrlHelper helper)
        {
            return PartialsStudent(helper, "_TakenQuizes.cshtml");
        }

        public static string PartialStudentQuizRank(this UrlHelper helper)
        {
            return PartialsStudent(helper, "_QuizRank.cshtml");
        }

        public static string PartialStudentPickTeacher(this UrlHelper helper)
        {
            return PartialsStudent(helper, "_PickTeacher.cshtml");
        }

        public static string PartialStudentClassroom(this UrlHelper helper)
        {
            return PartialsStudent(helper, "_StudentClassroom.cshtml");
        }

        #endregion

        #region Teacher Partials
        public static string PartialsTeacher(this UrlHelper helper, string view)
        {
            return "/Views/Partials/Profiles/TeacherPartials/" + view;
        }

        public static string PartialTeacherMyStudents(this UrlHelper helper)
        {
            return PartialsTeacher(helper, "_MyStudents.cshtml");
        }

        public static string PartialTeacherMyClassroom(this UrlHelper helper)
        {
            return PartialsTeacher(helper, "_TeacherClassroom.cshtml");
        }

        #endregion

        #region Admin Partials
        public static string PartialsAdmin(this UrlHelper helper, string view)
        {
            return "/Views/Partials/Profiles/AdminPartials/" + view;
        }

        public static string PartialAdminStatistics(this UrlHelper helper)
        {
            return PartialsAdmin(helper, "_Statistics.cshtml");
        }
        #endregion

        #endregion
    }
}