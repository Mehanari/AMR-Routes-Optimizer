using System;

namespace Src.Registry
{
    public class ServiceNotAvailableException : Exception
    {
        public ServiceNotAvailableException(string message) : base(message)
        {
        }
    }
}