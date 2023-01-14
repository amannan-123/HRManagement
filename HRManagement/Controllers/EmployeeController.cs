using HRManagement.Models;
using HRManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly IEmployeesService _employeeService;
		public EmployeeController(IEmployeesService employeeService)
		{
			_employeeService = employeeService;
		}
		
		public IActionResult Index(string page = "first", int rowlength = 1)
		{
			int employeeCount = _employeeService.GetEmployeesCount();
			if (employeeCount == 0)
				return View(new EmployeeViewModel());

			if (rowlength == 0)
				rowlength = 10;

			double pageCount = (double)(employeeCount / Convert.ToDecimal(rowlength));

			int parsedPage;
			if (page == null || page == "" || page == "first")
				parsedPage = 1;
			else if (page == "last")
				parsedPage = (int)Math.Ceiling(pageCount);
			else
			{
				bool canParse = int.TryParse(page, out parsedPage);
				if (!canParse) parsedPage = 1;
			}

			if (parsedPage == 0)
				parsedPage = 1;

			List<Employee> employees;
			if (employeeCount > rowlength)
				employees = _employeeService.GetEmployees(parsedPage, rowlength);
			else
				employees = _employeeService.GetAllEmployees();

			EmployeeViewModel model = new()
			{
				Employees = employees,
				TotalPages = (int)Math.Ceiling(pageCount),
				CurrentPage = parsedPage,
				RowLength = rowlength
			};

			return View(model);
		}

		//public IActionResult Delete(int id)
		//{
		//	bool deleted = _employeeService.DeleteEmployee(id);
		//	if (deleted)
		//		return RedirectToAction("Index");
		//	else
		//		return BadRequest("Invalid ID");
		//}

		public IActionResult Delete(int id)
		{
			bool deleted = _employeeService.DeleteEmployee(id);

			if (deleted)
				return Json("success");
			else
				return BadRequest("Invalid ID");
		}

		public IActionResult View(int id)
		{
			Employee? employee = _employeeService.GetEmployee(id);
			if (employee == null)
				return NotFound("Invalid ID");
			return View(employee);
		}

		public IActionResult Edit(int id)
		{
			Employee? employee = _employeeService.GetEmployee(id);
			if (employee == null)
				return NotFound("Invalid ID");
			return View(employee);
		}

		[HttpPost]
		public IActionResult Edit(Employee emp, IFormFile? EmployeeImage)
		{
			if (!ModelState.IsValid)
				return View("Edit", emp);

			if (EmployeeImage != null)
			{
				if (EmployeeImage.Length > 0 && EmployeeImage.Length < 2097152 && EmployeeImage.ContentType.Contains("image"))
					//Convert EmployeeImage to byte and save to database
					using (MemoryStream ms = new MemoryStream())
					{
						EmployeeImage.CopyTo(ms);
						emp.Image = ms.ToArray();
					}
				else
					ModelState.AddModelError("CustomError", "File must be a valid image and less than 2MB.");
			}
			else
			{
				Employee? _employee = _employeeService.GetEmployee(emp.Id);
				if (_employee != null)
					emp.Image = _employee.Image;
			}
			if (_employeeService.UpdateEmployee(emp))
			{
				return RedirectToAction("Index");
			}
			else
			{
				ModelState.AddModelError("CustomError", "Failed to edit employee.");
				return View("Edit", emp);
			}
		}

		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Add(Employee emp, IFormFile EmployeeImage)
		{
			if (!ModelState.IsValid)
				return View("Add", emp);

			if (EmployeeImage != null)
			{
				if (EmployeeImage.Length > 0 && EmployeeImage.Length < 2097152 && EmployeeImage.ContentType.Contains("image"))
					//Convert EmployeeImage to byte array
					using (MemoryStream ms = new())
					{
						EmployeeImage.CopyTo(ms);
						emp.Image = ms.ToArray();
					}
				else
					ModelState.AddModelError("CustomError", "File must be a valid image and less than 2MB.");

			}
			if (_employeeService.AddEmployee(emp))
			{
				return RedirectToAction("Index");
			}
			else
			{
				ModelState.AddModelError("CustomError", "Employee code already registered.");
				return View("Add", emp);
			}
		}
	}
}
