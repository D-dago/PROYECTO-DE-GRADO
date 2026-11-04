import { useEffect, useState } from 'react'
import { appApi } from '../services/appApi'
import type { Recommendation } from '../services/appApi'
import type { RiasecResult } from '../types/riasec'
import { UniversityLinks } from './UniversityLinks'

const MAX_DIMENSION_SCORE = 10 // 2 preguntas por dimension x valor maximo 5

export function RiasecResult({ result }: { result: RiasecResult }) {
  const [recommendation, setRecommendation] = useState<Recommendation | null>(null)
  const [failed, setFailed] = useState(false)

  useEffect(() => {
    appApi.recommendations({}).then(setRecommendation).catch(() => setFailed(true))
  }, [])

  const scores = [['R', result.rScore], ['I', result.iScore], ['A', result.aScore], ['S', result.sScore], ['E', result.eScore], ['C', result.cScore]]
  const interpretation = recommendation?.interpretation

  return (
    <main className="test-shell result-shell">
      <header className="test-header"><span className="eyebrow">Orientación vocacional</span><span className="version-label">Resultado guardado</span></header>
      <section className="result-panel">
        <span className="eyebrow">Test terminado</span>
        <h1>Tu perfil RIASEC</h1>
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

            <h2 className="result-heading">Universidades e institutos para ti</h2>
            <UniversityLinks careers={recommendation.items} />
          </>
        )}

        <p className="result-note">Este resultado resume tus respuestas. Es una herramienta de orientación vocacional, no un diagnóstico psicológico ni determina obligatoriamente una profesión.</p>
      </section>
    </main>
  )
}
