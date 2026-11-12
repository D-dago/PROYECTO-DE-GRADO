import { useState } from 'react'
import type { FormEvent } from 'react'
import { appApi } from '../services/appApi'
import type { AdminQuestion, AdminResult } from '../services/appApi'
import { InterpretationDetail } from '../components/InterpretationDetail'
import { useFormularioDeEdicion } from '../components/useFormularioDeEdicion'

const dimensionOptions = ['R', 'I', 'A', 'S', 'E', 'C']

type Report = (message: string, isError?: boolean) => void

export function ResultsSection({ results, report }: { results: AdminResult[]; report: Report }) {
  async function download(kind: 'excel' | 'pdf') {
    try { await (kind === 'excel' ? appApi.downloadExcel() : appApi.downloadPdf()) }
    catch (error) { report(error instanceof Error ? error.message : 'No se pudo descargar.', true) }
  }

  return (
    <section className="admin-section" id="resultados">
      <h2>Resultados de los estudiantes</h2>
      <div className="admin-row-actions">
        <button type="button" onClick={() => void download('excel')}>Descargar Excel (con respuestas)</button>
        <button type="button" onClick={() => void download('pdf')}>Descargar PDF</button>
      </div>
      <div className="admin-list">
        {results.map((result) => (
          <details className="admin-row admin-row-result" key={result.attemptId}>
            <summary className="admin-row-main">
              <strong>{result.studentName} · {result.primaryCode || result.secondaryCode ? `${result.primaryCode}${result.secondaryCode}` : 'Perfil plano'}</strong>
              <small>{result.email} · {new Date(result.completedAtUtc).toLocaleString()} · R{result.r} I{result.i} A{result.a} S{result.s} E{result.e} C{result.c} (sobre {result.maxScore})</small>
            </summary>
            <div className="admin-result-detail">
              <h3>¿Qué significa este perfil?</h3>
              <InterpretationDetail
                interpretation={result.interpretation}
                scores={[['R', result.r], ['I', result.i], ['A', result.a], ['S', result.s], ['E', result.e], ['C', result.c]]}
                maxScore={result.maxScore}
              />
            </div>
          </details>
        ))}
        {results.length === 0 && <p className="empty-state">Todavía ningún estudiante terminó el test.</p>}
      </div>
    </section>
  )
}

export function QuestionsSection({ questions, onChanged, report }: { questions: AdminQuestion[]; onChanged: () => void; report: Report }) {
  const [editing, setEditing] = useState<AdminQuestion | null>(null)
  const formulario = useFormularioDeEdicion(editing)
  const [filter, setFilter] = useState('')

  async function submit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    const form = event.currentTarget
    const data = Object.fromEntries(new FormData(form))
    const body = { text: data.text, dimension: data.dimension, isActive: editing ? editing.isActive : true }
    try {
      if (editing) await appApi.updateQuestion(editing.id, body)
      else await appApi.createQuestion(body)
      report(editing ? 'Pregunta actualizada.' : 'Pregunta creada.')
      setEditing(null)
      form.reset()
      onChanged()
    } catch (error) { report(error instanceof Error ? error.message : 'No se pudo guardar la pregunta.', true) }
  }

  async function toggleActive(question: AdminQuestion) {
    try { await appApi.updateQuestion(question.id, { text: question.text, dimension: question.dimension, isActive: !question.isActive }); onChanged() }
    catch (error) { report(error instanceof Error ? error.message : 'No se pudo cambiar el estado.', true) }
  }

  const visible = questions.filter((question) => !filter || question.dimension === filter)

  return (
    <section className="admin-section" id="preguntas">
      <h2>Preguntas del test</h2>
      <p className="empty-state">El test aplica el Perfil de Intereses O*NET: las mismas 30 actividades para todos, las 5 primeras activas de cada dimensión. Son textos oficiales: si se cambia o reemplaza uno, la licencia O*NET exige avisar que es una versión modificada y validarla. Cambiar el texto de una pregunta también cambia cómo se lee en respuestas ya guardadas.</p>
      <label>Filtrar por dimensión
        <select value={filter} onChange={(event) => setFilter(event.target.value)}>
          <option value="">Todas ({questions.length})</option>
          {dimensionOptions.map((dimension) => <option key={dimension} value={dimension}>{dimension}</option>)}
        </select>
      </label>
      <div className="admin-list">
        {visible.map((question) => (
          <div className="admin-row" key={question.id}>
            <div className="admin-row-main">
              <strong>{question.text}</strong>
              <small>{question.code} · dimensión {question.dimension}</small>
            </div>
            <div className="admin-row-actions">
              <span className={`badge ${question.isActive ? 'badge-active' : 'badge-inactive'}`}>{question.isActive ? 'Activa' : 'Inactiva'}</span>
              <button type="button" onClick={() => setEditing(question)}>Editar</button>
              <button type="button" onClick={() => void toggleActive(question)}>{question.isActive ? 'Desactivar' : 'Activar'}</button>
            </div>
          </div>
        ))}
      </div>
      <form ref={formulario} className={editing ? 'simple-form editando' : 'simple-form'} onSubmit={submit} key={editing?.id ?? 'new-question'}>
        <h3>{editing ? `Editar ${editing.code}` : 'Nueva pregunta'}</h3>
        <input name="text" placeholder="Texto de la pregunta" defaultValue={editing?.text} required minLength={5} maxLength={300} />
        <label>Dimensión
          <select name="dimension" defaultValue={editing?.dimension ?? 'C'} required>
            {dimensionOptions.map((dimension) => <option key={dimension} value={dimension}>{dimension}</option>)}
          </select>
        </label>
        <div className="admin-row-actions">
          <button type="submit">{editing ? 'Guardar cambios' : 'Crear pregunta'}</button>
          {editing && <button type="button" onClick={() => setEditing(null)}>Cancelar edición</button>}
        </div>
      </form>
    </section>
  )
}
