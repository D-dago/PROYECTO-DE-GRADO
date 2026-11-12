import { useEffect, useState } from 'react'
import { appApi } from '../services/appApi'

type FirebaseConfig = { apiKey: string; authDomain: string; projectId: string; appId: string }

// Si el navegador bloquea la ventana emergente se usa la redireccion: se marca aqui para que, al volver
// de Google, el boton termine el inicio de sesion (y para no descargar Firebase en cada visita).
const REDIRECT_FLAG = 'google-redirect'

/**
 * "Continuar con Google" con Firebase Authentication (plan gratuito Spark). Firebase se descarga solo al
 * pulsar el boton, para que la pagina no pese mas. El servidor comprueba el token de Firebase y entrega
 * la sesion del sistema.
 */
async function firebaseAuth() {
  const config = await appApi.firebaseConfig()
  const [{ initializeApp, getApps }, auth] = await Promise.all([import('firebase/app'), import('firebase/auth')])
  const app = getApps()[0] ?? initializeApp(config satisfies FirebaseConfig)
  const instance = auth.getAuth(app)
  instance.languageCode = 'es'
  return { auth, instance }
}

function explain(error: unknown): string | null {
  const code = (error as { code?: string } | null)?.code ?? ''
  if (code === 'auth/popup-closed-by-user' || code === 'auth/cancelled-popup-request') return null // el estudiante cerro la ventana
  if (code === 'auth/unauthorized-domain') return 'Esta dirección no está autorizada en Firebase. El administrador debe agregarla en Firebase > Authentication > Configuración > Dominios autorizados.'
  if (code === 'auth/operation-not-allowed') return 'El inicio con Google no está activado en Firebase (Authentication > Método de acceso > Google).'
  if (code === 'auth/network-request-failed') return 'No se pudo conectar con Google. Revisa tu conexión e inténtalo de nuevo.'
  if (error instanceof Error && !code) return error.message
  return 'No se pudo iniciar sesión con Google. Inténtalo de nuevo.'
}

export function GoogleButton({ onSuccess, onError }: { onSuccess: (accessToken: string) => void; onError: (message: string) => void }) {
  const [busy, setBusy] = useState(false)

  async function finish(idToken: string) {
    const result = await appApi.googleLogin(idToken)
    onSuccess(result.accessToken)
  }

  // Vuelta de Google tras una redireccion (cuando el navegador bloqueo la ventana emergente).
  useEffect(() => {
    let pending = false
    try { pending = sessionStorage.getItem(REDIRECT_FLAG) === '1'; sessionStorage.removeItem(REDIRECT_FLAG) } catch { /* sin almacenamiento */ }
    if (!pending) return
    setBusy(true)
    firebaseAuth()
      .then(async ({ auth, instance }) => {
        const result = await auth.getRedirectResult(instance)
        if (result) await finish(await result.user.getIdToken())
      })
      .catch((error) => { const message = explain(error); if (message) onError(message) })
      .finally(() => setBusy(false))
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])

  async function start() {
    if (busy) return
    setBusy(true)
    try {
      const { auth, instance } = await firebaseAuth()
      const provider = new auth.GoogleAuthProvider()
      provider.setCustomParameters({ prompt: 'select_account' })
      try {
        const result = await auth.signInWithPopup(instance, provider)
        await finish(await result.user.getIdToken())
      } catch (error) {
        if ((error as { code?: string }).code !== 'auth/popup-blocked') throw error
        try { sessionStorage.setItem(REDIRECT_FLAG, '1') } catch { /* sin almacenamiento */ }
        await auth.signInWithRedirect(instance, provider)
      }
    } catch (error) {
      const message = explain(error)
      if (message) onError(message)
    } finally {
      setBusy(false)
    }
  }

  return (
    <button type="button" className="google-fallback" onClick={() => void start()} disabled={busy} aria-busy={busy}>
      <svg width="18" height="18" viewBox="0 0 48 48" aria-hidden="true"><path fill="#EA4335" d="M24 9.5c3.5 0 6.6 1.2 9.1 3.6l6.8-6.8C35.8 2.4 30.3 0 24 0 14.6 0 6.5 5.4 2.6 13.3l7.9 6.1C12.4 13.6 17.7 9.5 24 9.5z" /><path fill="#4285F4" d="M46.5 24.5c0-1.6-.1-3.1-.4-4.5H24v9h12.7c-.6 3-2.3 5.5-4.8 7.2l7.5 5.8c4.4-4.1 7.1-10.1 7.1-17.5z" /><path fill="#FBBC05" d="M10.5 28.6A14.5 14.5 0 0 1 9.5 24c0-1.6.3-3.1.8-4.6l-7.9-6.1A24 24 0 0 0 0 24c0 3.9.9 7.5 2.6 10.7l7.9-6.1z" /><path fill="#34A853" d="M24 48c6.5 0 11.9-2.1 15.9-5.8l-7.5-5.8c-2.1 1.4-4.9 2.3-8.4 2.3-6.3 0-11.6-4.1-13.5-9.9l-7.9 6.1C6.5 42.6 14.6 48 24 48z" /></svg>
      {busy ? 'Conectando con Google…' : 'Continuar con Google'}
    </button>
  )
}
