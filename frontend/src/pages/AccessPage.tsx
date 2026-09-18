import { useCallback, useState } from 'react'
import type { FormEvent } from 'react'
import { appApi } from '../services/appApi'
import { PasswordInput } from '../components/PasswordInput'
import { GoogleButton } from '../components/GoogleButton'

export function AccessPage({ register = false }: { register?: boolean }) {
  const [error, setError] = useState<string | null>(null)
  const [done, setDone] = useState(false)

  const finish = useCallback((accessToken: string) => {
    localStorage.setItem('accessToken', accessToken)
    setDone(true)
  }, [])
  const fail = useCallback((message: string) => setError(message), [])

  async function submit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    setError(null)
    const data = Object.fromEntries(new FormData(event.currentTarget))
    try {
      const result = register ? await appApi.register(data) : await appApi.login(data)
      finish(result.accessToken)
    } catch (accessError) {
      setError(accessError instanceof Error ? accessError.message : 'No se pudo completar la operación.')
    }
  }

  return (
    <main className="form-shell">
      <span className="eyebrow">Orientación vocacional</span>
      <h1>{register ? 'Crear cuenta' : 'Iniciar sesión'}</h1>
      {done ? (
        <>
          <p className="success-message">Acceso correcto. Ya puedes continuar con tu perfil y test.</p>
          <a className="start-link" href="/test">Ir al test RIASEC</a>
        </>
      ) : (
        <>
          <form onSubmit={submit} className="simple-form">
            {register && (
              <>
                <label>Nombre<input name="firstName" required /></label>
                <label>Apellido<input name="lastName" required /></label>
              </>
            )}
            <label>Correo<input name="email" type="email" required /></label>
            <label>Contraseña<PasswordInput name="password" minLength={8} required autoComplete={register ? 'new-password' : 'current-password'} /></label>
            <button type="submit">{register ? 'Registrarme' : 'Entrar'}</button>
          </form>
          <div className="form-divider">o</div>
          <GoogleButton onSuccess={finish} onError={fail} />
          {error && <p className="error-message">{error}</p>}
        </>
      )}
    </main>
  )
}
