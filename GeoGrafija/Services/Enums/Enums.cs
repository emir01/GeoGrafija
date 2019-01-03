using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Enums
{
    public enum ErrorMessageKeys
    {
        InvalidId,
        NullObjectReference,
        RepositoryError,
        EntityNotFound,
        RelationshipDoesNotFound,
        UserNotFound
    }

    public enum InfoMessageKeys
    {

    }

    public enum QuizGetType
    {
        Pregenarated,
        GenerateRandom
    }

}
