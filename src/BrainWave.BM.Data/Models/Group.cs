namespace BrainWave.BM.Data.Models;

public class Group : BaseModel
{
    public string Name { get; set; } = default!;

    public List<Bookmark> Bookmarks { get; set; } = new();
}
