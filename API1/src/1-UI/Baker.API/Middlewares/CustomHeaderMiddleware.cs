namespace Baker.API.Middlewares
{
    public class CustomHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.Headers.Append("Access-Control-Allow-Origin", "*");

            await _next(context);
        }
    }
}
