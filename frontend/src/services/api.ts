const apiBaseUrl = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5194/api'

export async function request<T>(path: string, options: RequestInit = {}): Promise<T> {
  const token = localStorage.getItem('accessToken')
  const response = await fetch(`${apiBaseUrl}${path}`, {
    ...options,
    headers: {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...options.headers,
    },
  })

  if (!response.ok) {
    if (response.status === 401 && token) {
      // La sesion expiro o el token ya no es valido: se limpia y se manda a login
      // en vez de dejar que cada pantalla falle con un error generico.
      localStorage.removeItem('accessToken')
      if (window.location.pathname !== '/login') window.location.href = '/login'
    }
    const body = await response.json().catch(() => null) as { message?: string } | null
    throw new Error(body?.message ?? `Error HTTP ${response.status}`)
  }

  if (response.status === 204) return undefined as T
  return response.json() as Promise<T>
}

export const api = {
  baseUrl: apiBaseUrl,
}
