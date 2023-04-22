using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HowTo.WebApi.Configuration
{
    public static class SwaggerGenOptionsExtensions
    {
        public static void CreateCustomSecurityDefintion(this SwaggerGenOptions options, string authority)
        {
            var openApiOauthFlow = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{authority}/connect/authorize", UriKind.RelativeOrAbsolute),
                TokenUrl = new Uri($"{authority}/connect/token", UriKind.RelativeOrAbsolute),
                RefreshUrl = new Uri($"{authority}/connect/token", UriKind.RelativeOrAbsolute),
                Scopes = new Dictionary<string, string>
                        {
                            { "ReadOnlyScope", "Read Only" },
                            { "WebAppScope", "Web App" },
                        }
            };

            options.AddSecurityDefinition(ConfigurationConstants.BearerAuthentication, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Scheme = ConfigurationConstants.BearerSecurityScheme,
                In = ParameterLocation.Header,
                Name = "Authorization",
                Flows = new OpenApiOAuthFlows
                {
                    Password = openApiOauthFlow,
                    ClientCredentials = openApiOauthFlow
                }
            });
        }

        public static void CreateCustomSecurityRequirement(this SwaggerGenOptions options)
        {
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = ConfigurationConstants.BearerAuthentication
                        }
                    },
                    new string[]{ }
                }
            });
        }
    }
}
