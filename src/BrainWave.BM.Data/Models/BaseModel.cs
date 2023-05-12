namespace BrainWave.BM.Data.Models;

public class BaseModel
{
    public Guid Id { get; set; }

    public DateTime CreatedDate { get; set; }

    [GraphQLIgnore]
    public DateTime? EditedDate { get; set; }
}
