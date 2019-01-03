using System.Collections.Generic;
using System.Linq;
using GeoGrafija.Tests.Fake_Data;
using Model;
using Model.Interfaces;

namespace GeoGrafija.Tests.FakeRepositories
{
    public  class MockAnswerRepository : IAnswerRepository
    {
        private List<Answer> _context;

        public MockAnswerRepository()
        {
            _context = new List<Answer>();

            for (int i = 0; i < 2; i++)
            {
                var answer = GetFakeQuiz.Answer(i+1,i==0?true:false);
                _context.Add(answer);
            }    
        }

        public bool CreateItem(Answer item)
        {
            _context.Add(item);
            SaveChanges();
            return true;
        }

        public Answer GetItem(int id)
        {
            return _context.Where(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<Answer> GetAllItems()
        {
            return _context;
        }

        public bool UpdateItem(Answer item)
        {
            var itemTmp = GetItem(item.Id);
            itemTmp.QuestionId = item.QuestionId;
            itemTmp.isCorrectAnswer = item.isCorrectAnswer;
            itemTmp.AnswertText = item.AnswertText;

            return true;
        }

        public bool DeleteItem(Answer item)
        {
            _context.Remove(item);

            return true;
        }

        public bool DeleteItem(int id)
        {
            var item = GetItem(id);
            _context.Remove(item);

            return true;
        }

        public void SaveChanges()
        {
            
        }
    }
}
