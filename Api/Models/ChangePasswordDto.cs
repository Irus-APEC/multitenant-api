namespace Api.Models;

public record ChangePasswordDto(string Username, string CurrentPassword, string NewPassword);