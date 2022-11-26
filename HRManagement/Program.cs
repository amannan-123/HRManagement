using HRManagement.Middlewares;
using HRManagement.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
byte[] key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IAccountsService, AccountsService>();
builder.Services.AddSingleton<IEmployeesService, EmployeeService>();
builder.Services.AddSingleton<IDesignationService, DesignationService>();
builder.Services.AddSingleton<IDepartmentService, DepartmentService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSession();

builder.Services.AddAuthentication(x =>
{
	x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
{
	x.RequireHttpsMetadata = false;
	x.SaveToken = true;
	x.TokenValidationParameters = new TokenValidationParameters
	{
		ValidIssuer = builder.Configuration["JWT:Issuer"],
		ValidAudience = builder.Configuration["JWT:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(key),
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateIssuerSigningKey = true,
	};
});

builder.Services.AddAuthorization(options =>
{
	options.DefaultPolicy = new AuthorizationPolicyBuilder()
		.RequireAuthenticatedUser()
		.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
		.Build();
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseHeaderModifier();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
