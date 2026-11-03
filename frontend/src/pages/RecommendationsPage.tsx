import { useEffect, useState } from 'react'
import type { FormEvent } from 'react'
import { appApi } from '../services/appApi'
import type { Career, Institution } from '../services/appApi'

const budgetRanges = [
  { value: '', label: 'No estoy seguro / sin límite' },
  { value: '500', label: 'Hasta Bs. 500 al mes' },
  { value: '1000', label: 'Hasta Bs. 1.000 al mes' },
  { value: '1500', label: 'Hasta Bs. 1.500 al mes' },
]

function costTier(cost?: number): { label: string; className: string } | null {
  if (cost == null) return null
  if (cost <= 500) return { label: '🟢 Accesible', className: 'cost-tier-low' }
  if (cost <= 1000) return { label: '🟡 Costo medio', className: 'cost-tier-medium' }
  return { label: '🔴 Costo alto', className: 'cost-tier-high' }
}

export function RecommendationsPage() {
  const [institutions, setInstitutions] = useState<Institution[]>([])
  const [catalog, setCatalog] = useState<Career[]>([])
  const [institutionId, setInstitutionId] = useState('')
  const [municipality, setMunicipality] = useState('')
  const [items, setItems] = useState<Career[]>([])
  const [selected, setSelected] = useState<Career[]>([])
  const [message, setMessage] = useState<string | null>(null)

  useEffect(() => {
    void appApi.institutions().then(setInstitutions).catch(() => setMessage('Inicia sesión para consultar recomendaciones.'))
    void appApi.careers().then(setCatalog).catch(() => setCatalog([]))
  }, [])

  // Todas las listas salen de los datos ya cargados, para elegir en vez de escribir a mano y
  // arriesgarse a una falta de tilde o mayuscula que deja la busqueda vacia.
  const municipios = [...new Set(institutions.map((item) => item.municipality).filter((item): item is string => !!item))].sort()
  // Las carreras y modalidades ofrecidas se limitan a la institucion y al municipio elegidos:
  // asi nunca se ofrece una combinacion que no existe.
  const municipioDe = new Map(institutions.map((item) => [item.id, item.municipality]))
  const disponibles = catalog.filter((career) =>
    (!institutionId || career.institutionId === institutionId) &&
    (!municipality || municipioDe.get(career.institutionId) === municipality))
  const porArea = new Map<string, Set<string>>()
  for (const career of disponibles) {
    const area = career.area?.trim() || 'Otras'
    porArea.set(area, (porArea.get(area) ?? new Set()).add(career.name))
  }
  const areas = [...porArea.keys()].sort()
  const modalidades = [...new Set(disponibles.map((career) => career.modality).filter((item): item is string => !!item))].sort()

  async function submit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    const data = Object.fromEntries(new FormData(event.currentTarget))
    // Los campos vacios se omiten: el backend espera Guid?/decimal? y no
    // puede deserializar una cadena vacia en esos tipos (400 Bad Request).
    const filters = Object.fromEntries(Object.entries(data).filter(([, value]) => value !== ''))
    try {
      const result = await appApi.recommendations(filters)
      setItems(result.items)
      setMessage(result.items.length ? result.aiMessage ?? null : 'No hay alternativas con datos cargados que coincidan con tus filtros.')
    } catch (error) { setMessage(error instanceof Error ? error.message : 'No se pudieron cargar las recomendaciones.') }
  }

  return (
    <main className="content-shell">
      <span className="eyebrow">Motor de reglas</span>
      <h1>Alternativas compatibles</h1>
      <form className="filter-form" onSubmit={submit}>
        <select name="institutionId" value={institutionId} onChange={(event) => setInstitutionId(event.target.value)}>
          <option value="">Todas las instituciones</option>
          {institutions.map((item) => <option key={item.id} value={item.id}>{item.name}</option>)}
        </select>
        <select name="municipality" value={municipality} onChange={(event) => setMunicipality(event.target.value)}>
          <option value="">Todos los municipios</option>
          {municipios.map((municipio) => <option key={municipio} value={municipio}>{municipio}</option>)}
        </select>
        {/* key: al cambiar institucion o municipio se reinicia la eleccion, porque la carrera o
            modalidad elegida antes puede no existir en la nueva seleccion. */}
        <select name="area" defaultValue="" key={`carrera-${institutionId}-${municipality}`}>
          <option value="">Todas las carreras</option>
          {areas.map((area) => (
            <optgroup key={area} label={area}>
              {[...porArea.get(area)!].sort().map((nombre) => <option key={nombre} value={nombre}>{nombre}</option>)}
            </optgroup>
          ))}
        </select>
        <select name="modality" defaultValue="" key={`modalidad-${institutionId}-${municipality}`}>
          <option value="">Todas las modalidades</option>
          {modalidades.map((modalidad) => <option key={modalidad} value={modalidad}>{modalidad}</option>)}
        </select>
        <label>Presupuesto
          <select name="maximumCost" defaultValue="">
            {budgetRanges.map((range) => <option key={range.value} value={range.value}>{range.label}</option>)}
          </select>
        </label>
        <button type="submit">Buscar</button>
      </form>
      {message && <p className="empty-state">{message}</p>}
      <div className="career-grid">
        {items.map((item) => {
          const tier = costTier(item.cost)
          return (
            <article className="career-card" key={item.id}>
              <span>{item.compatibility}% compatible</span>
              {tier && <span className={`cost-tier ${tier.className}`}>{tier.label}</span>}
              <h2>{item.name}</h2>
              <p>{item.institutionName}</p>
              <p>{item.compatibilityReason}</p>
              <button type="button" onClick={() => setSelected((current) => current.some((x) => x.id === item.id) ? current.filter((x) => x.id !== item.id) : [...current, item])}>
                {selected.some((x) => x.id === item.id) ? 'Quitar de comparación' : 'Comparar'}
              </button>
            </article>
          )
        })}
      </div>
      {selected.length > 1 && (
        <section className="comparison">
          <h2>Comparación</h2>
          {selected.map((item) => (
            <div key={item.id}>
              <strong>{item.name}</strong>
              <span>{item.compatibility}% · {item.modality ?? 'Información pendiente de actualización'} · {item.durationMonths ? `${item.durationMonths} meses` : 'Información pendiente de actualización'}</span>
            </div>
          ))}
        </section>
      )}
    </main>
  )
}
