using System;
using System.Collections.Generic;
using System.Text;

namespace EntityModels.Abstractions
{
    public interface IUser
    {
        string Email { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string MobileNumber { get; set; }
        DateTime Birthday { get; set; }
    }
}
