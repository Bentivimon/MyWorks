using System;

namespace Models.CustomExceptions
{
    public class EntrantNotFoundException: Exception
    {
        public EntrantNotFoundException(string message = "Entrant not found") : base(message)
        {}
    }
}
