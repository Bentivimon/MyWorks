using System;
using EntityModels.Abstractions;

namespace EntityModels.Entitys
{
    public class UserEntity : IUser
    {
        public string Id { get; set; } 
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public DateTime Birthday { get; set; }

        public EntrantEntity Entrant { get; set; }
    }
}
