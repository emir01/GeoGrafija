using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using GeoGrafija.Tests.FakeRepositories;
using GeoGrafija.Tests.Fake_Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Model.Interfaces;
using Services.Interfaces;
using Services;

namespace GeoGrafija.Tests.Service_Tests
{
    [TestClass]
    public class TeacherQuizesServiceTests
    {
        private ITeacherQuizesService _service;

        [TestInitialize]
        public void Setup()
        {
            var quizRepository = new MockQuizRepository();
            var quizQuestionRespository = new MockQuestionRepository();
            var questionAnswerRepository = new MockAnswerRepository();
            var studentQuizResultRepository = new MockStudentQuizResultRepository();

            var basicCrudService = new BasicCrudService(new LocalizedMessagesService());

            _service = new TeacherQuizesService(quizRepository,quizQuestionRespository,questionAnswerRepository,studentQuizResultRepository,basicCrudService);
        }

        [TestMethod]
        public void GetQuiz()
        {
            // Arrange 
            var id = 1;
            
            // Act
            var result = _service.GetQuiz(id);

            //Assert
            Assert.IsTrue(result.IsOK);
            var data = result.GetData() as Quiz;
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Id==id);
            Assert.AreEqual(data.Name, "Name"+id);
        }

        [TestMethod]
        public void GetAllQuizes()
        {
            // Act
            var result = _service.GetAllQuizes();

            // Assert 
            Assert.IsNotNull(result.IsOK);
            var data = result.GetData() as IEnumerable<Quiz>;
            Assert.IsNotNull(data);
            Assert.AreEqual(data.Count(), 5, "There should be 5 quizes returned by service.");
        }

        [TestMethod]
        public void GetAllQuizesForTeacher()
        {
            // Act
            var teacherName = "UserName"+1;
            var result = _service.GetAllQuizes(teacherName);

            // Assert
            Assert.IsTrue(result.IsOK);
            var data = result.GetData() as IEnumerable<Quiz>;
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Count() == 2);
        }

        [TestMethod]
        public void CreateQuiz()
        {
            // Arrange 
            var id = 100;
            var quiz = GetFakeQuiz.Entity(id);

            // Act
            var result = _service.CreateQuiz(quiz);

            // Assert
            Assert.IsTrue(result.IsOK);
            var insertedQuiz = _service.GetQuiz(id).GetData();
            Assert.IsNotNull(insertedQuiz);
            Assert.AreEqual(insertedQuiz.Id,id);
            Assert.AreEqual(insertedQuiz.Name,"Name"+id);
        }

        [TestMethod]
        public void GetQuizQuestion()
        {
            // Arrange
            var id = 1; 

            // Service 
            var result = _service.GetQuizQuestion(id);

            // Assert
            Assert.IsTrue(result.IsOK);
            var data = result.GetData() as QuizQuestion;
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Id == id);
            Assert.IsTrue(data.QuestionText=="QuestionText"+id);
        }

        [TestMethod]
        public void GetAllQuizQuestions()
        {
            // Act
            var result = _service.GetAllQuizQuestions();

            // Assert
            Assert.IsTrue(result.IsOK);
            Assert.IsTrue(result.GetData().Count()==2,"There should be 2 quiz questions");
        }

        [TestMethod]
        public void GetAllQuizQuestionsForTeacher()
        {
            // Arrange 
            var id = 9644;
            var teacherId = 2;
            var quizQuestion = GetFakeQuiz.Question(id);
            quizQuestion.Quiz = _service.GetQuiz(1).GetData();
            _service.CreateQuizQuestion(quizQuestion);

            // Act
            var result = _service.GetAllQuizQuestions(teacherId);

            // Assert
            Assert.IsTrue(result.IsOK);
            var data = result.GetData() as IEnumerable<QuizQuestion>;
            Assert.IsNotNull(data);
            var dataList = data.ToList();
            Assert.IsTrue(dataList.Count() == 1);
            Assert.IsTrue(dataList.ToList()[0].Id == id);
        }

        [TestMethod]
        public void CreateQuizQuestion()
        {
            // Arrange
            int id = 543;
            var quizQesution = GetFakeQuiz.Question(id);

            // Act
            var result = _service.CreateQuizQuestion(quizQesution);

            // Aseert
            Assert.IsTrue(result.IsOK);
            var data = _service.GetQuizQuestion(id).GetData() as QuizQuestion;
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Id == id);
            Assert.IsTrue(data.QuestionText == "QuestionText"+id);
        }

        [TestMethod]
        public void GetQuestionAnswer()
        {
            // Assert
            int id = 1;

            // Act
            var result = _service.GetQuestionAnswer(id);

            // Assert
            Assert.IsTrue(result.IsOK);
            var data = result.GetData() as QuestionAnswer;
            Assert.IsNotNull(data);
            Assert.AreEqual(id,data.Id);
            Assert.AreEqual(data.AnswertText,"AnswerText"+id);
        }

        [TestMethod]
        public void GetAllQuestionAnswers()
        {
            // Act
            var result = _service.GetAllQuestionAnswers();

            // Assert
            Assert.IsTrue(result.IsOK);
            var data = result.GetData();
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Count() == 2);
        }

        [TestMethod]
        public void CreateQuestionAnswer()
        {
            // Act
            var id = 942;
            var answer = GetFakeQuiz.Answer(id,true);

            // Assert
            var result = _service.CreateQuestionAnswer(answer);

            // Assert
            Assert.IsTrue(result.IsOK);
            var createdAnswer = _service.GetQuestionAnswer(id).GetData();
            Assert.IsNotNull(createdAnswer);
            Assert.IsTrue(createdAnswer.Id==id);
        }

        [TestMethod]
        public void GetAllStudentAnswers()
        {
            // Act
            var result = _service.GetAllStudentResults();

            // Assert
            Assert.IsTrue(result.IsOK);
            var data = result.GetData() as IEnumerable<StudentQuizResult>;
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Count()==2);
        }
       
        // All Other Get Teacher Specific
        // This tests require some extra mocking.
        [TestMethod]
        public void GetAllQuestionAnswersForTeacher()
        {
            // Arrange
            const int teacherId = 2;

            const int questionId = 23;
            var question = GetFakeQuiz.Question(questionId);
            question.Quiz = _service.GetQuiz(1).GetData();
            _service.CreateQuizQuestion(question);

            _service.GetQuestionAnswer(1).GetData().QuizQuestion = question;

            // Act
            var result = _service.GetAllQuestionAnswers(teacherId);

            // Assert
            Assert.IsTrue(result.IsOK);
            var data = result.GetData() as IEnumerable<QuestionAnswer>;
            Assert.IsNotNull(data);
            var dataAsList = data.ToList();
            Assert.IsTrue(dataAsList.Count==1);
            Assert.IsTrue(dataAsList[0].QuizQuestion.Id == question.Id);
            Assert.IsTrue(dataAsList[0].QuizQuestion.Quiz.Id == _service.GetQuiz(1).GetData().Id);
        }

        [TestMethod]
        public void GetAllStudentResultsForTeacher()
        {
            // Arrange 
            var teacherId = 2;
            _service.GetAllStudentResults().GetData().ToList()[0].Quiz = _service.GetQuiz(1).GetData();

            // Act
            var result = _service.GetAllStudentResults(teacherId);

            // Assert
            Assert.IsTrue(result.IsOK);
            var data = result.GetData().ToList();
            Assert.IsTrue(data.Count == 1);
            Assert.IsTrue(data[0].Quiz.Id == 1);
        }
    }
}
