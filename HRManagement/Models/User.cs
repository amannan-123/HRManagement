using System.ComponentModel.DataAnnotations;

namespace HRManagement.Models
{
	public class User
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "First Name is required.")]
		[StringLength(100, MinimumLength = 3, ErrorMessage = "First Name must be between 3 and 100 characters.")]
		public string? FirstName { get; set; } = "";

		[Required(ErrorMessage = "Last Name is required.")]
		[StringLength(100, MinimumLength = 3, ErrorMessage = "Last Name must be between 3 and 100 characters.")]
		public string? LastName { get; set; } = "";

		[Required(ErrorMessage = "Email is required.")]
		[RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Please enter a valid email address.")]
		[DataType(DataType.EmailAddress)]
		public string? Email { get; set; } = "";

		[DataType(DataType.PhoneNumber)]
		public string? Phone { get; set; } = "123";

		[Required(ErrorMessage = "Password is required.")]
		[DataType(DataType.Password)]
		[StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be atleast 8 characters long.")]
		public string? Password { get; set; } = "";

		[Required(ErrorMessage = "Password confirmation is required.")]
		[DataType(DataType.Password)]
		[StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be atleast 8 characters long.")]
		[Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
		public string? ConfirmPassword { get; set; } = "";

		public int RoleId { get; set; } = 0;

		public int DepartmentId { get; set; } = 0;

		[DataType(DataType.Date)]
		public DateTime TimeStamp { get; set; } = DateTime.Now;
	}
}
