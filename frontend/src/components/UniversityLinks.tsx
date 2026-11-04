import { useState } from 'react'
import type { Career } from '../services/appApi'
import { SiteWindow } from './SiteWindow'

const CARRERAS_POR_INSTITUCION = 4

type InstitutionGroup = { id: string; name: string; website: string | null; careers: Career[] }

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

/** Agrupa las carreras por institucion y pone primero la institucion con la carrera mas compatible. */
function groupByInstitution(careers: Career[]): InstitutionGroup[] {
  const groups = new Map<string, InstitutionGroup>()
  for (const career of careers) {
    // 0 % significa que la carrera no tiene regla de compatibilidad: no es una recomendacion.
    if (career.compatibility <= 0) continue
    const group = groups.get(career.institutionId)
    if (group) group.careers.push(career)
    else groups.set(career.institutionId, { id: career.institutionId, name: career.institutionName, website: safeWebsite(career.institutionWebsite), careers: [career] })
  }
  const best = (group: InstitutionGroup) => Math.max(...group.careers.map((career) => career.compatibility))
  return [...groups.values()]
    .map((group) => ({ ...group, careers: [...group.careers].sort((a, b) => b.compatibility - a.compatibility) }))
    .sort((a, b) => best(b) - best(a))
}

export function UniversityLinks({ careers }: { careers: Career[] }) {
  const [open, setOpen] = useState<(InstitutionGroup & { website: string }) | null>(null)
  const groups = groupByInstitution(careers)

  if (groups.length === 0) {
    return <p className="result-note">Todavía no hay instituciones con carreras compatibles con tu perfil. Cuando el administrador las cargue aparecerán aquí.</p>
  }

  return (
    <>
      <p className="result-note university-note">Consulta costos, ubicación y requisitos actualizados en la página oficial de cada institución.</p>
      <div className="university-grid">
        {groups.map((group) => (
          <article className="university-card" key={group.id}>
            <h3>{group.name}</h3>
            <ul>
              {group.careers.slice(0, CARRERAS_POR_INSTITUCION).map((career) => (
                <li key={career.id} title={career.compatibilityReason}><span>{career.name}</span><strong>{career.compatibility}%</strong></li>
              ))}
            </ul>
            {group.website
              ? <button type="button" onClick={() => setOpen({ ...group, website: group.website! })}>Ver sitio oficial</button>
              : <small>Enlace oficial pendiente de confirmar</small>}
          </article>
        ))}
      </div>
      {open && <SiteWindow key={open.id} institutionId={open.id} name={open.name} website={open.website} onClose={() => setOpen(null)} />}
    </>
  )
}
