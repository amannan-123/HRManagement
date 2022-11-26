using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HRManagement.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HRManagement.Controllers
{
	public class HomeController : Controller
	{

		public IActionResult Index()
		{
			return View();
		}

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}