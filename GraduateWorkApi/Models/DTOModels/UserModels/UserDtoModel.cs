using System;
using System.Collections.Generic;
using System.Text;
using EntityModels.Abstractions;

namespace Models.DTOModels.UserModels
{
    public class UserDtoModel : IUser
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public DateTime Birthday { get; set; }
    }
}
