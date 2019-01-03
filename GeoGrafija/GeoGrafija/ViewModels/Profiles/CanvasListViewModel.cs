using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace GeoGrafija.ViewModels.Profiles
{
    public class CanvasListViewModel
    {
        public User Model { get; set; }
        public List<CanvasViewModel> CanvasElements { get; set; }
    }

    public class CanvasViewModel
    {
        public string CanvasTitleText { get; set; }
        public string CanvasId { get; set; }

    }

}