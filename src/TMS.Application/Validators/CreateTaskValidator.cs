using FluentValidation;
using TMS.Application.DTOs.Tasks;

namespace TMS.Application.Validators;

/// <summary>Validation rules for creating a task.</summary>
public class CreateTaskValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Priority must be a valid value.");

        RuleFor(x => x.AssignedTo)
            .MaximumLength(100).WithMessage("AssignedTo must not exceed 100 characters.");
    }
}
