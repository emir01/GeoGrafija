using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace GeoGrafija.ViewModels.TakeQuizViewModels
{
    public class AvailableQuizesViewModel
    {
        // constructor creates an  empty view model
        public AvailableQuizesViewModel()
        {
            AvailableQuizes = new List<AvailableQuizForStudent>();
        }
        
        // constructor that creates populated view model
        public AvailableQuizesViewModel(List<Quiz> availableQuizes)
        {
            AvailableQuizes = new List<AvailableQuizForStudent>();
            Count = availableQuizes.Count;
            foreach (var availableQuiz in availableQuizes)
            {
                var availableQuizForStudent = new AvailableQuizForStudent();
            
                availableQuizForStudent.QuizName = availableQuiz.Name;
                availableQuizForStudent.QuizId = availableQuiz.Id;
                availableQuizForStudent.QuizTypeName = "";
                availableQuizForStudent.NumberOfQuestions = availableQuiz.Questions.Count;

                AvailableQuizes.Add(availableQuizForStudent);
            }
        }

        public List<AvailableQuizForStudent> AvailableQuizes { get; set; }
        public  int Count { get; set; }
    }
}