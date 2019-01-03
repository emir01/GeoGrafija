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
    public class MockedResourceRepositoryTypeTests
    {
        private IResourceTypeRepository _resourceTypeRepository;
        private int _startItemCount = 1;

        [TestInitialize]
        public void Setup()
        {
            _resourceTypeRepository  =  new MockResourceTypeRepository();
        }

        [TestMethod]
        public void RepositoryShouldContainOneitems()
        {
            // Act
            var resources = _resourceTypeRepository.GetAllItems();

            // Assert
            Assert.IsTrue(resources.Count()==_startItemCount,"There should be only 1 resource type in mocked repository");
        }

        [TestMethod]
        public void GetItem()
        {
            var item = _resourceTypeRepository.GetItem(1);

            Assert.IsNotNull(item);
            Assert.AreEqual(item.ID,1);
        }

        [TestMethod]
        public void CreateItem()
        {
            // Arrange
            var itemId = 10;
            var item = GetFullResourceType(itemId);
            
            // Act
            var sucess = _resourceTypeRepository.CreateItem(item);
            
            // Assert
            Assert.IsTrue(sucess);
            Assert.AreEqual(_resourceTypeRepository.GetAllItems().Count(),_startItemCount+1,"New Item should be added");
            Assert.IsTrue(_resourceTypeRepository.GetAllItems().Where(x=>x.ID == itemId).Count()>0);
        }

        [TestMethod]
        public void DeleteItem()
        {
            // Arrange
            var itemId = 22;
            var mockItem = GetFullResourceType(itemId);
            var newTExt = "NewText";

            _resourceTypeRepository.CreateItem(mockItem);

            // Act
            var success = _resourceTypeRepository.DeleteItem(itemId);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(_resourceTypeRepository.GetAllItems().Count(),_startItemCount);
        }

        [TestMethod]
        public void UpdateItem()
        {
            // Arrange
            var itemId = 12;
            var mockItem = GetFullResourceType(itemId);
            var newName = "NewText";

            _resourceTypeRepository.CreateItem(mockItem);

            // Act
            var item = _resourceTypeRepository.GetItem(itemId);
            item.Name = newName;
            var succes = _resourceTypeRepository.UpdateItem(item);

            // Assert
            Assert.IsTrue(succes);
            Assert.AreEqual(_resourceTypeRepository.GetItem(itemId).Name,newName);
        }

        private ResourceType GetFullResourceType(int id)
        {
            var random = new Random();

            var resourceType = new ResourceType();
            resourceType.ID = id;
            resourceType.Name = "Randon Text " + random.Next(10000, 100000);

            return resourceType;
        }
    }
}
