import { downloadFile, request } from './api'

export type Institution = { id: string; name: string; type: string; location?: string; municipality?: string; status: string; source?: string; lastUpdatedUtc?: string; isActive: boolean }
export type Career = { id: string; name: string; area?: string; level?: string; modality?: string; durationMonths?: number; cost?: number; status: string; source?: string; lastUpdatedUtc?: string; isActive: boolean; institutionId: string; institutionName: string; compatibility: number; compatibilityReason: string }
export type DimensionInfo = { code: string; name: string; description: string; careerAreas: string[] }
export type ProfileInterpretation = { code: string; dimensions: DimensionInfo[]; summary: string; suggestedAreas: string[] }
export type Recommendation = { items: Career[]; historyId: string; aiMessage?: string; interpretation: ProfileInterpretation; aiGenerated: boolean }
export type StudentProfile = { userId: string; email: string; firstName: string; lastName: string; role: string; phone?: string; city?: string; school?: string; graduationYear?: number; coins: number; equippedSkin: string }
export type Skin = { id: string; name: string; description: string; price: number; owned: boolean; equipped: boolean }
export type SkinShop = { coins: number; equippedSkin: string; skins: Skin[] }
export type Compatibility = { id: string; careerId: string; careerName: string; dimension: string; compatibilityLevel: number; rule: string }
export type Student = { id: string; email: string; firstName: string; lastName: string; isActive: boolean; createdAtUtc: string }

export type AdminQuestion = { id: string; code: string; text: string; dimension: string; isActive: boolean; order: number }
export type AdminUser = { id: string; email: string; fullName: string; role: string; isActive: boolean; createdAtUtc: string; coins: number; completedTests: number }
export type AdminResult = { attemptId: string; studentName: string; email: string; completedAtUtc: string; primaryCode: string; secondaryCode: string; c: number; h: number; a: number; s: number; i: number; d: number; e: number }

export const appApi = {
  adminQuestions: () => request<AdminQuestion[]>('/admin/questions'),
  createQuestion: (body: unknown) => request<AdminQuestion>('/admin/questions', { method: 'POST', body: JSON.stringify(body) }),
  updateQuestion: (id: string, body: unknown) => request<AdminQuestion>(`/admin/questions/${id}`, { method: 'PUT', body: JSON.stringify(body) }),
  adminUsers: () => request<AdminUser[]>('/admin/users'),
  adminResults: () => request<AdminResult[]>('/admin/results'),
  downloadExcel: () => downloadFile('/admin/results/export/excel', 'resultados.xlsx'),
  downloadPdf: () => downloadFile('/admin/results/export/pdf', 'resultados.pdf'),
  register: (body: unknown) => request<{ accessToken: string }>('/auth/register', { method: 'POST', body: JSON.stringify(body) }),
  login: (body: unknown) => request<{ accessToken: string; role: string }>('/auth/login', { method: 'POST', body: JSON.stringify(body) }),
  skins: () => request<SkinShop>('/student/skins'),
  buySkin: (id: string) => request<SkinShop>(`/student/skins/${id}/buy`, { method: 'POST' }),
  equipSkin: (id: string) => request<SkinShop>(`/student/skins/${id}/equip`, { method: 'POST' }),
  googleLogin: (idToken: string) => request<{ accessToken: string; role: string }>('/auth/google', { method: 'POST', body: JSON.stringify({ idToken }) }),
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
