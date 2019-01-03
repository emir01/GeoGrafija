using System.Collections.Generic;
using Model;

namespace GeoGrafija.ViewModels.TakeQuizViewModels
{
    public class TakeQuestionDisplayViewModel
    {
        public int QuestionId { get; set; }
        public int UId { get; set; }
        public string QuestionText { get; set; }
        public int Points { get; set; }
        public int? LocationId { get; set; }
        public List<TakeAnswerDisplayViewModel> Answers { get; set; }

        public TakeQuestionDisplayViewModel()
        {
        }

        public TakeQuestionDisplayViewModel(Question question)
        {
            Answers = new List<TakeAnswerDisplayViewModel>();

            QuestionText = question.QuestionText;
            QuestionId = question.Id;
            UId = question.uId;
            Points = question.Points;
            LocationId = question.LocationId;

            foreach (var questionAnswer in question.Answers)
            {
                var takeAnswer = new TakeAnswerDisplayViewModel(questionAnswer);
                Answers.Add(takeAnswer);
            }
        }

        public Question  GetQuestion()
        {
            var question = new Question();

            question.Id = QuestionId;
            question.QuestionText = QuestionText;
            question.Points = Points;

            foreach (var takeAnswerDisplayViewModel in Answers)
            {
                question.Answers.Add(takeAnswerDisplayViewModel.GetAnswer());
            }

            return question;
        }
    }
}