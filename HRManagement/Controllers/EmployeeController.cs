﻿using HRManagement.Models;
using HRManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.ComponentModel;
using System.Reflection.Metadata;

namespace HRManagement.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly IEmployeesService _employeeService;
		public EmployeeController(IEmployeesService employeeService)
		{
			_employeeService = employeeService;
		}

		public IActionResult Index()
		{
			List<Employee> employees = _employeeService.GetEmployees();
			//return View(new List<Employee>());
			return View(employees);
		}

		public IActionResult Delete(int id)
		{
			bool deleted = _employeeService.DeleteEmployee(id);
			if (deleted)
				return RedirectToAction("Index");
			else
				return BadRequest("Invalid ID");
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

		//set display name for empImage
		[HttpPost]
		public IActionResult Add(Employee emp, IFormFile EmployeeImage)
		{
			if (!ModelState.IsValid)
				return View("Add", emp);

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
