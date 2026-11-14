export type Question = {
  id: string
  code: string
  text: string
  dimension: string
  order: number
  /** Ya tiene respuesta en este intento: al reanudar se sigue en la primera sin responder (ORION #35). */
  answered?: boolean
}

export type Attempt = {
  attemptId: string
  answered: number
  total: number
  progress: number
  status: 'IN_PROGRESS' | 'COMPLETED'
  coins: number
}

export type RiasecResult = {
  id: string
  testAttemptId: string
  rScore: number
  iScore: number
  aScore: number
  sScore: number
  eScore: number
  cScore: number
  primaryCode: string
  secondaryCode: string
  algorithmVersion: string
  /** Maximo por dimension del test de este resultado: 20 (24 preguntas), 50 (60 preguntas) o 10 (12 preguntas). */
  maxScore?: number
  /** Segundos que tomo responder el test, medidos por el servidor; null si el intento no lo registro. */
  durationSeconds?: number | null
}