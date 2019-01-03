using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace GeoGrafija.ViewModels.Profiles
{
    public class NewTeacherResultViewModel
    {
        public bool ChangeOk      { get; set; }
        public string Message     { get; set; }
        public int NewTeacherId   { get; set; }
        public string NewTeacherName { get; set; }

        public NewTeacherResultViewModel (User student)
        {
            if (student != null)
            {
                NewTeacherId = student.Teacher.UserID;
                NewTeacherName = student.Teacher.UserName;
            }
        }
    }
}