using FluentAssertions;
using Moq;
using TMS.Application.DTOs.Tasks;
using TMS.Application.Interfaces.Repositories;
using TMS.Application.Services;
using TMS.Domain.Entities;
using TMS.Domain.Enums;
using TaskStatus = TMS.Domain.Enums.TaskStatus;

namespace TMS.Application.Tests.Services;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _repo = new();
    private readonly TaskService _sut;

    public TaskServiceTests()
    {
        _sut = new TaskService(_repo.Object);
    }

    private static TaskItem SampleTask(int id = 1) => new()
    {
        Id = id,
        Title = "Existing",
        Description = "Existing description",
        Status = TaskStatus.InProgress,
        Priority = TaskPriority.High,
        AssignedTo = "Alice",
        IsDeleted = false,
        CreatedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    };

    // ---------- GetTasksAsync ----------

    [Fact]
    public async Task GetTasksAsync_RepositoryReturnsTasks_ReturnsMappedDtos()
    {
        // Arrange
        var tasks = new List<TaskItem> { SampleTask(1), SampleTask(2) };
        _repo.Setup(r => r.GetAllAsync(It.IsAny<TaskStatus?>(), It.IsAny<TaskPriority?>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(tasks);

        // Act
        var result = await _sut.GetTasksAsync(new TaskFilterDto());

        // Assert
        result.Should().HaveCount(2);
        result[0].Id.Should().Be(1);
        result[0].Title.Should().Be("Existing");
        result[0].Status.Should().Be(TaskStatus.InProgress);
        result[0].Priority.Should().Be(TaskPriority.High);
        result[0].AssignedTo.Should().Be("Alice");
    }

    [Fact]
    public async Task GetTasksAsync_RepositoryReturnsNoTasks_ReturnsEmptyList()
    {
        // Arrange
        _repo.Setup(r => r.GetAllAsync(It.IsAny<TaskStatus?>(), It.IsAny<TaskPriority?>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(new List<TaskItem>());

        // Act
        var result = await _sut.GetTasksAsync(new TaskFilterDto());

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTasksAsync_WithFilter_PassesFilterToRepository()
    {
        // Arrange
        _repo.Setup(r => r.GetAllAsync(It.IsAny<TaskStatus?>(), It.IsAny<TaskPriority?>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(new List<TaskItem>());

        // Act
        await _sut.GetTasksAsync(new TaskFilterDto { Status = TaskStatus.Done, Priority = TaskPriority.Low });

        // Assert
        _repo.Verify(r => r.GetAllAsync(TaskStatus.Done, TaskPriority.Low, It.IsAny<CancellationToken>()), Times.Once);
    }

    // ---------- GetTaskByIdAsync ----------

    [Fact]
    public async Task GetTaskByIdAsync_TaskExists_ReturnsTaskDto()
    {
        // Arrange
        _repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(SampleTask(1));

        // Act
        var result = await _sut.GetTaskByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Title.Should().Be("Existing");
    }

    [Fact]
    public async Task GetTaskByIdAsync_TaskDoesNotExist_ReturnsNull()
    {
        // Arrange
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((TaskItem?)null);

        // Act
        var result = await _sut.GetTaskByIdAsync(99);

        // Assert
        result.Should().BeNull();
    }

    // ---------- CreateTaskAsync ----------

    [Fact]
    public async Task CreateTaskAsync_ValidRequest_CreatesTaskSuccessfully()
    {
        // Arrange
        TaskItem? captured = null;
        _repo.Setup(r => r.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
             .Callback<TaskItem, CancellationToken>((t, _) => captured = t)
             .Returns(Task.CompletedTask);
        var dto = new CreateTaskDto { Title = "New", Description = "Desc", Priority = TaskPriority.Critical, AssignedTo = "Bob" };

        // Act
        var result = await _sut.CreateTaskAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("New");
        result.Description.Should().Be("Desc");
        result.Priority.Should().Be(TaskPriority.Critical);
        result.AssignedTo.Should().Be("Bob");
        captured.Should().NotBeNull();
        captured!.Title.Should().Be("New");
    }

    [Fact]
    public async Task CreateTaskAsync_AnyInput_AlwaysSetsStatusToToDo()
    {
        // Arrange
        TaskItem? captured = null;
        _repo.Setup(r => r.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
             .Callback<TaskItem, CancellationToken>((t, _) => captured = t)
             .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.CreateTaskAsync(new CreateTaskDto { Title = "X", Priority = TaskPriority.High });

        // Assert
        captured!.Status.Should().Be(TaskStatus.ToDo);
        result.Status.Should().Be(TaskStatus.ToDo);
    }

    [Fact]
    public async Task CreateTaskAsync_ValidRequest_CallsAddAsyncExactlyOnce()
    {
        // Arrange
        _repo.Setup(r => r.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        await _sut.CreateTaskAsync(new CreateTaskDto { Title = "X", Priority = TaskPriority.Low });

        // Assert
        _repo.Verify(r => r.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateTaskAsync_ValidRequest_CallsSaveChangesAsyncExactlyOnce()
    {
        // Arrange
        _repo.Setup(r => r.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        await _sut.CreateTaskAsync(new CreateTaskDto { Title = "X", Priority = TaskPriority.Low });

        // Assert
        _repo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    // ---------- UpdateTaskAsync ----------

    [Fact]
    public async Task UpdateTaskAsync_TaskExists_ReturnsUpdatedDto()
    {
        // Arrange
        _repo.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(SampleTask(5));

        // Act
        var result = await _sut.UpdateTaskAsync(5, new UpdateTaskDto { Title = "Updated" });

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Updated");
    }

    [Fact]
    public async Task UpdateTaskAsync_TaskDoesNotExist_ReturnsNull()
    {
        // Arrange
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((TaskItem?)null);

        // Act
        var result = await _sut.UpdateTaskAsync(99, new UpdateTaskDto { Title = "X" });

        // Assert
        result.Should().BeNull();
        _repo.Verify(r => r.Update(It.IsAny<TaskItem>()), Times.Never);
        _repo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateTaskAsync_PartialUpdate_UpdatesOnlySuppliedFields()
    {
        // Arrange
        _repo.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(SampleTask(5));

        // Act — supply only Status
        var result = await _sut.UpdateTaskAsync(5, new UpdateTaskDto { Status = TaskStatus.Done });

        // Assert
        result!.Status.Should().Be(TaskStatus.Done);              // changed
        result.Title.Should().Be("Existing");                     // unchanged
        result.Description.Should().Be("Existing description");   // unchanged
        result.Priority.Should().Be(TaskPriority.High);           // unchanged
        result.AssignedTo.Should().Be("Alice");                   // unchanged
    }

    [Fact]
    public async Task UpdateTaskAsync_NullFields_LeaveExistingValuesUnchanged()
    {
        // Arrange
        _repo.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(SampleTask(5));

        // Act — nothing supplied
        var result = await _sut.UpdateTaskAsync(5, new UpdateTaskDto());

        // Assert
        result!.Title.Should().Be("Existing");
        result.Description.Should().Be("Existing description");
        result.Status.Should().Be(TaskStatus.InProgress);
        result.Priority.Should().Be(TaskPriority.High);
        result.AssignedTo.Should().Be("Alice");
    }

    [Fact]
    public async Task UpdateTaskAsync_TaskExists_CallsUpdateExactlyOnce()
    {
        // Arrange
        _repo.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(SampleTask(5));

        // Act
        await _sut.UpdateTaskAsync(5, new UpdateTaskDto { Title = "X" });

        // Assert
        _repo.Verify(r => r.Update(It.IsAny<TaskItem>()), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskAsync_TaskExists_CallsSaveChangesAsyncExactlyOnce()
    {
        // Arrange
        _repo.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(SampleTask(5));

        // Act
        await _sut.UpdateTaskAsync(5, new UpdateTaskDto { Title = "X" });

        // Assert
        _repo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    // ---------- DeleteTaskAsync ----------

    [Fact]
    public async Task DeleteTaskAsync_TaskExists_ReturnsTrue()
    {
        // Arrange
        _repo.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(SampleTask(5));

        // Act
        var result = await _sut.DeleteTaskAsync(5);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteTaskAsync_TaskExists_SetsIsDeletedTrue()
    {
        // Arrange
        var existing = SampleTask(5);
        _repo.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(existing);

        // Act
        await _sut.DeleteTaskAsync(5);

        // Assert
        existing.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteTaskAsync_TaskExists_CallsUpdateAndSaveChanges()
    {
        // Arrange
        _repo.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(SampleTask(5));

        // Act
        await _sut.DeleteTaskAsync(5);

        // Assert
        _repo.Verify(r => r.Update(It.IsAny<TaskItem>()), Times.Once);
        _repo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteTaskAsync_TaskDoesNotExist_ReturnsFalse()
    {
        // Arrange
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((TaskItem?)null);

        // Act
        var result = await _sut.DeleteTaskAsync(99);

        // Assert
        result.Should().BeFalse();
        _repo.Verify(r => r.Update(It.IsAny<TaskItem>()), Times.Never);
        _repo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    // ---------- GetSummaryAsync ----------

    [Fact]
    public async Task GetSummaryAsync_ReturnsSummaryFromRepository()
    {
        // Arrange
        var summary = new TaskSummaryDto
        {
            Total = 3,
            Groups = new List<TaskSummaryItemDto>
            {
                new() { Status = TaskStatus.ToDo, Priority = TaskPriority.High, Count = 3 }
            }
        };
        _repo.Setup(r => r.GetSummaryAsync(It.IsAny<CancellationToken>())).ReturnsAsync(summary);

        // Act
        var result = await _sut.GetSummaryAsync();

        // Assert
        result.Should().BeSameAs(summary);
        result.Total.Should().Be(3);
        result.Groups.Should().ContainSingle();
    }
}
