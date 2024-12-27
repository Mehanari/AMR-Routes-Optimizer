using System;

namespace Src.Auth
{
    public class AuthServiceException : Exception
    {
        public AuthServiceException(string message) : base(message)
        {
        }
    }
}