using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoGrafija.ResultClasses
{
    public class OperationResult
    {
        public bool IsOK { get;  set; }
        public bool ExceptionThrown { get; set; }
        
        public List<string> Messages { get; set; }
        public List<string> ExceptionMessages { get; set; }

        public OperationResult()
        {
            Messages = new List<string>();
            ExceptionMessages = new List<string>();
        }

        public void SetStatus(bool status)
        {
            IsOK = status;
        }

        public void SetFail()
        {
            IsOK = false;
        }
        
        public void SetSuccess()
        {
            IsOK = true;
        }

        public void SetException()
        {
            ExceptionThrown = true;
        }

        public void ClearException()
        {
            ExceptionThrown = false;
        }

        public void AddMessage(string Message)
        {
            Messages.Add(Message);
        }

        public void AddExceptionMessage(string Message)
        {
            ExceptionMessages.Add(Message);
        }

        public void ClearMessages()
        {
            Messages.Clear();               
        }

        public void ClearExceptionMessages()
        {
            ExceptionMessages.Clear();
        }

        public static OperationResult GetResultObject()
        {
            var result = new OperationResult();
            result.IsOK = false;
            result.ExceptionThrown = false;
            return result;
        }

        public static GenericOperationResult<T> GetGenericResultObject<T>() where T : class
        {
            var result = new GenericOperationResult<T>();
            result.IsOK = false;
            result.ExceptionThrown = false;
            return result;
        }
    }

    /// <summary>
    /// Generic OperationResult, that extends OperationResult and provides type safe data to be returned with the OperationResultObject
    /// Finds it usesage in Read functionality for repositories or services.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericOperationResult<T>:OperationResult  where T: class
    {
        private T _data = default(T);

        public GenericOperationResult()
            : base()
        {}

        public T GetData()
        {
            return _data;
        }

        public void  SetData(T data)
        {
            _data = data;
        }
    }
}