namespace OrientacionVocacional.Domain.Constants;

public sealed record SkinDefinition(string Id, string Name, string Description, int Price);

/// <summary>
/// Catalogo de skins (ropa del avatar). Es un catalogo fijo de la app, no datos
/// institucionales. Se compran con las monedas que da el test RIASEC.
/// </summary>
public static class SkinCatalog
{
    public const string DefaultSkinId = "basic";

    public static readonly IReadOnlyList<SkinDefinition> All =
    [
        new("basic", "Camiseta básica", "Tu look de inicio.", 0),
        new("hoodie", "Sudadera rosa", "Cómoda para las tardes de estudio.", 40),
        new("overol", "Overol de ingeniero", "Casco y overol: para quien ama construir y reparar.", 60),
        new("labcoat", "Bata de científico", "Bata blanca y lentes de laboratorio.", 60),
        new("artist", "Boina de artista", "Boina y delantal salpicado de pintura.", 60),
        new("sweater", "Suéter solidario", "Verde, con un corazón: para quien ayuda a otros.", 60),
        new("suit", "Traje ejecutivo", "Traje azul y corbata para liderar proyectos.", 80),
        new("cardigan", "Cárdigan ordenado", "Cárdigan y lentes: todo en su lugar.", 60),
        new("kimono", "Kimono sakura", "Edición especial con flores de cerezo.", 120),
    ];

    public static SkinDefinition? Find(string id) => All.FirstOrDefault(skin => skin.Id == id);
}
