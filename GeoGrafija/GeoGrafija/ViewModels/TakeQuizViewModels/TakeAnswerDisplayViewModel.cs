using Model;

namespace GeoGrafija.ViewModels.TakeQuizViewModels
{
    public class TakeAnswerDisplayViewModel
    {
        public int AnswerId { get; set; }
        public int AnswerUid { get; set; }

        public int ParentQuestionId { get; set; }
        public int? ParentQuestionUid { get; set; }
        
        public string AnswerText { get; set; }
        public bool StudentAnswer { get; set; }

        public TakeAnswerDisplayViewModel()
        {
        }

        public TakeAnswerDisplayViewModel(Answer answer)
        {
            AnswerText = answer.AnswertText;
            StudentAnswer = false;

            AnswerId = answer.Id;
            AnswerUid = answer.uId;

            ParentQuestionId = answer.QuestionId;
            ParentQuestionUid = answer.questionUId;
        }

        public Answer GetAnswer()
        {
            var answer = new Answer();

            answer.Id = AnswerId;
            answer.QuestionId = ParentQuestionId;
            answer.isCorrectAnswer = StudentAnswer;
            answer.AnswertText = AnswerText;

            return answer;
        }
    }
}