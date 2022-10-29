using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace NET_API_1.Configurations.SwaggerSample
{
    public static class ConfigureApiVersioning
    {
        /// <summary>
        /// Configure the API versioning properties of the project, such as return headers, version format, etc.
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddApiVersioningConfigured(this IServiceCollection services)
        {

            services.AddApiVersioning(options =>
            {
                //Specify the default Api version as 1.0
                options.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                options.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                options.ReportApiVersions = true;

                // Combine (or not) API Versioning Mechanisms:
                //options.ApiVersionReader = ApiVersionReader.Combine(
                //    // The Default versioning mechanism which reads the API version from the "api-version" Query String paramater.
                //    new QueryStringApiVersionReader("api-version"),
                //    // Use the following, if you would like to specify the version as a custom HTTP Header.
                //    new HeaderApiVersionReader("Accept-Version"),
                //    // Use the following, if you would like to specify the version as a Media Type Header.
                //    new MediaTypeApiVersionReader("api-version")
                //);
            });
            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}
