using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoGrafija.ResultClasses;
using Model;
using Model.Interfaces;
using Services.Enums;
using Services.Interfaces;

namespace Services
{
    /// <summary>
    /// Implements IResourceService.
    /// Provides Basic Crud Functionality for diferent entities : Resources and Resource Types
    /// </summary>
    public class ResourceService:IResourceService
    {
        private readonly IResourceRepository       _resourceRepository;
        private readonly IResourceTypeRepository   _resourceTypeRepository;
        private readonly ILocationRepository       _locationRepository;
        private readonly ILocalizedMessagesService _localizedMessages;
        
        private readonly IBasicCrudService       _crudService;
        
        public ResourceService(IResourceRepository resourceRepository
                              ,IResourceTypeRepository resourceTypeRepository
                              ,IBasicCrudService crudService
                              ,ILocationRepository locationRepository
                              ,ILocalizedMessagesService localizedMessagesService)
        {
            _resourceRepository     = resourceRepository;
            _resourceTypeRepository = resourceTypeRepository;
            _crudService            = crudService;
            _locationRepository     = locationRepository;
            _localizedMessages      = localizedMessagesService;
        }


        public GenericOperationResult<Resource> CreateResource(Resource resource)
        {
            var or = OperationResult.GetGenericResultObject<Resource>();
            
            or.SetStatus( _crudService.CreateItem(resource, _resourceRepository, or));
            return or;
        }

        public GenericOperationResult<ResourceType> CreateResourceType(ResourceType resourceType)
        {
            var or = OperationResult.GetGenericResultObject<ResourceType>();
            or.SetStatus(_crudService.CreateItem(resourceType, _resourceTypeRepository, or));
            return or;
        }

        public GenericOperationResult<IEnumerable<ResourceType>> GetAllResourceTypes()
        {
            var or = OperationResult.GetGenericResultObject<IEnumerable<ResourceType>>();
            or.SetStatus(_crudService.GetItems(_resourceTypeRepository,or));
            return or;
        }

        public GenericOperationResult<IEnumerable<Resource>> GetAllResources()
        {
            var or = OperationResult.GetGenericResultObject<IEnumerable<Resource>>();
            or.SetStatus(_crudService.GetItems(_resourceRepository, or));
            return or;
        }

        public GenericOperationResult<Resource> GetResource(int id)
        {
            var result = OperationResult.GetGenericResultObject<Resource>();
            result.SetStatus(_crudService.GetItem(id,_resourceRepository,result));
            return result;
        }

        public GenericOperationResult<ResourceType> GetResourceType(int id)
        {
            var result = OperationResult.GetGenericResultObject<ResourceType>();
            result.SetStatus(_crudService.GetItem(id, _resourceTypeRepository, result));
            return result;
        }

        public OperationResult AddResourceToLocation(int locationId, int resourceId)
        {
            var result = OperationResult.GetResultObject();

            if (locationId == 0 || resourceId ==0)
            {
                result.SetFail();
                result.AddMessage(_localizedMessages.GetErrorMessage(ErrorMessageKeys.InvalidId));
                return result;
            }

            // Check if location excists
            var location = _locationRepository.getLocation(locationId);
            if (location == null)
            {
                result.SetFail();
                result.AddMessage(_localizedMessages.GetErrorMessage(ErrorMessageKeys.EntityNotFound)+" Location");
                return result;
            }

            //Check same for Resource 
            var resourceGetResult = GetResource(resourceId);
            if (!resourceGetResult.IsOK)
            {
                result.SetFail();
                result.AddMessage(_localizedMessages.GetErrorMessage(ErrorMessageKeys.EntityNotFound) + " Resource");
                return result;
            }

            var resource = resourceGetResult.GetData();
            resource.LocationId = locationId;

            result.SetStatus( _crudService.UpdateItem(resource, _resourceRepository, result));

            return result;
        }

        private Location GetLocationRerefence(int locationId, OperationResult or)
        {
            Location location = null;
            try
            {
                location = _locationRepository.getLocation(locationId);
            }
            catch (Exception e)
            {
                or.SetException();or.AddExceptionMessage(e.Message);
            }

            return location;
        }

        public OperationResult RemoveResourceFromLocaton(int locationId, int resourceId)
        {
            var result = OperationResult.GetResultObject();

            if (locationId == 0 || resourceId == 0)
            {
                result.SetFail();
                result.AddMessage(_localizedMessages.GetErrorMessage(ErrorMessageKeys.InvalidId));
                return result;
            }

            // Check if location excists
            var location = _locationRepository.getLocation(locationId);
            if (location == null)
            {
                result.SetFail();
                result.AddMessage(_localizedMessages.GetErrorMessage(ErrorMessageKeys.EntityNotFound) + " Location");
                return result;
            }

            //Check same for Resource 
            var resourceGetResult = GetResource(resourceId);
            if (!resourceGetResult.IsOK)
            {
                result.SetFail();
                result.AddMessage(_localizedMessages.GetErrorMessage(ErrorMessageKeys.EntityNotFound) + " Resource");
                return result;
            }

            //Check if resource was added to this location

            if (resourceGetResult.GetData().LocationId != locationId)
            {
                result.SetFail();
                result.AddMessage(_localizedMessages.GetErrorMessage(ErrorMessageKeys.RelationshipDoesNotFound) + " Resource");
                return result;
            }

            var resource = resourceGetResult.GetData();
            resource.LocationId = null;

            result.SetStatus(_crudService.UpdateItem(resource, _resourceRepository, result));

            return result;
        }

        public OperationResult DeleteResource(int resourceId)
        {
            var result = OperationResult.GetResultObject();

            var resource = GetResource(resourceId).GetData();

            if (resourceId == null)
            {
                result.SetSuccess();
                return result;
            }

            try
            {
                _resourceRepository.DeleteItem(resourceId);

                result.SetSuccess();
                return result;
            }
            catch (Exception ex)
            {
                result.SetFail();
                result.SetException();
                result.AddMessage(ex.Message);
                result.AddExceptionMessage(ex.Message);
                return result;
            }

            return result;
        }

        public GenericOperationResult<Resource> UpdateResource(Resource resource)
        {
            var result = OperationResult.GetGenericResultObject<Resource>();
                result.SetStatus(_crudService.UpdateItem(resource,_resourceRepository,result));

                if (result.IsOK)
                {
                    var updatedResource = GetResource(resource.ID).GetData();
                    result.SetData(updatedResource);
                }

            return result;
        }
    }
}
