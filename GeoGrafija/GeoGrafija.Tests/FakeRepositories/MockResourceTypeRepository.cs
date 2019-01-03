using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Model.Interfaces;

namespace GeoGrafija.Tests.FakeRepositories
{
    public class MockResourceTypeRepository:IResourceTypeRepository
    {
        private List<ResourceType> _resourceTypes;

        public  MockResourceTypeRepository()
        {
            _resourceTypes = new List<ResourceType>();

            var resourceType = GetFullResourceType();

            _resourceTypes.Add(resourceType);
        }

        private ResourceType GetFullResourceType()
        {
            var resourceType = new ResourceType();
            resourceType.ID = 1;
            resourceType.Name = "Text";
            return resourceType;
        }

        public bool CreateItem(ResourceType item)
        {
            _resourceTypes.Add(item);
            return true;
        }
        
        public ResourceType GetItem(int id)
        {
            return _resourceTypes.Where(x => x.ID == id).FirstOrDefault();
        }

        public IEnumerable<ResourceType> GetAllItems()
        {
            return _resourceTypes;
        }

        public bool UpdateItem(ResourceType item)
        {
            var resourceType = GetItem(item.ID);
            resourceType.Name = item.Name;
            return true;
        }

        public bool DeleteItem(ResourceType item)
        {
            _resourceTypes.Remove(GetItem(item.ID));
            return true;
        }

        public bool DeleteItem(int id)
        {
            _resourceTypes.Remove(GetItem(id));
            return true;
        }

        public void SaveChanges()
        {
            
        }
    }
}
