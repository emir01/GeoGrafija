using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Interfaces;

namespace GeoGrafija.Tests.FakeRepositories
{
    public class MockResourceRepository : IResourceRepository
    {
        private List<Resource> _resources;
        
        public MockResourceRepository()
        {
            _resources = new List<Resource>();
            
            var resource1 = GetFullResource();
            var resource2 = GetFullResource();

            resource1.ID = 1;
            resource2.ID = 2;

            _resources.Add(resource1);
            _resources.Add(resource2);
        }

        private Resource GetFullResource()
        {
            var random = new Random();

            var resource = new Resource();
            resource.Text = "Resource Text " + random.Next(10000, 100000);
            resource.TypeId =1;
            resource.LocationId = 1;

            return resource;
        } 

        public bool CreateItem(Resource item)
        {
            _resources.Add(item);
            return true;
        }

        public Resource GetItem(int id)
        {
            return _resources.Where(x => x.ID == id).FirstOrDefault();
        }

        public IEnumerable<Resource> GetAllItems()
        {
            return _resources;
        }

        public bool UpdateItem(Resource item)
        {
            var resource = GetItem(item.ID);
            resource.Text = item.Text;
            return true;
        }

        public bool DeleteItem(Resource item)
        {
            _resources.Remove(GetItem(item.ID));
            return true;
        }

        public bool DeleteItem(int id)
        {
            _resources.Remove(GetItem(id));
            return true;
        }

        public void SaveChanges()
        {

        }
    }
}
