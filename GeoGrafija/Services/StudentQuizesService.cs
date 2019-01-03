using System;
using System.Collections.Generic;
using System.Linq;
using Common.Quiz_Results;
using GeoGrafija.ResultClasses;
using Model;
using Model.Interfaces;
using Services.Enums;
using Services.Interfaces;
using Services.ResultClasses;

namespace Services
{
    public class StudentQuizesService : IStudnetQuizesService
    {
        //  repositories
        private readonly ILocalizedMessagesService _messagesService;
        private readonly IAnswerRepository _answerRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IStudentQuizResultRepository _studentQuizResultRepository;
        private readonly IUserRepository _userRepository;

        // services
        private readonly IUserService _userService;
        private IBasicCrudService _basicCrudService;

        public  StudentQuizesService(IQuizRepository quizRepository, 
                                     IQuestionRepository questionRepository,
                                     IAnswerRepository answerRepository,
                                     IStudentQuizResultRepository studentQuizResultRepository,
                                     IBasicCrudService basicCrudService, IUserService userService,
                                     ILocalizedMessagesService messagesService, IUserRepository userRepository)
        {
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _studentQuizResultRepository = studentQuizResultRepository;
            _basicCrudService = basicCrudService;
            _userService = userService;
            _messagesService = messagesService;
            _userRepository = userRepository;
        }

        #region IStudnetQuizesService Members

        public GenericOperationResult<IEnumerable<Quiz>> GetAvailableQuies(string studentName, QuizGetType type)
        {
            GenericOperationResult<IEnumerable<Quiz>> result =
                OperationResult.GetGenericResultObject<IEnumerable<Quiz>>();

            // check student  
            User student = _userService.GetUser(studentName);

            if (student == null)
            {
                result.AddMessage(_messagesService.GetErrorMessage(ErrorMessageKeys.UserNotFound));
                return result;
            }
            else
            {
                switch (type)
                {
                    case QuizGetType.Pregenarated:
                        return GetPredefinedQuizes(student);
                        break;
                }
            }

            return result;
        }

        public GenericOperationResult<Answer> SubmitAnswers(int studentId, int quiId,
                                                                    List<Answer> quizAnswers)
        {
            throw new NotImplementedException();
        }

        // either returns a pregenerated quiz or creates a new quiz from random question
        public GenericOperationResult<QuizForStudentResult> TakeQuiz(int quizId, string studentName)
        {
            // check if valid student
            User student;
            var validStudent = CheckStudent(studentName);
            
            if (validStudent.IsOK)
            {
                student = validStudent.GetData();
            }
            else
            {
                var result = OperationResult.GetGenericResultObject<QuizForStudentResult>();
                result.AddMessage(_messagesService.GetErrorMessage(ErrorMessageKeys.EntityNotFound));
                return result;
            }

            if (quizId == 0)
            {
                return GenerateRandomQuiz(student);
            }
            else
            {
                return PredfinedQuiz(quizId, student);
            }
        }

        public GenericOperationResult<StudentQuizResult> ProcessResults(Quiz takenQuiz, string studentName)
        {
            var result = OperationResult.GetGenericResultObject<StudentQuizResult>();
            try
            {
                //create a new student quiz result 
                var studentQuizResult = new StudentQuizResult();
                
                //get the actual quiz and student that took the quiz
                var actualQuiz = _quizRepository.GetItem(takenQuiz.Id);
                var student = _userService.GetUser(studentName);

                //compare the results
                var compareResults = CompareQuizes(actualQuiz, takenQuiz);

                // get the measurments from the compared result and the actual quiz
                var totalPoints = actualQuiz.Questions.Sum(x => x.Points);
                var studentPoints = compareResults.StudentPoints;

                // populate the student quiz result object
                var date = DateTime.Now;
                studentQuizResult.DateUpdate = date;
                studentQuizResult.DateOpen = date;
                studentQuizResult.QuizId = actualQuiz.Id;
                studentQuizResult.StudentId = student.UserID;
                studentQuizResult.PointsTotal = totalPoints;
                studentQuizResult.PointsStudent = studentPoints;
                studentQuizResult.CorrectQuestions = compareResults.CorrectQuestions;
                studentQuizResult.DetailResult =
                    DetailResultSerializer.SerializeDetailedQuizResultToXml(compareResults.QuizDetailResults);

                //store the new student quiz result object
                _studentQuizResultRepository.CreateItem(studentQuizResult);

                ProcessStudentRank(student.UserName);
                
                var actualStudentQuizResult =
                    _studentQuizResultRepository.GetAllItems().Where(
                        x => x.StudentId == student.UserID && x.QuizId == actualQuiz.Id && x.DateUpdate >= date).FirstOrDefault();

                result.SetSuccess();
                result.SetData(actualStudentQuizResult);
                return result;
            }
            catch (Exception e)
            {
                result.SetException();
                result.AddMessage(e.Message);
                result.AddExceptionMessage(e.Message);
                return result;
            }
        }

