import { useState } from 'react'
import type { FormEvent } from 'react'
import { appApi } from '../services/appApi'

export function AccessPage({ register = false }: { register?: boolean }) {
  const [error, setError] = useState<string | null>(null)
  const [done, setDone] = useState(false)
  async function submit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault(); setError(null)
    const data = Object.fromEntries(new FormData(event.currentTarget))
    try { const result = register ? await appApi.register(data) : await appApi.login(data); localStorage.setItem('accessToken', result.accessToken); setDone(true) } catch (accessError) { setError(accessError instanceof Error ? accessError.message : 'No se pudo completar la operación.') }
  }
  return <main className="form-shell"><span className="eyebrow">Orientación vocacional</span><h1>{register ? 'Crear cuenta' : 'Iniciar sesión'}</h1>{done ? <p className="success-message">Acceso correcto. Ya puedes continuar con tu perfil y test.</p> : <form onSubmit={submit} className="simple-form">{register && <><label>Nombre<input name="firstName" required /></label><label>Apellido<input name="lastName" required /></label></>}<label>Correo<input name="email" type="email" required /></label><label>Contraseña<input name="password" type="password" minLength={8} required /></label><button type="submit">{register ? 'Registrarme' : 'Entrar'}</button>{error && <p className="error-message">{error}</p>}</form>}</main>
}