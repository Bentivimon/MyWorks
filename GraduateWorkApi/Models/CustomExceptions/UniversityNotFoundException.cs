using System;

namespace Models.CustomExceptions
{
    public class UniversityNotFoundException: Exception
    {
        public UniversityNotFoundException(string message = "University not found"): base(message)
        {}
    }
}
