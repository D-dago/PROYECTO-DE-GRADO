namespace OrientacionVocacional.Application.Contracts;

/// <summary>Reglas de entrada compartidas: solo letras en nombres, solo digitos en telefono, etc.</summary>
public static class InputRules
{
    // Letras (con tildes y ñ), separadas por un espacio, apostrofe o guion. Sin digitos ni simbolos.
    // Se tolera un espacio sobrante al principio o al final (\s* a los costados): el teclado del
    // celular y el autocompletado lo agregan solos y no es motivo real para rechazar el nombre.
    public const string LettersOnly = @"^\s*\p{L}+([ '\-]\p{L}+)*\s*$";
    public const string LettersOnlyMessage = "Solo se permiten letras (sin números ni símbolos).";

    // Ciudad / colegio: igual que arriba pero se admite el punto (p. ej. "Sta. Cruz").
    public const string Place = @"^\s*\p{L}[\p{L} '\-.]*\s*$";

    public const string DigitsOnly = @"^\d{7,15}$";
    public const string DigitsOnlyMessage = "El teléfono debe tener solo dígitos (7 a 15).";

    // Contrasena: al menos una letra y un numero.
    public const string Password = @"^(?=.*\p{L})(?=.*\d).{8,64}$";
    public const string PasswordMessage = "La contraseña debe tener de 8 a 64 caracteres, con al menos una letra y un número.";
}
