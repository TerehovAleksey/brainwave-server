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

    //Application Services
    builder.Services.AddBmValidation();

    //Data Context
    var connectionString = builder.Configuration.GetConnectionString("BookmarksDb");
    builder.Services.AddPooledDbContextFactory<BookmarksDbContext>(opt => opt.UseSqlServer(connectionString));

    //GraphQL
    builder.Services.AddGraphQLServer()
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

    var app = builder.Build();

    //Init and Seed Databases
    app.Services.InitBookmarksDatabase();

    Log.Warning($"Environment: {app.Environment.EnvironmentName}");

    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();
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