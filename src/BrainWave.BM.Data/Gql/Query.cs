namespace BrainWave.BM.Data.Gql;

public class Query
{
    [UseProjection]
    [UseSorting]
    [UseFiltering]
    public IQueryable<Bookmark> GetBookmarks(BookmarksDbContext context) => context.Bookmarks;

    [UseProjection]
    [UseSorting]
    public IQueryable<Group> GetGroups(BookmarksDbContext context) => context.Groups;

    [UseProjection]
    [UseSorting]
    public IQueryable<Tag> GetTags(BookmarksDbContext context) => context.Tags;
}
