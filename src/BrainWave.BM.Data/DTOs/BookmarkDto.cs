namespace BrainWave.BM.Data.DTOs;

public class BookmarkDto
{
    public Guid Id { get; set; }
}

public class BookmarkCreateDto
{
    public string Link { get; set; } = default!;
}

public class BookmarkEditDto
{
    public Guid Id { get; set; }
    public string? Link { get; set; } = default!;
    public int? Rating { get; set; }
    public bool? IsPinned { get; set; }
    public int? Order { get; set; }
}
