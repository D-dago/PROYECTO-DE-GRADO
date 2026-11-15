namespace OrientacionVocacional.Domain.Constants;

/// <summary>
/// Cuerpo del avatar (ORION #38). El estudiante lo elige gratis en Skins y lo puede cambiar cuando quiera;
/// las skins (ropa) sirven para los dos. Los estudiantes que ya existian quedan con el avatar de siempre.
/// </summary>
public static class AvatarStyles
{
    public const string Male = "hombre";
    public const string Female = "mujer";
    public static readonly string[] All = [Male, Female];

    public static bool IsValid(string? style) => style is not null && All.Contains(style);
}
