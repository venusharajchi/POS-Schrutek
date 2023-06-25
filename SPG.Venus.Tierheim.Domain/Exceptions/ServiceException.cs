using System;
namespace SPG.Venus.Tierheim.Domain.Exceptions
{
    public class ServiceException : Exception
    {
        public ServiceException()
            : base()
        { }

        public ServiceException(string message)
            : base(message)
        { }

        public ServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

