using System;
using System.Collections.Generic;
using Services.Enums;
using Services.Interfaces;

namespace Services
{
    public class LocalizedMessagesService : ILocalizedMessagesService
    {

        public string GetErrorMessage(ErrorMessageKeys key)
        {
            return ErrorMessages.GetMessage((int) key);
        }

        public string GetInfoMessage(InfoMessageKeys key)
        {
            return InfoMessages.GetMessage((int)key);
        }

        private  static class ErrorMessages
        {
            private static string _defaultErrorMessage = "ERROR!";
            private static Dictionary<int, string> _errorMessages = new Dictionary<int, string>()
                                                                        {
                                                                            {(int) ErrorMessageKeys.NullObjectReference,"Null value for object reference!"}
                                                                            ,
                                                                            {(int) ErrorMessageKeys.InvalidId, "Invalid entity Id value"}
                                                                            ,
                                                                            {(int) ErrorMessageKeys.RepositoryError, "Error in repository access"}
                                                                            ,
                                                                            {(int) ErrorMessageKeys.EntityNotFound, "Enity Not Found in Repositorries : "}
                                                                            ,
                                                                            {(int) ErrorMessageKeys.RelationshipDoesNotFound, "Relationship not Found"}
                                                                            ,
                                                                            {(int) ErrorMessageKeys.UserNotFound, "User not found"}
                                                                        };

            public static string GetMessage(int key)
            {
                if (!_errorMessages.ContainsKey(key))
                {
                    return _defaultErrorMessage;
                }

                return String.IsNullOrWhiteSpace(_errorMessages[key])?_defaultErrorMessage:_errorMessages[key];
            }
        }

        private static class InfoMessages
        {
            private static string _defaultInfoMessage = "INFO!";
            private static Dictionary<int, string> _infoMessages = new Dictionary<int, string>();

            public static string GetMessage(int key)
            {
                if (!_infoMessages.ContainsKey(key))
                {
                    return _defaultInfoMessage;
                }
                return String.IsNullOrWhiteSpace(_infoMessages[key]) ? _defaultInfoMessage : _infoMessages[key];
            }
        }
    }
}