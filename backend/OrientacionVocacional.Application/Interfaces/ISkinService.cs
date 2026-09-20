using OrientacionVocacional.Application.Contracts;

namespace OrientacionVocacional.Application.Interfaces;

public interface ISkinService
{
    Task<SkinShopResponse?> GetShopAsync(Guid userId, CancellationToken cancellationToken);
    Task<(bool Success, string? Error, SkinShopResponse? Shop)> BuyAsync(Guid userId, string skinId, CancellationToken cancellationToken);
    Task<(bool Success, string? Error, SkinShopResponse? Shop)> EquipAsync(Guid userId, string skinId, CancellationToken cancellationToken);
}
