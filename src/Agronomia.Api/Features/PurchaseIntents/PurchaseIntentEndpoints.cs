using Agronomia.Api.Features.PurchaseIntents.CreatePurchaseIntent;
using Agronomia.Api.Features.PurchaseIntents.GetMyFarmPurchaseIntents;
using Agronomia.Api.Features.PurchaseIntents.GetMySellerPurchaseIntents;
using Agronomia.Api.Features.PurchaseIntents.UpdatePurchaseIntentStatus;

namespace Agronomia.Api.Features.PurchaseIntents;

public static class PurchaseIntentEndpoints
{
    public static void MapPurchaseIntentEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapCreatePurchaseIntent();
        app.MapGetMyFarmPurchaseIntents();
        app.MapGetMySellerPurchaseIntents();
        app.MapUpdatePurchaseIntentStatus();
    }
}
