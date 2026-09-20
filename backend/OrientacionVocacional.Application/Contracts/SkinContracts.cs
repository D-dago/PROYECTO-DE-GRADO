namespace OrientacionVocacional.Application.Contracts;

public sealed record SkinResponse(string Id, string Name, string Description, int Price, bool Owned, bool Equipped);

public sealed record SkinShopResponse(int Coins, string EquippedSkin, IReadOnlyList<SkinResponse> Skins);
