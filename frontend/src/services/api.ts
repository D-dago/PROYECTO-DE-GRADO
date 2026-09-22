// Publicada (npm run build), la API sirve la web: se llama a la misma direccion, "/api", y funciona
// igual en la PC, en el celular o por internet. En desarrollo (npm run dev) la web y la API estan en
// puertos distintos: se usa el host con el que se abrio la pagina (con "localhost" fijo el celular
// buscaria la API en si mismo y fallaria).
const apiBaseUrl = import.meta.env.VITE_API_BASE_URL ?? (import.meta.env.DEV ? `http://${window.location.hostname}:5194/api` : '/api')

export async function request<T>(path: string, options: RequestInit = {}): Promise<T> {
  const token = localStorage.getItem('accessToken')
  const response = await fetch(`${apiBaseUrl}${path}`, {
    ...options,
    headers: {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...options.headers,
    },
  }).catch(() => {
    throw new Error('No se pudo conectar con el servidor. Verifica que el sistema esté encendido (no necesita internet).')
  })

  if (!response.ok) {
    if (response.status === 401 && token) {
      // La sesion expiro o el token ya no es valido: se limpia y se manda a login
      // en vez de dejar que cada pantalla falle con un error generico.
      localStorage.removeItem('accessToken')
      if (window.location.pathname !== '/login') window.location.href = '/login'
    }
    const body = await response.json().catch(() => null) as { message?: string; errors?: Record<string, string[]> } | null
    // Los errores de validacion del servidor llegan como { errors: { Campo: [mensaje] } }.
    const validation = body?.errors ? Object.values(body.errors).flat()[0] : undefined
    throw new Error(body?.message ?? validation ?? (response.status === 413 ? 'El envío es demasiado grande.' : `Error HTTP ${response.status}`))
  }

  // Ademas de 204, cualquier respuesta exitosa con cuerpo vacio (un 200 sin
  // contenido, por ejemplo) rompe response.json(). Se lee como texto primero
  // para no depender de que cada endpoint use el status code exacto.
  const text = await response.text()
  return (text ? JSON.parse(text) : undefined) as T
}

// Descarga un archivo protegido: el token va en el header, asi que no sirve un simple <a href>.
export async function downloadFile(path: string, fallbackName: string) {
  const token = localStorage.getItem('accessToken')
  const response = await fetch(`${apiBaseUrl}${path}`, { headers: token ? { Authorization: `Bearer ${token}` } : {} })
  if (!response.ok) throw new Error(response.status === 403 ? 'Solo el administrador puede descargar esto.' : `No se pudo descargar (HTTP ${response.status}).`)
  const name = /filename="?([^";]+)"?/.exec(response.headers.get('Content-Disposition') ?? '')?.[1] ?? fallbackName
  const url = URL.createObjectURL(await response.blob())
  const link = document.createElement('a')
  link.href = url
  link.download = name
  link.click()
  URL.revokeObjectURL(url)
}

export const api = {
  baseUrl: apiBaseUrl,
}
