using FluentAssertions;
using Moq;
using TMS.Application.DTOs.Auth;
using TMS.Application.Interfaces.Repositories;
using TMS.Application.Interfaces.Security;
using TMS.Application.Services;
using TMS.Domain.Entities;

namespace TMS.Application.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepo = new();
    private readonly Mock<IPasswordHasher> _hasher = new();
    private readonly Mock<IJwtTokenGenerator> _jwt = new();
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _sut = new AuthService(_userRepo.Object, _hasher.Object, _jwt.Object);
    }

    private static User SampleUser() => new()
    {
        Id = 1,
        Username = "admin",
        PasswordHash = "hashed",
        Role = "Admin"
    };

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsLoginResponseDto()
    {
        // Arrange
        var user = SampleUser();
        var expiry = new DateTime(2030, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        _userRepo.Setup(r => r.GetByUsernameAsync("admin", It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _hasher.Setup(h => h.Verify("pw", "hashed")).Returns(true);
        _jwt.Setup(j => j.GenerateToken(user)).Returns(new AuthTokenResult("token-123", expiry));

        // Act
        var result = await _sut.LoginAsync(new LoginRequestDto { Username = "admin", Password = "pw" });

        // Assert
        result.Should().NotBeNull();
        result!.Token.Should().Be("token-123");
        result.Username.Should().Be("admin");
        result.Role.Should().Be("Admin");
        result.ExpiresAt.Should().Be(expiry);
    }

    [Fact]
    public async Task LoginAsync_UserDoesNotExist_ReturnsNull()
    {
        // Arrange
        _userRepo.Setup(r => r.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((User?)null);

        // Act
        var result = await _sut.LoginAsync(new LoginRequestDto { Username = "ghost", Password = "pw" });

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ReturnsNull()
    {
        // Arrange
        _userRepo.Setup(r => r.GetByUsernameAsync("admin", It.IsAny<CancellationToken>())).ReturnsAsync(SampleUser());
        _hasher.Setup(h => h.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        // Act
        var result = await _sut.LoginAsync(new LoginRequestDto { Username = "admin", Password = "wrong" });

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_CallsGenerateTokenOnce()
    {
        // Arrange
        var user = SampleUser();
        _userRepo.Setup(r => r.GetByUsernameAsync("admin", It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _hasher.Setup(h => h.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        _jwt.Setup(j => j.GenerateToken(It.IsAny<User>())).Returns(new AuthTokenResult("t", DateTime.UtcNow));

        // Act
        await _sut.LoginAsync(new LoginRequestDto { Username = "admin", Password = "pw" });

        // Assert
        _jwt.Verify(j => j.GenerateToken(user), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_UserDoesNotExist_DoesNotCallGenerateToken()
    {
        // Arrange
        _userRepo.Setup(r => r.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((User?)null);

        // Act
        await _sut.LoginAsync(new LoginRequestDto { Username = "ghost", Password = "pw" });

        // Assert
        _jwt.Verify(j => j.GenerateToken(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_DoesNotCallGenerateToken()
    {
        // Arrange
        _userRepo.Setup(r => r.GetByUsernameAsync("admin", It.IsAny<CancellationToken>())).ReturnsAsync(SampleUser());
        _hasher.Setup(h => h.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        // Act
        await _sut.LoginAsync(new LoginRequestDto { Username = "admin", Password = "wrong" });

        // Assert
        _jwt.Verify(j => j.GenerateToken(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ExpiresAtComesFromAuthTokenResult()
    {
        // Arrange
        var expiry = new DateTime(2027, 6, 13, 12, 0, 0, DateTimeKind.Utc);
        _userRepo.Setup(r => r.GetByUsernameAsync("admin", It.IsAny<CancellationToken>())).ReturnsAsync(SampleUser());
        _hasher.Setup(h => h.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        _jwt.Setup(j => j.GenerateToken(It.IsAny<User>())).Returns(new AuthTokenResult("t", expiry));

        // Act
        var result = await _sut.LoginAsync(new LoginRequestDto { Username = "admin", Password = "pw" });

        // Assert
        result!.ExpiresAt.Should().Be(expiry);
    }
}
