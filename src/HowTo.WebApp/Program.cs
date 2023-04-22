var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddAuthentication();
//builder.Services.AddAuthorization();
builder.Services.AddHealthChecks();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles()
    .UseRouting()
    .UseHealthChecks("/health")
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
    });

app.Run();
