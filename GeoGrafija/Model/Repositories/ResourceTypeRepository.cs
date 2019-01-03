using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Interfaces;

namespace Model.Repositories
{
    public class ResourceTypeRepository:IResourceTypeRepository
    {
        private GeoGrafijaEntities context;

        public ResourceTypeRepository(IDbContext context)
        {
            this.context = (GeoGrafijaEntities)context;
        }

        public bool CreateItem(ResourceType item)
        {
            context.ResourceTypes.AddObject(item);
            SaveChanges();
            return true;
        }

        public ResourceType GetItem(int id)
        {
            return context.ResourceTypes.Where(x => x.ID == id).FirstOrDefault();
        }

        public IEnumerable<ResourceType> GetAllItems()
        {
            return context.ResourceTypes;
        }

        public bool UpdateItem(ResourceType item)
        {
            var oldItem = GetItem(item.ID);
            
            oldItem.Name = item.Name;
            oldItem.Description = item.Description;
            
            SaveChanges();
            return true;
        }

        public bool DeleteItem(ResourceType item)
        {
            context.ResourceTypes.DeleteObject(item);
            return true;
        }

        public bool DeleteItem(int id)
        {
            var item = GetItem(id);
            context.ResourceTypes.DeleteObject(item);
            return true;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
