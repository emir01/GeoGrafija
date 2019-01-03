using System.Collections.Generic;

namespace Services.ResultClasses
{
    public sealed class RegisterResult
    {
        public bool Status { get; set; }
        public List<RegisterErrorCodes> ErrorCodes { get; set; }

        public RegisterResult()
        {
            ErrorCodes = new List<RegisterErrorCodes>();
        }

        public static string GetErrorMessage(RegisterErrorCodes Code)
        {
            switch (Code)
            {
                case RegisterErrorCodes.ConnectionIssues :
                    return "Грешка при Креирање на нова Корисничка Сметка. Обидетесе повторно подоцна.";
                    
                case RegisterErrorCodes.EmailAlreadyTaken:
                    return "Внесената Email адреса е веќе искористена. Пробајте со друга.";

                case RegisterErrorCodes.UsernameAlreadyTaken:
                    return "Корисничкото име е зафатено.Пробајте со друго корисничко име.";
            }

            return "Не успеавме да ве регистрираме. Обидете се повротно, подоцна.";
        }
    }
}