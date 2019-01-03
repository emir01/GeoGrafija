using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoGrafija.ResultClasses;
using Model;

namespace Services.Interfaces
{
    public interface ITeacherQuizesService
    {
        
        GenericOperationResult<Quiz>           CreateQuiz(Quiz quiz);
        
        /// <summary>
        /// Creates afull quiz entity. Iterating through all the questions and their answers, storing them in the database.
        /// </summary>
        /// <param name="quiz">A full quiz entity with all the questions and answers</param>
        /// <returns></returns>
        GenericOperationResult<Quiz>           CreateFullQuiz(Quiz quiz);
        GenericOperationResult<Question>   CreateQuizQuestion(Question quizQuestion);
        GenericOperationResult<Answer> CreateQuestionAnswer(Answer quizQuestion);

        // Single gets for Teacher Quiz service
        GenericOperationResult<Quiz>           GetQuiz(int quizId);
        GenericOperationResult<Question>   GetQuizQuestion(int quizQuestionId);
        GenericOperationResult<Answer> GetQuestionAnswer(int answerId);

        //General Get All 
        GenericOperationResult<IEnumerable<Quiz>>              GetAllQuizes();
        GenericOperationResult<IEnumerable<Question>>      GetAllQuizQuestions();
        GenericOperationResult<IEnumerable<Answer>>    GetAllQuestionAnswers();
        GenericOperationResult<IEnumerable<StudentQuizResult>> GetAllStudentResults();

        // Specific Per Teacher Get All
        GenericOperationResult<IEnumerable<Quiz>> GetAllQuizes(string teacherName);
        GenericOperationResult<IEnumerable<Question>>      GetAllQuizQuestions(int teacherId);
        GenericOperationResult<IEnumerable<Answer>>    GetAllQuestionAnswers(int teacherId);
        GenericOperationResult<IEnumerable<StudentQuizResult>> GetAllStudentResults(int teacherId);
    }
}
