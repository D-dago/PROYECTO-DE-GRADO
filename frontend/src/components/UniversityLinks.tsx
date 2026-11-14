import { useState } from 'react'
import type { Career } from '../services/appApi'
import { SiteWindow } from './SiteWindow'
import { areaTermsFor, groupByCareer, matchesArea, matchScore, searchWords, type InstitutionOption } from '../services/careerSearch'

const CARRERAS_INICIALES = 8
export const CAREER_SEARCH_ID = 'buscador-carreras'

/**
 * La busqueda puede venir de fuera (al tocar un area afin del resultado): en ese caso el componente la
 * recibe en `query` y avisa cada cambio con `onQueryChange`. Sin esas props maneja su propia busqueda.
 */
export function UniversityLinks({ careers, query: externalQuery, onQueryChange, areaTerms }: { careers: Career[]; query?: string; onQueryChange?: (query: string) => void; areaTerms?: Record<string, string[]> }) {
  const [open, setOpen] = useState<(InstitutionOption & { website: string }) | null>(null)
  const [showAll, setShowAll] = useState(false)
  const [ownQuery, setOwnQuery] = useState('')
  const query = externalQuery ?? ownQuery
  const setQuery = onQueryChange ?? setOwnQuery
  const groups = groupByCareer(careers)
  // Si se toco o escribio un area afin ("Agronomía y veterinaria"), se buscan sus terminos; si no, por palabras
  // y sin tildes: "Biologia quimica o fisica" encuentra "Bioquímica y Farmacia" e "Ingeniería Química".
  const terms = areaTermsFor(query, areaTerms)
  const words = searchWords(query)
  const searching = words.length > 0
  const found = terms?.length
    ? groups.filter((group) => matchesArea(group, terms))
    : searching
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
              ? terms?.length
                ? `Todavía no hay carreras de «${query.trim()}» en el catálogo de Cochabamba.`
                : `Ninguna carrera compatible coincide con «${query.trim()}». Prueba con una sola palabra del nombre de la carrera.`
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
