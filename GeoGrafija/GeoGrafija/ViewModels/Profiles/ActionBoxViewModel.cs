using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoGrafija.ViewModels.Profiles
{
    public class ActionBoxViewModel
    {
        public  string Text { get; set; }
        public string ExtraClasses { get; set; }
        public int UId { get; set; }
        public string Image { get; set; }
        public int ActionId { get; set; }

        public ActionBoxViewModel(string text,int uid= 0, int actionId = 0, string image = "", string extraClasses = "")
        {
            Text = text;
            ExtraClasses = extraClasses;
            UId = uid;
            ActionId = actionId;
            Image = image;
        }
    }
}