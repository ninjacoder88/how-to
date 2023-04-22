using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("1.0", new OpenApiInfo { Title = "How To Web Api", Version = "1.0" });
});
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/1.0/swagger.json", $"How To Web Api - {app.Environment.EnvironmentName}"); });
}

app.UseAuthorization()
    .UseHealthChecks("/health");

app.MapControllers();

app.Run();
