using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Interfaces
{
    public interface ICrudRepository<T>
    {
        bool CreateItem(T item);

        T GetItem(int id);
        IEnumerable<T> GetAllItems();

        bool UpdateItem(T item);

        bool DeleteItem(T item);
        bool DeleteItem(int id);
    }
}
