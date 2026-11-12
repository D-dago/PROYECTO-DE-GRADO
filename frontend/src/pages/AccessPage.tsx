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
      {/* Los textos dejan claro que es una cuenta de ESTA pagina: hay quien cree que le piden entrar a su
          correo y escribe la contrasena de su Gmail. */}
      <span className="eyebrow">Orientación vocacional</span>
      <h1>{register ? 'Crea tu cuenta en Orientación Vocacional' : 'Entra a tu cuenta de Orientación Vocacional'}</h1>
      <p className="access-intro">
        {register
          ? 'Es gratis. Con esta cuenta haces el test vocacional, guardas tus resultados y ves dónde estudiar en Cochabamba.'
          : 'Usa el correo y la contraseña con los que te registraste en esta página.'}
      </p>
      <form onSubmit={submit} className="simple-form">
        {register && (
          <>
            <label>Tu nombre<input name="firstName" required onInput={lettersOnly} {...inputRules.name} /></label>
            <label>Tu apellido<input name="lastName" required onInput={lettersOnly} {...inputRules.name} /></label>
          </>
        )}
        <label>
          Tu correo electrónico
          <input name="email" type="email" required maxLength={254} autoComplete="email" placeholder="ejemplo@gmail.com" aria-describedby="email-hint" />
          <small id="email-hint">{register ? 'Será tu usuario para entrar a esta página. No entramos a tu correo ni te pedimos su contraseña.' : 'El mismo correo con el que creaste tu cuenta.'}</small>
        </label>
        <label>
          {register ? 'Crea una contraseña para esta página' : 'Tu contraseña de esta página'}
          <PasswordInput name="password" minLength={8} maxLength={64} {...(register ? { pattern: '(?=.*\\p{L})(?=.*\\d).{8,64}', title: 'De 8 a 64 caracteres, con al menos una letra y un número.' } : {})} required autoComplete={register ? 'new-password' : 'current-password'} aria-describedby="password-hint" />
          <small id="password-hint">{register ? 'Inventa una nueva: no uses la contraseña de tu correo. Mínimo 8 caracteres, con letras y números.' : 'La que inventaste al registrarte (no es la contraseña de tu correo).'}</small>
        </label>
        <button type="submit" disabled={busy} aria-busy={busy}>{busy ? (register ? 'Creando tu cuenta…' : 'Entrando…') : (register ? 'Crear mi cuenta' : 'Entrar')}</button>
        {busy && queueSeconds !== null && <p className="queue-message" role="status">Hay muchos estudiantes entrando a la vez. Estás en fila; seguimos intentando solos cada {queueSeconds} segundos. No cierres la página.</p>}
      </form>
      <div className="form-divider">{register ? 'o regístrate sin crear contraseña' : 'o entra sin contraseña'}</div>
      <GoogleButton onSuccess={finish} onError={fail} />
      {error && <p className="error-message">{error}</p>}
      <p className="access-switch">
        {register
          ? <>¿Ya tienes cuenta? <a href="/login">Inicia sesión</a></>
          : <>¿Todavía no tienes cuenta? <a href="/registro">Créala gratis</a></>}
      </p>
    </main>
  )
}
