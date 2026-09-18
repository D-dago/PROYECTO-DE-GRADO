import { useEffect, useState } from 'react'
import { appApi } from '../services/appApi'
import { getTokenRole } from '../services/auth'

const links = [
  { path: '/test', label: 'Test RIASEC' },
  { path: '/recomendaciones', label: 'Recomendaciones' },
  { path: '/historial', label: 'Historial' },
  { path: '/perfil', label: 'Perfil' },
]

export function Header() {
  const token = localStorage.getItem('accessToken')
  const role = token ? getTokenRole(token) : ''
  const isStudent = role === 'ESTUDIANTE'
  const path = window.location.pathname
  const [profile, setProfile] = useState<{ firstName: string; coins: number } | null>(null)

  useEffect(() => {
    // El perfil (y las monedas) solo existen para estudiantes: el backend
    // rechaza /student/profile con 403 para un administrador, asi que ni
    // se intenta pedir en ese caso.
    if (!token || !isStudent) { setProfile(null); return }
    void appApi.getProfile().then(setProfile).catch(() => setProfile(null))
  }, [token, isStudent])

  function logout() {
    localStorage.removeItem('accessToken')
    window.location.href = '/'
  }

  return (
    <header className="app-header">
      <a href="/" className="app-logo"><span className="app-logo-mark">OV</span> Orientación Vocacional</a>
      {token && (
        <nav className="app-header-nav">
          {isStudent && links.map((link) => <a key={link.path} href={link.path} className={path === link.path ? 'active' : ''}>{link.label}</a>)}
          {role === 'ADMINISTRADOR' && <a href="/admin" className={path === '/admin' ? 'active' : ''}>Administración</a>}
        </nav>
      )}
      <div className="app-header-actions">
        {token && isStudent && <span className="coin-badge" title="Monedas acumuladas">🪙 {profile?.coins ?? '···'}</span>}
        {token ? (
          <>
            <span className="app-user">{isStudent ? (profile?.firstName ?? '') : 'Administrador'}</span>
            <button type="button" className="header-logout" onClick={logout}>Salir</button>
          </>
        ) : (
          <>
            <a className="header-link" href="/login">Iniciar sesión</a>
            <a className="header-cta" href="/registro">Registrarme</a>
          </>
        )}
      </div>
    </header>
  )
}
