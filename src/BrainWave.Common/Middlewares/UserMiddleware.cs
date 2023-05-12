using BrainWave.Common.Models;
using HotChocolate.Resolvers;
using System.Security.Claims;

namespace BrainWave.Common.Middlewares;

public class UserMiddleware
{
    internal const string USER_CONTEXT_KEY = "User";
    private readonly FieldDelegate _next;

    public UserMiddleware(FieldDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(IMiddlewareContext context)
    {
        if(context.ContextData.TryGetValue("ClaimsPrincipal", out object? rawClaimsPrincipal) 
            && rawClaimsPrincipal is ClaimsPrincipal claimsPrincipal)
        {
            var user = new User
            {
                Id = claimsPrincipal.FindFirst("FirebaseUserClaimType.ID")?.Value ?? string.Empty,
                Username = claimsPrincipal.FindFirst("FirebaseUserClaimType.USERNAME")?.Value ?? string.Empty,
                Email = claimsPrincipal.FindFirst("FirebaseUserClaimType.EMAIL")?.Value ?? string.Empty,
                EmailVerified = bool.TryParse(claimsPrincipal.FindFirst("FirebaseUserClaimType.EMAIL_VERIFIED")?.Value, out bool emailVerifierd) && emailVerifierd,
            };

            context.ContextData.Add(USER_CONTEXT_KEY, user);
        }


        await _next(context);
    }
}
