using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoGrafija.ResultClasses;
using Model.Interfaces;

namespace Services.Interfaces
{
    public interface IBasicCrudService
    {
        bool CreateItem<T>(T item,ICrudRepository<T> repository,GenericOperationResult<T> or)where T:class,new();

        bool GetItem<T>(int id, ICrudRepository<T> repository,GenericOperationResult<T> or)where T:class,new();
        bool GetItems<T>(ICrudRepository<T> repository,GenericOperationResult<IEnumerable<T>> or);

        bool UpdateItem<T>(T item, ICrudRepository<T> repository, OperationResult or) where T : class;

        bool DeleteItem<T>(T item, ICrudRepository<T> repository, OperationResult or) where T : class;
        bool DeleteItem<T>(int id, ICrudRepository<T> repository, OperationResult or);

    }
}
