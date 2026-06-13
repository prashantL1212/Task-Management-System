using FluentAssertions;
using TMS.Application.DTOs.Tasks;
using TMS.Application.Validators;
using TMS.Domain.Enums;

namespace TMS.Application.Tests.Validators;

public class CreateTaskDtoValidatorTests
{
    private readonly CreateTaskValidator _validator = new();

    private static CreateTaskDto Valid() => new()
    {
        Title = "Valid title",
        Description = "Valid description",
        Priority = TaskPriority.Medium,
        AssignedTo = "Alice"
    };

    [Fact]
    public void Validate_ValidModel_Passes()
    {
        var result = _validator.Validate(Valid());
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_EmptyTitle_Fails()
    {
        var dto = Valid();
        dto.Title = "";

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateTaskDto.Title));
    }

    [Fact]
    public void Validate_TitleExceedsMaxLength_Fails()
    {
        var dto = Valid();
        dto.Title = new string('a', 201);

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateTaskDto.Title));
    }

    [Fact]
    public void Validate_TitleAtMaxLengthBoundary_Passes()
    {
        var dto = Valid();
        dto.Title = new string('a', 200);

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DescriptionExceedsMaxLength_Fails()
    {
        var dto = Valid();
        dto.Description = new string('a', 1001);

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateTaskDto.Description));
    }

    [Fact]
    public void Validate_AssignedToExceedsMaxLength_Fails()
    {
        var dto = Valid();
        dto.AssignedTo = new string('a', 101);

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateTaskDto.AssignedTo));
    }

    [Fact]
    public void Validate_InvalidPriorityEnum_Fails()
    {
        var dto = Valid();
        dto.Priority = (TaskPriority)999;

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateTaskDto.Priority));
    }

    [Fact]
    public void Validate_NullOptionalFields_Passes()
    {
        var dto = Valid();
        dto.Description = null;
        dto.AssignedTo = null;

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }
}
