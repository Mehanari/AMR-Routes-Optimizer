using System;

namespace Src.Solutions
{
    public class SolutionsServiceException : Exception
    {
        public SolutionsServiceException(string message) : base(message)
        {
        }
    }
}