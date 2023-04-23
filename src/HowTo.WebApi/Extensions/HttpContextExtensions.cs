namespace HowTo.WebApi.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUsername(this HttpContext context)
        {
            var claimDictionary = context.User.Claims.ToDictionary(k => k.Type, k => k.Value);
            return context?.User?.Identity?.Name ?? claimDictionary["client_id"] ?? throw new Exception("Invalid identity");
        }
    }
}
