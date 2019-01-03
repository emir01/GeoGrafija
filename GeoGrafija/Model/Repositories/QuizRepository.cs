using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Interfaces;
namespace Model.Repositories
{
    public class QuizRepository:IQuizRepository
    {
        private GeoGrafijaEntities _context;

        public QuizRepository(IDbContext context)
        {
            _context = (GeoGrafijaEntities) context;
        }

        public bool CreateItem(Quiz item)
        {
            _context.Quizs.AddObject(item);
            
            SaveChanges();
            return true;
        }

        public Quiz GetItem(int id)
        {
            return _context.Quizs.Where(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<Quiz> GetAllItems()
        {
            return _context.Quizs;
        }

        public bool UpdateItem(Quiz item)
        {
            var oldItem = GetItem(item.Id);

            oldItem.Name = item.Name;
            oldItem.Description = item.Description;

            SaveChanges();
            return true;}

        public bool DeleteItem(Quiz item)
        {
            _context.Quizs.DeleteObject(item);
            
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
