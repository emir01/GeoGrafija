using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [MetadataType(typeof(User_Validation))]
    public partial class User
    {
        public bool UserHasRole(string Role)
        {
            foreach (var role in Roles)
            {
                if(role.RoleName.ToLower().Equals(Role.ToLower()))
                {
                    return true;
                }
            }
        
            return false;
        }
    }

    public class User_Validation
    {

        [Required(ErrorMessage="Мора да внесете корисничко име.")]
        [DisplayName("Корисничко Име")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Мора да внесете лозинка.")]
        public string Password { get; set; }
        [DisplayName("Email Aдреса")]
        public string Email { get; set; }
        
    }
}