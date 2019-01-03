using System.Collections.Generic;
using System.Linq;
using GeoGrafija.Tests.Fake_Data;
using Model;
using Model.Interfaces;

namespace GeoGrafija.Tests.FakeRepositories
{
    public class MockStudentQuizResultRepository : IStudentQuizResultRepository
    {
        private List<StudentQuizResult> _context;

        public MockStudentQuizResultRepository()
        {
            _context = new List<StudentQuizResult>();

            for (int i = 1; i <= 2; i++)
            {
                var result = GetFakeQuiz.StudentResult(i);
                _context.Add(result);
            }
        }

        public bool CreateItem(StudentQuizResult item)
        {
            _context.Add(item);

            return true;
        }

        public StudentQuizResult GetItem(int id)
        {
            return _context.Where(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<StudentQuizResult> GetAllItems()
        {
            return _context;
        }

        public bool UpdateItem(StudentQuizResult item)
        {
            var oldItem = GetItem(item.Id);
            
            oldItem.QuizId = item.QuizId;
            oldItem.StudentId = item.StudentId;
            oldItem.DateUpdate = item.DateUpdate;
            oldItem.DateOpen = item.DateOpen;
            oldItem.PointsTotal = item.PointsTotal;
            oldItem.PointsStudent = item.PointsStudent;
            
            return true;
        }

        public bool DeleteItem(StudentQuizResult item)
        {
            return DeleteItem(item.Id);
        }

        public bool DeleteItem(int id)
        {
            var tmpItem = GetItem(id);
            _context.Remove(tmpItem);
            
            return true;
        }

        public void SaveChanges()
        {
         
        }
    }
}
