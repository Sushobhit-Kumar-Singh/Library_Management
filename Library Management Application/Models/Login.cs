using System.ComponentModel.DataAnnotations;

namespace Library_Management_Application.Models
{
    public class Login
    {
        [Required(ErrorMessage ="E Mail Id is Required")]
        [EmailAddress(ErrorMessage = "E Mail Id is not Valid ")]
        public string EMail { get; set; } = null!;

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password, ErrorMessage = "Password is not Valid")]
        public string Password { get; set; } = null!;

        public bool IsApprovalPending { get; set; }

        public string? ReturnUrl { get; set; }

    }
}
