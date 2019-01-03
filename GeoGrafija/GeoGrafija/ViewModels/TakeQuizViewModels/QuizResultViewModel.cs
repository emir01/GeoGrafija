using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Quiz_Results;
using Model;

namespace GeoGrafija.ViewModels.TakeQuizViewModels
{
    public class QuizResultViewModel
    {
        public bool IsOk { get; set; }
        public string Message { get; set; }

        public string QuizName { get; set; }
        public int QuizId { get; set; }

        public string StudentUserName { get; set; }

        public int TotalPoints { get; set; }
        public int StudentPoints { get; set; }
        public int CorrectQuestions { get; set; }

        public QuizDetailedResults DetailedResults { get; set; }
      
        public QuizResultViewModel(string studentName ,StudentQuizResult quizResult)
        {
            if (!String.IsNullOrWhiteSpace(studentName) && quizResult != null) { 
                StudentUserName = studentName;

                QuizName = quizResult.Quiz.Name;
                QuizId = quizResult.Quiz.Id;

                TotalPoints = quizResult.PointsTotal;
                StudentPoints = quizResult.PointsStudent;
                CorrectQuestions = quizResult.CorrectQuestions == null ? 0 : (int)quizResult.CorrectQuestions;

                if (quizResult.DetailResult != null) { 
                    DetailedResults = DetailResultSerializer.DesirializeFromXml(quizResult.DetailResult);
                }
            }
        }
    }
}