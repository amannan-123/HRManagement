using HRManagement.Models;
using HRManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.Controllers
{
	public class DepartmentController : Controller
	{
		private readonly IDepartmentService _departmentService;
		public DepartmentController(IDepartmentService departmentService)
		{
			_departmentService = departmentService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Delete(int id)
		{
			bool deleted = _departmentService.DeleteDepartment(id);

			if (deleted)
				return Json("success");
			else
				return BadRequest("Invalid ID");
		}

		[HttpPost]
		public IActionResult Edit(int id, string code, string name)
		{
			Department des = new() { Id = id, Code = code, Name = name };

			if (_departmentService.UpdateDepartment(des))
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
			Department dep = new() { Code = code, Name = name };

			if (_departmentService.AddDepartment(dep))
				return Json("success");
			else
				return BadRequest("Department code already exists.");
		}

		[HttpGet]
		public IActionResult GetDepsPartial()
		{
			List<Department> departments = _departmentService.GetDepartments();
			return PartialView("_Departments", departments);
		}
	}
}
