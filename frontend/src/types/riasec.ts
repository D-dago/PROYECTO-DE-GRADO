export type Question = {
  id: string
  code: string
  text: string
  dimension: string
  order: number
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
  /** Maximo por dimension del test de este resultado: 20 (O*NET de 30 actividades o 24 preguntas) o 10 (12 preguntas). */
  maxScore?: number
}