namespace TimeTracker.Shared.Models.Login;

public record struct LoginResponse(bool Success, string? Error, string? Token);