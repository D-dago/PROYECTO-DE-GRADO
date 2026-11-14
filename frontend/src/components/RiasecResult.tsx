import { useEffect, useState } from 'react'
import { appApi } from '../services/appApi'
import type { Recommendation } from '../services/appApi'
import type { RiasecResult } from '../types/riasec'
import { CAREER_SEARCH_ID, UniversityLinks } from './UniversityLinks'
import { countAreaCareers } from '../services/careerSearch'
import { useLiveCareers } from '../services/liveData'

const MAX_DIMENSION_SCORE = 20 // 4 preguntas por dimension x valor maximo 5 (test vigente)

// Cronometro total (ORION #31): "7 min 32 s", "45 s" o "1 h 05 min". Solo aparece aqui, con el resultado.
function formatDuration(totalSeconds: number) {
  const hours = Math.floor(totalSeconds / 3600)
  const minutes = Math.floor((totalSeconds % 3600) / 60)
  const seconds = totalSeconds % 60
  if (hours > 0) return `${hours} h ${String(minutes).padStart(2, '0')} min`
  return minutes > 0 ? `${minutes} min ${seconds} s` : `${seconds} s`
}

export function RiasecResult({ result }: { result: RiasecResult }) {
  const [recommendation, setRecommendation] = useState<Recommendation | null>(null)
  const [failed, setFailed] = useState(false)
  // Tocar un area afin la busca en "Carreras para ti": el estudiante ve de inmediato que carreras de esa
  // area tiene en Cochabamba (antes copiaba el texto en el buscador y, como no es un nombre de carrera, no salia nada).
  const [careerQuery, setCareerQuery] = useState('')
  function showArea(area: string) {
    setCareerQuery(area)
    window.setTimeout(() => document.getElementById(CAREER_SEARCH_ID)?.scrollIntoView({ behavior: 'smooth', block: 'center' }), 50)
  }
  useLiveCareers(recommendation, setRecommendation)

  useEffect(() => {
    appApi.recommendations({}).then(setRecommendation).catch(() => setFailed(true))
  }, [])

  const maxScore = result.maxScore ?? MAX_DIMENSION_SCORE
  const scores = [['R', result.rScore], ['I', result.iScore], ['A', result.aScore], ['S', result.sScore], ['E', result.eScore], ['C', result.cScore]]
  const interpretation = recommendation?.interpretation

  return (
    <main className="test-shell result-shell">
      <header className="test-header"><span className="eyebrow">Orientación vocacional</span><span className="version-label">Resultado guardado</span></header>
      <section className="result-panel">
        <span className="eyebrow">Test terminado</span>
        <h1>Tu perfil RIASEC</h1>
        <p className="result-code">{result.primaryCode}<small>{result.secondaryCode}</small></p>
        {result.durationSeconds != null && (
          <p className="result-duration">⏱ Tiempo para responder el test: <strong>{formatDuration(result.durationSeconds)}</strong></p>
        )}

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
            <div key={code} className="score-row"><strong>{code}</strong><div className="score-bar"><span style={{ width: `${Math.min(Number(score) / maxScore, 1) * 100}%` }} /></div><span>{score}<small> / {maxScore}</small></span></div>
          ))}
        </div>

        {recommendation && interpretation && (
          <>
            <h2 className="result-heading">Explicación de tu resultado <span className="source-tag">{recommendation.aiGenerated ? '✨ Generada con IA' : 'Motor de reglas'}</span></h2>
            <p className="result-explanation">{recommendation.aiGenerated && recommendation.aiMessage ? recommendation.aiMessage : interpretation.summary}</p>

            <h2 className="result-heading">Áreas profesionales afines</h2>
            <p className="area-hint">Toca un área para ver sus carreras en Cochabamba.</p>
            {/* Un area sin carreras en el catalogo no es un boton: llevaria a una busqueda vacia (ORION #32). */}
            <div className="area-chips">{interpretation.suggestedAreas.map((area) => countAreaCareers(recommendation.items, interpretation.areaSearchTerms?.[area]) > 0
              ? <button type="button" key={area} className="area-chip area-chip-button" onClick={() => showArea(area)} aria-label={`Ver carreras de ${area}`}>{area}</button>
              : <span key={area} className="area-chip area-chip-empty" title="Todavía no hay carreras de esta área en el catálogo de Cochabamba">{area}</span>)}</div>

            <h2 className="result-heading">Carreras para ti y dónde estudiarlas en Cochabamba</h2>
            <UniversityLinks careers={recommendation.items} query={careerQuery} onQueryChange={setCareerQuery} areaTerms={interpretation.areaSearchTerms} profileLetters={result.primaryCode} />
          </>
        )}

        <p className="result-note">Este resultado resume tus respuestas. Es una herramienta de orientación vocacional, no un diagnóstico psicológico ni determina obligatoriamente una profesión.</p>
      </section>
    </main>
  )
}
