using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Interfaces;
namespace Model.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private GeoGrafijaEntities _context;

        public AnswerRepository(IDbContext context)
        {
            _context= (GeoGrafijaEntities) context;
        }

        public bool CreateItem(Answer item)
        {
            _context.Answers.AddObject(item);
            SaveChanges();
            return true;
        }

        public Answer GetItem(int id)
        {
            return _context.Answers.Where(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<Answer> GetAllItems()
        {
            return _context.Answers;
        }

        public bool UpdateItem(Answer item)
        {
            var itemTmp = GetItem(item.Id);
            itemTmp.QuestionId = item.QuestionId;
            itemTmp.isCorrectAnswer = item.isCorrectAnswer;
            itemTmp.AnswertText = item.AnswertText;
            
            SaveChanges();
            return true;
        }

        public bool DeleteItem(Answer item)
        {
            _context.Answers.DeleteObject(item);
            
            SaveChanges();
            return true;
        }

        public bool DeleteItem(int id)
        {
            var item = GetItem(id);
            _context.Answers.DeleteObject(item);
            
            SaveChanges();
            return true;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
