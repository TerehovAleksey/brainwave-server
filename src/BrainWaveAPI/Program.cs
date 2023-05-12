Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

Log.Information("Starting up!");

try
{
    var builder = WebApplication.CreateBuilder(args);

    //Serilog configuration
    builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console());

    //Firebase configuration
    var firebaseConfigPath = builder.Configuration.GetValue<string>("FirebaseConfig");
    builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions()
    {
        Credential = GoogleCredential.FromFile(firebaseConfigPath),
    }));

    //Application Services
    builder.Services.AddBmValidation();

    //Data Context
    var connectionString = builder.Configuration.GetConnectionString("BookmarksDb");
    builder.Services.AddPooledDbContextFactory<BookmarksDbContext>(opt => opt.UseSqlServer(connectionString));

    //GraphQL configuration
    builder.Services.AddGraphQLServer()
        .AddAuthorization()
        //.AddMaxExecutionDepthRule(4)
        .RegisterDbContext<BookmarksDbContext>(DbContextKind.Pooled)
        .AddQueryType<Query>()
        .AddMutationType<Mutations>()
        .AddProjections()
        .AddSorting()
        .AddFiltering()
        .AddFluentValidation(opt =>
        {
            opt.UseDefaultErrorMapper();
        });

    //Authentication
    builder.Services.AddFirebaseAuthentication();

    builder.Services.AddAuthorization();

    var app = builder.Build();

    //Init and Seed Databases
    app.Services.InitBookmarksDatabase();

    Log.Warning($"Environment: {app.Environment.EnvironmentName}");

    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapGraphQL();
    app.Run();

    Log.Information("Stopped cleanly");
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}