        public bool ProcessStudentRank(string studentName)
        {
            var student = _userService.GetUser(studentName);

            if (student == null)
            {
                return false;
            }

            var currentRank = student.Rank;
            var currentPoints = _userService.CalculateStudentTotalPoints(student.UserName);

            if (currentRank.ParentRank == null)
            {
                // we are hightest rank
                return true;
            }

            if (currentPoints >= currentRank.ParentRank.RequiredPoints)
            {
                return AdvanceStudentRank(student, currentRank);
            }
            else
            {
                return true;
            }
            
            return false;
        }

        private bool AdvanceStudentRank(User student, Rank currentRank)
        {
            if (currentRank.ParentRankId == null)
            {
                return true;
            }
            else
            {
                student.CurrentRank = (int)currentRank.ParentRankId;
            }

            try
            {
                _userRepository.UpdateUser(student);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
            return false;
        }

        // The main logic of the quiz processing results. Compares 2 quizes.
        private QuizCompareResult CompareQuizes(Quiz actualQuiz, Quiz takenQuiz)
        {
            var compareResult = new QuizCompareResult();

            compareResult.QuizDetailResults.QuizId = actualQuiz.Id;
            compareResult.QuizDetailResults.QuizName = actualQuiz.Name;

            foreach (var quizQuestion in actualQuiz.Questions)
            {
                var questionFromStudent = takenQuiz.Questions.Where(x => x.Id == quizQuestion.Id).FirstOrDefault();

                //  the correct answers for the question
                var correctAnswers = quizQuestion.Answers.Where(x => x.isCorrectAnswer == true);

                // the student correct answers for the question
                var studentCorrectAnswers = questionFromStudent.Answers.Where(x => x.isCorrectAnswer == true);

                //compare the 2 enumerables
                var questionCorrect = true;
                
                foreach (var answer in correctAnswers)
                {
                    var reflectionAnswer = studentCorrectAnswers.Where(x => x.Id == answer.Id).FirstOrDefault();

                    if (reflectionAnswer ==null || reflectionAnswer.Id != answer.Id)
                    {
                        questionCorrect = false;
                        break;
                    }
                }

                if (studentCorrectAnswers.Count() != correctAnswers.Count())
                {
                    questionCorrect = false;
                }

                if (questionCorrect)
                {
                    compareResult.StudentPoints += quizQuestion.Points;
                    compareResult.CorrectQuestions += 1;
                    compareResult.QuizDetailResults.StoreCorrectQuestion(quizQuestion);
                }
                else
                {
                    compareResult.QuizDetailResults.StoreWrongQuestion(quizQuestion, questionFromStudent); 
                }
            }
           
            return compareResult;
        }

        #endregion

        private  GenericOperationResult<User> CheckStudent(string studentName)
        {
            var result = OperationResult.GetGenericResultObject<User>();

            try
            {
                var user = _userService.GetUser(studentName);

                if (user != null)
                {
                    result.SetData(user);
                    result.SetSuccess();
                    return result;
                }
                else
                {
                    result.AddMessage(_messagesService.GetErrorMessage(ErrorMessageKeys.EntityNotFound));
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

        private GenericOperationResult<QuizForStudentResult> PredfinedQuiz(int quizId, User student)
        {
            var result = OperationResult.GetGenericResultObject<QuizForStudentResult>();
            
            try
            {
                var quiz = _quizRepository.GetItem(quizId);

                if(quiz == null)
                {
                    result.AddMessage(_messagesService.GetErrorMessage(ErrorMessageKeys.EntityNotFound));
                    return result;
                }
                else
                {
                    var data = new QuizForStudentResult();
                    data.Student = student;
                    data.Quiz = quiz;

                    result.SetData(data);
                    result.SetSuccess();

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

        private GenericOperationResult<QuizForStudentResult> GenerateRandomQuiz(User student)
        {
            var result = OperationResult.GetGenericResultObject<QuizForStudentResult>();

            try
            {
                var newRandomQuiz = new Quiz();
                newRandomQuiz.Name = "Случаен Квиз";
                newRandomQuiz.IsRandom = true;
                newRandomQuiz.TypeId = 1;
                newRandomQuiz.TeacherId = 1; 
                
                var allQuestions = _questionRepository.GetAllItems();
                var allQuizes = _quizRepository.GetAllItems();
                var allRandomQuizes = allQuizes.Where(x => x.IsRandom == true).ToList();

                int nameNumber = 1;
                if (allRandomQuizes.Count() > 0) { 
                    var randomQuizesName = allRandomQuizes.Select(x => x.Name);
                    var randomQuizNameNumbers = randomQuizesName.Select(x => Int32.Parse(x.Split(' ')[2]));
                    var maxNumber = randomQuizNameNumbers.Max();
                    nameNumber = maxNumber+1;
                }

                newRandomQuiz.Name += " " + nameNumber.ToString();

                // pick 5 random questions.
                var randomQuestions = PickRandomQuestions(allQuestions, 5);

                //add the random picked questions
                foreach (var randomQuestion in randomQuestions)
                {
                    newRandomQuiz.Questions.Add(randomQuestion);    
                }
                
                _quizRepository.CreateItem(newRandomQuiz);

                var quizForStudent = new QuizForStudentResult();
                quizForStudent.Quiz = newRandomQuiz;
                quizForStudent.Student = student;
                
                result.SetSuccess();
                result.SetData(quizForStudent);
            }
            catch (Exception ex)
            {
                result.SetException();
                result.AddMessage(ex.Message);
                result.AddExceptionMessage(ex.Message);
                return result;
            }
            return result;
        }


        private IEnumerable<QuizQuestion> PickRandomQuestions<QuizQuestion>(IEnumerable<QuizQuestion> someTypes, int maxCount)
        {
            Random random = new Random(DateTime.Now.Millisecond);

            Dictionary<double, QuizQuestion> randomSortTable = new Dictionary<double, QuizQuestion>();

            foreach (QuizQuestion someType in someTypes)
                randomSortTable[random.NextDouble()] = someType;

            return randomSortTable.OrderBy(KVP => KVP.Key).Take(maxCount).Select(KVP => KVP.Value);
        }

        private GenericOperationResult<IEnumerable<Quiz>> GetPredefinedQuizes(User student)
        {
            GenericOperationResult<IEnumerable<Quiz>> result =
                OperationResult.GetGenericResultObject<IEnumerable<Quiz>>();
            try
            {
                // get all the tests information that  the student has taken
                var quizesStudentHasTaken = _studentQuizResultRepository.GetAllItems().ToList().Where(x=>x.StudentId.Equals(student.UserID));

                //get all the quizes 
                var allQuizes = _quizRepository.GetAllItems().Where(x=> x.IsRandom == null || x.IsRandom == false);

                //filter only non taken quizes
                var takenQuizesIds = quizesStudentHasTaken.Select(x => x.QuizId).ToList();

                var filteredQuizes = (from quiz in allQuizes let quizId = quiz.Id where !takenQuizesIds.Contains(quizId) select quiz).ToList();

                //if everything goes well
                result.SetData(filteredQuizes);
                result.SetSuccess();
                return result;
            }
            catch (Exception e)
            {
                result.SetException();
                result.AddExceptionMessage(e.Message);
            }

            return result;
        }
    }
}

