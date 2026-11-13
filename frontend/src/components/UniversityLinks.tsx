import { useState } from 'react'
import type { Career } from '../services/appApi'
import { SiteWindow } from './SiteWindow'

const CARRERAS_INICIALES = 8
export const CAREER_SEARCH_ID = 'buscador-carreras'

type InstitutionOption = { id: string; name: string; website: string | null }
type CareerGroup = { key: string; name: string; areas: string; compatibility: number; reason: string; institutions: InstitutionOption[] }

// La direccion viene de la base de datos: se vuelve a comprobar aqui para que nunca llegue a un enlace
// o a la ventana interna algo que no sea una pagina web (por ejemplo "javascript:").
function safeWebsite(website?: string | null): string | null {
  if (!website) return null
  try {
    const { protocol } = new URL(website)
    return protocol === 'https:' || protocol === 'http:' ? website : null
  } catch {
    return null
  }
}

// Sin tildes ni mayusculas, para no separar "Ingenieria" de "Ingeniería" si el administrador la escribe distinto.
function careerKey(name: string) {
  return name.normalize('NFD').replace(/\p{M}/gu, '').trim().toLowerCase()
}

// Palabras que no ayudan a buscar ("Biología, química o física" -> biologia, quimica, fisica).
const STOPWORDS = new Set(['del', 'las', 'los', 'con', 'para', 'por', 'una', 'uno', 'que', 'sus'])
// Palabras tan generales que, solas, traerian medio catalogo: solo ordenan, salvo que sean lo unico buscado.
const GENERIC = new Set(['ingenieria', 'ciencia', 'ciencias', 'licenciatura', 'tecnico', 'tecnica', 'carrera', 'carreras', 'estudios', 'profesional'])

function searchWords(query: string) {
  return careerKey(query).split(/[^a-z0-9]+/).filter((word) => word.length >= 3 && !STOPWORDS.has(word))
}

/**
 * Cuantas palabras buscadas aparecen en la carrera (nombre, area o institucion). 0 = no coincide.
 * Si se busco alguna palabra especifica, la carrera debe contener al menos una; las generales solo suman.
 */
function matchScore(group: CareerGroup, words: string[]) {
  const text = `${group.key} ${group.areas} ${group.institutions.map((item) => careerKey(item.name)).join(' ')}`
  const specific = words.filter((word) => !GENERIC.has(word))
  const required = specific.length > 0 ? specific : words
  if (!required.some((word) => text.includes(word))) return 0
  return words.filter((word) => text.includes(word)).length
}

/** Una tarjeta por carrera, con todas las instituciones de Cochabamba que la ofrecen. */
function groupByCareer(careers: Career[]): CareerGroup[] {
  const groups = new Map<string, CareerGroup>()
  for (const career of careers) {
    // 0 % significa que la carrera no tiene regla de compatibilidad: no es una recomendacion.
    if (career.compatibility <= 0) continue
    const key = careerKey(career.name)
    const group = groups.get(key) ?? { key, name: career.name, areas: '', compatibility: 0, reason: career.compatibilityReason, institutions: [] }
    if (career.area && !group.areas.includes(careerKey(career.area))) group.areas += ` ${careerKey(career.area)}`
    if (career.compatibility > group.compatibility) {
      group.compatibility = career.compatibility
      group.reason = career.compatibilityReason
    }
    if (!group.institutions.some((item) => item.id === career.institutionId)) {
      group.institutions.push({ id: career.institutionId, name: career.institutionName, website: safeWebsite(career.institutionWebsite) })
    }
    groups.set(key, group)
  }
  return [...groups.values()]
    .map((group) => ({ ...group, institutions: [...group.institutions].sort((a, b) => a.name.localeCompare(b.name)) }))
    .sort((a, b) => b.compatibility - a.compatibility || a.name.localeCompare(b.name))
}

/**
 * La busqueda puede venir de fuera (al tocar un area afin del resultado): en ese caso el componente la
 * recibe en `query` y avisa cada cambio con `onQueryChange`. Sin esas props maneja su propia busqueda.
 */
export function UniversityLinks({ careers, query: externalQuery, onQueryChange }: { careers: Career[]; query?: string; onQueryChange?: (query: string) => void }) {
  const [open, setOpen] = useState<(InstitutionOption & { website: string }) | null>(null)
  const [showAll, setShowAll] = useState(false)
  const [ownQuery, setOwnQuery] = useState('')
  const query = externalQuery ?? ownQuery
  const setQuery = onQueryChange ?? setOwnQuery
  const groups = groupByCareer(careers)
  // Por palabras y sin tildes: "Biologia quimica o fisica" encuentra "Bioquímica y Farmacia" e "Ingeniería Química".
  const words = searchWords(query)
  const searching = words.length > 0
  const found = searching
    ? groups.map((group) => ({ group, score: matchScore(group, words) })).filter((item) => item.score > 0).sort((a, b) => b.score - a.score).map((item) => item.group)
    : groups

  if (groups.length === 0) {
    return <p className="result-note">Todavía no hay carreras compatibles con tu perfil. Cuando el administrador las cargue aparecerán aquí.</p>
  }

  // Al buscar se muestran todas las coincidencias, no solo las primeras.
  const visible = showAll || searching ? found : found.slice(0, CARRERAS_INICIALES)

  return (
    <>
      <p className="result-note university-note">Cada carrera muestra las universidades e institutos de Cochabamba donde la encontramos. Confirma costos, ubicación, requisitos y sede en la página oficial de cada uno.</p>
      <label className="university-search">
        <span aria-hidden="true">🔍</span>
        <input id={CAREER_SEARCH_ID} type="search" value={query} onChange={(event) => setQuery(event.target.value)} placeholder="Busca una carrera o institución (ej.: derecho)" aria-label="Buscar carrera o institución" />
        {query && <button type="button" onClick={() => setQuery('')} aria-label="Borrar búsqueda">✕</button>}
      </label>
      {query.trim() && (
        <p className="university-count" aria-live="polite">
          {!searching
            ? 'Escribe al menos una palabra de 3 letras o más.'
            : found.length === 0
              ? `Ninguna carrera compatible coincide con «${query.trim()}». Prueba con una sola palabra del nombre de la carrera.`
              : `${found.length} ${found.length === 1 ? 'carrera coincide' : 'carreras coinciden'} con «${query.trim()}».`}
        </p>
      )}
      <div className="university-grid">
        {visible.map((group) => (
          <article className="university-card" key={group.key} title={group.reason}>
            <header className="university-card-top">
              <h3>{group.name}</h3>
              <strong>{group.compatibility}%</strong>
            </header>
            <ul>
              {group.institutions.map((institution) => (
                <li key={institution.id}>
                  <span>{institution.name}</span>
                  {institution.website
                    ? <button type="button" onClick={() => setOpen({ ...institution, website: institution.website! })} aria-label={`Ver sitio oficial de ${institution.name}`}>Ver sitio</button>
                    : <small>Sin sitio oficial</small>}
                </li>
              ))}
            </ul>
          </article>
        ))}
      </div>
      {!searching && groups.length > CARRERAS_INICIALES && (
        <button type="button" className="university-more" onClick={() => setShowAll((current) => !current)}>
          {showAll ? 'Ver menos carreras' : `Ver las ${groups.length} carreras`}
        </button>
      )}
      {open && <SiteWindow key={open.id} institutionId={open.id} name={open.name} website={open.website} onClose={() => setOpen(null)} />}
    </>
  )
}
