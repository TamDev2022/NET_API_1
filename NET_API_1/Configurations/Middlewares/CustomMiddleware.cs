namespace NET_API_1.Configurations.Middlewares
{
    public static class CustomMiddleware
    {
        public static void ConfigureCustomMiddleware(this WebApplication app)
        {
            app.UseMiddleware<JWTMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
