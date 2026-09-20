using Microsoft.EntityFrameworkCore;
using OrientacionVocacional.Infrastructure.Services;

namespace OrientacionVocacional.Tests;

public sealed class SkinTests
{
    [Fact]
    public async Task New_student_owns_only_the_free_skin_and_has_it_equipped()
    {
        await using var context = TestSupport.CreateContext();
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var service = new SkinService(context);

        var shop = await service.GetShopAsync(userId, CancellationToken.None);

        Assert.Equal("basic", shop!.EquippedSkin);
        Assert.Equal(new[] { "basic" }, shop.Skins.Where(skin => skin.Owned).Select(skin => skin.Id));
    }

    [Fact]
    public async Task Buying_without_enough_coins_is_rejected_and_keeps_the_balance()
    {
        await using var context = TestSupport.CreateContext();
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var service = new SkinService(context);

        var result = await service.BuyAsync(userId, "kimono", CancellationToken.None);

        Assert.False(result.Success);
        Assert.Contains("Te faltan 120 monedas", result.Error);
        Assert.Equal(0, (await context.Users.SingleAsync()).Coins);
    }

    [Fact]
    public async Task Buying_deducts_coins_and_equipping_requires_ownership()
    {
        await using var context = TestSupport.CreateContext();
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var user = await context.Users.SingleAsync();
        user.Coins = 100;
        await context.SaveChangesAsync();
        var service = new SkinService(context);

        var notOwnedYet = await service.EquipAsync(userId, "suit", CancellationToken.None);
        var bought = await service.BuyAsync(userId, "suit", CancellationToken.None);
        var boughtAgain = await service.BuyAsync(userId, "suit", CancellationToken.None);
        var equipped = await service.EquipAsync(userId, "suit", CancellationToken.None);

        Assert.False(notOwnedYet.Success);
        Assert.True(bought.Success);
        Assert.Equal(20, bought.Shop!.Coins);
        Assert.False(boughtAgain.Success);
        Assert.True(equipped.Success);
        Assert.Equal("suit", equipped.Shop!.EquippedSkin);
        Assert.Equal(20, (await context.Users.SingleAsync()).Coins);
    }

    [Fact]
    public async Task Unknown_skin_is_rejected()
    {
        await using var context = TestSupport.CreateContext();
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var service = new SkinService(context);

        Assert.False((await service.BuyAsync(userId, "no-existe", CancellationToken.None)).Success);
        Assert.False((await service.EquipAsync(userId, "no-existe", CancellationToken.None)).Success);
    }
}
