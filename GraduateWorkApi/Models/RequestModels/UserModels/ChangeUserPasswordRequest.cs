using System.ComponentModel.DataAnnotations;

namespace Models.RequestModels.UserModels
{
    public class ChangeUserPasswordRequest
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
