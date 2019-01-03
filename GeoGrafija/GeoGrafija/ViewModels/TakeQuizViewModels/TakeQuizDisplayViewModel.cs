using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace GeoGrafija.ViewModels.TakeQuizViewModels
{
    public class TakeQuizDisplayViewModel
    {
        public bool IsOk { get; set; }
        public string QuizName { get; set; }
        public int QuizId { get; set; }
        public bool StudentHasTakenQuiz { get; set; }
        public string TimeTaken { get; set; }

        public List<TakeQuestionDisplayViewModel> Questions { get; set; }

        public TakeQuizDisplayViewModel()
        {

        }

        public TakeQuizDisplayViewModel(Quiz quiz, User student)
        {
            if (quiz != null && student != null) { 
                Questions = new List<TakeQuestionDisplayViewModel>();

                QuizName = quiz.Name;
                QuizId = quiz.Id;
                StudentHasTakenQuiz = (student.Quizes.Contains(quiz));

                foreach (var quizQuestion in quiz.Questions)
                {
                    var takeQuestion = new TakeQuestionDisplayViewModel(quizQuestion);
                    Questions.Add(takeQuestion);
                }
            }
        }

        public Quiz GetQuizFromModel()
        {
            var quiz = new Quiz();
            quiz.Id = QuizId;
            QuizName = QuizName;

            foreach (var takeQuestionDisplayViewModel in Questions)
            {
                quiz.Questions.Add(takeQuestionDisplayViewModel.GetQuestion());
            }

            return quiz;
        }
    }
}