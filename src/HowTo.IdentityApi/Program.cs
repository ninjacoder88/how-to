using HowTo.DataAccess;
using HowTo.IdentityApi;
using HowTo.IdentityApi.Configuration;
using HowTo.IdentityApi.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Validation;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllersWithViews();

var identityServerBuilder = 
    builder.Services.AddIdentityServer(options =>
    {
        options.EmitScopesAsSpaceDelimitedStringInJwt = true;
    }).AddInMemoryIdentityResources(IdentityResourceConfiguration.IdentityResources)
    .AddInMemoryApiScopes(ApiScopeConfiguration.ApiScopes)
    .AddInMemoryClients(ClientConfiguration.Clients)
    .AddProfileService<HowToProfileService>();

builder.Services.AddScoped<IProfileService, HowToProfileService>()
    .AddScoped<IRepository, Repository>()
    .AddScoped<IResourceOwnerPasswordValidator, HowToResourceOwnerPasswordValidator>();

if (builder.Environment.IsProduction())
    identityServerBuilder.AddProductionCredential(builder.Configuration);

if(builder.Environment.IsStaging() || builder.Environment.IsDevelopment())
{
    identityServerBuilder.AddDeveloperSigningCredential();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("DoNotDoThisInProduction", policy =>
        {
            policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
        });
    });
}    

var app = builder.Build();
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseDeveloperExceptionPage();
    app.UseCors("DoNotDoThisInProduction");
}

//app.UseStaticFiles();
//app.UseRouting();

app.UseIdentityServer();

//app.UseAuthorization();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapDefaultControllerRoute();
//});

app.Run();