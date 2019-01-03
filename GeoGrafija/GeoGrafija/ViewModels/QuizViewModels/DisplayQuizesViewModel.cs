using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace GeoGrafija.ViewModels.QuizViewModels
{
    public class DisplayQuizesViewModel
    {
        public List<SimpleDisplayQuizViewModel> Quizes { get; set; }

        public DisplayQuizesViewModel()
        {
            
        }

        public DisplayQuizesViewModel(IEnumerable<Quiz> quizes )
        {
            Quizes=new List<SimpleDisplayQuizViewModel>();

            foreach (var quiz in quizes)
            {
                var quizViewModel = new SimpleDisplayQuizViewModel();

                quizViewModel.QuizName = quiz.Name;
                quizViewModel.NumberOfQuestions = quiz.Questions.Count;
                quizViewModel.TotalPoints = quiz.Questions.Sum(x => x.Points);
                quizViewModel.TotalStudents = quiz.StudentQuizResults.Count;

                Quizes.Add(quizViewModel);
            }
        }

    }
}