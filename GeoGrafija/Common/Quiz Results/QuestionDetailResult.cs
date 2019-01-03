using System.Collections.Generic;

namespace Common.Quiz_Results
{
    public class QuestionDetailResult
    {
        public string QuestionText { get; set; }
        public int QuestionId { get; set; }
        public bool Correct { get; set; }
        public int Points { get; set; }
            
        public List<string> CorrectAnswers { get; set; }
        public List<string> UserAnswers { get; set; }

        public QuestionDetailResult()
        {
            Correct = false;
            CorrectAnswers  = new List<string>();        
            UserAnswers = new List<string>();
        }
    }
}