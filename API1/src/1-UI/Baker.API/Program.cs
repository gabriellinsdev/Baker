using Baker.API.Middlewares;
using Baker.Infra.IoC;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Baker.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "BakerAPI", Version = "v1" });

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin", build =>
                {
                    build
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    ;
                });
            });

            builder.Services.AddDependencies(builder.Configuration);
            builder.Services.AddDatabase(builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("v1/swagger.json", "v1");
                });
            }

            app.UseMiddleware<CustomHeaderMiddleware>();
            app.UseCors("AllowAnyOrigin");
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
