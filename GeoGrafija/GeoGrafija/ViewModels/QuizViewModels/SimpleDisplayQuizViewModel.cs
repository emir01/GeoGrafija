using System.ComponentModel;

namespace GeoGrafija.ViewModels.QuizViewModels
{
    public class SimpleDisplayQuizViewModel
    {
        [DisplayName("Име на Тест")]
        public string QuizName { get; set; }

        [DisplayName("Број на Прашања")]
        public int NumberOfQuestions { get; set;}

        [DisplayName("Вкупно поени")]
        public int TotalPoints { get; set; }

        //Number of students that took the quiz
        [DisplayName("Број на ученици")]
        public int TotalStudents { get; set; }
        
    }
}