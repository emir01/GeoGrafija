using System.Collections.Generic;
using System.Linq;
using GeoGrafija.Tests.Fake_Data;
using Model;
using Model.Interfaces;

namespace GeoGrafija.Tests.FakeRepositories
{
    public class MockQuizRepository:IQuizRepository
    {
        private List<Quiz> _context;

        public MockQuizRepository()
        {
            _context = new List<Quiz>();

            for (int i = 0; i < 5; i++)
            {
                var quiz = GetFakeQuiz.Entity(i+1);

                if ((i+1) % 2 == 0)
                    quiz.TeacherId = 1;
                else
                    quiz.TeacherId = 2;

                quiz.QuizQuestions.Add(GetFakeQuiz.FullQuestion((i+1)));
                quiz.QuizQuestions.Add(GetFakeQuiz.FullQuestion((i+1)));
                _context.Add(quiz);
            }
        }

        public bool CreateItem(Quiz item)
        {
            _context.Add(item);
            return true;
        }

        public Quiz GetItem(int id)
        {
            return _context.Where(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<Quiz> GetAllItems()
        {
            return _context;
        }

        public bool UpdateItem(Quiz item)
        {
            var oldItem = GetItem(item.Id);

            oldItem.Name = item.Name;
            oldItem.Description = item.Description;

            return true;
        }

        public bool DeleteItem(Quiz item)
        {
            _context.Remove(item);
            
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
