using Agronomia.Api.Features.Users.RegisterUser;

namespace Agronomia.Api.Features.Users;

public static class UsersEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapRegisterUser();
    }
}
