using HRManagement.Models;
using HRManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.Controllers
{
	public class SignupController : Controller
	{
		private readonly IAccountsService _accountsService;

		public SignupController(IAccountsService accountsService)
		{
			_accountsService = accountsService;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Index(User emp)
		{
			if (!ModelState.IsValid)
			{
				return View("Index", emp);
			}

			if (_accountsService.AddEmployee(emp))
			{
				return RedirectToAction("Index", "Login");
			}
			else
			{
				ModelState.AddModelError("CustomError", "Email already registered.");
				return View("Index", emp);
			}
		}
	}
}
