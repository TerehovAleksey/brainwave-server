namespace BrainWave.BM.Data.Validators;

internal class BookmarkCreateDtoValidator : AbstractValidator<BookmarkCreateDto>
{
    public BookmarkCreateDtoValidator()
    {
        RuleFor(x => x.Link)
            .NotEmpty()
            .MaximumLength(200);
    }
}
