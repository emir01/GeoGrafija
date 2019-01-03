using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Classroom;
using Model;

namespace GeoGrafija.ViewModels.Profiles
{
    public class StudentProfileViewModel
    {
        public int NextPointBorder { get; set; }
        public string RankImage { get; set; }
        public string RankName { get; set; }
        public string ProgressPercent { get; set; }

        /// <summary>
        /// The total quiz points of all the quizes the student has taken
        /// </summary>
        public int TotalQuizPoints { get; set; }

        /// <summary>
        /// The total points of the correct questions the student has answered
        /// </summary>
        public int TotalStudentPoints { get; set; }

        /// <summary>
        /// The student global rank, of all students in the system givent he student points.
        /// </summary>
        public int GlobalRank { get; set; }

        public int StudentsBeforeYou { get; set; }
        public int StudentsAfterYou { get; set; }

        //================================================
        public int NumberOfResourcesOpened { get; set; }
        public int NumberOfLocationsDetailsOpened { get; set; }

        public List<User> AvailableTeachers { get; set; }

        public int? RankOrder { get; set; }

        public Classroom Classroom { get; set; }
        
        public SelectList  GetTeacherDDViewModel()
        {
            var select = new SelectList(AvailableTeachers,"UserID","UserName");

            return select;
        }

        public bool HasTeacher { get; set; }
    }

}