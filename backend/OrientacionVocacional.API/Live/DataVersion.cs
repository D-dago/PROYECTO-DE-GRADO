namespace OrientacionVocacional.API.Live;

public enum DataArea
{
    /// <summary>Instituciones, carreras y reglas: lo que ve el estudiante.</summary>
    Catalog,
    /// <summary>Lo que solo interesa al administrador: tests terminados, registros y preguntas.</summary>
    Activity
}

/// <summary>
/// Contadores en memoria que cambian cuando cambian los datos. Las pantallas abiertas los consultan cada
/// pocos segundos y solo recargan si cambiaron. Asi preguntar no toca la base de datos. Empiezan en un valor
/// al azar: al reiniciar el servidor las pantallas recargan una vez, sin riesgo de creer que nada cambio.
/// </summary>
public sealed class DataVersion
{
    private long _catalog = Random.Shared.Next();
    private long _activity = Random.Shared.Next();

    public long Catalog => Interlocked.Read(ref _catalog);
    public long Activity => Interlocked.Read(ref _activity);

    public void Bump(DataArea area)
    {
        if (area == DataArea.Catalog) Interlocked.Increment(ref _catalog);
        else Interlocked.Increment(ref _activity);
    }
}
