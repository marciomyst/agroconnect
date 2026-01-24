using Agronomia.Application.Abstractions.CQRS;

namespace Agronomia.Application.Features.Identity.GetCurrentUserContext;

public sealed record GetCurrentUserContextQuery
    : IQuery<CurrentUserContextResponse>;
