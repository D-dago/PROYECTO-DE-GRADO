import { useEffect, useState } from 'react'
import { riasecApi } from '../services/riasecApi'
import type { Attempt, Question, RiasecResult } from '../types/riasec'

const options = [
  { value: 1, label: 'Nada' }, { value: 2, label: 'Poco' }, { value: 3, label: 'Neutral' },
  { value: 4, label: 'Mucho' }, { value: 5, label: 'Totalmente' },
]

const dimensions = new Map([
  ['R', 'Realista'], ['I', 'Investigador'], ['A', 'Artistico'],
  ['S', 'Social'], ['E', 'Emprendedor'], ['C', 'Convencional'],
])

export function TestPage() {
  const [questions, setQuestions] = useState<Question[]>([])
  const [attempt, setAttempt] = useState<Attempt | null>(null)
  const [result, setResult] = useState<RiasecResult | null>(null)
  const [currentIndex, setCurrentIndex] = useState(0)
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)

  async function loadTest() {
    try {
      const [loadedQuestions, loadedAttempt] = await Promise.all([
        riasecApi.getQuestions(), riasecApi.createAttempt(),
      ])
      setQuestions(loadedQuestions)
      setAttempt(loadedAttempt)
      setCurrentIndex(Math.min(loadedAttempt.answered, loadedQuestions.length - 1))
      if (loadedAttempt.status === 'COMPLETED') setResult(await riasecApi.getResult(loadedAttempt.attemptId))
    } catch (loadError) {
      setError(loadError instanceof Error ? loadError.message : 'No se pudo cargar el test.')
    } finally { setLoading(false) }
  }

  useEffect(() => { void loadTest() }, [])

  async function answer(value: number) {
    if (!attempt || !questions[currentIndex] || saving) return
    setSaving(true)
    setError(null)
    try {
      const updatedAttempt = await riasecApi.submitAnswer(attempt.attemptId, questions[currentIndex].id, value)
      setAttempt(updatedAttempt)
      if (updatedAttempt.answered === updatedAttempt.total) {
        const completed = await riasecApi.complete(attempt.attemptId)
        setResult(completed)
        setAttempt({ ...updatedAttempt, status: 'COMPLETED' })
      } else setCurrentIndex((index) => Math.min(index + 1, questions.length - 1))
    } catch (answerError) {
      setError(answerError instanceof Error ? answerError.message : 'No se pudo guardar la respuesta.')
    } finally { setSaving(false) }
  }

  if (loading) return <main className="test-shell"><p className="status-message">Cargando tu test...</p></main>
  if (error && !attempt) return <main className="test-shell"><p className="error-message">{error}</p></main>
  if (result) return <ResultView result={result} />
  if (!attempt || questions.length === 0) return <main className="test-shell"><p className="error-message">No hay preguntas disponibles.</p></main>

  const question = questions[currentIndex]
  const progress = attempt.answered / attempt.total * 100

  return (
    <main className="test-shell">
      <header className="test-header"><span className="eyebrow">Orientacion vocacional</span><span className="version-label">RIASEC · 60 preguntas</span></header>
      <section className="quiz-panel" aria-live="polite">
        <div className="quiz-topline"><div><span className="eyebrow">Test vocacional</span><h1>Descubre que actividades te motivan.</h1></div><strong className="question-count">Pregunta {Math.min(attempt.answered + 1, 60)} <span>de 60</span></strong></div>
        <div className="progress-track" aria-label={`${Math.round(progress)}% completado`}><span style={{ width: `${progress}%` }} /></div>
        <div className="question-meta"><span className="dimension-tag">{question.dimension} · {dimensions.get(question.dimension)}</span><span>{Math.round(progress)}% completado</span></div>
        <p className="question-text">{question.text}</p>
        <div className="answer-grid">{options.map((option) => <button key={option.value} type="button" onClick={() => void answer(option.value)} disabled={saving}><strong>{option.value}</strong><span>{option.label}</span></button>)}</div>
        {error && <p className="error-message">{error}</p>}
        <p className="quiz-hint">Elige la opcion que mejor describa tu forma de ser.</p>
      </section>
    </main>
  )
}

function ResultView({ result }: { result: RiasecResult }) {
  const scores = [['R', result.rScore], ['I', result.iScore], ['A', result.aScore], ['S', result.sScore], ['E', result.eScore], ['C', result.cScore]]
  return <main className="test-shell result-shell"><header className="test-header"><span className="eyebrow">Orientacion vocacional</span><span className="version-label">Resultado guardado</span></header><section className="result-panel"><span className="eyebrow">Test terminado</span><h1>Tu perfil RIASEC</h1><p className="result-code">{result.primaryCode}<small>{result.secondaryCode}</small></p><div className="score-list">{scores.map(([code, score]) => <div key={code} className="score-row"><strong>{code}</strong><div className="score-bar"><span style={{ width: `${Number(score) / 50 * 100}%` }} /></div><span>{score}</span></div>)}</div><p className="result-note">Este resultado resume tus respuestas. Es una herramienta de orientacion vocacional, no un diagnostico psicologico ni determina obligatoriamente una profesion.</p><a className="start-link" href="/recomendaciones">Ver recomendaciones</a></section></main>
}