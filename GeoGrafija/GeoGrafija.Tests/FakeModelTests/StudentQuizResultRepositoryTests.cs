using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using GeoGrafija.Tests.FakeRepositories;
using GeoGrafija.Tests.Fake_Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Model.Interfaces;

namespace GeoGrafija.Tests.FakeModelTests
{
    [TestClass]
    public class StudentQuizResultRepositoryTests
    {
        private IStudentQuizResultRepository repository;
        private const int ITEMS_COUNT = 2;

        [TestInitialize]
        public void Setup()
        {
            repository = new MockStudentQuizResultRepository();
        }

        [TestMethod]
        public void Get()
        {
            // Arrange 
            int requestedId = 1;
            
            // Act
            var item = repository.GetItem(requestedId);

            // Assert
            Assert.IsNotNull(item);
            Assert.IsTrue(item.Id == requestedId);
        }

        [TestMethod]
        public void GetAll()
        {
            // Act
            var items = repository.GetAllItems() as IEnumerable<StudentQuizResult>;

            // Assert
            Assert.IsNotNull(items);
            Assert.IsTrue(items.Count() == ITEMS_COUNT);
        }

        [TestMethod]
        public void Create()
        {
            // Arrange
            var id = 888;
            var item = GetFakeQuiz.StudentResult(id);

            // Act
            var result = repository.CreateItem(item);

            // Assert
            Assert.IsTrue(result);
            var insertedItem = repository.GetItem(id);
            Assert.IsTrue(insertedItem.Id == id);
            var items = repository.GetAllItems();
            Assert.IsTrue(items.Count() == ITEMS_COUNT+1);
        }

        [TestMethod]
        public void UpdateItem()
        {
            //// Arrange 
            //var id = 1;
            //var item = repository.GetItem(id);
            //Assert.IsTrue(item.CorrectAnswers == id);
            //var newScore = 3;
            //item.CorrectAnswers = newScore;

            //// Act
            //var success = repository.UpdateItem(item);

            //// Assert
            //Assert.IsTrue(success);
            //var updatedItem = repository.GetItem(id);
            //Assert.IsTrue(updatedItem.CorrectAnswers == newScore);
        }

        [TestMethod]
        public void DeleteItem()
        {
            // Arrange 
            var id = 1;
            
            // Act
            var success = repository.DeleteItem(id);

            // Assert
            Assert.IsTrue(success);
            var allItems = repository.GetAllItems();
            Assert.IsTrue(allItems.Count() == ITEMS_COUNT -1 );
        }
    }
}
