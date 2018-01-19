using System.ComponentModel.DataAnnotations;

namespace Models.RequestModels.UserModels
{
    public class ChangeUserEmailRequest
    {
        [Required]
        public string OldEmail { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string NewEmail { get; set; }
    }
}
