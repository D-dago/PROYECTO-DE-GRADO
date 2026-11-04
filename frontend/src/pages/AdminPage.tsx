import { useEffect, useState } from 'react'
import type { FormEvent } from 'react'
import { appApi } from '../services/appApi'
import type { AdminQuestion, AdminResult, AdminUser, Career, Compatibility, Institution } from '../services/appApi'
import { QuestionsSection, ResultsSection } from './AdminExtras'
import { useFormularioDeEdicion } from '../components/useFormularioDeEdicion'

const dashboardLabels: [keyof NonNullable<DashboardData>, string][] = [['students', 'Estudiantes registrados'], ['institutions', 'Instituciones'], ['careers', 'Carreras'], ['compatibilityRules', 'Reglas de compatibilidad']]
const dimensionOptions = ['R', 'I', 'A', 'S', 'E', 'C']

type DashboardData = { students: number; institutions: number; careers: number; compatibilityRules: number } | null

export function AdminPage() {
  const [success, setSuccess] = useState('')
  const [error, setError] = useState('')
  const [dashboard, setDashboard] = useState<DashboardData | null>(null)
  const [institutions, setInstitutions] = useState<Institution[]>([])
  const [careers, setCareers] = useState<Career[]>([])
  const [compatibilities, setCompatibilities] = useState<Compatibility[]>([])
  const [students, setStudents] = useState<AdminUser[]>([])
  const [questions, setQuestions] = useState<AdminQuestion[]>([])
  const [results, setResults] = useState<AdminResult[]>([])

  function report(message: string, isError = false) {
    if (isError) { setError(message); setSuccess('') } else { setSuccess(message); setError('') }
  }

  async function loadAll() {
    try {
      const [dashboardData, institutionsData, careersData, compatibilitiesData, usersData, questionsData, resultsData] = await Promise.all([
        appApi.dashboard(), appApi.adminInstitutions(), appApi.adminCareers(), appApi.compatibilities(), appApi.adminUsers(), appApi.adminQuestions(), appApi.adminResults(),
      ])
      setQuestions(questionsData)
      setResults(resultsData)
      setDashboard(dashboardData)
      setInstitutions(institutionsData)
      setCareers(careersData)
      setCompatibilities(compatibilitiesData)
      setStudents(usersData.filter((user) => user.role === 'ESTUDIANTE'))
    } catch (loadError) {
      report(loadError instanceof Error ? loadError.message : 'Se requiere una sesión de administrador.', true)
    }
  }

  useEffect(() => { void loadAll() }, [])

  return (
    <main className="content-shell">
      <span className="eyebrow">Solo administrador</span>
      <h1>Panel de datos</h1>
      {dashboard && <div className="dashboard-grid">{dashboardLabels.map(([key, label]) => <strong key={key}>{dashboard[key]}<small> {label}</small></strong>)}</div>}
      <p className="empty-state">Carga únicamente información institucional verificada. La aplicación no incluye datos inventados.</p>
      {success && <p className="success-message">{success}</p>}
      {error && <p className="error-message">{error}</p>}

      <InstitutionsSection institutions={institutions} onChanged={loadAll} report={report} />
      <CareersSection careers={careers} institutions={institutions} onChanged={loadAll} report={report} />
      <CompatibilitiesSection compatibilities={compatibilities} careers={careers} onChanged={loadAll} report={report} />
      <ResultsSection results={results} report={report} />
      <QuestionsSection questions={questions} onChanged={loadAll} report={report} />
      <StudentsSection students={students} onChanged={loadAll} report={report} />
    </main>
  )
}

