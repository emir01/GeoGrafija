using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace GeoGrafija.ViewModels.Profiles
{
    public class ActionBoxesListViewModel
    {
        public  List<ActionBoxViewModel> ActionBoxes { get; set;}
        public ProfileViewModel Model { get; set; }

        public ActionBoxesListViewModel(ProfileViewModel model)
        {
            Model = model;
            ActionBoxes = new List<ActionBoxViewModel>();
        }
   }
}