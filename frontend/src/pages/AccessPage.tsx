import { useCallback, useEffect, useState } from 'react'
import type { FormEvent } from 'react'
import { appApi } from '../services/appApi'
import { PasswordInput } from '../components/PasswordInput'
import { GoogleButton } from '../components/GoogleButton'
import { inputRules, lettersOnly } from '../services/inputFilters'
import { getTokenRole, saveToken } from '../services/auth'

export function AccessPage({ register = false }: { register?: boolean }) {
  const [error, setError] = useState<string | null>(null)
  // Mientras espera, el boton queda desactivado: cada clic extra seria otra contrasena que verificar
  // y alargaria la fila de todo el salon.
  const [busy, setBusy] = useState(false)
  const [queueSeconds, setQueueSeconds] = useState<number | null>(null)

  useEffect(() => {
    const onQueue = (event: Event) => setQueueSeconds((event as CustomEvent<number>).detail)
    window.addEventListener('auth-queue', onQueue)
    return () => window.removeEventListener('auth-queue', onQueue)
  }, [])

  // Se carga una pagina nueva para que el encabezado muestre la sesion: quedarse aqui con un mensaje dejaba
  // "Iniciar sesion" a la vista y parecia que el acceso habia fallado. El estudiante va a la portada y no al
  // test, porque abrir el test crea un intento y arranca el cronometro aunque solo quisiera ver su historial.
  const finish = useCallback((accessToken: string) => {
    saveToken(accessToken)
    window.location.href = getTokenRole(accessToken) === 'ADMINISTRADOR' ? '/admin' : '/'
  }, [])
  const fail = useCallback((message: string) => setError(message), [])

  async function submit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    if (busy) return
    setError(null)
    setBusy(true)
    const data = Object.fromEntries(new FormData(event.currentTarget))
    try {
      const result = register ? await appApi.register(data) : await appApi.login(data)
      finish(result.accessToken)
    } catch (accessError) {
      setError(accessError instanceof Error ? accessError.message : 'No se pudo completar la operación.')
      setBusy(false)
    } finally {
      setQueueSeconds(null)
    }
  }

  return (
    <main className="form-shell">
      <span className="eyebrow">Orientación vocacional</span>
      <h1>{register ? 'Crear cuenta' : 'Iniciar sesión'}</h1>
      <form onSubmit={submit} className="simple-form">
        {register && (
          <>
            <label>Nombre<input name="firstName" required onInput={lettersOnly} {...inputRules.name} /></label>
            <label>Apellido<input name="lastName" required onInput={lettersOnly} {...inputRules.name} /></label>
          </>
        )}
        <label>Correo<input name="email" type="email" required maxLength={254} autoComplete="email" /></label>
        <label>Contraseña<PasswordInput name="password" minLength={8} maxLength={64} {...(register ? { pattern: '(?=.*\\p{L})(?=.*\\d).{8,64}', title: 'De 8 a 64 caracteres, con al menos una letra y un número.' } : {})} required autoComplete={register ? 'new-password' : 'current-password'} /></label>
        <button type="submit" disabled={busy} aria-busy={busy}>{busy ? (register ? 'Creando tu cuenta…' : 'Entrando…') : (register ? 'Registrarme' : 'Entrar')}</button>
        {busy && queueSeconds !== null && <p className="queue-message" role="status">Hay muchos estudiantes entrando a la vez. Estás en fila; seguimos intentando solos cada {queueSeconds} segundos. No cierres la página.</p>}
      </form>
      <div className="form-divider">o</div>
      <GoogleButton onSuccess={finish} onError={fail} />
      {error && <p className="error-message">{error}</p>}
    </main>
  )
}
