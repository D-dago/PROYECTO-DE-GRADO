import { request } from './api'

export type Institution = { id: string; name: string; type: string; location?: string; municipality?: string; source?: string; isActive: boolean }
export type Career = { id: string; name: string; area?: string; level?: string; modality?: string; durationMonths?: number; cost?: number; institutionId: string; institutionName: string; compatibility: number; compatibilityReason: string }
export type Recommendation = { items: Career[]; historyId: string; aiMessage?: string }
export type StudentProfile = { userId: string; email: string; firstName: string; lastName: string; role: string; phone?: string; city?: string; school?: string; graduationYear?: number }

export const appApi = {
  register: (body: unknown) => request<{ accessToken: string }>('/auth/register', { method: 'POST', body: JSON.stringify(body) }),
  login: (body: unknown) => request<{ accessToken: string; role: string }>('/auth/login', { method: 'POST', body: JSON.stringify(body) }),
  getProfile: () => request<StudentProfile>('/student/profile'),
  updateProfile: (body: unknown) => request<StudentProfile>('/student/profile', { method: 'PUT', body: JSON.stringify(body) }),
  institutions: () => request<Institution[]>('/catalog/institutions'),
  careers: () => request<Career[]>('/catalog/careers'),
  recommendations: (body: unknown) => request<Recommendation>('/catalog/recommendations', { method: 'POST', body: JSON.stringify(body) }),
  history: () => request<{ id: string; testAttemptId: string; createdAtUtc: string; filterSummary?: string }[]>('/catalog/history'),
  createInstitution: (body: unknown) => request<Institution>('/admin/catalog/institutions', { method: 'POST', body: JSON.stringify(body) }),
  createCareer: (body: unknown) => request<Career>('/admin/catalog/careers', { method: 'POST', body: JSON.stringify(body) }),
  dashboard: () => request<{ students: number; institutions: number; careers: number; compatibilityRules: number }>('/admin/catalog/dashboard'),
}