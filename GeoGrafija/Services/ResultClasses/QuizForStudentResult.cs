using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace Services.ResultClasses
{
    public class QuizForStudentResult
    {
        public User Student { get; set; }
        public Quiz Quiz { get; set; }
    }
}
