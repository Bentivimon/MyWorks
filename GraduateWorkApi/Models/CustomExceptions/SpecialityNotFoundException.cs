using System;

namespace Models.CustomExceptions
{
    public class SpecialityNotFoundException: Exception
    {
        public SpecialityNotFoundException(string message = "Speciality not found"): base(message)
        {}
    }
}
