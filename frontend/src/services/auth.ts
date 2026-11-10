// La sesion vive en sessionStorage, no en localStorage: el navegador la borra al cerrar la pestana o
// la ventana, asi que al volver a entrar se ve la portada con "Iniciar sesion" / "Registrarme" en vez
// de entrar directo. Recargar o navegar dentro del sistema la conserva.
// Algunos navegadores (sobre todo en el celular) restauran las pestanas con su sessionStorage: por eso
// ademas se guarda la hora del ultimo uso y, si pasaron mas de 30 minutos, la sesion se descarta al cargar.
const TOKEN_KEY = 'accessToken'
const LAST_USE_KEY = 'accessTokenLastUse'
const MAX_IDLE_MS = 30 * 60 * 1000

function storage(): Storage | null {
  try { return window.sessionStorage } catch { return null }
}

/** Marca que la sesion se esta usando (cada pagina abierta y cada llamada al servidor). */
function touch(store: Storage) {
  try { store.setItem(LAST_USE_KEY, String(Date.now())) } catch { /* sin almacenamiento */ }
}

export function getToken(): string | null {
  const store = storage()
  if (!store) return null
  try {
    const token = store.getItem(TOKEN_KEY)
    if (!token) return null
    touch(store)
    return token
  } catch {
    return null
  }
}

export function saveToken(token: string) {
  const store = storage()
  if (!store) return
  try { store.setItem(TOKEN_KEY, token); touch(store) } catch { /* sin almacenamiento */ }
}

export function clearToken() {
  const store = storage()
  try { store?.removeItem(TOKEN_KEY); store?.removeItem(LAST_USE_KEY) } catch { /* sin almacenamiento */ }
}

/**
 * Se llama una vez al cargar la pagina, antes de dibujar nada: borra la sesion vieja que versiones
 * anteriores dejaban en localStorage para siempre y la que lleva demasiado tiempo sin usarse.
 */
export function startSession() {
  try { window.localStorage.removeItem(TOKEN_KEY) } catch { /* sin almacenamiento */ }
  const store = storage()
  if (!store) return
  try {
    const lastUse = Number(store.getItem(LAST_USE_KEY) ?? 0)
    if (store.getItem(TOKEN_KEY) && Date.now() - lastUse > MAX_IDLE_MS) clearToken()
  } catch { /* sin almacenamiento */ }
}

export function getTokenRole(token: string): string {
  try {
    const payload = JSON.parse(atob(token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/'))) as Record<string, unknown>
    return (payload.role ?? payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']) as string
  } catch {
    return ''
  }
}
