namespace OrientacionVocacional.Domain.Constants;

/// <summary>Limites para que la base de datos no se llene de cuentas basura.</summary>
public static class RegistrationPolicy
{
    // Tope de cuentas en total: mas alla de esto se cierra el registro hasta que el admin limpie.
    public const int MaxUsers = 5000;

    // Correos desechables mas comunes: sirven para crear cuentas en masa sin bandeja real.
    private static readonly HashSet<string> DisposableDomains = new(StringComparer.OrdinalIgnoreCase)
    {
        "mailinator.com", "guerrillamail.com", "guerrillamail.net", "sharklasers.com", "10minutemail.com",
        "tempmail.com", "temp-mail.org", "yopmail.com", "trashmail.com", "getnada.com", "dispostable.com",
        "maildrop.cc", "throwawaymail.com", "fakeinbox.com", "mailnesia.com", "mintemail.com", "spamgourmet.com"
    };

    /// <summary>Devuelve un mensaje si el correo no se acepta para registro, o null si esta bien.</summary>
    public static string? CheckEmail(string email)
    {
        var at = email.LastIndexOf('@');
        if (at < 1 || at == email.Length - 1) return "El correo no es válido.";
        var local = email[..at];
        var domain = email[(at + 1)..];
        // "nombre+1@gmail.com", "nombre+2@..." llegan a la misma bandeja: permitirlo crea cuentas infinitas.
        if (local.Contains('+')) return "El correo no puede contener el símbolo +.";
        if (!domain.Contains('.') || domain.StartsWith('.') || domain.EndsWith('.')) return "El correo no es válido.";
        if (DisposableDomains.Contains(domain)) return "No se aceptan correos temporales o desechables.";
        return null;
    }
}
