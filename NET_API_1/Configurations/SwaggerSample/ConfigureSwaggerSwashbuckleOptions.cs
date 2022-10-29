using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NET_API_1.Configurations.SwaggerSample
{
    //public class ConfigureSwaggerSwashbuckleOptions : IConfigureOptions<SwaggerGenOptions>
    //{
    //    private readonly IApiVersionDescriptionProvider provider;

    //    public ConfigureSwaggerSwashbuckleOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;
    //    public void Configure(SwaggerGenOptions options)
    //    {
    //        // Add a swagger document for each discovered API version.
    //        // Note: you might choose to skip or document deprecated API versions differently.
    //        foreach (var description in provider.ApiVersionDescriptions)
    //        {
    //            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
    //        }
    //    }
    //    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    //    {
    //        var info = new OpenApiInfo()
    //        {
    //            Title = "Web API Documentation Tutorial",
    //            Version = description.ApiVersion.ToString(),
    //            Description = "A tutorial project to provide documentation for our existing APIs.",
    //            Contact = new OpenApiContact() { Name = "Duc Tam", Email = "info@dotnetnakama.com" },
    //            License = new OpenApiLicense() { Name = "MIT License", Url = new Uri("https://opensource.org/licenses/MIT") }
    //        };

    //        if (description.IsDeprecated)
    //        {
    //            info.Description += " [This API version has been deprecated]";
    //        }

    //        return info;
    //    }
    //}
}
