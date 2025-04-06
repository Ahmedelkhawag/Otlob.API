namespace Otlob.API.ExtensionMethods
{
    public static class SwaggerExtension
    {
        public static WebApplication UseSwaggerMiddleware(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
