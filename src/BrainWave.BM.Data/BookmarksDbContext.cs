namespace BrainWave.BM.Data;

public class BookmarksDbContext : DbContext
{
    public BookmarksDbContext(DbContextOptions<BookmarksDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookmarkConfiguration());
        modelBuilder.ApplyConfiguration(new GroupConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());
    }

    public DbSet<Bookmark> Bookmarks => Set<Bookmark>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Tag> Tags => Set<Tag>();
}
