using Services.Enums;

namespace Services.Interfaces
{
    public interface ILocalizedMessagesService
    {
        string GetErrorMessage(ErrorMessageKeys key);
        string GetInfoMessage(InfoMessageKeys key);
    }
}
