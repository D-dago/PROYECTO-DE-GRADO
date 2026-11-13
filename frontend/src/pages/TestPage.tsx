import { useEffect, useRef, useState } from 'react'
import { riasecApi } from '../services/riasecApi'
import type { Attempt, Question, RiasecResult } from '../types/riasec'
import { RiasecResult as ResultView } from '../components/RiasecResult'

const options = [
  { value: 1, label: 'Nada' }, { value: 2, label: 'Poco' }, { value: 3, label: 'Neutral' },
  { value: 4, label: 'Mucho' }, { value: 5, label: 'Totalmente' },
]

// Sin cronometro ni respuesta automatica (revision de Gabriela Lino, ORION #27): si el estudiante
// tardaba en leer o se distraia, el sistema marcaba "Neutral" por el y esa respuesta no reflejaba su
// interes. Tampoco se muestra la dimension de cada pregunta mientras responde: podria asociar las
// actividades con profesiones y sesgar sus respuestas. Las dimensiones se ven en el resultado.

export function TestPage() {
  const [questions, setQuestions] = useState<Question[]>([])
  const [attempt, setAttempt] = useState<Attempt | null>(null)
  const [result, setResult] = useState<RiasecResult | null>(null)
  const [currentIndex, setCurrentIndex] = useState(0)
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  // Evita enviar dos veces la misma respuesta si se pulsa dos veces seguidas.
  const sending = useRef(false)

  async function loadTest() {
    try {
      // Primero se crea o reanuda el intento y despues se piden sus preguntas (las mismas 60 para
      // todos, en el orden guardado en el intento).
      const loadedAttempt = await riasecApi.createAttempt()
      const loadedQuestions = await riasecApi.getAssignedQuestions(loadedAttempt.attemptId)
      setQuestions(loadedQuestions)
      setAttempt(loadedAttempt)
      setCurrentIndex(Math.min(loadedAttempt.answered, loadedQuestions.length - 1))
      if (loadedAttempt.status === 'COMPLETED') setResult(await riasecApi.getResult(loadedAttempt.attemptId))
      // Respondio todo pero el paso final fallo (se corto la red) y recargo la pagina: se termina aqui. Si no,
      // quedaba frente a la ultima pregunta, ya respondida, sin ninguna forma de terminar el test.
      else if (loadedAttempt.total > 0 && loadedAttempt.answered >= loadedAttempt.total) setResult(await riasecApi.complete(loadedAttempt.attemptId))
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
    if (!attempt || !questions[currentIndex] || sending.current) return
    sending.current = true
    setSaving(true)
    setError(null)
    try {
      // Si ya estan todas guardadas es un reintento del paso final: volver a enviar la respuesta se rechazaria.
      const updatedAttempt = attempt.answered >= attempt.total ? attempt : await riasecApi.submitAnswer(attempt.attemptId, questions[currentIndex].id, value)
      setAttempt(updatedAttempt)
      window.dispatchEvent(new Event('profile-changed')) // el encabezado se quedaba con las monedas de antes del test
      if (updatedAttempt.answered >= updatedAttempt.total) {
        const completed = await riasecApi.complete(attempt.attemptId)
        setResult(completed)
        setAttempt({ ...updatedAttempt, status: 'COMPLETED' })
        window.dispatchEvent(new Event('profile-changed')) // suma el bono por terminar
      } else setCurrentIndex((index) => Math.min(index + 1, questions.length - 1))
    } catch (answerError) {
      setError(answerError instanceof Error ? answerError.message : 'No se pudo guardar la respuesta.')
    } finally {
      sending.current = false
      setSaving(false)
    }
  }

  if (loading) return <main className="test-shell"><p className="status-message">Cargando tu test...</p></main>
  if (error && !attempt) return <main className="test-shell"><p className="error-message">{error}</p></main>
  if (result) return <ResultView result={result} />
  if (!attempt || questions.length === 0) return <main className="test-shell"><p className="error-message">No hay preguntas disponibles.</p></main>

  const question = questions[currentIndex]
  const progress = attempt.answered / attempt.total * 100

  return (
    <main className="test-shell">
      <header className="test-header"><span className="eyebrow">Orientación vocacional</span><span className="version-label">RIASEC · {attempt.total} preguntas</span><span className="coin-badge">🪙 {attempt.coins}</span></header>
      <section className="quiz-panel" aria-live="polite">
        <div className="quiz-topline"><div><span className="eyebrow">Test vocacional</span><h1>Descubre qué actividades te motivan.</h1></div><strong className="question-count">Pregunta {Math.min(attempt.answered + 1, attempt.total)} <span>de {attempt.total}</span></strong></div>
        <div className="progress-track" aria-label={`${Math.round(progress)}% completado`}><span style={{ width: `${progress}%` }} /></div>
        <div className="question-meta"><span>{Math.round(progress)}% completado</span></div>
        {/* La instruccion va antes de la pregunta: asi se lee antes de responder. Pregunta por intereses,
            no por la forma de ser (revision de Gabriela Lino). */}
        <p className="quiz-hint">Lee cada actividad y elige qué tanto te gusta o te interesa, aunque nunca la hayas realizado o todavía no sepas hacerla. No hay respuestas correctas o incorrectas. Responde según tus intereses, no según lo que otras personas esperan de ti.</p>
        <QuestionText text={question.text} />
        <div className="answer-grid">{options.map((option) => <button key={option.value} type="button" onClick={() => void answer(option.value)} disabled={saving}><strong>{option.value}</strong><span>{option.label}</span></button>)}</div>
        {error && <p className="error-message">{error}</p>}
      </section>
    </main>
  )
}

// El banco guarda "¿Qué tanto te interesa...? Ejemplo: ..."; la pregunta se muestra grande y el ejemplo
// debajo, en letra secundaria. Tambien entiende el formato anterior "Actividad (ej.: ejemplo)".
function QuestionText({ text }: { text: string }) {
  const match = /^(.*?)\s+(Ejemplo:.*)$/.exec(text) ?? /^(.*?)\s*\((ej\.:[^)]*)\)\s*$/.exec(text)
  if (!match) return <p className="question-text">{text}</p>
  return <p className="question-text">{match[1]}<span className="question-example">{match[2]}</span></p>
}


