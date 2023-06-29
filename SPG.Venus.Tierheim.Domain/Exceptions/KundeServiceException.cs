using System;
namespace SPG.Venus.Tierheim.Domain.Exceptions
{
    public class KundeServiceException : Exception
    {
        public KundeServiceException()
            : base()
        { }

        public KundeServiceException(string message)
            : base(message)
        { }

        public KundeServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

