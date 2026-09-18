import { request } from './api'

export type Institution = { id: string; name: string; type: string; location?: string; municipality?: string; status: string; source?: string; lastUpdatedUtc?: string; isActive: boolean }
export type Career = { id: string; name: string; area?: string; level?: string; modality?: string; durationMonths?: number; cost?: number; status: string; source?: string; lastUpdatedUtc?: string; isActive: boolean; institutionId: string; institutionName: string; compatibility: number; compatibilityReason: string }
export type Recommendation = { items: Career[]; historyId: string; aiMessage?: string }
export type StudentProfile = { userId: string; email: string; firstName: string; lastName: string; role: string; phone?: string; city?: string; school?: string; graduationYear?: number }
export type Compatibility = { id: string; careerId: string; careerName: string; dimension: string; compatibilityLevel: number; rule: string }
export type Student = { id: string; email: string; firstName: string; lastName: string; isActive: boolean; createdAtUtc: string }

export const appApi = {
  register: (body: unknown) => request<{ accessToken: string }>('/auth/register', { method: 'POST', body: JSON.stringify(body) }),
  login: (body: unknown) => request<{ accessToken: string; role: string }>('/auth/login', { method: 'POST', body: JSON.stringify(body) }),
  getProfile: () => request<StudentProfile>('/student/profile'),
  updateProfile: (body: unknown) => request<StudentProfile>('/student/profile', { method: 'PUT', body: JSON.stringify(body) }),
  institutions: () => request<Institution[]>('/catalog/institutions'),
  careers: () => request<Career[]>('/catalog/careers'),
  recommendations: (body: unknown) => request<Recommendation>('/catalog/recommendations', { method: 'POST', body: JSON.stringify(body) }),
  history: () => request<{ id: string; testAttemptId: string; createdAtUtc: string; filterSummary?: string }[]>('/catalog/history'),

  dashboard: () => request<{ students: number; institutions: number; careers: number; compatibilityRules: number }>('/admin/catalog/dashboard'),

  createInstitution: (body: unknown) => request<Institution>('/admin/catalog/institutions', { method: 'POST', body: JSON.stringify(body) }),
  updateInstitution: (id: string, body: unknown) => request<Institution>(`/admin/catalog/institutions/${id}`, { method: 'PUT', body: JSON.stringify(body) }),
  setInstitutionActive: (id: string, active: boolean) => request<void>(`/admin/catalog/institutions/${id}/active?active=${active}`, { method: 'PATCH' }),

  adminCareers: () => request<Career[]>('/admin/catalog/careers'),
  createCareer: (body: unknown) => request<Career>('/admin/catalog/careers', { method: 'POST', body: JSON.stringify(body) }),
  updateCareer: (id: string, body: unknown) => request<Career>(`/admin/catalog/careers/${id}`, { method: 'PUT', body: JSON.stringify(body) }),
  setCareerActive: (id: string, active: boolean) => request<void>(`/admin/catalog/careers/${id}/active?active=${active}`, { method: 'PATCH' }),

  compatibilities: () => request<Compatibility[]>('/admin/catalog/compatibilities'),
  addCompatibility: (body: unknown) => request<void>('/admin/catalog/compatibilities', { method: 'POST', body: JSON.stringify(body) }),
  updateCompatibility: (id: string, body: unknown) => request<void>(`/admin/catalog/compatibilities/${id}`, { method: 'PUT', body: JSON.stringify(body) }),
  deleteCompatibility: (id: string) => request<void>(`/admin/catalog/compatibilities/${id}`, { method: 'DELETE' }),

  students: () => request<Student[]>('/admin/catalog/students'),
  setStudentActive: (id: string, active: boolean) => request<void>(`/admin/catalog/students/${id}/active?active=${active}`, { method: 'PATCH' }),
}
