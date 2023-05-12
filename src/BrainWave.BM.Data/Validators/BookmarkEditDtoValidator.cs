namespace BrainWave.BM.Data.Validators;

public class BookmarkEditDtoValidator : AbstractValidator<BookmarkEditDto>
{
    public BookmarkEditDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x=>x.Link)
            .MaximumLength(200);

        RuleFor(x => x.Rating)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(5);

        RuleFor(x => x.Order)
            .GreaterThanOrEqualTo(0);
    }
}
