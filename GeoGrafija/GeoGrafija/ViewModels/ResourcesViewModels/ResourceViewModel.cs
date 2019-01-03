using Model;

namespace GeoGrafija.ViewModels.ResourcesViewModels
{
    public  class ResourceViewModel
    {
        public bool IsOk { get; set; }
        public string Message { get; set; }
      
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }

        public int? LocaitonId { get; set; }
        public string LocationName { get; set; }

        public int? LocationTypeId { get; set; }
        public string LocationTypeName { get; set; }

        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string Typecolor { get; set; }

        public ResourceViewModel()
        {
            
        }

        public ResourceViewModel(Resource resource)
        {
            if (resource == null)
            {
                return;
            }
            
            Id = resource.ID;
            Name = resource.Name;
            Text = resource.Text;
            

            LocaitonId = resource.LocationId;
            LocationTypeId = resource.LocationTypeId;

            if (resource.Location != null) { 
                LocationName = resource.Location.Name;
            }

            if (resource.LocationType != null) { 
                LocationTypeName = resource.LocationType.TypeName;
            }

            TypeId = resource.ResourceType.ID;
            TypeName = resource.ResourceType.Name;
            Typecolor = resource.ResourceType.Color;
        }

        public Resource GetSimpleResource()
        {
            var resource = new Resource();

            resource.Name = Name;
            resource.ID = Id;
            resource.Text = Text;

            return resource;
        }
    }
}