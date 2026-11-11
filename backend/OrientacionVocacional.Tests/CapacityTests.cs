using Microsoft.Extensions.Caching.Memory;
using OrientacionVocacional.API.Security;

namespace OrientacionVocacional.Tests;

/// <summary>Piezas que permiten a un colegio entero (una sola IP) usar el sistema a la vez.</summary>
public sealed class CapacityTests
{
    [Fact]
    public void Login_guard_locks_an_account_after_too_many_failures_and_reset_unlocks_it()
    {
        var guard = new LoginAttemptGuard(new MemoryCache(new MemoryCacheOptions()));

        for (var i = 0; i < LoginAttemptGuard.MaxFailures - 1; i++) guard.RegisterFailure("Ana@Colegio.edu");
        Assert.False(guard.IsLocked("ana@colegio.edu"));

        guard.RegisterFailure("ana@colegio.edu ");
        Assert.True(guard.IsLocked("ANA@colegio.edu")); // el correo se compara sin mayusculas ni espacios
        Assert.False(guard.IsLocked("otro@colegio.edu")); // las demas cuentas del colegio siguen entrando

        guard.Reset("ana@colegio.edu");
        Assert.False(guard.IsLocked("ana@colegio.edu"));
    }

    [Fact]
    public async Task Password_gate_lets_people_in_when_idle_and_asks_to_retry_when_the_queue_is_too_long()
    {
        var gate = new PasswordWorkGate();
        Assert.Null(gate.RetryAfterSeconds());

        // Mucha gente esperando su turno, cada uno con trabajo que todavia no termina.
        var release = new TaskCompletionSource<int>();
        var waiting = Enumerable.Range(0, 200 * Math.Max(1, Environment.ProcessorCount))
            .Select(_ => gate.RunAsync(() => release.Task, CancellationToken.None))
            .ToList();

        var retry = gate.RetryAfterSeconds();
        Assert.NotNull(retry);
        Assert.InRange(retry!.Value, 5, 30);

        release.SetResult(1);
        await Task.WhenAll(waiting);
        Assert.Null(gate.RetryAfterSeconds());
    }
}
