using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoGrafija.ViewModels.Profiles
{
    public class AdminProfileViewModel
    {
        public int TotalUsers { get; set; }
        public int TeacherNumber { get; set; }
        public int StudentNumber { get; set; }
        public int AdminNumber { get; set; }

        public int LocationsNumber { get; set; }
        public int LocationTypesNumber { get; set; }
        public int ResourcesNumber { get; set; }
        public double AverageResourcePerLocation { get; set; }

        public int QuizesNumber { get; set; }
        public int QuestionsNumber { get; set; }
        public int StudentTakenQuizesNumber { get; set; }
    }
}