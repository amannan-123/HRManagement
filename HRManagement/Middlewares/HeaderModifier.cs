namespace HRManagement.Middlewares
{
	public class HeaderModifier
	{
		private readonly RequestDelegate _next;

		public HeaderModifier(RequestDelegate next)
		{
			_next = next;
		}

		public Task Invoke(HttpContext httpContext)
		{
			//remove auth header if it exists
			if (httpContext.Request.Headers.ContainsKey("Authorization"))
				httpContext.Request.Headers.Remove("Authorization");

			// add new auth header using JWT
			string? token = httpContext.Session.GetString("Token");
			if (!string.IsNullOrEmpty(token))
				httpContext.Request.Headers.Add("Authorization", $"Bearer {token}");

			return _next(httpContext);
		}
	}

	// Extension method used to add the middleware to the HTTP request pipeline.
	public static class HeaderModifierExtensions
	{
		public static IApplicationBuilder UseHeaderModifier(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<HeaderModifier>();
		}
	}
}
