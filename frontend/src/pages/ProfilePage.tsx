import { useEffect, useState } from 'react'
import type { FormEvent } from 'react'
import { appApi } from '../services/appApi'
import type { StudentProfile } from '../services/appApi'

export function ProfilePage() {
  const [profile, setProfile] = useState<Partial<StudentProfile>>({})
  const [message, setMessage] = useState('')
  useEffect(() => { void appApi.getProfile().then(setProfile).catch((error) => setMessage(error instanceof Error ? error.message : 'No se pudo cargar el perfil.')) }, [])
  async function save(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    const data = Object.fromEntries(new FormData(event.currentTarget))
    try {
      const updated = await appApi.updateProfile({ ...data, graduationYear: data.graduationYear ? Number(data.graduationYear) : null })
      setProfile(updated)
      setMessage('Perfil actualizado.')
    } catch (error) { setMessage(error instanceof Error ? error.message : 'No se pudo actualizar el perfil.') }
  }
  return <main className="form-shell"><span className="eyebrow">Perfil del estudiante</span><h1>{profile.firstName ?? 'Mi perfil'}</h1><form className="simple-form" onSubmit={save}><label>Teléfono<input name="phone" defaultValue={String(profile.phone ?? '')} /></label><label>Ciudad<input name="city" defaultValue={String(profile.city ?? '')} /></label><label>Colegio<input name="school" defaultValue={String(profile.school ?? '')} /></label><label>Año de graduación<input name="graduationYear" type="number" defaultValue={String(profile.graduationYear ?? '')} /></label><button type="submit">Guardar cambios</button></form>{message && <p className="success-message">{message}</p>}</main>
}