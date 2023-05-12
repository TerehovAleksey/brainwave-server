namespace BrainWave.BM.Data.Validators;

internal class GroupCreateDtoValidator : AbstractValidator<GroupCreateDto>
{
    public GroupCreateDtoValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(2)
            .MaximumLength(50);
    }
}
