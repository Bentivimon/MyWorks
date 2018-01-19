using System;

namespace Models.CustomExceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string errorMessage = "User not Found"): base(errorMessage) { }
    }
}
