using System.ComponentModel.DataAnnotations;

namespace HRManagement.Models
{
	public class Designation
	{

		public int Id { get; set; }

		[Required(ErrorMessage = "Designation Code is required.")]
		[StringLength(100, MinimumLength = 1, ErrorMessage = "Input cannot be greater than 100 characters.")]
		public string Code { get; set; } = "";

		[Required(ErrorMessage = "Designation Name is required.")]
		[StringLength(100, MinimumLength = 1, ErrorMessage = "Input cannot be greater than 100 characters.")]
		[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Designation Name must be alphabetic.")]
		public string Name { get; set; } = "";

	}
}
