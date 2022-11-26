using System.ComponentModel.DataAnnotations;

namespace HRManagement.Models
{
	public class Department
	{
		
		public int Id { get; set; }

		[Required(ErrorMessage = "Department Code is required.")]
		[StringLength(100, MinimumLength = 1, ErrorMessage = "Input cannot be greater than 100 characters.")]
		public string Code { get; set; } = "";

		[Required(ErrorMessage = "Department Name is required.")]
		[StringLength(100, MinimumLength = 1, ErrorMessage = "Input cannot be greater than 100 characters.")]
		[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Department Name must be alphabetic.")]
		public string Name { get; set; } = "";
		
	}
}
