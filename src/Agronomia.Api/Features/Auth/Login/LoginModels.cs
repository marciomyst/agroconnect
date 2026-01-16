namespace Agronomia.Api.Features.Auth.Login;

public sealed record LoginRequest(string EmailOrPhone, string Password);

public sealed record LoginResponse(bool Success, string? AccessToken, string? Message);
