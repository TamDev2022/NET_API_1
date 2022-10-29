using NET_API_1.Interfaces.IServices;

namespace NET_API_1.Configurations.Middlewares
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;
        public JWTMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            //check token expired 
            await _next(context);
        }
    }
}
