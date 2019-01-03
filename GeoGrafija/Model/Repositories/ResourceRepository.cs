using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Interfaces;

namespace Model.Repositories
{
    public class ResourceRepository:IResourceRepository
    {
        private GeoGrafijaEntities context;

        public ResourceRepository(IDbContext context)
        {
            this.context = (GeoGrafijaEntities)context;
        }

        public bool CreateItem(Resource item)
        {
            context.Resources.AddObject(item);
            context.SaveChanges();
            return true;
        }

        public Resource GetItem(int id)
        {
            return context.Resources.Where(x => x.ID == id).FirstOrDefault();
        }

        public IEnumerable<Resource> GetAllItems()
        {
            return context.Resources;
        }

        public bool UpdateItem(Resource item)
        {
            var oldResource = GetItem(item.ID);
            
            oldResource.Text = item.Text;
            oldResource.Name = item.Name;
            
            SaveChanges();
            return true;
        }

        public bool DeleteItem(Resource item)
        {
            context.Resources.DeleteObject(item);
            SaveChanges();
            return true;
        }

        public bool DeleteItem(int id)
        {
            var item = GetItem(id);
            context.Resources.DeleteObject(item);
            SaveChanges();
            return true;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
