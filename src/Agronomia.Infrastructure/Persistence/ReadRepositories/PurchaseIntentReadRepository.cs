using System.Data;
using Agronomia.Application.Abstractions.Orders;
using Agronomia.Application.Features.PurchaseIntents.GetMyFarmPurchaseIntents;
using Agronomia.Application.Features.PurchaseIntents.GetMySellerPurchaseIntents;
using Dapper;

namespace Agronomia.Infrastructure.Persistence.ReadRepositories;

public sealed class PurchaseIntentReadRepository(IDbConnection connection) : IPurchaseIntentReadRepository
{
    private const string FarmIntentsSql = """
        SELECT
            pi."Id"               AS "PurchaseIntentId",
            pi."ProductId"        AS "ProductId",
            p."Name"              AS "ProductName",
            pi."SellerId"         AS "SellerId",
            s."CorporateName"     AS "SellerName",
            pi."SellerProductId"  AS "SellerProductId",
            pi."Quantity"         AS "Quantity",
            pi."Status"           AS "Status",
            pi."RequestedAtUtc"   AS "RequestedAtUtc"
        FROM purchase_intents pi
        INNER JOIN products p ON p."Id" = pi."ProductId"
        INNER JOIN sellers s ON s."Id" = pi."SellerId"
        WHERE pi."FarmId" = @FarmId
        ORDER BY pi."RequestedAtUtc" DESC;
        """;

    private const string SellerIntentsSql = """
        SELECT
            pi."Id"               AS "PurchaseIntentId",
            pi."ProductId"        AS "ProductId",
            p."Name"              AS "ProductName",
            pi."FarmId"           AS "FarmId",
            f."Name"              AS "FarmName",
            pi."SellerProductId"  AS "SellerProductId",
            pi."Quantity"         AS "Quantity",
            pi."Status"           AS "Status",
            pi."RequestedAtUtc"   AS "RequestedAtUtc"
        FROM purchase_intents pi
        INNER JOIN products p ON p."Id" = pi."ProductId"
        INNER JOIN farms f ON f."Id" = pi."FarmId"
        WHERE pi."SellerId" = @SellerId
        ORDER BY pi."RequestedAtUtc" DESC;
        """;

    public async Task<IReadOnlyList<FarmPurchaseIntentDto>> GetFarmPurchaseIntentsAsync(
        Guid farmId,
        CancellationToken ct)
    {
        var command = new CommandDefinition(
            FarmIntentsSql,
            new { FarmId = farmId },
            cancellationToken: ct);

        var items = await connection.QueryAsync<FarmPurchaseIntentDto>(command);
        return items.ToList();
    }

    public async Task<IReadOnlyList<SellerPurchaseIntentDto>> GetSellerPurchaseIntentsAsync(
        Guid sellerId,
        CancellationToken ct)
    {
        var command = new CommandDefinition(
            SellerIntentsSql,
            new { SellerId = sellerId },
            cancellationToken: ct);

        var items = await connection.QueryAsync<SellerPurchaseIntentDto>(command);
        return items.ToList();
    }
}
