using HowTo.IdentityApi;
using HowTo.IdentityApi.Configuration;
using HowTo.IdentityApi.DataAccess;
using HowTo.IdentityApi.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Validation;

var builder = WebApplication.CreateBuilder(args);
var identityServerBuilder = 
    builder.Services.AddIdentityServer(options =>
    {
        options.EmitScopesAsSpaceDelimitedStringInJwt = true;
    }).AddInMemoryIdentityResources(IdentityResourceConfiguration.IdentityResources)
    .AddInMemoryApiScopes(ApiScopeConfiguration.ApiScopes)
    .AddInMemoryClients(ClientConfiguration.Clients)
    .AddProfileService<HowToProfileService>();

builder.Services.AddScoped<IProfileService, HowToProfileService>()
    .AddScoped<IUserRepository, UserRepository>()
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
if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
{
    app.UseDeveloperExceptionPage();
    app.UseCors("DoNotDoThisInProduction");
}
app.UseIdentityServer();

app.Run();