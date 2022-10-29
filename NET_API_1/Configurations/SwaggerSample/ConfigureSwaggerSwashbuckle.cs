using System.Reflection;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace NET_API_1.Configurations.SwaggerSample
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConfigureSwaggerSwashbuckle
    {
        /// <summary>
        /// Configure the Swagger generator with XML comments, bearer authentication, etc.
        /// Additional configuration files:
        /// <list type="bullet">
        ///     <item>ConfigureSwaggerSwashbuckleOptions.cs</item>
        /// </list>
        /// </summary>
        /// <param name="services"></param> 
        public static void AddSwaggerSwashbuckleConfigured(this IServiceCollection services)
        {
            //services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerSwashbuckleOptions>();

            // Configures ApiExplorer (needed from ASP.NET Core 6.0).
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                // If we would like to provide request and response examples (Part 1/2)
                // Enable the Automatic (or Manual) annotation of the [SwaggerRequestExample] and [SwaggerResponseExample].
                // Read more here: https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters
                options.ExampleFilters();

                // If we would like to provide security information about the authorization scheme that we are using (e.g. Bearer).
                // Add Security information to each operation for bearer tokens and define the scheme.
                options.OperationFilter<SecurityRequirementsOperationFilter>(true, "Bearer");

                var _provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in _provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName,
                        new Microsoft.OpenApi.Models.OpenApiInfo()
                        {
                            Title = Assembly.GetExecutingAssembly().GetName().Name,
                            Version = description.ApiVersion.ToString()
                        });
                }

                options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });

                // If we use the [Authorize] attribute to specify which endpoints require Authorization, then we can
                // Show an "(Auth)" info to the summary so that we can easily see which endpoints require Authorization.
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                //options.OperationFilter<AuthOperationFilter>();


                //// If we would like to include documentation comments in the OpenAPI definition file and SwaggerUI.
                //// Set the comments path for the XmlComments file.
                //// Read more here: https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio#xml-comments
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //options.IncludeXmlComments(xmlPath, true);

            });
            // If we would like to provide request and response examples (Part 2/2)
            // Register examples with the ServiceProvider based on the location assembly or example type.
            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

            // If we are using FluentValidation, then we can register the following service to add the  fluent validation rules to swagger.
            // Adds FluentValidationRules staff to Swagger. (Minimal configuration)
            services.AddFluentValidationRulesToSwagger();

        }
    }
}
