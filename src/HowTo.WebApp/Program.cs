using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var internalIdentity = builder.Configuration.GetValue<string>("ServiceConnections:IdentityApiInternal");

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = internalIdentity;//this is different from the authority below because this is being called within the network
        options.TokenValidationParameters = new TokenValidationParameters { ValidateAudience = false };
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddHealthChecks();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles()
    .UseRouting()
    .UseHealthChecks("/health");

app.UseAuthentication()
    .UseAuthorization();

app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
    });

app.Run();
