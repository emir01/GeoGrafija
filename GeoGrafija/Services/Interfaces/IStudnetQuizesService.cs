using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoGrafija.ResultClasses;
using Model;
using Services.Enums;
using Services.ResultClasses;

namespace Services.Interfaces
{
    public interface IStudnetQuizesService
    {
        // Get 
        GenericOperationResult<IEnumerable<Quiz>> GetAvailableQuies(string studentName, QuizGetType type);

        // Submit Answers
        GenericOperationResult<Answer> SubmitAnswers(int studentId, int quiId, List<Answer> quizAnswers);

        // Tries to start a quiz taking process by returning the quiz object
        GenericOperationResult<QuizForStudentResult> TakeQuiz(int quizId, string studentName);
        
        // Precss the quiz result from the student
        GenericOperationResult<StudentQuizResult> ProcessResults(Quiz takenQuiz, string studentName);

        // Process the student rank information based on the points.
       bool ProcessStudentRank(string studentName);
    }
}
