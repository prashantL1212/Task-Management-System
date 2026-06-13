using FluentAssertions;
using TMS.Application.DTOs.Tasks;
using TMS.Application.Validators;
using TMS.Domain.Enums;
using TaskStatus = TMS.Domain.Enums.TaskStatus;

namespace TMS.Application.Tests.Validators;

public class UpdateTaskDtoValidatorTests
{
    private readonly UpdateTaskValidator _validator = new();

    [Fact]
    public void Validate_AllFieldsNull_Passes()
    {
        // Partial update with nothing supplied is valid.
        var result = _validator.Validate(new UpdateTaskDto());
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_AllFieldsSuppliedAndValid_Passes()
    {
        var dto = new UpdateTaskDto
        {
            Title = "Updated",
            Description = "Updated description",
            Status = TaskStatus.Done,
            Priority = TaskPriority.High,
            AssignedTo = "Bob"
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_EmptyTitleWhenProvided_Fails()
    {
        var result = _validator.Validate(new UpdateTaskDto { Title = "" });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateTaskDto.Title));
    }

    [Fact]
    public void Validate_TitleExceedsMaxLength_Fails()
    {
        var result = _validator.Validate(new UpdateTaskDto { Title = new string('a', 201) });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateTaskDto.Title));
    }

    [Fact]
    public void Validate_TitleAtMaxLengthBoundary_Passes()
    {
        var result = _validator.Validate(new UpdateTaskDto { Title = new string('a', 200) });

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidStatusEnum_Fails()
    {
        var result = _validator.Validate(new UpdateTaskDto { Status = (TaskStatus)999 });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateTaskDto.Status));
    }

    [Fact]
    public void Validate_InvalidPriorityEnum_Fails()
    {
        var result = _validator.Validate(new UpdateTaskDto { Priority = (TaskPriority)999 });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateTaskDto.Priority));
    }

    [Fact]
    public void Validate_DescriptionExceedsMaxLength_Fails()
    {
        var result = _validator.Validate(new UpdateTaskDto { Description = new string('a', 1001) });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateTaskDto.Description));
    }

    [Fact]
    public void Validate_AssignedToExceedsMaxLength_Fails()
    {
        var result = _validator.Validate(new UpdateTaskDto { AssignedTo = new string('a', 101) });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateTaskDto.AssignedTo));
    }
}
