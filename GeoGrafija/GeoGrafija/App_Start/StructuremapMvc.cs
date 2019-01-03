using System.Web.Mvc;
using StructureMap;


[assembly: WebActivator.PreApplicationStartMethod(typeof(GeoGrafija.App_Start.StructuremapMvc), "Start")]

namespace GeoGrafija.App_Start {
    public static class StructuremapMvc {
        public static void Start() {
            
            var container = (IContainer) IoC.Initialize();
            DependencyResolver.SetResolver(new SmDependencyResolver(container));

        }
    }
}