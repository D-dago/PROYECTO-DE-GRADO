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

// Tras mostrar una pregunta se ignoran los clics durante este tiempo: nadie la lee tan rapido, y asi un doble
// clic o varios clics seguidos no responden la pregunta siguiente sin verla (ORION #35).
const CLICK_GUARD_MS = 600
// Reintentos de una respuesta que no se pudo guardar (red lenta o cortada). El servidor acepta la misma
// respuesta dos veces sin error, asi que reintentar es seguro.
const SAVE_RETRIES = 4

type PendingAnswer = { questionId: string; value: number }
const sleep = (ms: number) => new Promise((resolve) => setTimeout(resolve, ms))

export function TestPage() {
  const [questions, setQuestions] = useState<Question[]>([])
  const [attempt, setAttempt] = useState<Attempt | null>(null)
  const [result, setResult] = useState<RiasecResult | null>(null)
  const [currentIndex, setCurrentIndex] = useState(0)
  // Preguntas respondidas en pantalla (guardadas o en camino al servidor): de aqui salen el avance y la
  // pregunta siguiente, sin esperar la confirmacion del servidor.
  const [answeredIds, setAnsweredIds] = useState<Set<string>>(new Set())
  const [loading, setLoading] = useState(true)
  const [finishing, setFinishing] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [saveError, setSaveError] = useState<string | null>(null)
  const pending = useRef<PendingAnswer[]>([])
  const flushing = useRef<Promise<void> | null>(null)
  const lockedUntil = useRef(0)

  async function loadTest() {
    try {
      // Primero se crea o reanuda el intento y despues se piden sus preguntas (las 24 sorteadas para
      // ese intento, en el orden guardado: al reanudar son las mismas).
      const loadedAttempt = await riasecApi.createAttempt()
      const loadedQuestions = await riasecApi.getAssignedQuestions(loadedAttempt.attemptId)
      const answered = new Set(loadedQuestions.filter((question) => question.answered).map((question) => question.id))
      setQuestions(loadedQuestions)
      setAttempt(loadedAttempt)
      setAnsweredIds(answered)
      // Sigue en la primera pregunta sin responder (no en "cuantas van"): asi nunca queda frente a una ya respondida.
      const firstOpen = loadedQuestions.findIndex((question) => !answered.has(question.id))
      setCurrentIndex(firstOpen >= 0 ? firstOpen : loadedQuestions.length - 1)
      if (loadedAttempt.status === 'COMPLETED') setResult(await riasecApi.getResult(loadedAttempt.attemptId))
      // Respondio todo pero el paso final fallo (se corto la red) y recargo la pagina: se termina aqui.
      else if (loadedQuestions.length > 0 && firstOpen < 0) setResult(await riasecApi.complete(loadedAttempt.attemptId))
      lockedUntil.current = performance.now() + CLICK_GUARD_MS
    } catch (loadError) {
      setError(loadError instanceof Error ? loadError.message : 'No se pudo cargar el test.')
    } finally { setLoading(false) }
  }

  /** Envia en orden las respuestas pendientes, reintentando las que fallan. Si ya esta enviando, espera a ese envio. */
  function flush(attemptId: string): Promise<void> {
    flushing.current ??= (async () => {
      while (pending.current.length > 0) {
        const next = pending.current[0]
        for (let tries = 1; ; tries++) {
          try {
            const updated = await riasecApi.submitAnswer(attemptId, next.questionId, next.value)
            setAttempt(updated)
            setSaveError(null)
            window.dispatchEvent(new Event('profile-changed')) // el encabezado se quedaba con las monedas de antes del test
            break
          } catch (sendError) {
            if (tries >= SAVE_RETRIES) {
              setSaveError('No se pudo guardar una respuesta. Revisa tu conexión: se reintentará al responder la siguiente.')
              throw sendError
            }
            await sleep(700 * tries)
          }
        }
        pending.current.shift()
      }
    })().finally(() => { flushing.current = null })
    return flushing.current
  }

  // Si se cierra o recarga la pagina con respuestas sin guardar, el navegador avisa.
  useEffect(() => {
    const warn = (event: BeforeUnloadEvent) => { if (pending.current.length > 0) event.preventDefault() }
    window.addEventListener('beforeunload', warn)
    return () => window.removeEventListener('beforeunload', warn)
  }, [])

  // React StrictMode (dev) ejecuta los efectos dos veces: sin este guard se crean
  // dos intentos en paralelo y el usuario queda con dos "en progreso".
  const started = useRef(false)
  useEffect(() => {
    if (started.current) return
    started.current = true
    void loadTest()
  }, [])

  /** Termina el test cuando todas las respuestas quedaron guardadas. Tambien sirve para reintentar. */
  async function finish(attemptId: string) {
    setFinishing(true)
    setError(null)
    try {
      await flush(attemptId)
      setResult(await riasecApi.complete(attemptId))
      window.dispatchEvent(new Event('profile-changed')) // suma el bono por terminar
    } catch (finishError) {
      setError(finishError instanceof Error ? `${finishError.message} Tus respuestas no se perdieron: pulsa Reintentar.` : 'No se pudo terminar el test. Pulsa Reintentar.')
    } finally { setFinishing(false) }
  }

  // Avanza de inmediato a la pregunta siguiente y guarda la respuesta en segundo plano (ORION #35): antes se
  // esperaba al servidor (cerca de un segundo en internet) y, si su confirmacion no llegaba, el test quedaba
  // trabado en una pregunta ya respondida.
  // clickedAt: marca de tiempo del clic (misma escala que performance.now()).
  function answer(value: number, clickedAt: number) {
    const question = questions[currentIndex]
    if (!attempt || !question || finishing || answeredIds.has(question.id) || clickedAt < lockedUntil.current) return
    lockedUntil.current = clickedAt + CLICK_GUARD_MS
    const answered = new Set(answeredIds).add(question.id)
    setAnsweredIds(answered)
    pending.current.push({ questionId: question.id, value })
    const nextOpen = questions.findIndex((candidate) => !answered.has(candidate.id))
    if (nextOpen < 0) { void finish(attempt.attemptId); return }
    setCurrentIndex(nextOpen)
    void flush(attempt.attemptId).catch(() => { /* queda pendiente: se reintenta con la siguiente respuesta */ })
  }

  if (loading) return <main className="test-shell"><p className="status-message">Cargando tu test...</p></main>
  if (error && !attempt) return <main className="test-shell"><p className="error-message">{error}</p></main>
  if (result) return <ResultView result={result} />
  if (!attempt || questions.length === 0) return <main className="test-shell"><p className="error-message">No hay preguntas disponibles.</p></main>

  const question = questions[currentIndex]
  const total = questions.length
  const progress = answeredIds.size / total * 100
  const allAnswered = answeredIds.size >= total

  return (
    <main className="test-shell">
      <header className="test-header"><span className="eyebrow">Orientación vocacional</span><span className="version-label">RIASEC · {total} preguntas</span><span className="coin-badge">🪙 {attempt.coins}</span></header>
      <section className="quiz-panel" aria-live="polite">
        <div className="quiz-topline"><div><span className="eyebrow">Test vocacional</span><h1>Descubre qué actividades te motivan.</h1></div><strong className="question-count">Pregunta {Math.min(answeredIds.size + 1, total)} <span>de {total}</span></strong></div>
        <div className="progress-track" aria-label={`${Math.round(progress)}% completado`}><span style={{ width: `${progress}%` }} /></div>
        <div className="question-meta"><span>{Math.round(progress)}% completado</span></div>
        {/* La instruccion va antes de la pregunta: asi se lee antes de responder. Pregunta por intereses,
            no por la forma de ser (revision de Gabriela Lino). */}
        <p className="quiz-hint">Lee cada actividad y elige qué tanto te gusta o te interesa, aunque nunca la hayas realizado o todavía no sepas hacerla. No hay respuestas correctas o incorrectas. Responde según tus intereses, no según lo que otras personas esperan de ti.</p>
        {/* key: al cambiar de pregunta el texto entra con una transicion corta, para que se note el cambio. */}
        <QuestionText key={question.id} text={question.text} />
        <div className="answer-grid">{options.map((option) => <button key={option.value} type="button" onClick={(event) => answer(option.value, event.timeStamp)} disabled={finishing || allAnswered}><strong>{option.value}</strong><span>{option.label}</span></button>)}</div>
        {finishing && <p className="status-message">Guardando tus respuestas…</p>}
        {saveError && !error && <p className="error-message">{saveError}</p>}
        {error && <p className="error-message">{error} <button type="button" className="retry-button" onClick={() => void finish(attempt.attemptId)}>Reintentar</button></p>}
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


