using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoGrafija.ResultClasses;
using Model;

namespace Services.Interfaces
{
    public interface IResourceService
    {
        //Create    
        GenericOperationResult<Resource> CreateResource(Resource resource);
        GenericOperationResult<ResourceType> CreateResourceType(ResourceType resourceType);

        //Read
        GenericOperationResult<IEnumerable<ResourceType>> GetAllResourceTypes();
        GenericOperationResult<IEnumerable<Resource>> GetAllResources();
        GenericOperationResult<Resource> GetResource(int id);
        GenericOperationResult<ResourceType> GetResourceType(int id);

        OperationResult AddResourceToLocation(int locationId, int resourceId);
        OperationResult RemoveResourceFromLocaton(int locationId, int resourceId);

        // Delete 
        OperationResult DeleteResource(int resourceId);

        // Update
       GenericOperationResult<Resource> UpdateResource(Resource resource);
    }
}
