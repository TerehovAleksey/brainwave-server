namespace BrainWave.BM.Data.DTOs;

public class GroupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}

public class GroupCreateDto
{
    public string Name { get; set; } = default!;
}

