import type { Career } from './appApi'

// Busqueda y agrupacion de carreras del resultado (sin React), para el buscador y las areas afines.

export type InstitutionOption = { id: string; name: string; website: string | null }
// dimension: letra RIASEC que dio la afinidad; support: lo que aportan las demas reglas (desempate, ORION #34).
export type CareerGroup = { key: string; name: string; areas: string; compatibility: number; support: number; dimension?: string; reason: string; institutions: InstitutionOption[] }

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
    const group = groups.get(key) ?? { key, name: career.name, areas: '', compatibility: 0, support: 0, reason: career.compatibilityReason, institutions: [] }
    if (career.area && !group.areas.includes(careerKey(career.area))) group.areas += ` ${careerKey(career.area)}`
    const support = career.support ?? 0
    if (career.compatibility > group.compatibility || (career.compatibility === group.compatibility && support > group.support)) {
      group.compatibility = career.compatibility
      group.support = support
      group.dimension = career.matchedDimension ?? undefined
      group.reason = career.compatibilityReason
    }
    if (!group.institutions.some((item) => item.id === career.institutionId)) {
      group.institutions.push({ id: career.institutionId, name: career.institutionName, website: safeWebsite(career.institutionWebsite) })
    }
    groups.set(key, group)
  }
  const sorted = [...groups.values()]
    .map((group) => ({ ...group, institutions: [...group.institutions].sort((a, b) => a.name.localeCompare(b.name)) }))
    // A igual afinidad, primero la carrera cuyas otras reglas tambien coinciden con el perfil (ORION #34).
    .sort((a, b) => b.compatibility - a.compatibility || b.support - a.support || a.name.localeCompare(b.name))
  return spreadInstitutions(sorted)
}

/** La unica institucion de la tarjeta, o null si la ofrecen varias. */
function soleInstitution(group: CareerGroup) {
  return group.institutions.length === 1 ? group.institutions[0].id : null
}

/**
 * Entre tarjetas con la misma afinidad no pone seguidas dos de la misma unica institucion si hay otra opcion,
 * para que el estudiante no vea la misma universidad una y otra vez (ORION #34). Respeta el orden por apoyo
 * salvo para evitar esa repeticion, y nunca sube una carrera de menor afinidad.
 */
function spreadInstitutions(groups: CareerGroup[]) {
  const result: CareerGroup[] = []
  for (let start = 0; start < groups.length;) {
    let end = start
    while (end < groups.length && groups[end].compatibility === groups[start].compatibility) end++
    const tie = groups.slice(start, end)
    while (tie.length > 0) {
      const previous = result.length > 0 ? soleInstitution(result[result.length - 1]) : null
      // La institucion con mas tarjetas en el empate: si las demas ya no alcanzan para intercalarla
      // (quedan menos que sus tarjetas), toca ponerla ahora; si no, se sigue el orden por apoyo.
      const counts = new Map<string, number>()
      for (const group of tie) { const sole = soleInstitution(group); if (sole) counts.set(sole, (counts.get(sole) ?? 0) + 1) }
      const [crowded, crowdedCount] = [...counts.entries()].sort((a, b) => b[1] - a[1])[0] ?? [null, 0]
      const forced = crowded !== null && crowded !== previous && crowdedCount > tie.length - crowdedCount
        ? tie.findIndex((group) => soleInstitution(group) === crowded)
        : -1
      const next = forced >= 0 ? forced : tie.findIndex((group) => previous === null || soleInstitution(group) !== previous)
      result.push(tie.splice(next < 0 ? 0 : next, 1)[0])
    }
    start = end
  }
  return result
}

/**
 * Carreras afines al perfil: las que deben su afinidad a una de las dos letras principales del codigo
 * (p. ej. R o I en un perfil RI). Las demas se muestran aparte, como de menor afinidad (ORION #34).
 */
export function isProfileCareer(group: CareerGroup, profileLetters: string) {
  return group.dimension !== undefined && profileLetters.toUpperCase().includes(group.dimension)
}

/** Si dos textos se diferencian en a lo sumo una letra: cambiada, faltante o sobrante. */
function withinOneEdit(a: string, b: string) {
  if (Math.abs(a.length - b.length) > 1) return false
  let i = 0
  let j = 0
  let edits = 0
  while (i < a.length && j < b.length) {
    if (a[i] === b[j]) { i++; j++; continue }
    if (++edits > 1) return false
    if (a.length > b.length) i++
    else if (a.length < b.length) j++
    else { i++; j++ }
  }
  return edits + (a.length - i) + (b.length - j) <= 1
}

/**
 * Busqueda tolerante para el panel (ORION #41): la palabra puede venir pegada ("sansimon") o con una letra
 * cambiada, faltante o sobrante ("samsimon" encuentra "San Simón"; "veterinria", "Veterinaria"). Solo tolera
 * el error en palabras de 5 letras o mas, para que una sigla corta no encuentre cualquier cosa.
 */
export function looselyIncludes(text: string, word: string) {
  if (text.includes(word)) return true
  const compact = text.replace(/\s+/g, '')
  if (compact.includes(word)) return true
  if (word.length < 5) return false
  for (const size of [word.length - 1, word.length, word.length + 1]) {
    for (let start = 0; start + size <= compact.length; start++) {
      if (withinOneEdit(compact.slice(start, start + size), word)) return true
    }
  }
  return false
}
