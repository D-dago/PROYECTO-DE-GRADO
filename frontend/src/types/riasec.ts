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
  cScore: number
  hScore: number
  aScore: number
  sScore: number
  iScore: number
  dScore: number
  eScore: number
  primaryCode: string
  secondaryCode: string
  algorithmVersion: string
}