function InstitutionsSection({ institutions, onChanged, report }: { institutions: Institution[]; onChanged: () => void; report: (message: string, isError?: boolean) => void }) {
  const [editing, setEditing] = useState<Institution | null>(null)
  const formulario = useFormularioDeEdicion(editing)

  async function submit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    const form = event.currentTarget
    const data = Object.fromEntries(new FormData(form))
    try {
      if (editing) await appApi.updateInstitution(editing.id, { ...data, isActive: editing.isActive })
      else await appApi.createInstitution(data)
      report(editing ? 'Institución actualizada.' : 'Institución guardada.')
      setEditing(null)
      form.reset()
      onChanged()
    } catch (error) { report(error instanceof Error ? error.message : 'No se pudo guardar la institución.', true) }
  }

  async function toggleActive(institution: Institution) {
    try { await appApi.setInstitutionActive(institution.id, !institution.isActive); onChanged() }
    catch (error) { report(error instanceof Error ? error.message : 'No se pudo cambiar el estado.', true) }
  }

  return (
    <section className="admin-section" id="instituciones">
      <h2>Instituciones</h2>
      <div className="admin-list">
        {institutions.map((institution) => (
          <div className="admin-row" key={institution.id}>
            <div className="admin-row-main">
              <strong>{institution.name}</strong>
              <small>{institution.type} · {institution.municipality ?? 'Sin municipio'} · {institution.status} · {institution.website ?? 'Sin página oficial'}</small>
            </div>
            <div className="admin-row-actions">
              <span className={`badge ${institution.isActive ? 'badge-active' : 'badge-inactive'}`}>{institution.isActive ? 'Activa' : 'Inactiva'}</span>
              <button type="button" onClick={() => setEditing(institution)}>Editar</button>
              <button type="button" onClick={() => void toggleActive(institution)}>{institution.isActive ? 'Desactivar' : 'Activar'}</button>
            </div>
          </div>
        ))}
        {institutions.length === 0 && <p className="empty-state">Todavía no hay instituciones cargadas.</p>}
      </div>
      <form ref={formulario} className={editing ? 'simple-form editando' : 'simple-form'} onSubmit={submit} key={editing?.id ?? 'new-institution'}>
        <h3>{editing ? `Editar: ${editing.name}` : 'Nueva institución'}</h3>
        <input name="name" placeholder="Nombre oficial" defaultValue={editing?.name} required maxLength={150} />
        <input name="type" placeholder="Tipo" defaultValue={editing?.type} required />
        <input name="municipality" placeholder="Municipio" defaultValue={editing?.municipality} />
        <input name="location" placeholder="Ubicación" defaultValue={editing?.location} />
        <input name="website" type="url" placeholder="Página oficial (https://...)" pattern="https?://.+" title="Debe empezar con http:// o https://" maxLength={300} defaultValue={editing?.website ?? ''} />
        <input name="source" placeholder="Fuente verificable" defaultValue={editing?.source} />
        <input name="status" placeholder="Estado" defaultValue={editing?.status ?? 'PENDIENTE'} required />
        <div className="admin-row-actions">
          <button type="submit">{editing ? 'Guardar cambios' : 'Guardar institución'}</button>
          {editing && <button type="button" onClick={() => setEditing(null)}>Cancelar edición</button>}
        </div>
      </form>
    </section>
  )
}

