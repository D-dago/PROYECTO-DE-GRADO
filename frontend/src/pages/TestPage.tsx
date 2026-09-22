import { useEffect, useRef, useState } from 'react'
import { riasecApi } from '../services/riasecApi'
import type { Attempt, Question, RiasecResult } from '../types/riasec'
import { RiasecResult as ResultView } from '../components/RiasecResult'

const options = [
  { value: 1, label: 'Nada' }, { value: 2, label: 'Poco' }, { value: 3, label: 'Neutral' },
  { value: 4, label: 'Mucho' }, { value: 5, label: 'Totalmente' },
]

const dimensions = new Map([
  ['C', 'Administrativa'], ['H', 'Humanidades y Sociales'], ['A', 'Artística'],
  ['S', 'Ciencias de la Salud'], ['I', 'Técnica'], ['D', 'Defensa y Seguridad'],
  ['E', 'Ciencias Experimentales'],
])

const QUESTION_SECONDS = 30
const AUTO_ANSWER_VALUE = 3 // "Neutral": lo que se registra si se acaba el tiempo sin responder

export function TestPage() {
  const [questions, setQuestions] = useState<Question[]>([])
  const [attempt, setAttempt] = useState<Attempt | null>(null)
  const [result, setResult] = useState<RiasecResult | null>(null)
  const [currentIndex, setCurrentIndex] = useState(0)
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [timeLeft, setTimeLeft] = useState(QUESTION_SECONDS)

  async function loadTest() {
    try {
      // Las preguntas se sortean por intento (2 por dimension, al azar), asi que
      // primero hay que crear/reanudar el intento y recien despues pedir cuales
      // 14 preguntas especificas le tocaron - ya no se puede pedir en paralelo.
      const loadedAttempt = await riasecApi.createAttempt()
      const loadedQuestions = await riasecApi.getAssignedQuestions(loadedAttempt.attemptId)
      setQuestions(loadedQuestions)
      setAttempt(loadedAttempt)
      setCurrentIndex(Math.min(loadedAttempt.answered, loadedQuestions.length - 1))
      if (loadedAttempt.status === 'COMPLETED') setResult(await riasecApi.getResult(loadedAttempt.attemptId))
    } catch (loadError) {
      setError(loadError instanceof Error ? loadError.message : 'No se pudo cargar el test.')
    } finally { setLoading(false) }
  }

  // React StrictMode (dev) ejecuta los efectos dos veces: sin este guard se crean
  // dos intentos en paralelo y el usuario queda con dos "en progreso".
  const started = useRef(false)
  useEffect(() => {
    if (started.current) return
    started.current = true
    void loadTest()
  }, [])

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

  // Cronometro por pregunta: se reinicia cada vez que cambia currentIndex. Si llega a
  // cero sin respuesta, se auto-responde "Neutral" y se avanza (igual que un click
  // manual), para no dejar el intento con preguntas huerfanas que nunca se podrian
  // volver a contestar y bloquearian la finalizacion del test.
  useEffect(() => {
    if (loading || result || !attempt) return
    setTimeLeft(QUESTION_SECONDS)
    const startedAt = Date.now()
    const interval = setInterval(() => {
      const remaining = QUESTION_SECONDS - Math.floor((Date.now() - startedAt) / 1000)
      if (remaining <= 0) {
        clearInterval(interval)
        setTimeLeft(0)
        void answer(AUTO_ANSWER_VALUE)
      } else {
        setTimeLeft(remaining)
      }
    }, 250)
    return () => clearInterval(interval)
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [currentIndex, loading, result, attempt])

  if (loading) return <main className="test-shell"><p className="status-message">Cargando tu test...</p></main>
  if (error && !attempt) return <main className="test-shell"><p className="error-message">{error}</p></main>
  if (result) return <ResultView result={result} />
  if (!attempt || questions.length === 0) return <main className="test-shell"><p className="error-message">No hay preguntas disponibles.</p></main>

  const question = questions[currentIndex]
  const progress = attempt.answered / attempt.total * 100

  return (
    <main className="test-shell">
      <header className="test-header"><span className="eyebrow">Orientación vocacional</span><span className="version-label">CHASIDE · {attempt.total} preguntas</span><span className="coin-badge">🪙 {attempt.coins}</span></header>
      <section className="quiz-panel" aria-live="polite">
        <div className="quiz-topline"><div><span className="eyebrow">Test vocacional</span><h1>Descubre qué actividades te motivan.</h1></div><strong className="question-count">Pregunta {Math.min(attempt.answered + 1, attempt.total)} <span>de {attempt.total}</span></strong></div>
        <div className="progress-track" aria-label={`${Math.round(progress)}% completado`}><span style={{ width: `${progress}%` }} /></div>
        <div className="question-meta"><span className="dimension-tag">{question.dimension} · {dimensions.get(question.dimension)}</span><span className={`timer-badge${timeLeft <= 5 ? ' timer-low' : ''}`}>⏱ {timeLeft}s</span><span>{Math.round(progress)}% completado</span></div>
        <QuestionText text={question.text} />
        <div className="answer-grid">{options.map((option) => <button key={option.value} type="button" onClick={() => void answer(option.value)} disabled={saving}><strong>{option.value}</strong><span>{option.label}</span></button>)}</div>
        {error && <p className="error-message">{error}</p>}
        <p className="quiz-hint">Elige la opción que mejor describa tu forma de ser. Si el tiempo se acaba, se registra "Neutral" y avanza sola.</p>
      </section>
    </main>
  )
}

// El banco guarda "Actividad (ej.: ejemplo)"; se muestra la actividad grande y el ejemplo mas suave.
function QuestionText({ text }: { text: string }) {
  const match = /^(.*?)\s*\((ej\.:[^)]*)\)\s*$/.exec(text)
  if (!match) return <p className="question-text">{text}</p>
  return <p className="question-text">{match[1]}<span className="question-example">{match[2]}</span></p>
}


