import { useEffect, useState } from 'react'
import type { FormEvent } from 'react'
import { appApi } from '../services/appApi'
import type { StudentProfile } from '../services/appApi'

export function ProfilePage() {
  const [profile, setProfile] = useState<Partial<StudentProfile>>({})
  const [success, setSuccess] = useState('')
  const [error, setError] = useState('')

  useEffect(() => { void appApi.getProfile().then(setProfile).catch((loadError) => setError(loadError instanceof Error ? loadError.message : 'No se pudo cargar el perfil.')) }, [])

  async function save(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    setError('')
    setSuccess('')
    const data = Object.fromEntries(new FormData(event.currentTarget))
    try {
      const updated = await appApi.updateProfile({ ...data, graduationYear: data.graduationYear ? Number(data.graduationYear) : null })
      setProfile(updated)
      setSuccess('Perfil actualizado.')
    } catch (saveError) { setError(saveError instanceof Error ? saveError.message : 'No se pudo actualizar el perfil.') }
  }

  return (
    <main className="form-shell">
      <span className="eyebrow">Perfil del estudiante</span>
      <h1>{profile.firstName ?? 'Mi perfil'}</h1>
      <form className="simple-form" onSubmit={save}>
        <label>Teléfono<input name="phone" defaultValue={String(profile.phone ?? '')} /></label>
        <label>Ciudad<input name="city" defaultValue={String(profile.city ?? '')} /></label>
        <label>Colegio<input name="school" defaultValue={String(profile.school ?? '')} /></label>
        <label>Año de graduación<input name="graduationYear" type="number" defaultValue={String(profile.graduationYear ?? '')} /></label>
        <button type="submit">Guardar cambios</button>
      </form>
      {success && <p className="success-message">{success}</p>}
      {error && <p className="error-message">{error}</p>}
    </main>
  )
}
