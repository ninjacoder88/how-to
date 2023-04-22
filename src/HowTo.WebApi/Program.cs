using HowTo.WebApi.Configuration;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(ConfigurationConstants.BearerSecurityScheme)
    .AddJwtBearer(ConfigurationConstants.BearerSecurityScheme, options =>
    {
        options.Authority = "http://localhost:8092";
        options.TokenValidationParameters = new TokenValidationParameters { ValidateAudience = false };
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("DoNotDoThisInProduction", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddControllers();
builder.Services.AddHealthChecks();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("1.0", new OpenApiInfo { Title = "How To Web Api", Version = "1.0" });
    options.OperationFilter<ApiVersionOperationFilter>();
    options.CreateCustomSecurityDefintion("http://localhost:8092");
    options.CreateCustomSecurityRequirement();
});

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version")
    );
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseCors("DoNotDoThisInProduction");
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/1.0/swagger.json", $"How To Web Api - {app.Environment.EnvironmentName}"); });
}

app.UseAuthorization()
    .UseAuthentication()
    .UseHealthChecks("/health")
    .UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });

app.MapControllers();

app.Run();
