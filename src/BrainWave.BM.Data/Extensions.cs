namespace BrainWave.BM.Data;

public static class Extensions
{
    public static void InitBookmarksDatabase(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<BookmarksDbContext>>();
        using var context = factory.CreateDbContext();
        context.Database.Migrate();
    }

    public static IServiceCollection AddBmValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<TagCreateDto>, TagCreateDtoValidator>();
        services.AddScoped<IValidator<TagDto>, TagDtoValidator>();

        services.AddScoped<IValidator<GroupCreateDto>, GroupCreateDtoValidator>();
        services.AddScoped<IValidator<GroupDto>, GroupDtoValidator>();

        services.AddScoped<IValidator<BookmarkCreateDto>, BookmarkCreateDtoValidator>();
        services.AddScoped<IValidator<BookmarkEditDto>, BookmarkEditDtoValidator>();

        return services;
    }
}