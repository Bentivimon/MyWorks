using System;

namespace GraduateWork.Server.Models.CustomExceptions
{
    /// <summary>
    /// Custom exception for represent not found model.
    /// </summary>
    public class NotFoundException : BaseException
    {
        public NotFoundException()
        { }

        public NotFoundException(string message) : base(message)
        { }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
