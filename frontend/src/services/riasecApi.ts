import type { Attempt, Question, RiasecResult } from '../types/riasec'
import { request } from './api'

export const riasecApi = {
  getQuestions: () => request<Question[]>('/test/questions'),
  createAttempt: () => request<Attempt>('/test/attempts', { method: 'POST' }),
  submitAnswer: (attemptId: string, questionId: string, value: number) =>
    request<Attempt>(`/test/attempts/${attemptId}/answers`, {
      method: 'POST',
      body: JSON.stringify({ questionId, value }),
    }),
  complete: (attemptId: string) =>
    request<RiasecResult>(`/test/attempts/${attemptId}/complete`, { method: 'POST' }),
  getResult: (attemptId: string) => request<RiasecResult>(`/test/attempts/${attemptId}/result`),
}