using HowTo.DataAccess;
using HowTo.WebApi.Configuration;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var internalIdentity = builder.Configuration.GetValue<string>("ServiceConnections:IdentityApiInternal");

builder.Services.AddAuthentication(ConfigurationConstants.BearerSecurityScheme)
    .AddJwtBearer(ConfigurationConstants.BearerSecurityScheme, options =>
    {
        options.Authority = internalIdentity;//this is different from the authority below because this is being called within the network
        options.TokenValidationParameters = new TokenValidationParameters { ValidateAudience = false };
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy(ConfigurationConstants.AllowEverythingCorsPolicy, p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddControllers();
builder.Services.AddHealthChecks();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var publicIdentity = builder.Configuration.GetValue<string>("ServiceConnections:IdentityApiPublic");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "How To Web Api", Version = "v1" });
    options.OperationFilter<ApiVersionOperationFilter>();
    options.CreateCustomSecurityDefintion(publicIdentity);
    options.CreateCustomSecurityRequirement();
});

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver")
    );
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "redis:6379";
});

builder.Services.AddScoped<IRepository, Repository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseCors("DoNotDoThisInProduction");
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", $"How To Web Api - {app.Environment.EnvironmentName}"); });
}

app.UseStaticFiles()
    .UseRouting()
    .UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });

app.UseAuthentication()
    .UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthChecks("/health");
});

app.Run();
