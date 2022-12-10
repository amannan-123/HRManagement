namespace HRManagement.Models
{
	public class EmployeeViewModel
	{
		public List<Employee> Employees { get; set; } = new();
		public int TotalPages { get; set; } = 0;
		public int CurrentPage { get; set; } = 1;
		public int RowLength { get; set; } = 1;
	}
}
