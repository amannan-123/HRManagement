using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace HRManagement.Models
{
	public class Employee
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Employee Code is required.")]
		[StringLength(100, MinimumLength = 1, ErrorMessage = "Input cannot be greater than 100 characters.")]
		public string EmployeeCode { get; set; } = "";

		[Required(ErrorMessage = "Employee Name is required.")]
		[StringLength(100, MinimumLength = 1, ErrorMessage = "Input cannot be greater than 100 characters.")]
		[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Employee Name must be alphabetic.")]
		public string EmployeeName { get; set; } = "";

		[Required(ErrorMessage = "AppointedBy is required.")]
		[StringLength(100, MinimumLength = 1, ErrorMessage = "AppointedBy cannot be greater than 100 characters.")]
		public string AppointedBy { get; set; } = "";

		[Required(ErrorMessage = "JoinDate is required.")]
		[DataType(DataType.Date)]
		public DateTime JoinDate { get; set; }

		[Required(ErrorMessage = "Qualification is required.")]
		[StringLength(100, MinimumLength = 1, ErrorMessage = "Qualification cannot be greater than 100 characters.")]
		public string Qualification { get; set; } = "";

		[Required(ErrorMessage = "Gender is required.")]
		[StringLength(10, MinimumLength = 1, ErrorMessage = "Gender cannot be greater than 10 characters.")]
		public string Gender { get; set; } = "";

		[Required(ErrorMessage = "Date of birth is required.")]
		[DataType(DataType.Date)]
		public DateTime DateOfBirth { get; set; }

		[Required(ErrorMessage = "Religion is required.")]
		[StringLength(50, MinimumLength = 1, ErrorMessage = "Religion cannot be greater than 50 characters.")]
		public string Religion { get; set; } = "";

		[Required(ErrorMessage = "MaritalStatus is required.")]
		[StringLength(50, MinimumLength = 1, ErrorMessage = "MaritalStatus cannot be greater than 50 characters.")]
		public string MaritalStatus { get; set; } = "";

		[Required(ErrorMessage = "Dependants are required.")]
		public int Dependants { get; set; }

		[Required(ErrorMessage = "Email is required.")]
		[StringLength(100, MinimumLength = 1, ErrorMessage = "Email cannot be greater than 100 characters.")]
		[RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Please enter a valid email address.")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; } = "";

		[Required(ErrorMessage = "PostBox is required.")]
		[StringLength(50, MinimumLength = 1, ErrorMessage = "PostBox cannot be greater than 50 characters.")]
		[DataType(DataType.PostalCode)]
		public string PostBox { get; set; } = "";

		[Required(ErrorMessage = "Country is required.")]
		[StringLength(50, MinimumLength = 1, ErrorMessage = "Country cannot be greater than 50 characters.")]
		public string Country { get; set; } = "";

		[Required(ErrorMessage = "City is required.")]
		[StringLength(50, MinimumLength = 1, ErrorMessage = "City cannot be greater than 50 characters.")]
		public string City { get; set; } = "";

		[Required(ErrorMessage = "Landline is required.")]
		[DataType(DataType.PhoneNumber)]
		public int Landline { get; set; }

		[Required(ErrorMessage = "Mobile is required.")]
		[DataType(DataType.PhoneNumber)]
		public int Mobile { get; set; }

		[Required(ErrorMessage = "Address is required.")]
		[StringLength(500, MinimumLength = 1, ErrorMessage = "Address cannot be greater than 500 characters.")]
		[DataType(DataType.MultilineText)]
		public string Address { get; set; } = "";

		[BindNever]
		public byte[]? Image { get; set; }

		public string? ImageData()
		{
			if (Image == null)
				return null;

			return $"data:image/jpg;base64,{Convert.ToBase64String(Image)}";
		}
	}
}
