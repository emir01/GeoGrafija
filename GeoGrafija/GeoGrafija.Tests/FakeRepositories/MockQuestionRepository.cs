using System.Collections.Generic;
using System.Linq;
using GeoGrafija.Tests.Fake_Data;
using Model;
using Model.Interfaces;

namespace GeoGrafija.Tests.FakeRepositories
{
    public  class MockQuestionRepository : IQuestionRepository
    {
        private List<Question> _context;

        public MockQuestionRepository()
        {
            _context = new List<Question>();

            for (int i = 0; i < 2; i++)
            {
                var question = GetFakeQuiz.FullQuestion(i+1);
                _context.Add(question);
            }
        }

        public bool CreateItem(Question item)
        {
            _context.Add(item);
            
            return true;
        }

        public Question GetItem(int id)
        {
            return _context.Where(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<Question> GetAllItems()
        {
            return _context;
        }

        public bool UpdateItem(Question item)
        {
            var oldItem = GetItem(item.Id);

            oldItem.LocationId = item.LocationId;
            oldItem.QuizId = item.QuizId;
            oldItem.QuestionText = item.QuestionText;

            return true;
        }

        public bool DeleteItem(Question item)
        {
            var itemToDelete = GetItem(item.Id);
            _context.Remove(itemToDelete);

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
            
        }
    }
}
