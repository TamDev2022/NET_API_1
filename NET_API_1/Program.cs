using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using NET_API_1.Infrastructure.Data;
using NET_API_1.Configurations.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NET_API_1.Interfaces.IServices;
using NET_API_1.Services;
using NET_API_1.Interfaces.IRepositories;
using NET_API_1.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using NET_API_1.Configurations.SwaggerSample;
using NET_API_1.Configurations.Mapper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static NET_API_1.Configurations.AppSettings;
using NET_API_1.Configurations.Middlewares;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.")));

// Add services to the container.
builder.Services.AddControllers();

// JWTSettings
var JWTSection = builder.Configuration.GetRequiredSection("JWTSettings");
builder.Services.Configure<JWTSettings>(JWTSection);
//to validate the token which has been sent by clients
var JWTIntence = JWTSection.Get<JWTSettings>();
var SecretKey = Encoding.ASCII.GetBytes(JWTIntence.SecretKey);
builder.Services.AddSingleton(typeof(JWTSettings), JWTIntence);

// BlobAzureSettings
var BlobAzureSection = builder.Configuration.GetRequiredSection("AzureBlob");
builder.Services.Configure<BlobAzureSettings>(BlobAzureSection);
var BlobAzureIntence = BlobAzureSection.Get<BlobAzureSettings>();
builder.Services.AddSingleton(typeof(BlobAzureSettings), BlobAzureIntence);

// Mailkit
var mailsettings = builder.Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(mailsettings);
builder.Services.AddTransient<ISendMailService, SendMailService>();

// Authentication Bearer Token
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opts =>
{
    opts.RequireHttpsMetadata = false;
    opts.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = JWTIntence.Issuer,
        ValidAudience = JWTIntence.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(SecretKey),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

// Mapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPasswordEncoderService, PasswordEncoderService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IAzureStorageService, AzureStorageService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Configure the API versioning properties of the project. 
builder.Services.AddApiVersioningConfigured();
// Add a Swagger generator and Automatic Request and Response annotations:
builder.Services.AddSwaggerSwashbuckleConfigured();

builder.Services.ConfigureCors();

builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["AzureBlob:BlobConnectionString:blob"], preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["AzureBlob:BlobConnectionString:queue"], preferMsi: true);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Swagger v2
    //app.UseSwagger(options =>
    //{
    //    options.SerializeAsV2 = true;
    //});
    app.UseSwagger(); // v3
    app.UseSwaggerUI(options =>
    {
        var _provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        // build a swagger endpoint for each discovered API version
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            options.DefaultModelsExpandDepth(-1);
            options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
        }

        options.DefaultModelsExpandDepth(-1);

    });
}
else
    app.UseHsts();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.ConfigureCustomMiddleware();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGet("/", context => Task.Run(() => context.Response.Redirect("/swagger")));
});


app.Run();
