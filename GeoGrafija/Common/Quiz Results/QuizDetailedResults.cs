using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace Common.Quiz_Results
{
        public class QuizDetailedResults
        {
            public string QuizName { get; set; }
            public int QuizId { get; set; }

            public List<QuestionDetailResult> DetailedQuestions;

            public QuizDetailedResults()
            {
                DetailedQuestions  = new List<QuestionDetailResult>();
            }

            public void StoreCorrectQuestion(Question question)
            {
                var questionResult = new QuestionDetailResult();
                
                questionResult.Correct = true;
                questionResult.QuestionText = question.QuestionText;
                questionResult.QuestionId = question.Id;
                questionResult.Points = question.Points;

                foreach (var answer in question.Answers.Where(x=>x.isCorrectAnswer == true))
                {
                    questionResult.CorrectAnswers.Add(answer.AnswertText);
                    questionResult.UserAnswers.Add(answer.AnswertText);
                }

                DetailedQuestions.Add(questionResult);
            }

            public void StoreWrongQuestion(Question actualQuestion, Question studentQuestion)
            {
                var questionResult = new QuestionDetailResult();

                questionResult.Correct = false;
                questionResult.QuestionText = actualQuestion.QuestionText;
                questionResult.QuestionId = actualQuestion.Id;
                questionResult.Points = actualQuestion.Points;

                foreach (var correctAnswer in actualQuestion.Answers.Where(x=>x.isCorrectAnswer == true))
                {
                    questionResult.CorrectAnswers.Add(correctAnswer.AnswertText);
                }

                foreach (var userAnswer in studentQuestion.Answers.Where(x=>x.isCorrectAnswer == true))
                {
                    questionResult.UserAnswers.Add(userAnswer.AnswertText);
                }

                DetailedQuestions.Add(questionResult);
            }
        }
}
