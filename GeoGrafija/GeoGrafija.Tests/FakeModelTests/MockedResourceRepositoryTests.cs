using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using GeoGrafija.Tests.FakeRepositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Model.Interfaces;

namespace GeoGrafija.Tests.FakeModelTests
{
    [TestClass]
    public class MockedResourceRepositoryTests
    {
        private IResourceRepository _resourceRepository;
        private int _startItemCount = 2;

        [TestInitialize]
        public void Setup()
        {
            _resourceRepository  =  new MockResourceRepository();
        }

        [TestMethod]
        public void RepositoryShouldContainTwoitems()
        {
            // Act
            var resources = _resourceRepository.GetAllItems();

            // Assert
            Assert.IsTrue(resources.Count()==_startItemCount,"There should be only 2 resources in mocked repository");
        }

        [TestMethod]
        public void GetItem()
        {
            var item = _resourceRepository.GetItem(1);

            Assert.IsNotNull(item);
            Assert.AreEqual(item.ID,1);
        }

        [TestMethod]
        public void CreateItem()
        {
            // Arrange
            var itemId = 10;
            var item = GetFullResource(itemId);
            
            // Act
            var sucess = _resourceRepository.CreateItem(item);
            
            // Assert
            Assert.IsTrue(sucess);
            Assert.AreEqual(_resourceRepository.GetAllItems().Count(),_startItemCount+1,"New Item should be added");
            Assert.IsTrue(_resourceRepository.GetAllItems().Where(x=>x.ID == itemId).Count()>0);
        }

        [TestMethod]
        public void DeleteItem()
        {
            // Arrange
            var itemId = 22;
            var mockItem = GetFullResource(itemId);
            var newTExt = "NewText";

            _resourceRepository.CreateItem(mockItem);

            // Act
            var success = _resourceRepository.DeleteItem(itemId);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(_resourceRepository.GetAllItems().Count(),_startItemCount);
        }

        [TestMethod]
        public void UpdateItem()
        {
            // Arrange
            var itemId = 12;
            var mockItem = GetFullResource(itemId);
            var newTExt = "NewText";

            _resourceRepository.CreateItem(mockItem);

            // Act
            var item = _resourceRepository.GetItem(itemId);
            item.Text = newTExt;
            var succes = _resourceRepository.UpdateItem(item);

            // Assert
            Assert.IsTrue(succes);
            Assert.AreEqual(_resourceRepository.GetItem(itemId).Text,newTExt);
        }

        private Resource GetFullResource(int id)
        {
            var random = new Random();

            var resource = new Resource();
            resource.ID = id;
            resource.Text = "Randon Text " + random.Next(10000, 100000);

            return resource;
        }
    }
}
