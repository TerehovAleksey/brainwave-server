namespace BrainWave.BM.Data.Gql;

public class Query
{
    [Authorize]
    [UseUser]
    [UseProjection]
    [UseSorting]
    [UseFiltering]
    public IQueryable<Bookmark> GetBookmarks(BookmarksDbContext context, [User] User user) =>
        context.Bookmarks.Where(x => x.UserId == user.Id);

    [Authorize]
    [UseUser]
    [UseProjection]
    [UseSorting]
    public IQueryable<Group> GetGroups(BookmarksDbContext context, [User] User user) =>
        context.Groups.Where(x => x.UserId == user.Id);

    [Authorize]
    [UseUser]
    [UseProjection]
    [UseSorting]
    public IQueryable<Tag> GetTags(BookmarksDbContext context, [User] User user) =>
        context.Tags.Where(x => x.UserId == user.Id);
}
