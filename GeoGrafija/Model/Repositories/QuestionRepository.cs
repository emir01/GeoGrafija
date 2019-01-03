using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using  Model.Interfaces;

namespace Model.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private GeoGrafijaEntities _context;

        public QuestionRepository(IDbContext context)
        {
            _context = (GeoGrafijaEntities) context;
        }

        public bool CreateItem(Question item)
        {
            _context.Questions.AddObject(item);

            SaveChanges();
            return true;
        }

        public Question GetItem(int id)
        {
            return _context.Questions.Where(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<Question> GetAllItems()
        {
            return _context.Questions;
        }

        public bool UpdateItem(Question item)
        {
            var oldItem = GetItem(item.Id);

            oldItem.LocationId = item.LocationId;
            oldItem.QuizId = item.QuizId;
            oldItem.QuestionText = item.QuestionText;

            SaveChanges();
            return true;
        }

        public bool DeleteItem(Question item)
        {
            var itemToDelete = GetItem(item.Id);
            _context.Questions.DeleteObject(itemToDelete);

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
