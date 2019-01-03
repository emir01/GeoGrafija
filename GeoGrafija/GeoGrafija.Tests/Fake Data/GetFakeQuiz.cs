using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace GeoGrafija.Tests.Fake_Data
{
    public static class GetFakeQuiz
    {
        public static Quiz Entity(int id)
        {
            var quiz = new Quiz();

            quiz.Id = id;
            quiz.Name = "Name"+id;
            quiz.Description = "Description" + id;

            return quiz;
        }

        public static Question Question(int id)
        {
            var quizQestion = new Question();

            quizQestion.Id = id;
            quizQestion.QuestionText = "QuestionText"+id;

            return quizQestion;
        }

        public static Question FullQuestion(int id)
        {
            var question = GetFakeQuiz.Question(id);

            for (int i = 0; i < 2; i++)
            {
                var answer = GetFakeQuiz.Answer((id * 100) + i, (i == 0 ? true : false));
                question.Answers.Add(answer);
            }

            return question;
        }

        public static Answer Answer(int id, bool isCorrect)
        {
            var answer = new Answer();

            answer.Id = id;
            answer.isCorrectAnswer = isCorrect;
            answer.AnswertText = "AnswerText" + id;

            return answer;
        }
        
        public static StudentQuizResult StudentResult(int id)
        {
            var result = new StudentQuizResult();

            result.Id = id;
            result.DateOpen = DateTime.Now.AddDays(-1);
            result.DateUpdate = DateTime.Now;
            result.PointsTotal = id+2;
            result.PointsStudent = id+1;

            return result;
        }
    }
}
