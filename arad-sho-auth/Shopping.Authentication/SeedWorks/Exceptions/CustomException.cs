using System;

namespace Shopping.Authentication.SeedWorks.Exceptions
{
    public class CustomException : Exception
    {
        public CustomException(string message) : base(message)
        {
        }
    }
}