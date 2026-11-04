import { useEffect, useState } from 'react'
import { appApi } from '../services/appApi'
import { getTokenRole } from '../services/auth'
import { Avatar } from './Avatar'

const links = [
  { path: '/test', label: 'Test vocacional' },
  { path: '/recomendaciones', label: 'Dónde estudiar' },
  { path: '/historial', label: 'Historial' },
  { path: '/skins', label: 'Skins' },
  { path: '/perfil', label: 'Perfil' },
]

const adminLinks = [
  { href: '/admin#resultados', label: 'Resultados' },
  { href: '/admin#preguntas', label: 'Preguntas' },
  { href: '/admin#carreras', label: 'Carreras' },
  { href: '/admin#estudiantes', label: 'Estudiantes' },
]

export function Header() {
  const token = localStorage.getItem('accessToken')
  const role = token ? getTokenRole(token) : ''
  const isStudent = role === 'ESTUDIANTE'
  const path = window.location.pathname
  const [profile, setProfile] = useState<{ firstName: string; coins: number; equippedSkin: string } | null>(null)

  useEffect(() => {
    // El perfil (monedas, skin) solo existe para estudiantes: el backend rechaza
    // /student/profile con 403 para un administrador, asi que ni se pide.
    if (!token || !isStudent) return
    const load = () => { void appApi.getProfile().then(setProfile).catch(() => setProfile(null)) }
    load()
    // Las pantallas avisan (p. ej. al comprar una skin) para refrescar monedas y avatar.
    window.addEventListener('profile-changed', load)
    return () => window.removeEventListener('profile-changed', load)
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
          {role === 'ADMINISTRADOR' && adminLinks.map((link) => <a key={link.href} href={link.href}>{link.label}</a>)}
        </nav>
      )}
      <div className="app-header-actions">
        {token && isStudent && <span className="coin-badge" title="Monedas acumuladas">🪙 {profile?.coins ?? '···'}</span>}
        {token ? (
          <>
            {isStudent && profile && <a href="/skins" className="avatar-mini" title="Cambiar skin"><Avatar skin={profile.equippedSkin} size={30} /></a>}
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
