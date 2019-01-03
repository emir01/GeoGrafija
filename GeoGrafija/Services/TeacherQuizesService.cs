using System;
using System.Collections.Generic;
using System.Linq;
using GeoGrafija.ResultClasses;
using Model;
using Model.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class TeacherQuizesService : ITeacherQuizesService
    {
        private IQuizRepository              _quizRepository;
        private IQuestionRepository      _questionRepository;
        private IAnswerRepository    _answerRepository;
        private IStudentQuizResultRepository _studentQuizResultRepository;

        private IBasicCrudService _basicCrudService;

        public TeacherQuizesService(IQuizRepository quizRepository, IQuestionRepository questionRepository, IAnswerRepository answerRepository, IStudentQuizResultRepository studentQuizResultRepository, IBasicCrudService basicCrudService)
        {
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _studentQuizResultRepository = studentQuizResultRepository;
            _basicCrudService = basicCrudService;
        }

        public GenericOperationResult<Quiz> CreateQuiz(Quiz quiz)
        {
            var result = OperationResult.GetGenericResultObject<Quiz>();

            result.SetStatus(_basicCrudService.CreateItem(quiz,_quizRepository,result));

            return result;
        }

        public GenericOperationResult<Quiz> CreateFullQuiz(Quiz quiz)
        {
            var result = OperationResult.GetGenericResultObject<Quiz>();

            try
            {
                var createQuizOk = _quizRepository.CreateItem(quiz);

                if (createQuizOk) { 
                    result.SetSuccess();
                    return result;
                }
                else
                {
                    result.AddMessage("Неуспешно зачувување на квиз!");
                    return result;
                }
            }
            catch (Exception e)
            {
                result.SetException();
                result.AddExceptionMessage(e.Message);
                return result;
            }
       }

        public GenericOperationResult<Question> CreateQuizQuestion(Question quizQuestion)
        {
            var result = OperationResult.GetGenericResultObject<Question>();

            result.SetStatus(_basicCrudService.CreateItem(quizQuestion, _questionRepository, result));

            return result;
        }

        public GenericOperationResult<Answer> CreateQuestionAnswer(Answer quizQuestion)
        {
            var result = OperationResult.GetGenericResultObject<Answer>();

            result.SetStatus(_basicCrudService.CreateItem(quizQuestion, _answerRepository, result));

            return result;
        }

        public GenericOperationResult<Quiz> GetQuiz(int quizId)
        {
            var result = OperationResult.GetGenericResultObject<Quiz>();

            result.SetStatus(_basicCrudService.GetItem(quizId, _quizRepository, result));

            return result;
        }

        public GenericOperationResult<Question> GetQuizQuestion(int quizQuestionId)
        {
            var result = OperationResult.GetGenericResultObject<Question>();

            result.SetStatus(_basicCrudService.GetItem(quizQuestionId, _questionRepository, result));

            return result;
        }

        public GenericOperationResult<Answer> GetQuestionAnswer(int answerId)
        {
            var result = OperationResult.GetGenericResultObject<Answer>();

            result.SetStatus(_basicCrudService.GetItem(answerId, _answerRepository, result));

            return result;
        }

        public GenericOperationResult<IEnumerable<Quiz>> GetAllQuizes()
        {
            var result = OperationResult.GetGenericResultObject<IEnumerable<Quiz>>();

            result.SetStatus(_basicCrudService.GetItems(_quizRepository,result));

            return result;
        }

        public GenericOperationResult<IEnumerable<Question>> GetAllQuizQuestions()
        {
            var result = OperationResult.GetGenericResultObject<IEnumerable<Question>>();

            result.SetStatus(_basicCrudService.GetItems(_questionRepository, result));

            return result;
        }

        public GenericOperationResult<IEnumerable<Answer>> GetAllQuestionAnswers()
        {
            var result = OperationResult.GetGenericResultObject<IEnumerable<Answer>>();

            result.SetStatus(_basicCrudService.GetItems(_answerRepository, result));

            return result;
        }

        public GenericOperationResult<IEnumerable<StudentQuizResult>> GetAllStudentResults()
        {
            var result = OperationResult.GetGenericResultObject<IEnumerable<StudentQuizResult>>();

            result.SetStatus(_basicCrudService.GetItems(_studentQuizResultRepository, result));

            return result;
        }

        public GenericOperationResult<IEnumerable<Quiz>> GetAllQuizes(string teacherName)
        {
            var result = OperationResult.GetGenericResultObject<IEnumerable<Quiz>>();

            result.SetStatus(_basicCrudService.GetItems(_quizRepository, result));


            var resultDataSet = result.GetData().Where(x => x!=null && x.User.UserName.Equals(teacherName));
            
            result.SetData(resultDataSet);
            return result;
        }

        public GenericOperationResult<IEnumerable<Question>> GetAllQuizQuestions(int teacherId)
        {
            var result = OperationResult.GetGenericResultObject<IEnumerable<Question>>();

            result.SetStatus(_basicCrudService.GetItems(_questionRepository, result));

            //Apply the filter by teacher id
            var allQuestions = result.GetData();
            var filteredQuestions = (from question in allQuestions let allQuizesForQuestion = question.Quizes where allQuizesForQuestion.Select(x => x.TeacherId).Contains(teacherId) select question).ToList();
            
            result.SetData(filteredQuestions);

            return result;
        }

        public GenericOperationResult<IEnumerable<Answer>> GetAllQuestionAnswers(int teacherId)
        {
            var result = OperationResult.GetGenericResultObject<IEnumerable<Answer>>();

            result.SetStatus(_basicCrudService.GetItems(_answerRepository, result));

            var allAnswers = result.GetData();
            var teacherAnswers = (from answer in allAnswers let asnwerQuestion = answer.Question let allQuizesForQuestion = asnwerQuestion.Quizes let allTeacherIds = allQuizesForQuestion.Select(x => x.TeacherId) where allTeacherIds.Contains(teacherId) select answer).ToList();

            result.SetData(teacherAnswers);

            return result;
        }

        public GenericOperationResult<IEnumerable<StudentQuizResult>> GetAllStudentResults(int teacherId)
        {
            var result = OperationResult.GetGenericResultObject<IEnumerable<StudentQuizResult>>();

            result.SetStatus(_basicCrudService.GetItems(_studentQuizResultRepository, result));

            //Apply the filter by teacher id
            var resultDataSet = result.GetData().Where(x => x.Quiz!=null &&  x.Quiz.TeacherId == teacherId);
            result.SetData(resultDataSet);

            return result;
        }
    }
}