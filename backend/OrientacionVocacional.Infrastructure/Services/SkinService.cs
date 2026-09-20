using Microsoft.EntityFrameworkCore;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Domain.Constants;
using OrientacionVocacional.Domain.Entities;
using OrientacionVocacional.Infrastructure.Persistence;

namespace OrientacionVocacional.Infrastructure.Services;

public sealed class SkinService(OrientacionVocacionalDbContext dbContext) : ISkinService
{
    public async Task<SkinShopResponse?> GetShopAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.AsNoTracking().SingleOrDefaultAsync(candidate => candidate.Id == userId, cancellationToken);
        return user is null ? null : await BuildShopAsync(user, cancellationToken);
    }

    public async Task<(bool Success, string? Error, SkinShopResponse? Shop)> BuyAsync(
        Guid userId, string skinId, CancellationToken cancellationToken)
    {
        var skin = SkinCatalog.Find(skinId);
        if (skin is null) return (false, "Esa skin no existe.", null);

        var user = await dbContext.Users.SingleOrDefaultAsync(candidate => candidate.Id == userId, cancellationToken);
        if (user is null) return (false, "El usuario no existe.", null);

        if (await IsOwnedAsync(userId, skin, cancellationToken)) return (false, "Ya tienes esa skin.", null);
        if (user.Coins < skin.Price) return (false, $"Te faltan {skin.Price - user.Coins} monedas para comprar esta skin.", null);

        user.Coins -= skin.Price;
        dbContext.UserSkins.Add(new UserSkin { Id = Guid.NewGuid(), UserId = userId, SkinId = skin.Id });
        await dbContext.SaveChangesAsync(cancellationToken);
        return (true, null, await BuildShopAsync(user, cancellationToken));
    }

    public async Task<(bool Success, string? Error, SkinShopResponse? Shop)> EquipAsync(
        Guid userId, string skinId, CancellationToken cancellationToken)
    {
        var skin = SkinCatalog.Find(skinId);
        if (skin is null) return (false, "Esa skin no existe.", null);

        var user = await dbContext.Users.SingleOrDefaultAsync(candidate => candidate.Id == userId, cancellationToken);
        if (user is null) return (false, "El usuario no existe.", null);

        if (!await IsOwnedAsync(userId, skin, cancellationToken)) return (false, "Primero tienes que comprar esa skin.", null);

        user.EquippedSkin = skin.Id;
        await dbContext.SaveChangesAsync(cancellationToken);
        return (true, null, await BuildShopAsync(user, cancellationToken));
    }

    // La skin gratis (precio 0) cuenta como propia sin necesidad de registrarla.
    private async Task<bool> IsOwnedAsync(Guid userId, SkinDefinition skin, CancellationToken cancellationToken) =>
        skin.Price == 0 || await dbContext.UserSkins.AnyAsync(owned => owned.UserId == userId && owned.SkinId == skin.Id, cancellationToken);

    private async Task<SkinShopResponse> BuildShopAsync(User user, CancellationToken cancellationToken)
    {
        var ownedIds = (await dbContext.UserSkins.AsNoTracking()
            .Where(owned => owned.UserId == user.Id)
            .Select(owned => owned.SkinId)
            .ToListAsync(cancellationToken)).ToHashSet();

        var skins = SkinCatalog.All
            .Select(skin => new SkinResponse(
                skin.Id, skin.Name, skin.Description, skin.Price,
                Owned: skin.Price == 0 || ownedIds.Contains(skin.Id),
                Equipped: user.EquippedSkin == skin.Id))
            .ToList();
        return new SkinShopResponse(user.Coins, user.EquippedSkin, skins);
    }
}
