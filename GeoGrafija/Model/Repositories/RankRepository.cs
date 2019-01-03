
using System.Collections.Generic;
using System.Linq;
using Model.Interfaces;

namespace Model.Repositories
{
    class RankRepository : IRankRepository
    {
        private readonly GeoGrafijaEntities _context;

        public RankRepository(IDbContext context)
        {
            _context = (GeoGrafijaEntities)context;
        }

        public bool CreateItem(Rank item)
        {
            _context.Ranks.AddObject(item);

            SaveChanges();
            return true;
        }

        public Rank GetItem(int id)
        {
            var item = _context.Ranks.Where(x => x.Id == id).FirstOrDefault();
            return item;
        }

        public IEnumerable<Rank> GetAllItems()
        {
            return _context.Ranks;
        }

        public bool UpdateItem(Rank item)
        {
            var oldItem = GetItem(item.Id);

            oldItem.RankName = item.RankName;
            oldItem.RequiredPoints = item.RequiredPoints;
            oldItem.ParentRankId = item.ParentRankId;
            oldItem.ParentRank = item.ParentRank;
            oldItem.RankImage = item.RankImage;

            SaveChanges();
            return true;
        }

        public bool DeleteItem(Rank item)
        {
            _context.Ranks.DeleteObject(item);

            SaveChanges();
            return true;
        }

        public bool DeleteItem(int id)
        {
            var item = GetItem(id);
            return DeleteItem(item);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}