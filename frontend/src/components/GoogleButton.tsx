import { useEffect, useRef } from 'react'
import { appApi } from '../services/appApi'

type GoogleId = {
  initialize: (config: { client_id: string; callback: (response: { credential: string }) => void }) => void
  renderButton: (element: HTMLElement, options: Record<string, unknown>) => void
}
declare global { interface Window { google?: { accounts: { id: GoogleId } } } }

const clientId = import.meta.env.VITE_GOOGLE_CLIENT_ID as string | undefined

function loadGoogleScript(): Promise<void> {
  return new Promise((resolve, reject) => {
    if (window.google) return resolve()
    const script = document.createElement('script')
    script.src = 'https://accounts.google.com/gsi/client'
    script.async = true
    script.onload = () => resolve()
    script.onerror = () => reject(new Error('No se pudo cargar Google.'))
    document.head.appendChild(script)
  })
}

export function GoogleButton({ onSuccess, onError }: { onSuccess: (accessToken: string) => void; onError: (message: string) => void }) {
  const slot = useRef<HTMLDivElement>(null)

  useEffect(() => {
    if (!clientId || !slot.current) return
    const element = slot.current
    loadGoogleScript().then(() => {
      window.google?.accounts.id.initialize({
        client_id: clientId,
        callback: (response) => {
          appApi.googleLogin(response.credential)
            .then((result) => onSuccess(result.accessToken))
            .catch((error) => onError(error instanceof Error ? error.message : 'No se pudo iniciar sesión con Google.'))
        },
      })
      window.google?.accounts.id.renderButton(element, { theme: 'outline', size: 'large', shape: 'pill', text: 'continue_with', width: 360 })
    }).catch((error) => onError(error instanceof Error ? error.message : 'No se pudo cargar Google.'))
  }, [onSuccess, onError])

  if (!clientId) {
    return (
      <button type="button" className="google-fallback" onClick={() => onError('Falta configurar el Client ID de Google (VITE_GOOGLE_CLIENT_ID en frontend/.env y Google__ClientId en el backend).')}>
        <svg width="18" height="18" viewBox="0 0 48 48" aria-hidden="true"><path fill="#EA4335" d="M24 9.5c3.5 0 6.6 1.2 9.1 3.6l6.8-6.8C35.8 2.4 30.3 0 24 0 14.6 0 6.5 5.4 2.6 13.3l7.9 6.1C12.4 13.6 17.7 9.5 24 9.5z" /><path fill="#4285F4" d="M46.5 24.5c0-1.6-.1-3.1-.4-4.5H24v9h12.7c-.6 3-2.3 5.5-4.8 7.2l7.5 5.8c4.4-4.1 7.1-10.1 7.1-17.5z" /><path fill="#FBBC05" d="M10.5 28.6A14.5 14.5 0 0 1 9.5 24c0-1.6.3-3.1.8-4.6l-7.9-6.1A24 24 0 0 0 0 24c0 3.9.9 7.5 2.6 10.7l7.9-6.1z" /><path fill="#34A853" d="M24 48c6.5 0 11.9-2.1 15.9-5.8l-7.5-5.8c-2.1 1.4-4.9 2.3-8.4 2.3-6.3 0-11.6-4.1-13.5-9.9l-7.9 6.1C6.5 42.6 14.6 48 24 48z" /></svg>
        Continuar con Google
      </button>
    )
  }
  return <div ref={slot} className="google-slot" />
}
