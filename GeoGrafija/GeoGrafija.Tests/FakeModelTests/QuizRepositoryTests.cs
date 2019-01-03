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
    public class QuizRepositoryTests
    {
        private IQuizRepository repository;
        private const int ITEMS_COUNT = 5;

        [TestInitialize]
        public void Setup()
        {
            repository = new MockQuizRepository();
        }

        [TestMethod]
        public void Get()
        {
            // Arrange 
            int requestedId = 1;
            // Act
            var item = repository.GetItem(requestedId) as Quiz;

            // Assert
            Assert.IsNotNull(item);
            Assert.IsTrue(item.Id == requestedId);
        }

        [TestMethod]
        public void GetAll()
        {
            // Act
            var items = repository.GetAllItems() as IEnumerable<Quiz>;

            // Assert
            Assert.IsNotNull(items);
            Assert.IsTrue(items.Count() == ITEMS_COUNT);
        }

        [TestMethod]
        public void Create()
        {
            // Arrange
            var id = 888;
            var item = GetFakeQuiz.Entity(id);

            // Act
            var result = repository.CreateItem(item);

            // Assert
            Assert.IsTrue(result);
            var insertedItem = repository.GetItem(id);
            Assert.IsTrue(insertedItem.Id == id);
            Assert.IsTrue(insertedItem.Name == "Name" + id);
            var items = repository.GetAllItems();
            Assert.IsTrue(items.Count() == ITEMS_COUNT+1);
        }

        [TestMethod]
        public void UpdateItem()
        {
            // Arrange 
            var id = 1;
            var item = repository.GetItem(id);
            Assert.IsTrue(item.Name == "Name" + id);
            var newName = "NewName";
            item.Name = newName;

            // Act
            var success = repository.UpdateItem(item);

            // Assert
            Assert.IsTrue(success);
            var updatedItem = repository.GetItem(id);
            Assert.IsTrue(updatedItem.Name == newName);
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
