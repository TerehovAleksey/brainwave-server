namespace BrainWave.BM.Data.Models;

public class Bookmark : BaseModel
{
    //Ссылка
    public string Link { get; set; } = default!;

    //Рейтинг/Важность
    public int Rating { get; set; }

    //Закреплена
    public bool IsPinned { get; set; }

    //Порядок отображения
    public int Order { get; set; }

    [GraphQLIgnore]
    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }

    public List<Tag> Tags { get; set; } = new();
}
