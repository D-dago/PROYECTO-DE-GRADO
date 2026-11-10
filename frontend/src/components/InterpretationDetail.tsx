import type { ProfileInterpretation } from '../services/appApi'

// Test vigente: 4 preguntas por dimension x valor maximo 5. Los resultados del test anterior
// de 12 preguntas llegan con su propio maximo (10) desde el servidor.
const MAX_DIMENSION_SCORE = 20

/**
 * El detalle completo de un resultado (tarjetas por area, barras de puntaje, resumen y areas
 * afines) - el mismo contenido que se ve justo despues de terminar el test. Se reutiliza en el
 * Historial del estudiante y en "Resultados" del administrador, para no mostrar solo el codigo.
 */
export function InterpretationDetail({ interpretation, scores, maxScore = MAX_DIMENSION_SCORE }: {
  interpretation: ProfileInterpretation
  scores: [string, number][]
  maxScore?: number
}) {
  return (
    <>
      <div className="dimension-cards">
        {interpretation.dimensions.map((dimension) => (
          <article className="dimension-card" key={dimension.code}>
            <strong className="dimension-letter">{dimension.code}</strong>
            <div><h3>{dimension.name}</h3><p>{dimension.description}</p></div>
          </article>
        ))}
      </div>

      <div className="score-list">
        {scores.map(([code, score]) => (
          <div key={code} className="score-row"><strong>{code}</strong><div className="score-bar"><span style={{ width: `${Math.min(score / maxScore, 1) * 100}%` }} /></div><span>{score}<small> / {maxScore}</small></span></div>
        ))}
      </div>

      <p className="result-explanation">{interpretation.summary}</p>

      <div className="area-chips">{interpretation.suggestedAreas.map((area) => <span key={area} className="area-chip">{area}</span>)}</div>
    </>
  )
}
