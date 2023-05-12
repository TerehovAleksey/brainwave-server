namespace BrainWave.BM.Data.Configurations;

internal class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
{
    public void Configure(EntityTypeBuilder<Bookmark> builder)
    {
        builder.ToTable("Bookmarks")
            .HasKey(x => x.Id);

        builder.Property(x => x.Link)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasOne(x => x.Group)
            .WithMany(x => x.Bookmarks)
            .HasForeignKey(x => x.GroupId);

        builder.HasMany(x => x.Tags)
            .WithMany(x => x.Bookmarks)
            .UsingEntity("BookmarkTags");
    }
}
