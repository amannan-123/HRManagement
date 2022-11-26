using HRManagement.Models;
using HRManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRManagement.Controllers
{
	public class LoginController : Controller
	{
		private readonly IAccountsService _accountsService;
		private readonly IConfiguration _configuration;

		public LoginController(IAccountsService accountsService, IConfiguration configuration)
		{
			_accountsService = accountsService;
			_configuration = configuration;
		}

		[HttpGet]
		public IActionResult Index()
		{
			if (TempData.ContainsKey("Message"))
			{
				ViewBag.Message = TempData["Message"];
			}
			return View();
		}

		[HttpPost]
		public IActionResult Index(string userEmail, string userPassword, string userRemember)
		{
			User? employee = _accountsService.GetEmployee(userEmail, userPassword);
			if (employee == null)
			{
				ModelState.AddModelError("CustomError", "Invalid email or password.");
				return View("Index");
			}
			else
			{
				DateTime expiry = DateTime.Now.AddHours(4);
				if (!string.IsNullOrEmpty(userRemember) && userRemember.Equals("on"))
					expiry = DateTime.Now.AddDays(7);
				
				List<Claim> authClaims = new()
				{
					new Claim(ClaimTypes.Name, userEmail),
					new Claim(ClaimTypes.Role, "Admin"),
					new Claim(ClaimTypes.Email, userEmail),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
					new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
				};
				
				SymmetricSecurityKey authSigninKey = new(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

				JwtSecurityToken token = new(
					issuer: _configuration["JWT:Issuer"],
					audience: _configuration["JWT:Audience"],
					claims: authClaims,
					expires: expiry,
					signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
				);

				JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
				string savedToken = jwtSecurityTokenHandler.WriteToken(token);
				
				HttpContext.Session.SetString("Token", savedToken);

				return RedirectToAction("Index", "Home");
			}
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Home");
		}
	}
}
