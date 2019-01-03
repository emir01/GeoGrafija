using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.ComponentModel;

namespace GeoGrafija.ViewModels.UserViewModels
{
    [PropertiesMustMatch("Password", "PasswordAgain", ErrorMessage = "Внесените лозинки не се совпаѓаат.")]
    public class RegisterViewModel
    {   
        [DisplayName("Корисничко Име")]
        [Required(ErrorMessage="Мора да внесете корисничко име")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Мора да внесете лозинка")]
        [StringLength(20, ErrorMessage = "Лозинката мора да биде  6 карактери",MinimumLength=6)]
        [DisplayName("Лозинка")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Мора повторно да ја внесете лозинката")]
        [StringLength(20, ErrorMessage = "Лозинката мора да биде барем 6 карактери", MinimumLength = 6)]
        [DisplayName("Повторете ја Лозинката")]
        [DataType(DataType.Password)]
        public string PasswordAgain { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage="Мора да внесете Email Адреса")]
        [DisplayName("Email Адреса")]
        public string  Email { get; set; }

    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' and '{1}' do not match.";
        private readonly object _typeId = new object();

        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(_defaultErrorMessage)
        {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        public string ConfirmProperty { get; private set; }
        public string OriginalProperty { get; private set; }

        public override object TypeId
        {
            get
            {
                return _typeId;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                OriginalProperty, ConfirmProperty);
        }

        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
            object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
            return Object.Equals(originalValue, confirmValue);
        }
    }
}
