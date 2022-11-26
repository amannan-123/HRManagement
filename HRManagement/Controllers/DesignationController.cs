using HRManagement.Models;
using HRManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.Controllers
{
	public class DesignationController : Controller
	{
		private readonly IDesignationService _designationService;
		public DesignationController(IDesignationService designationService)
		{
			_designationService = designationService;
		}

		public IActionResult Index()
		{
			List<Designation> designations = _designationService.GetDesignations();
			return View(designations);
		}

		public IActionResult Delete(int id)
		{
			bool deleted = _designationService.DeleteDesignation(id);

			if (deleted)
				return Json("success");
			else
				return BadRequest("Invalid ID");
		}

		[HttpPost]
		public IActionResult Edit(int id, string code, string name)
		{
			Designation des = new() { Id = id, Code = code, Name = name };

			if (_designationService.UpdateDesignation(des))
				return Json("success");
			else
				return BadRequest("Designation code already exists.");
		}

		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Add(string code, string name)
		{
			Designation des = new() { Code = code, Name = name };

			if (_designationService.AddDesignation(des))
				return Json("success");
			else
				return BadRequest("Designation code already exists.");
		}
	}
}
