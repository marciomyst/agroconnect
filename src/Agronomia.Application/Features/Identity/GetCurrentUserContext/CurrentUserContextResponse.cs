namespace Agronomia.Application.Features.Identity.GetCurrentUserContext;

public sealed record CurrentUserContextResponse(
    Guid UserId,
    string Email,
    string? Name,
    IReadOnlyList<CurrentUserOrganizationDto> Organizations);

public sealed record CurrentUserOrganizationDto(
    Guid OrganizationId,
    string OrganizationName,
    string Type,
    IReadOnlyList<string> Roles);
