namespace BrainWave.BM.Data.DTOs;

public class TagDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}

public class TagCreateDto
{
    public string Name { get; set; } = default!;
}
