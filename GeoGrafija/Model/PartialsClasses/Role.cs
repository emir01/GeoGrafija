using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [MetadataType(typeof(Role_Metadata))]
    public partial class Role
    {
        public bool RoleHasPrivilege(string PrivilegeName)
        {
            var hasIt = false;
            
            foreach (var priv in Privileges)
            {
                if (priv.PrivilegeName.Equals(PrivilegeName))
                {
                    hasIt = true;
                }
            }
            
            return hasIt;
        }
    }

    public class Role_Metadata
    {
        [DisplayName("Име на Улога")]
        public String RoleName { get; set; }
        [DisplayName("Опис на Улога")]
        public String RoleDescription { get; set; }
    }
}