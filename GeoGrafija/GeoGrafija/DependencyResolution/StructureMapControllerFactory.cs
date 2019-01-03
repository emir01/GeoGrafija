using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StructureMap;

namespace GeoGrafija
{
    public class StructureMapControllerFactory:DefaultControllerFactory
    {
        public override IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            try
            {
                var controllerType = base.GetControllerType(requestContext, controllerName);
                var toReturn= ObjectFactory.GetInstance(controllerType) as IController;
                return toReturn;
            }
            catch(Exception ex)
            {
                return base.CreateController(requestContext, controllerName);
            }
        }
    }
}