namespace BrainWave.BM.Data.Validators;

internal class GroupDtoValidator: AbstractValidator<GroupDto>
{
    public GroupDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .MinimumLength(2)
            .MaximumLength(50);
    }
}
