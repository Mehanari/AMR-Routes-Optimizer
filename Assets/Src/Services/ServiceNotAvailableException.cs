using System;

namespace Src.Services
{
    public class ServiceNotAvailableException : Exception
    {
        public ServiceNotAvailableException(string message) : base(message)
        {
        }
    }
}