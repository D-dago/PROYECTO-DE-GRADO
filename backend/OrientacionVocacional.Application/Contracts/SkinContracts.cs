namespace OrientacionVocacional.Application.Contracts;

public sealed record SkinResponse(string Id, string Name, string Description, int Price, bool Owned, bool Equipped);

// AvatarStyle: cuerpo del avatar, "hombre" o "mujer" (ORION #38).
public sealed record SkinShopResponse(int Coins, string EquippedSkin, IReadOnlyList<SkinResponse> Skins, string AvatarStyle = "hombre");
