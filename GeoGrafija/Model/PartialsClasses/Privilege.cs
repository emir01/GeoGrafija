using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [MetadataType(typeof(PrivilegeMetadata))]
    public partial  class Privilege
    {
    }
    
    public  class PrivilegeMetadata
    {
        [DisplayName("Име на привилегија")]
        public String PrivilegeName { get; set; }
        
        [DisplayName("Опис на Привилегија")]
        public String PrivilegeDescription { get; set; }
    }
}