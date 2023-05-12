namespace BrainWave.Common.Models;

public class User
{
    public string Id { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Username { get; set; } = default!;
    public bool EmailVerified { get; set; }
}
