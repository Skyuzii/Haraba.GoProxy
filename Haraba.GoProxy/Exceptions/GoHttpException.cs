using System;

namespace Haraba.GoProxy.Exceptions
{
    public class GoHttpException : Exception
    {
        public GoHttpException(string message) : base(message)
        {
        }
    }
}