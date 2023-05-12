namespace BrainWave.BM.Data.Gql;

public class Mutations
{
    #region Tags

    /// <summary>
    /// Создание тега
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    [UseProjection]
    public async Task<IQueryable<Tag>> CreateTagAsync(BookmarksDbContext context, [UseFluentValidation] TagCreateDto tag)
    {
        var created = new Tag
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.UtcNow,
            Name = tag.Name,
        };

        context.Tags.Add(created);
        await context.SaveChangesAsync();

        return context.Tags.Where(x => x.Id == created.Id);
    }

    /// <summary>
    /// Переименование тега
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    /// <exception cref="GraphQLException"></exception>
    [UseProjection]
    public async Task<IQueryable<Tag>> UpdateTagAsync(BookmarksDbContext context, [UseFluentValidation] TagDto tag)
    {
        var result = await context.Tags.FirstOrDefaultAsync(x => x.Id == tag.Id);
        if (result is null)
        {
            throw new GraphQLException("Record not found!");
        }

        result.Name = tag.Name;
        result.EditedDate = DateTime.UtcNow;
        await context.SaveChangesAsync();

        return context.Tags.Where(x => x.Id == tag.Id);
    }

    /// <summary>
    /// Удаление тега
    /// </summary>
    /// <param name="id">Tag ID</param>
    /// <returns></returns>
    public async Task<bool> DeleteTagAsync(BookmarksDbContext context, Guid id)
    {
        var result = await context.Tags
             .Where(x => x.Id == id)
             .ExecuteDeleteAsync();

        return result > 0;
    }

    #endregion

    #region Groups

    /// <summary>
    /// Создание группы
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    [UseProjection]
    public async Task<IQueryable<Group>> CreateGroup(BookmarksDbContext context, [UseFluentValidation] GroupCreateDto group)
    {
        var created = new Group
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.UtcNow,
            Name = group.Name,
        };

        context.Groups.Add(created);
        await context.SaveChangesAsync();

        return context.Groups.Where(x => x.Id == created.Id);
    }

    /// <summary>
    /// Переименование группы
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    /// <exception cref="GraphQLException"></exception>
    [UseProjection]
    public async Task<IQueryable<Group>> UpdateGroup(BookmarksDbContext context, [UseFluentValidation] GroupDto group)
    {
        var result = await context.Groups.FirstOrDefaultAsync(x => x.Id == group.Id);
        if (result is null)
        {
            throw new GraphQLException("Record not found!");
        }

        result.Name = group.Name;
        result.EditedDate = DateTime.UtcNow;
        await context.SaveChangesAsync();

        return context.Groups.Where(x => x.Id == group.Id);
    }

    /// <summary>
    /// Удаление группы
    /// </summary>
    /// <param name="id">Group ID</param>
    /// <returns></returns>
    public async Task<bool> DeleteGroup(BookmarksDbContext context, Guid id)
    {
        var result = await context.Groups
             .Where(x => x.Id == id)
             .ExecuteDeleteAsync();

        return result > 0;
    }

    #endregion

    #region Bookmarks

    /// <summary>
    /// Создание закладки
    /// </summary>
    /// <param name="bookmark"></param>
    /// <returns></returns>
    [UseProjection]
    public async Task<IQueryable<Bookmark>> CreateBookmarkAsync(BookmarksDbContext context, [UseFluentValidation] BookmarkCreateDto bookmark)
    {
        var result = new Bookmark
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.UtcNow,
            Link = bookmark.Link,
            IsPinned = false,
            Order = 0,
            Rating = 0
        };

        context.Bookmarks.Add(result);
        await context.SaveChangesAsync();

        return context.Bookmarks.Where(x => x.Id == result.Id);
    }

    /// <summary>
    /// Добавление тега к закладке
    /// </summary>
    /// <param name="id">Bookmark ID</param>
    /// <param name="tagId">Tag ID</param>
    /// <returns></returns>
    /// <exception cref="GraphQLException"></exception>
    [UseProjection]
    public async Task<IQueryable<Bookmark>> AddTagToBookmarkAsync(BookmarksDbContext context, Guid id, Guid tagId)
    {
        var result = await context.Bookmarks
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == id) ??
            throw new GraphQLException(Constants.NotFoundMessage);

        var tag = await context.Tags.FirstOrDefaultAsync(x => x.Id == tagId) ?? 
            throw new GraphQLException(Constants.NotFoundMessage);

        result.Tags.Add(tag);
        result.EditedDate = DateTime.UtcNow;
        await context.SaveChangesAsync();

        return context.Bookmarks.Where(x => x.Id == id);
    }

    /// <summary>
    /// Удаление тега закладки
    /// </summary>
    /// <param name="id">Bookmark ID</param>
    /// <param name="tagId">Tag ID</param>
    /// <returns></returns>
    /// <exception cref="GraphQLException"></exception>
    [UseProjection]
    public async Task<IQueryable<Bookmark>> RemoveTagFromBookmarkAsync(BookmarksDbContext context, Guid id, Guid tagId)
    {
        var result = await context.Bookmarks
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == id) ?? 
            throw new GraphQLException(Constants.NotFoundMessage);

        var deleted = result.Tags.Find(x => x.Id == tagId);
        if (deleted is not null)
        {
            result.Tags.Remove(deleted);
            result.EditedDate = DateTime.UtcNow;
        }
        await context.SaveChangesAsync();

        return context.Bookmarks.Where(x => x.Id == id);
    }

    /// <summary>
    /// Перемещение закладки из группу
    /// </summary>
    /// <param name="id">Bookmark ID</param>
    /// <param name="groupId">Group ID</param>
    /// <returns></returns>
    [UseProjection]
    public async Task<IQueryable<Bookmark>> MoveBookmarkToGroupAsync(BookmarksDbContext context, Guid id, Guid groupId)
    {
        var groupExist = await context.Groups.AnyAsync(x => x.Id == groupId);
        if (!groupExist)
        {
            throw new GraphQLException(Constants.NotFoundMessage);
        }

        var result = await context.Bookmarks.FirstOrDefaultAsync(x => x.Id == id) ??
            throw new GraphQLException(Constants.NotFoundMessage);

        result.GroupId = groupId;
        result.EditedDate = DateTime.UtcNow;
        await context.SaveChangesAsync();

        return context.Bookmarks.Where(x => x.Id == id);
    }

    /// <summary>
    /// Удаление закладки из группы
    /// </summary>
    /// <param name="id">Bookmark ID</param>
    /// <returns></returns>
    [UseProjection]
    public async Task<IQueryable<Bookmark>> RemoveBookmarkFromGroupAsync(BookmarksDbContext context, Guid id)
    {
        var result = await context.Bookmarks.FirstOrDefaultAsync(x => x.Id == id) ?? 
            throw new GraphQLException(Constants.NotFoundMessage);

        result.GroupId = null;
        result.EditedDate = DateTime.UtcNow;
        await context.SaveChangesAsync();

        return context.Bookmarks.Where(x => x.Id == id);
    }

    /// <summary>
    /// Удаление закладки
    /// </summary>
    /// <param name="id">Bookmark ID</param>
    /// <returns></returns>
    public async Task<bool> DeleteBookmarkAsync(BookmarksDbContext context, Guid id)
    {
        var result = await context.Bookmarks
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        return result > 0;
    }

    /// <summary>
    /// Редактирование закладки (ссылка, закрепление, рейтинг, порядок)
    /// </summary>
    /// <param name="bookmark"></param>
    /// <returns></returns>
    /// <exception cref="GraphQLException"></exception>
    [UseProjection]
    public async Task<IQueryable<Bookmark>> EditBookmarkAsync(BookmarksDbContext context, [UseFluentValidation] BookmarkEditDto bookmark)
    {
        var result = await context.Bookmarks.FirstOrDefaultAsync(x => x.Id == bookmark.Id) ??
            throw new GraphQLException(Constants.NotFoundMessage);

        if (bookmark.Link != null)
        {
            result.Link = bookmark.Link;
        }

        if (bookmark.Rating != null)
        {
            result.Rating = (int)bookmark.Rating;
        }

        if (bookmark.IsPinned != null)
        {
            result.IsPinned = (bool)bookmark.IsPinned;
        }

        if (bookmark.Order != null)
        {
            result.Order = (int)bookmark.Order;
        }

        await context.SaveChangesAsync();

        return context.Bookmarks.Where(x => x.Id == bookmark.Id);
    }

    #endregion
}
