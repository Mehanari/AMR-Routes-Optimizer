using System;

namespace Src.Schemas
{
    public class SchemasServiceException : Exception
    {
        public SchemasServiceException(string message) : base(message)
        {
        }
    }
}