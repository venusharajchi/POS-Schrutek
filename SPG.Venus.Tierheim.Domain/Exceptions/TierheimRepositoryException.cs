using System;
namespace SPG.Venus.Tierheim.Domain.Exceptions
{
    public class TierheimRepositoryException : Exception
    {
        public TierheimRepositoryException()
            : base()
        { }

        public TierheimRepositoryException(string message)
            : base(message)
        { }

        public TierheimRepositoryException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