function CareersSection({ careers, institutions, onChanged, report }: { careers: Career[]; institutions: Institution[]; onChanged: () => void; report: (message: string, isError?: boolean) => void }) {
  const [editing, setEditing] = useState<Career | null>(null)
  const formulario = useFormularioDeEdicion(editing)

  async function submit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    const form = event.currentTarget
    const data = Object.fromEntries(new FormData(form))
    const body = { ...data, durationMonths: data.durationMonths ? Number(data.durationMonths) : null, cost: data.cost ? Number(data.cost) : null, isActive: editing ? editing.isActive : true }
    try {
      if (editing) await appApi.updateCareer(editing.id, body)
      else await appApi.createCareer(body)
      report(editing ? 'Carrera actualizada.' : 'Carrera guardada.')
      setEditing(null)
      form.reset()
      onChanged()
    } catch (error) { report(error instanceof Error ? error.message : 'No se pudo guardar la carrera.', true) }
  }

  async function toggleActive(career: Career) {
    try { await appApi.setCareerActive(career.id, !career.isActive); onChanged() }
    catch (error) { report(error instanceof Error ? error.message : 'No se pudo cambiar el estado.', true) }
  }

  return (
    <section className="admin-section" id="carreras">
      <h2>Carreras</h2>
      <div className="admin-list">
        {careers.map((career) => (
          <div className="admin-row" key={career.id}>
            <div className="admin-row-main">
              <strong>{career.name}</strong>
              <small>{career.institutionName} · {career.area ?? 'Sin área'} · {career.status}</small>
            </div>
            <div className="admin-row-actions">
              <span className={`badge ${career.isActive ? 'badge-active' : 'badge-inactive'}`}>{career.isActive ? 'Activa' : 'Inactiva'}</span>
              <button type="button" onClick={() => setEditing(career)}>Editar</button>
              <button type="button" onClick={() => void toggleActive(career)}>{career.isActive ? 'Desactivar' : 'Activar'}</button>
            </div>
          </div>
        ))}
        {careers.length === 0 && <p className="empty-state">Todavía no hay carreras cargadas.</p>}
      </div>
      <form ref={formulario} className={editing ? 'simple-form editando' : 'simple-form'} onSubmit={submit} key={editing?.id ?? 'new-career'}>
        <h3>{editing ? `Editar: ${editing.name}` : 'Nueva carrera'}</h3>
        <input name="name" placeholder="Nombre oficial" defaultValue={editing?.name} required maxLength={150} />
        <label>Institución
          <select name="institutionId" defaultValue={editing?.institutionId ?? ''} required>
            <option value="" disabled>Selecciona una institución</option>
            {institutions.map((institution) => <option key={institution.id} value={institution.id}>{institution.name}</option>)}
          </select>
        </label>
        <input name="area" placeholder="Área" defaultValue={editing?.area} />
        <input name="level" placeholder="Nivel" defaultValue={editing?.level} />
        <input name="modality" placeholder="Modalidad" defaultValue={editing?.modality} />
        <input name="durationMonths" type="number" min={1} max={240} placeholder="Duración en meses" defaultValue={editing?.durationMonths} />
        <input name="cost" type="number" min={0} max={1000000} placeholder="Costo si existe" defaultValue={editing?.cost} />
        <input name="source" placeholder="Fuente verificable" defaultValue={editing?.source} />
        <input name="status" placeholder="Estado" defaultValue={editing?.status ?? 'PENDIENTE'} required />
        <div className="admin-row-actions">
          <button type="submit">{editing ? 'Guardar cambios' : 'Guardar carrera'}</button>
          {editing && <button type="button" onClick={() => setEditing(null)}>Cancelar edición</button>}
        </div>
      </form>
    </section>
  )
}

