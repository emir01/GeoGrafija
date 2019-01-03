using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using Common.Classroom;

namespace GeoGrafija.ViewModels.Profiles
{
    public class TeacherProfileViewModel
    {
        public List<MyStudentInfoViewModel> MyStudents { get; set; }

        public Classroom TeacherClassroom { get; set; }

        public TeacherProfileViewModel()
        {
            MyStudents = new List<MyStudentInfoViewModel>();
        }
    }
}