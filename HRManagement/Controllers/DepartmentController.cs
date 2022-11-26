using HRManagement.Models;
using HRManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.Controllers
{
	public class DepartmentController : Controller
	{
		private readonly IDepartmentService _DepartmentService;
		public DepartmentController(IDepartmentService DepartmentService)
		{
			_DepartmentService = DepartmentService;
		}

		public IActionResult Index()
		{
			List<Department> Departments = _DepartmentService.GetDepartments();
			return View(Departments);
		}

		public IActionResult Delete(int id)
		{
			bool deleted = _DepartmentService.DeleteDepartment(id);

			if (deleted)
				return Json("success");
			else
				return BadRequest("Invalid ID");
		}

		[HttpPost]
		public IActionResult Edit(int id, string code, string name)
		{
			Department des = new() { Id = id, Code = code, Name = name };

			if (_DepartmentService.UpdateDepartment(des))
				return Json("success");
			else
				return BadRequest("Department code already exists.");
		}

		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Add(string code, string name)
		{
			Department des = new() { Code = code, Name = name };

			if (_DepartmentService.AddDepartment(des))
				return Json("success");
			else
				return BadRequest("Department code already exists.");
		}
	}
}
