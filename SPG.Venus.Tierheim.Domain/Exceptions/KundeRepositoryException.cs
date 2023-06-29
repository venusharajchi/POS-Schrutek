using System;
namespace SPG.Venus.Tierheim.Domain.Exceptions
{
    public class KundeRepositoryException : Exception
    {
        public KundeRepositoryException()
            : base()
        { }

        public KundeRepositoryException(string message)
            : base(message)
        { }

        public KundeRepositoryException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

