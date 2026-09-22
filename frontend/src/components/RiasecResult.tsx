import { useEffect, useState } from 'react'
import { appApi } from '../services/appApi'
import type { Recommendation } from '../services/appApi'
import type { RiasecResult } from '../types/riasec'

const MAX_DIMENSION_SCORE = 10 // 2 preguntas por dimension x valor maximo 5

function costTier(cost?: number) {
  if (cost == null) return null
  if (cost <= 500) return { label: '🟢 Accesible', className: 'cost-tier-low' }
  if (cost <= 1000) return { label: '🟡 Costo medio', className: 'cost-tier-medium' }
  return { label: '🔴 Costo alto', className: 'cost-tier-high' }
}

export function RiasecResult({ result }: { result: RiasecResult }) {
  const [recommendation, setRecommendation] = useState<Recommendation | null>(null)
  const [failed, setFailed] = useState(false)

  useEffect(() => {
    appApi.recommendations({}).then(setRecommendation).catch(() => setFailed(true))
  }, [])

  const scores = [['C', result.cScore], ['H', result.hScore], ['A', result.aScore], ['S', result.sScore], ['I', result.iScore], ['D', result.dScore], ['E', result.eScore]]
  const interpretation = recommendation?.interpretation

  return (
    <main className="test-shell result-shell">
      <header className="test-header"><span className="eyebrow">Orientación vocacional</span><span className="version-label">Resultado guardado</span></header>
      <section className="result-panel">
        <span className="eyebrow">Test terminado</span>
        <h1>Tu perfil vocacional</h1>
        <p className="result-code">{result.primaryCode}<small>{result.secondaryCode}</small></p>

        <h2 className="result-heading">¿Qué significa tu perfil?</h2>
        {interpretation ? (
          <div className="dimension-cards">
            {interpretation.dimensions.map((dimension) => (
              <article className="dimension-card" key={dimension.code}>
                <strong className="dimension-letter">{dimension.code}</strong>
                <div><h3>{dimension.name}</h3><p>{dimension.description}</p></div>
              </article>
            ))}
          </div>
        ) : (
          <p className="status-message">{failed ? 'No se pudo cargar la interpretación de tu perfil.' : 'Interpretando tu perfil...'}</p>
        )}

        <div className="score-list">
          {scores.map(([code, score]) => (
            <div key={code} className="score-row"><strong>{code}</strong><div className="score-bar"><span style={{ width: `${Number(score) / MAX_DIMENSION_SCORE * 100}%` }} /></div><span>{score}</span></div>
          ))}
        </div>

        {recommendation && interpretation && (
          <>
            <h2 className="result-heading">Explicación de tu resultado <span className="source-tag">{recommendation.aiGenerated ? '✨ Generada con IA' : 'Motor de reglas'}</span></h2>
            <p className="result-explanation">{recommendation.aiGenerated && recommendation.aiMessage ? recommendation.aiMessage : interpretation.summary}</p>

            <h2 className="result-heading">Áreas profesionales afines</h2>
            <div className="area-chips">{interpretation.suggestedAreas.map((area) => <span key={area} className="area-chip">{area}</span>)}</div>

            <h2 className="result-heading">Carreras y universidades compatibles</h2>
            {recommendation.items.length === 0 ? (
              <p className="result-note">Todavía no hay carreras cargadas en el catálogo, por eso aquí no aparecen universidades. Cuando el administrador cargue instituciones y carreras verificadas, se mostrarán automáticamente ordenadas por compatibilidad con tu perfil.</p>
            ) : (
              <div className="result-careers">
                {recommendation.items.slice(0, 6).map((career) => {
                  const tier = costTier(career.cost)
                  return (
                    <article className="result-career" key={career.id}>
                      <span className="result-career-match">{career.compatibility}% compatible</span>
                      <h3>{career.name}</h3>
                      <p className="result-career-institution">🏛 {career.institutionName}</p>
                      <p>{career.compatibilityReason}</p>
                      {tier && <span className={`cost-tier ${tier.className}`}>{tier.label}</span>}
                    </article>
                  )
                })}
              </div>
            )}
          </>
        )}

        <p className="result-note">Este resultado resume tus respuestas. Es una herramienta de orientación vocacional, no un diagnóstico psicológico ni determina obligatoriamente una profesión.</p>
        <a className="start-link" href="/recomendaciones">Filtrar por presupuesto, municipio y más</a>
      </section>
    </main>
  )
}
