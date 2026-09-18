import { useEffect, useState } from 'react'
import { appApi } from '../services/appApi'

const filterLabels: Record<string, string> = {
  Municipality: 'Municipio',
  Modality: 'Modalidad',
  Level: 'Nivel',
  Area: 'Área',
  MaximumCost: 'Costo máximo',
  MaximumDurationMonths: 'Duración máxima (meses)',
}

/** El backend guarda el filtro como JSON crudo (FilterSummary = JsonSerializer.Serialize(filter));
 * esto lo convierte en un resumen legible en vez de mostrar el JSON tal cual al estudiante. */
function summarizeFilters(raw?: string): string {
  if (!raw) return 'Sin filtros'
  try {
    const filters = JSON.parse(raw) as Record<string, unknown>
    const parts = Object.entries(filters)
      .filter(([key, value]) => key in filterLabels && value !== null && value !== '')
      .map(([key, value]) => `${filterLabels[key]}: ${value}`)
    return parts.length ? parts.join(' · ') : 'Sin filtros'
  } catch {
    return 'Sin filtros'
  }
}

export function HistoryPage() {
  const [items, setItems] = useState<{ id: string; testAttemptId: string; createdAtUtc: string; filterSummary?: string }[]>([])

  useEffect(() => { void appApi.history().then(setItems) }, [])

  return (
    <main className="content-shell">
      <span className="eyebrow">Seguimiento</span>
      <h1>Historial</h1>
      {items.length === 0 ? (
        <p className="empty-state">Todavía no hay recomendaciones guardadas.</p>
      ) : (
        items.map((item) => (
          <article className="history-row" key={item.id}>
            <strong>{new Date(item.createdAtUtc).toLocaleString()}</strong>
            <span>{summarizeFilters(item.filterSummary)}</span>
          </article>
        ))
      )}
    </main>
  )
}
