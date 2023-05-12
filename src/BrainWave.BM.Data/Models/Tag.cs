namespace BrainWave.BM.Data.Models;

public class Tag : BaseModel
{
    public string Name { get; set; } = default!;

    public List<Bookmark> Bookmarks { get; set; } = new();
}
