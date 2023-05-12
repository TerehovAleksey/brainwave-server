namespace BrainWaveAPI.Firebase;

public class FirebaseAuthenticationFunctionHandler
{
    private const string BEARER_PREFIX = "Bearer ";

    private readonly FirebaseApp _firebaseApp;

    public FirebaseAuthenticationFunctionHandler(FirebaseApp firebaseApp)
    {
        _firebaseApp = firebaseApp;
    }

    public Task<AuthenticateResult> HandleAuthenticateAsync(HttpRequest request) =>
        HandleAuthenticateAsync(request.HttpContext);

    public async Task<AuthenticateResult> HandleAuthenticateAsync(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.NoResult();
        }

        string? bearerToken = context.Request.Headers["Authorization"];

        if (bearerToken == null || !bearerToken.StartsWith(BEARER_PREFIX))
        {
            return AuthenticateResult.Fail("Invalid scheme.");
        }

        string token = bearerToken[BEARER_PREFIX.Length..];

        try
        {
            var firebaseToken = await FirebaseAuth.GetAuth(_firebaseApp).VerifyIdTokenAsync(token);

            return AuthenticateResult.Success(CreateAuthenticationTicket(firebaseToken));
        }
        catch (Exception ex)
        {
            return AuthenticateResult.Fail(ex);
        }
    }

    private AuthenticationTicket CreateAuthenticationTicket(FirebaseToken firebaseToken)
    {
        var claimsPrincipal = new ClaimsPrincipal(new List<ClaimsIdentity>()
        {
           new ClaimsIdentity(ToClaims(firebaseToken.Claims), nameof(ClaimsIdentity))
        });

        return new AuthenticationTicket(claimsPrincipal, JwtBearerDefaults.AuthenticationScheme);
    }

    private IEnumerable<Claim> ToClaims(IReadOnlyDictionary<string, object> claims)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        return new List<Claim>
        {
            new Claim(UserClaimType.ID, claims.GetValueOrDefault("user_id", "").ToString()),
            new Claim(UserClaimType.EMAIL, claims.GetValueOrDefault("email", "").ToString()),
            new Claim(UserClaimType.EMAIL_VERIFIED, claims.GetValueOrDefault("email_verified", "").ToString()),
            new Claim(UserClaimType.USERNAME, claims.GetValueOrDefault("name", "").ToString()),
        };
#pragma warning restore CS8604 // Possible null reference argument.
    }
}
