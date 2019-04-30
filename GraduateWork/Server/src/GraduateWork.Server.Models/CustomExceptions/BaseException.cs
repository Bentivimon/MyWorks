using System;
using System.Net;

namespace GraduateWork.Server.Models.CustomExceptions
{
    public class BaseException : Exception
    {
        public int StatusCode { get; protected set; } = 400;

        public int ErrorCode { get; protected set; } = 0;

        public object Errors { get; protected set; }

        public BaseException()
        { }

        public BaseException(string message)
            : base(message)
        { }

        public BaseException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = (int)statusCode;
        }

        public BaseException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
