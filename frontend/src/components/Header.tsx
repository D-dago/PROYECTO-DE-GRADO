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
  const path = window.location.pathname
  const [profile, setProfile] = useState<{ firstName: string; coins: number } | null>(null)

  useEffect(() => {
    if (!token) { setProfile(null); return }
    void appApi.getProfile().then(setProfile).catch(() => setProfile(null))
  }, [token])

  function logout() {
    localStorage.removeItem('accessToken')
    window.location.href = '/'
  }

  return (
    <header className="app-header">
      <a href="/" className="app-logo"><span className="app-logo-mark">OV</span> Orientación Vocacional</a>
      {token && (
        <nav className="app-header-nav">
          {links.map((link) => <a key={link.path} href={link.path} className={path === link.path ? 'active' : ''}>{link.label}</a>)}
          {role === 'ADMINISTRADOR' && <a href="/admin" className={path === '/admin' ? 'active' : ''}>Administración</a>}
        </nav>
      )}
      <div className="app-header-actions">
        {token && <span className="coin-badge" title="Monedas acumuladas">🪙 {profile?.coins ?? '···'}</span>}
        {token ? (
          <>
            <span className="app-user">{profile?.firstName ?? ''}</span>
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
