using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoGrafija.ResultClasses;
using Model.Interfaces;
using Services.Enums;
using Services.Interfaces;

namespace Services
{
    public class BasicCrudService:IBasicCrudService
    {
        private readonly ILocalizedMessagesService _localizedMessagesService;

        public BasicCrudService(ILocalizedMessagesService localizedMessagesService)
        {
            _localizedMessagesService = localizedMessagesService;
        }

        public bool CreateItem<T>(T item, ICrudRepository<T> repository, GenericOperationResult<T> or) where T : class, new()
        {
            if(item == null)
            {
                or.AddMessage(_localizedMessagesService.GetErrorMessage(ErrorMessageKeys.NullObjectReference));
                or.SetFail();
                return false;
            }

            try
            {
                var success = repository.CreateItem(item);

                if (success)
                {
                    or.SetData(item);
                    return true;
                }
                else
                {
                    or.AddMessage(_localizedMessagesService.GetErrorMessage(ErrorMessageKeys.RepositoryError));
                    return false;
                }
            }
            catch (Exception e)
            {
                or.AddMessage(_localizedMessagesService.GetErrorMessage(ErrorMessageKeys.RepositoryError));
                return false;
            }
        }

        public bool GetItem<T>(int id, ICrudRepository<T> repository, GenericOperationResult<T> or) where T : class, new()
        {
            if (id == 0)
            {
                or.AddMessage(_localizedMessagesService.GetErrorMessage(ErrorMessageKeys.InvalidId));
                return false;
            }

            try
            {
                var item = repository.GetItem(id);
                or.SetData(item);
                return true;
            }
            catch (Exception e)
            {
                or.AddMessage(_localizedMessagesService.GetErrorMessage(ErrorMessageKeys.RepositoryError));
                return false;
            }}

        public bool GetItems<T>(ICrudRepository<T> repository, GenericOperationResult<IEnumerable<T>> or)
        {
            try
            {
                var items = repository.GetAllItems();
                or.SetData(items);
                return true;
            }
            catch (Exception e)
            {
                or.AddMessage(_localizedMessagesService.GetErrorMessage(ErrorMessageKeys.RepositoryError));
                return false;
            }
        }

        public bool UpdateItem<T>(T item, ICrudRepository<T> repository, OperationResult or) where T:class 
        {
            if (item == null) 
            {
                or.AddMessage(_localizedMessagesService.GetErrorMessage(ErrorMessageKeys.NullObjectReference));
                return false;
            }
            
            try
            {
                return repository.UpdateItem(item); 
            }
            catch (Exception)
            {
                or.AddMessage(_localizedMessagesService.GetErrorMessage(ErrorMessageKeys.RepositoryError));
                return false;
            }
        }

        public bool DeleteItem<T>(T item, ICrudRepository<T> repository, OperationResult or)where T:class 
        {
            if (item == null) 
             { 
                or.AddMessage(_localizedMessagesService.GetErrorMessage(ErrorMessageKeys.NullObjectReference));
                return false;
            }

            try
            {
                return repository.DeleteItem(item);    
            }
            catch (Exception)
            {
                or.AddMessage(_localizedMessagesService.GetErrorMessage(ErrorMessageKeys.RepositoryError));
                return false;
            }
        }

        public bool DeleteItem<T>(int id, ICrudRepository<T> repository, OperationResult or)
        {
            if(id==0)
            {
                or.AddMessage(_localizedMessagesService.GetErrorMessage(ErrorMessageKeys.InvalidId));
                return false;
            }
            
            try
            {
                return repository.DeleteItem(id);
            }
            catch (Exception e)
            {
                or.AddMessage(_localizedMessagesService.GetErrorMessage(ErrorMessageKeys.RepositoryError));
                return false;
            }
        }
    }
}
