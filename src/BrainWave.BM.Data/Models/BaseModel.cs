namespace BrainWave.BM.Data.Models;

public class BaseModel
{
    public Guid Id { get; set; }

    [GraphQLIgnore]
    public string UserId { get; set; } = default!;

    public DateTime CreatedDate { get; set; }

    [GraphQLIgnore]
    public DateTime? EditedDate { get; set; }
}
