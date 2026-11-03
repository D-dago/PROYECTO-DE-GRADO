import { useEffect, useState } from 'react'
import { appApi } from '../services/appApi'
import type { HistoryEntry } from '../services/appApi'
import { InterpretationDetail } from '../components/InterpretationDetail'

type ScoreKey = 'rScore' | 'iScore' | 'aScore' | 'sScore' | 'eScore' | 'cScore'
const dimensionLabels: [ScoreKey, string][] = [
  ['rScore', 'R'], ['iScore', 'I'], ['aScore', 'A'], ['sScore', 'S'], ['eScore', 'E'], ['cScore', 'C'],
]

const filterLabels: Record<string, string> = {
  Municipality: 'Municipio',
  Modality: 'Modalidad',
  Level: 'Nivel',
  Area: 'Área',
  MaximumCost: 'Costo máximo',
  MaximumDurationMonths: 'Duración máxima (meses)',
}

/** El backend guarda el filtro como JSON crudo (FilterSummary = JsonSerializer.Serialize(filter));
 * esto lo convierte en un resumen legible en vez de mostrar el JSON tal cual al estudiante.
 * Devuelve '' cuando la busqueda fue sin filtros, para no ensuciar la tarjeta con "Sin filtros". */
function summarizeFilters(raw?: string): string {
  if (!raw) return ''
  try {
    const filters = JSON.parse(raw) as Record<string, unknown>
    return Object.entries(filters)
      .filter(([key, value]) => key in filterLabels && value !== null && value !== '')
      .map(([key, value]) => `${filterLabels[key]}: ${value}`)
      .join(' · ')
  } catch {
    return ''
  }
}

export function HistoryPage() {
  const [items, setItems] = useState<HistoryEntry[]>([])

  useEffect(() => { void appApi.history().then(setItems) }, [])

  return (
    <main className="content-shell">
      <span className="eyebrow">Seguimiento</span>
      <h1>Historial</h1>
      <p className="empty-state">Cada vez que terminas el test se guarda aquí tu resultado de esa vez, aunque más tarde lo repitas y te salga un perfil distinto.</p>
      {items.length === 0 ? (
        <p className="empty-state">Todavía no hay resultados guardados.</p>
      ) : (
        <div className="history-list">
          {items.map((item) => {
            const filtros = summarizeFilters(item.filterSummary)
            return (
            <details className="history-card" key={item.id}>
              <summary className="history-card-top">
                <strong>{new Date(item.createdAtUtc).toLocaleString()}</strong>
                {item.primaryCode && <span className="result-code-mini">{item.primaryCode}<small>{item.secondaryCode}</small></span>}
                <span className="history-toggle-hint">Ver detalle ▾</span>
              </summary>
              {item.primaryCode ? (
                <div className="history-scores">
                  {dimensionLabels.map(([key, label]) => (
                    <span key={label}><b>{label}</b>{item[key] ?? 0}</span>
                  ))}
                </div>
              ) : (
                <p className="result-note">Este intento es de una versión anterior del test; ya no se puede mostrar el detalle.</p>
              )}
              {filtros && <span className="history-filter">Última búsqueda · {filtros}</span>}
              {item.interpretation && (
                <div className="history-detail">
                  <h2 className="result-heading">¿Qué significa este perfil?</h2>
                  <InterpretationDetail
                    interpretation={item.interpretation}
                    scores={[['R', item.rScore ?? 0], ['I', item.iScore ?? 0], ['A', item.aScore ?? 0], ['S', item.sScore ?? 0], ['E', item.eScore ?? 0], ['C', item.cScore ?? 0]]}
                  />
                </div>
              )}
            </details>
            )
          })}
        </div>
      )}
    </main>
  )
}
