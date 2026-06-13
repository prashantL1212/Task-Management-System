using FluentValidation;
using TMS.Application.DTOs.Tasks;

namespace TMS.Application.Validators;

/// <summary>
/// Validation rules for updating a task. Because updates are partial, each rule
/// runs only when its field is supplied (non-null). Omitted fields are skipped.
/// </summary>
public class UpdateTaskValidator : AbstractValidator<UpdateTaskDto>
{
    public UpdateTaskValidator()
    {
        // Title may be omitted, but if supplied it must be non-blank and bounded.
        When(x => x.Title is not null, () =>
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title cannot be empty when provided.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");
        });

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.")
            .When(x => x.Description is not null);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid value.")
            .When(x => x.Status.HasValue);

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Priority must be a valid value.")
            .When(x => x.Priority.HasValue);

        RuleFor(x => x.AssignedTo)
            .MaximumLength(100).WithMessage("AssignedTo must not exceed 100 characters.")
            .When(x => x.AssignedTo is not null);
    }
}
