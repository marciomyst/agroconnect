using Agronomia.Crosscutting.Security;
using System.Security.Claims;

namespace Agronomia.Api.Extensions.Auth
{
    public class AgronomiaJwtTokenGenerator : JwtTokenGenerator
    {
        public AgronomiaJwtTokenGenerator(JwtTokenSettings settings) : base(settings)
        {
        }

        public override IEnumerable<Claim> GenerateClaims()
        {
            throw new NotImplementedException();
        }
    }
}
