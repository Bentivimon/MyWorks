using EntityModels.Abstractions;
using System;
using System.ComponentModel.DataAnnotations;

namespace Models.RequestModels.UserModels
{
    public class UserRegistrarionModelRequest : IUser
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string MobileNumber { get; set; }

        [Required]
        public DateTime Birthday { get; set; }
    }
}
