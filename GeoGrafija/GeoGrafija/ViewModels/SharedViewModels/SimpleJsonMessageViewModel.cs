using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoGrafija.ViewModels.SharedViewModels
{
    public class SimpleJsonMessageViewModel
    {
        public bool IsOk { get; set; }
        public string Message { get; set; }
        public int Type { get; set; }
        public Object Data { get; set; }

        public SimpleJsonMessageViewModel(string message,int type,Object data,bool isOk)
        {
            Message = message;
            Type = type;
            Data = data;
            IsOk = isOk;
        }

        public SimpleJsonMessageViewModel ()
        {
            
        }

    }
}