using System;
using System.Runtime.Serialization;

namespace Foundation.Social
{
    [Serializable]
    public class SocialRepositoryException : Exception
    {
        public SocialRepositoryException(string message)
            : base(message)
        {
        }

        public SocialRepositoryException(string message, Exception ex)
            : base(message, ex)
        {
        }

        public SocialRepositoryException()
        {
        }

        protected SocialRepositoryException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
        }
    }
}