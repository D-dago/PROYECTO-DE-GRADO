import type { Career } from './appApi'

// Busqueda y agrupacion de carreras del resultado (sin React), para el buscador y las areas afines.

export type InstitutionOption = { id: string; name: string; website: string | null }
export type CareerGroup = { key: string; name: string; areas: string; compatibility: number; reason: string; institutions: InstitutionOption[] }

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
export function careerKey(name: string) {
  return name.normalize('NFD').replace(/\p{M}/gu, '').trim().toLowerCase()
}

// Palabras que no ayudan a buscar ("Biología, química o física" -> biologia, quimica, fisica).
const STOPWORDS = new Set(['del', 'las', 'los', 'con', 'para', 'por', 'una', 'uno', 'que', 'sus'])
// Palabras tan generales que, solas, traerian medio catalogo: solo ordenan, salvo que sean lo unico buscado.
const GENERIC = new Set(['ingenieria', 'ingeniero', 'ingeniera', 'ciencia', 'ciencias', 'licenciatura', 'licenciado', 'licenciada', 'tecnico', 'tecnica', 'carrera', 'carreras', 'estudios', 'profesional'])

/**
 * Raiz de la palabra: sin las 2 ultimas letras si es larga. Asi "agronomia" encuentra "Agronómica",
 * "veterinario" encuentra "Veterinaria" y "contador" encuentra "Contaduría" (ORION #32). Las palabras
 * cortas quedan enteras para que "fisica" no traiga "Fisioterapia".
 */
function stem(word: string) {
  return word.length >= 7 ? word.slice(0, -2) : word
}

export function searchWords(query: string) {
  return careerKey(query).split(/[^a-z0-9]+/).filter((word) => word.length >= 3 && !STOPWORDS.has(word))
}

/**
 * Cuantas palabras buscadas aparecen en la carrera (nombre, area o institucion). 0 = no coincide.
 * Si se busco alguna palabra especifica, la carrera debe contener al menos una; las generales solo suman.
 */
export function matchScore(group: CareerGroup, words: string[]) {
  const text = `${group.key} ${group.areas} ${group.institutions.map((item) => careerKey(item.name)).join(' ')}`
  const has = (word: string) => text.includes(stem(word))
  const specific = words.filter((word) => !GENERIC.has(word))
  const required = specific.length > 0 ? specific : words
  if (!required.some(has)) return 0
  return words.filter(has).length
}

/**
 * Carreras de un area afin: su nombre contiene alguno de los terminos del area y ninguno de los que
 * empiezan con "-". Se mira solo el nombre: el area del catalogo es una categoria amplia ("Ingeniería").
 */
export function matchesArea(group: { key: string }, terms: string[]) {
  const include = terms.filter((term) => !term.startsWith('-'))
  const exclude = terms.filter((term) => term.startsWith('-')).map((term) => term.slice(1))
  return include.some((term) => group.key.includes(term)) && !exclude.some((term) => group.key.includes(term))
}

/** Cuantas carreras del catalogo tiene un area afin (0 = el area todavia no tiene carreras cargadas). */
export function countAreaCareers(careers: Career[], terms: string[] | undefined) {
  if (!terms?.length) return 0
  return groupByCareer(careers).filter((group) => matchesArea(group, terms)).length
}

/** Los terminos del area si la busqueda es exactamente el nombre de un area afin (tocada o escrita). */
export function areaTermsFor(query: string, areaTerms?: Record<string, string[]>) {
  if (!areaTerms) return undefined
  const key = careerKey(query)
  return Object.entries(areaTerms).find(([area]) => careerKey(area) === key)?.[1]
}

/** Una tarjeta por carrera, con todas las instituciones de Cochabamba que la ofrecen. */
export function groupByCareer(careers: Career[]): CareerGroup[] {
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
