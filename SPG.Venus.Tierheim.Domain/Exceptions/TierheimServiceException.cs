using System;
namespace SPG.Venus.Tierheim.Domain.Exceptions
{
	public class TierheimServiceException : Exception
	{
        public TierheimServiceException()
            : base()
        { }

        public TierheimServiceException(string message)
            : base(message)
        { }

        public TierheimServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

