using Microsoft.Extensions.Caching.Memory;

namespace OrientacionVocacional.API.Security;

/// <summary>
/// Freno contra adivinar contrasenas, por cuenta y no por IP: tras <see cref="MaxFailures"/> intentos
/// fallidos con el mismo correo, ese correo queda bloqueado <see cref="LockMinutes"/> minutos. Asi el
/// limite por IP puede ser holgado (un colegio entero comparte una sola IP) sin dejar que alguien
/// pruebe contrasenas sin fin contra una cuenta.
/// </summary>
public sealed class LoginAttemptGuard(IMemoryCache cache)
{
    public const int MaxFailures = 10;
    public const int LockMinutes = 15;

    private static string Key(string email) => "login-fallos:" + email.Trim().ToLowerInvariant();

    public bool IsLocked(string email) => cache.TryGetValue(Key(email), out Counter? counter) && counter!.Count >= MaxFailures;

    public void RegisterFailure(string email)
    {
        var counter = cache.GetOrCreate(Key(email), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(LockMinutes);
            entry.Size = 1;
            return new Counter();
        })!;
        Interlocked.Increment(ref counter.Count);
    }

    public void Reset(string email) => cache.Remove(Key(email));

    private sealed class Counter
    {
        public int Count;
    }
}
