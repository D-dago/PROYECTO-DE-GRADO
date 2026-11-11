using System.Diagnostics;

namespace OrientacionVocacional.API.Security;

/// <summary>
/// Turno para el trabajo con contrasenas (registro e inicio de sesion). Calcular o comprobar un hash
/// PBKDF2 es, con diferencia, lo que mas CPU gasta en todo el sistema, y en Render gratis hay 0,1 de CPU:
/// cada hash tarda ~1 s alli. Si un salon entero pulsa "Entrar" a la vez, 100 hashes en paralelo se
/// reparten la CPU con los que ya estan respondiendo el test y todo se congela. Con un turno por nucleo,
/// los inicios de sesion esperan en fila y las respuestas del test, que son baratas, pasan entre uno y otro.
/// Si la fila es tan larga que la espera superaria <see cref="MaxWaitSeconds"/> (internet corta las
/// peticiones que tardan mas de 100 s), se responde al instante que se reintente en unos segundos.
/// </summary>
public sealed class PasswordWorkGate
{
    public const int MaxWaitSeconds = 40;
    private readonly int slotCount = Math.Max(1, Environment.ProcessorCount);
    private readonly SemaphoreSlim slots;
    private int pending;
    // Duracion media de un turno (ms), aprendida sobre la marcha; empieza con un valor prudente.
    private double averageMs = 500;

    public PasswordWorkGate() => slots = new SemaphoreSlim(slotCount);

    /// <summary>Segundos que conviene esperar antes de reintentar, o null si se puede atender ya.</summary>
    public int? RetryAfterSeconds()
    {
        var expectedWait = (Volatile.Read(ref pending) + 1) * Volatile.Read(ref averageMs) / slotCount / 1000;
        return expectedWait > MaxWaitSeconds ? (int)Math.Clamp(expectedWait / 3, 5, 30) : null;
    }

    public async Task<T> RunAsync<T>(Func<Task<T>> work, CancellationToken cancellationToken)
    {
        Interlocked.Increment(ref pending);
        try
        {
            await slots.WaitAsync(cancellationToken);
            try
            {
                var clock = Stopwatch.StartNew();
                var result = await work();
                Volatile.Write(ref averageMs, Volatile.Read(ref averageMs) * 0.8 + clock.Elapsed.TotalMilliseconds * 0.2);
                return result;
            }
            finally
            {
                slots.Release();
            }
        }
        finally
        {
            Interlocked.Decrement(ref pending);
        }
    }
}
