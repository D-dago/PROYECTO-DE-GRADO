import { useEffect, useState } from 'react'
import { appApi } from '../services/appApi'
import type { HistoryEntry } from '../services/appApi'
import { InterpretationDetail } from '../components/InterpretationDetail'

type ScoreKey = 'rScore' | 'iScore' | 'aScore' | 'sScore' | 'eScore' | 'cScore'
const dimensionLabels: [ScoreKey, string][] = [
  ['rScore', 'R'], ['iScore', 'I'], ['aScore', 'A'], ['sScore', 'S'], ['eScore', 'E'], ['cScore', 'C'],
]

export function HistoryPage() {
  const [items, setItems] = useState<HistoryEntry[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    appApi.history()
      .then(setItems)
      .catch((loadError) => setError(loadError instanceof Error ? loadError.message : 'No se pudo cargar el historial.'))
      .finally(() => setLoading(false))
  }, [])

  return (
    <main className="content-shell">
      <span className="eyebrow">Seguimiento</span>
      <h1>Historial</h1>
      <p className="empty-state">Cada vez que terminas el test se guarda aquí tu resultado de esa vez, aunque más tarde lo repitas y te salga un perfil distinto.</p>
      {/* Antes, un fallo o la propia espera se mostraban como "todavia no hay resultados". */}
      {error && <p className="error-message">{error}</p>}
      {loading && <p className="status-message">Cargando tu historial...</p>}
      {!loading && !error && items.length === 0 && <p className="empty-state">Todavía no hay resultados guardados.</p>}
      {items.length > 0 && (
        <div className="history-list">
          {items.map((item) => (
            <details className="history-card" key={item.id}>
              <summary className="history-card-top">
                <strong>{new Date(item.createdAtUtc).toLocaleString()}</strong>
                {/* Un resultado O*NET sin areas sobre el promedio no tiene codigo (perfil plano), pero si puntajes. */}
                {item.interpretation && <span className="result-code-mini">{item.primaryCode ? <>{item.primaryCode}<small>{item.secondaryCode}</small></> : <small>Perfil plano</small>}</span>}
                <span className="history-toggle-hint">Ver detalle ▾</span>
              </summary>
              {item.interpretation ? (
                <div className="history-scores">
                  {dimensionLabels.map(([key, label]) => (
                    <span key={label}><b>{label}</b>{item[key] ?? 0}</span>
                  ))}
                </div>
              ) : (
                <p className="result-note">Este intento es de una versión anterior del test; ya no se puede mostrar el detalle.</p>
              )}
              {item.interpretation && (
                <div className="history-detail">
                  <h2 className="result-heading">¿Qué significa este perfil?</h2>
                  <InterpretationDetail
                    interpretation={item.interpretation}
                    scores={[['R', item.rScore ?? 0], ['I', item.iScore ?? 0], ['A', item.aScore ?? 0], ['S', item.sScore ?? 0], ['E', item.eScore ?? 0], ['C', item.cScore ?? 0]]}
                    maxScore={item.maxScore}
                  />
                </div>
              )}
            </details>
          ))}
        </div>
      )}
    </main>
  )
}
