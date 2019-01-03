using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Interfaces;

namespace Model.Repositories
{
    public class StudentQuizResultRepository : IStudentQuizResultRepository
    {
        private GeoGrafijaEntities _context;

        public StudentQuizResultRepository(IDbContext context)
        {
            _context = (GeoGrafijaEntities) context;
        }

        public bool CreateItem(StudentQuizResult item)
        {
            _context.StudentQuizResults.AddObject(item);

            SaveChanges();
            return true;
        }

        public StudentQuizResult GetItem(int id)
        {
            return _context.StudentQuizResults.Where(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<StudentQuizResult> GetAllItems()
        {
            return _context.StudentQuizResults;
        }

        public bool UpdateItem(StudentQuizResult item)
        {
            var oldItem = GetItem(item.Id);
            
            oldItem.QuizId = item.QuizId;
            oldItem.StudentId = item.StudentId;
            oldItem.DateUpdate = item.DateUpdate;
            oldItem.DateOpen = item.DateOpen;
            oldItem.PointsStudent = item.PointsStudent;
            oldItem.PointsTotal = item.PointsTotal;
            
            SaveChanges();
            return true;
        }

        public bool DeleteItem(StudentQuizResult item)
        {
            return DeleteItem(item.Id);
        }

        public bool DeleteItem(int id)
        {
            var tmpItem = GetItem(id);
            _context.StudentQuizResults.DeleteObject(tmpItem);
            SaveChanges();
            return true;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