function CompatibilitiesSection({ compatibilities, careers, onChanged, report }: { compatibilities: Compatibility[]; careers: Career[]; onChanged: () => void; report: (message: string, isError?: boolean) => void }) {
  const [editing, setEditing] = useState<Compatibility | null>(null)
  const formulario = useFormularioDeEdicion(editing)

  async function submit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    const form = event.currentTarget
    const data = Object.fromEntries(new FormData(form))
    const body = { careerId: editing?.careerId ?? data.careerId, dimension: data.dimension, compatibilityLevel: Number(data.compatibilityLevel), rule: data.rule }
    try {
      if (editing) await appApi.updateCompatibility(editing.id, body)
      else await appApi.addCompatibility(body)
      report(editing ? 'Regla actualizada.' : 'Regla creada.')
      setEditing(null)
      form.reset()
      onChanged()
    } catch (error) { report(error instanceof Error ? error.message : 'No se pudo guardar la regla.', true) }
  }

  async function remove(compatibility: Compatibility) {
    try { await appApi.deleteCompatibility(compatibility.id); onChanged() }
    catch (error) { report(error instanceof Error ? error.message : 'No se pudo eliminar la regla.', true) }
  }

  return (
    <section className="admin-section">
      <h2>Compatibilidades por dimensión (RIASEC)</h2>
      <div className="admin-list">
        {compatibilities.map((compatibility) => (
          <div className="admin-row" key={compatibility.id}>
            <div className="admin-row-main">
              <strong>{compatibility.careerName}</strong>
              <small>{compatibility.dimension} · nivel {compatibility.compatibilityLevel} · {compatibility.rule}</small>
            </div>
            <div className="admin-row-actions">
              <button type="button" onClick={() => setEditing(compatibility)}>Editar</button>
              <button type="button" onClick={() => void remove(compatibility)}>Eliminar</button>
            </div>
          </div>
        ))}
        {compatibilities.length === 0 && <p className="empty-state">Todavía no hay reglas de compatibilidad.</p>}
      </div>
      <form ref={formulario} className={editing ? 'simple-form editando' : 'simple-form'} onSubmit={submit} key={editing?.id ?? 'new-compatibility'}>
        <h3>{editing ? 'Editar regla' : 'Nueva regla'}</h3>
        <label>Carrera
          <select name="careerId" defaultValue={editing?.careerId ?? ''} required disabled={Boolean(editing)}>
            <option value="" disabled>Selecciona una carrera</option>
            {careers.map((career) => <option key={career.id} value={career.id}>{career.name}</option>)}
          </select>
        </label>
        {editing && <small>La carrera de una regla existente no se puede cambiar; elimina y crea una nueva si es necesario.</small>}
        <label>Dimensión
          <select name="dimension" defaultValue={editing?.dimension ?? 'C'} required>
            {dimensionOptions.map((dimension) => <option key={dimension} value={dimension}>{dimension}</option>)}
          </select>
        </label>
        <input name="compatibilityLevel" type="number" min={0} max={100} placeholder="Nivel (0-100)" defaultValue={editing?.compatibilityLevel} required />
        <input name="rule" placeholder="Descripción de la regla" defaultValue={editing?.rule} required />
        <div className="admin-row-actions">
          <button type="submit">{editing ? 'Guardar cambios' : 'Crear regla'}</button>
          {editing && <button type="button" onClick={() => setEditing(null)}>Cancelar edición</button>}
        </div>
      </form>
    </section>
  )
}

function StudentsSection({ students, onChanged, report }: { students: AdminUser[]; onChanged: () => void; report: (message: string, isError?: boolean) => void }) {
  async function toggleActive(student: AdminUser) {
    try { await appApi.setStudentActive(student.id, !student.isActive); onChanged() }
    catch (error) { report(error instanceof Error ? error.message : 'No se pudo cambiar el estado.', true) }
  }

  return (
    <section className="admin-section" id="estudiantes">
      <h2>Estudiantes</h2>
      <div className="admin-list">
        {students.map((student) => (
          <div className="admin-row" key={student.id}>
            <div className="admin-row-main">
              <strong>{student.fullName}</strong>
              <small>{student.email} · registrado el {new Date(student.createdAtUtc).toLocaleDateString()} · {student.completedTests} test(s) terminado(s) · 🪙 {student.coins}</small>
            </div>
            <div className="admin-row-actions">
              <span className={`badge ${student.isActive ? 'badge-active' : 'badge-inactive'}`}>{student.isActive ? 'Activo' : 'Inactivo'}</span>
              <button type="button" onClick={() => void toggleActive(student)}>{student.isActive ? 'Desactivar' : 'Activar'}</button>
            </div>
          </div>
        ))}
        {students.length === 0 && <p className="empty-state">Todavía no hay estudiantes registrados.</p>}
      </div>
    </section>
  )
}
