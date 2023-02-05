using ExcelDataReader;
using HRManagement.Models;
using HRManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data;

namespace HRManagement.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly IEmployeesService _employeeService;
		public EmployeeController(IEmployeesService employeeService)
		{
			_employeeService = employeeService;
		}

		public IActionResult Index(int rowlength, string page = "first")
		{
			int employeeCount = _employeeService.GetEmployeesCount();
			if (employeeCount == 0)
				return View(new EmployeeViewModel());

			if (rowlength == 0)
				rowlength = 10;

			int pageCount = (int)Math.Ceiling((employeeCount / Convert.ToDecimal(rowlength)));

			int parsedPage;
			if (page == null || page == "" || page == "first")
				parsedPage = 1;
			else if (page == "last")
				parsedPage = pageCount;
			else
			{
				bool canParse = int.TryParse(page, out parsedPage);
				if (!canParse) parsedPage = 1;
			}

			if (parsedPage == 0)
				parsedPage = 1;

			List<Employee> employees;

			employees = _employeeService.GetEmployees(parsedPage, rowlength);


			EmployeeViewModel model = new()
			{
				Employees = employees,
				TotalPages = pageCount,
				CurrentPage = parsedPage,
				RowLength = rowlength
			};

			return View(model);
		}

		[HttpPost]
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

		public IActionResult Import()
		{
			try
			{
				var excFile = Request.Form.Files[0];

				using var reader = ExcelReaderFactory.CreateReader(excFile.OpenReadStream());

				var result = reader.AsDataSet(new ExcelDataSetConfiguration()
				{
					ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
					{
						UseHeaderRow = true
					}
				});

				DataTableCollection tables = result.Tables;

				JObject ImportResults = new();

				foreach (DataTable dt in tables)
				{
					int success = 0, failed = 0;

					foreach (DataRow dr in dt.Rows)
					{
						Employee emp = new()
						{
							EmployeeCode = dr["EmployeeCode"].ToString(),
							EmployeeName = dr["EmployeeName"].ToString(),
							AppointedBy = dr["AppointedBy"].ToString(),
							JoinDate = DateTime.Parse(dr["JoinDate"].ToString()),
							Qualification = dr["Qualification"].ToString(),
							Gender = dr["Gender"].ToString(),
							DateOfBirth = DateTime.Parse(dr["DateOfBirth"].ToString()),
							Religion = dr["Religion"].ToString(),
							MaritalStatus = dr["MaritalStatus"].ToString(),
							Dependants = Convert.ToInt32(dr["Dependants"]),
							Email = dr["Email"].ToString(),
							PostBox = dr["PostBox"].ToString(),
							Country = dr["Country"].ToString(),
							City = dr["City"].ToString(),
							Landline = Convert.ToInt32(dr["Landline"]),
							Mobile = Convert.ToInt32(dr["Mobile"]),
							Address = dr["Address"].ToString()
						};

						if (_employeeService.AddEmployee(emp))
							success++;
						else
							failed++;
					}

					//create JSON object in format: TableName : {success, failed}
					ImportResults.Add(dt.TableName, new JObject
					{
						{ "success", success },
						{ "failed", failed }
					});

				}

				return Content(ImportResults.ToString(), "application/json");

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
