using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Model;

namespace GeoGrafija.ViewModels.QuizViewModels
{
    public class CreateQuizViewModel
    {
        public CreateQuizViewModel()
        {
            Questions= new List<CreateQuestionViewModel>();    
        }

        [DisplayName("Име на квиз :")]
        [StringLength(999, ErrorMessage = "Името на квизот мора да биде минимум 3 карактери", MinimumLength = 3)]
        [Required(ErrorMessage = "Мора да внесете име на квиз")]
        public string Name { get; set; }

        [DisplayName("Прашања :")]
        public List<CreateQuestionViewModel> Questions { get; set; }

        /// <summary>
        /// return a model entity object that will be then passed to the service that will take 
        /// care of saving the quiz
        /// </summary>
        public Quiz GetQuiz()
        {
            //create the quiz entity and set basic properties
            var quiz = new Quiz();
            
            quiz.Name = Name;
            quiz.TypeId = 1;
            
            // create all the QuizQuestion entities for the current Quiz
            foreach (var question in Questions)
            {
                // create quiz question
                var quizQuestion = new Question();

                //set basic properties
                quizQuestion.Points = 1;
                quizQuestion.LocationId = null;
                quizQuestion.QuestionText = question.Text;
                quizQuestion.uId = question.Id;

                //for this question create/add tall the QuestionAnswer entities
                foreach (var answer in question.Answers)
                {
                    //create a new question answer entity
                    var questionAnswer = new Answer();

                    // set  basic properties
                    questionAnswer.AnswertText = answer.AnswerText;
                    questionAnswer.isCorrectAnswer = answer.IsTrue;
                    questionAnswer.uId = answer.Id;
                    questionAnswer.questionUId = question.Id;
    
                    // add to the current question answer list
                    quizQuestion.Answers.Add(questionAnswer);
                }

                //add to the current quiz question list
                quiz.Questions.Add(quizQuestion);
            }

            return quiz;
        }
    }
}