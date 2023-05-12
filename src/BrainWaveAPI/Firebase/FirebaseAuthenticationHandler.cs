namespace BrainWaveAPI.Firebase;

public class FirebaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly FirebaseAuthenticationFunctionHandler _authenticationFunctionHandler;

    public FirebaseAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        FirebaseAuthenticationFunctionHandler authenticationFunctionHandler)
        : base(options, logger, encoder, clock)
    {
        _authenticationFunctionHandler = authenticationFunctionHandler;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        return _authenticationFunctionHandler.HandleAuthenticateAsync(Context);
    }
}