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
}