using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using GeoGrafija.Tests.FakeRepositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Services;
using Services.Interfaces;

namespace GeoGrafija.Tests.Service_Tests
{
    [TestClass]
    public class ResourceServiceTests
    {
        private IResourceService _service;
        private ILocationService _locationService;

        [TestInitialize]
        public void Setup()
        {
            var resourceRespository     = new MockResourceRepository();
            var resourceTypeRespository = new MockResourceTypeRepository();
            var locationRespository = new FakeLocationRepository();
            var localizedMessageService = new LocalizedMessagesService();
            var crudService = new BasicCrudService(localizedMessageService);

            _service = new ResourceService(resourceRespository
                                          ,resourceTypeRespository
                                          ,crudService
                                          ,locationRespository
                                          ,localizedMessageService
                                          );

            // Location service is already tested and it works . 
            // We share share the repository.
            _locationService =  new LocationService(locationRespository);
        }

        [TestMethod]
        public void GetResource()
        {
            // Arrange 
            var id = 1;

            // Act
            var result = _service.GetResource(id);

            // Assert
            Assert.IsTrue(result.IsOK,"Returned result should be OK.");
            Assert.AreEqual(result.GetData().ID,id,"The returned resource should be the requested resource");
        }

        [TestMethod]
        public void GetResourceType()
        {
            // Arrange 
            var id = 1;

            // Act
            var result = _service.GetResourceType(id);

            // Assert
            Assert.IsTrue(result.IsOK, "Returned result should be OK.");
            Assert.AreEqual(result.GetData().ID, id, "The returned resource type should be the requested resource type");
        }

        [TestMethod]
        public void CreateResource()
        {
            // Arrange 
            var id = 987;
            var text = "TTT";
            var resource = GetResourceEntity();
            resource.ID = 987;
            resource.Text = text;

            // Act
            var result = _service.CreateResource(resource);

            // Assert 
            Assert.IsTrue(result.IsOK, "Create resource resould should be OK.");
            resource = _service.GetResource(987).GetData();
            Assert.AreEqual(resource.ID,id);
            Assert.AreEqual(resource.Text, text);
        }

        [TestMethod]
        public void CreateResourceType()
        {
            // Arrange
            var resourceType = GetResourceTypeEntity();

            // Act
            var result = _service.CreateResourceType(resourceType);

            // Assert
            Assert.IsTrue(result.IsOK,"Create Resource Type result should be OK.");
            resourceType = _service.GetResourceType(999).GetData();
            Assert.AreEqual(resourceType.ID,999,"Resource type should be stored with correct id");
            Assert.AreEqual(resourceType.Name, "TEXT","Resource type should be stored with corect name");
        }

        [TestMethod]
        public void GetAllResources()
        {
            // Act
            var result = _service.GetAllResources();

            // Assert
            Assert.IsTrue(result.IsOK);
            Assert.AreEqual(result.GetData().Count(),2);
        }

        [TestMethod]
        public void GetAllResourceTypes()
        {
            // Act
            var result = _service.GetAllResourceTypes();

            // Assert
            Assert.IsTrue(result.IsOK);
            Assert.AreEqual(result.GetData().Count(), 1);
        }

        [TestMethod]
        public void AddResourceToLocaton()
        {   
            // Arrange
            var resource = _service.GetResource(1).GetData();

            // Act 
            var result = _service.AddResourceToLocation(1, resource.ID);

            // Assert
            Assert.IsTrue(result.IsOK);
            resource = _service.GetResource(1).GetData();
            Assert.IsTrue(resource.LocationId==1); 
        }

        [TestMethod]
        public void RemoveResourceFromLocation()
        {
            // Arrange
            var resource = _service.GetResource(1).GetData();
            _service.AddResourceToLocation(1, resource.ID);

            // Act 
            var result = _service.RemoveResourceFromLocaton(1, resource.ID);

            // Assert
            Assert.IsTrue(result.IsOK);
            resource = _service.GetResource(1).GetData();
            Assert.IsTrue(resource.LocationId == null); 
            
        }

        #region Helpers
        
        private Resource GetResourceEntity()
        {
            var resource = new Resource();
            resource.ID = 999;
            resource.Text = "Resource Text";
            return resource;
        }

        private ResourceType GetResourceTypeEntity()
        {
            var resourceType = new ResourceType();
            resourceType.ID = 999;
            resourceType.Name = "TEXT";
            return resourceType;
        }

        #endregion
    }
}
