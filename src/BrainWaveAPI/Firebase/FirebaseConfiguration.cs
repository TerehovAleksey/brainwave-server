namespace BrainWaveAPI.Firebase;

public static class FirebaseConfiguration
{
    public static IServiceCollection AddFirebaseAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, _ => { });

        services.AddScoped<FirebaseAuthenticationFunctionHandler>();

        return services;
    }
}
