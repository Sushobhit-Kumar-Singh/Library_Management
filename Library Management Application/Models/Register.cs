using System.ComponentModel.DataAnnotations;

namespace Library_Management_Application.Models
{
    public class Register
    {
        public int MemberId { get; set; }

		[Required(ErrorMessage = "Name is Required")]
		public string? Name { get; set; }

		[Required(ErrorMessage = "Address is Required")]
		public string? Address { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$",ErrorMessage ="Phone No is not valid")]
		[Required(ErrorMessage = "Phone No is Required")]
		public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "E Mail Id is Required")]
        [EmailAddress(ErrorMessage = "E Mail Id is not Valid ")]
        public string EMail { get; set; } = null!;

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password, ErrorMessage = "Password is not Valid")]
        public string Password { get; set; } = null!;

        public bool IsApproved { get; set; }
    }
}
