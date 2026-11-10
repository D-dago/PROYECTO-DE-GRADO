import { useEffect, useId, useRef, useState } from 'react'
import { appApi } from '../services/appApi'
import { clearToken, getToken, getTokenRole } from '../services/auth'
import { Avatar } from './Avatar'

const links = [
  { href: '/test', label: 'Test vocacional' },
  { href: '/recomendaciones', label: 'Dónde estudiar' },
  { href: '/historial', label: 'Historial' },
  { href: '/skins', label: 'Skins' },
  { href: '/perfil', label: 'Perfil' },
]

const adminLinks = [
  { href: '/admin#resultados', label: 'Resultados' },
  { href: '/admin#preguntas', label: 'Preguntas' },
  { href: '/admin#carreras', label: 'Carreras' },
  { href: '/admin#estudiantes', label: 'Estudiantes' },
]

export function Header() {
  const token = getToken()
  const role = token ? getTokenRole(token) : ''
  const isStudent = role === 'ESTUDIANTE'
  const path = window.location.pathname
  const [profile, setProfile] = useState<{ firstName: string; coins: number; equippedSkin: string } | null>(null)
  // El menu vive detras del boton de tres rayitas, en celular y en computadora: asi el encabezado
  // ocupa una sola linea y no se come media pantalla del celular.
  const [open, setOpen] = useState(false)
  const toggleRef = useRef<HTMLButtonElement>(null)
  const panelId = useId()

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

  useEffect(() => {
    if (!open) return
    // Esc cierra y devuelve el foco al boton, para no perderse con el teclado.
    const onKey = (event: KeyboardEvent) => {
      if (event.key === 'Escape') { setOpen(false); toggleRef.current?.focus() }
    }
    window.addEventListener('keydown', onKey)
    return () => window.removeEventListener('keydown', onKey)
  }, [open])

  function logout() {
    clearToken()
    window.location.href = '/'
  }

  const menuLinks = isStudent ? links : role === 'ADMINISTRADOR' ? adminLinks : []
  const name = isStudent ? (profile?.firstName ?? '') : 'Administrador'

  return (
    <header className="app-header">
      <a href="/" className="app-logo"><span className="app-logo-mark">OV</span> Orientación Vocacional</a>
      {token ? (
        <>
          <button
            ref={toggleRef}
            type="button"
            className={`menu-toggle${open ? ' is-open' : ''}`}
            aria-expanded={open}
            aria-controls={panelId}
            aria-label={open ? 'Cerrar menú' : 'Abrir menú'}
            onClick={() => setOpen((current) => !current)}
          >
            <span aria-hidden="true" /><span aria-hidden="true" /><span aria-hidden="true" />
          </button>
          {open && <div className="menu-backdrop" onClick={() => setOpen(false)} aria-hidden="true" />}
          <nav id={panelId} className={`menu-panel${open ? ' is-open' : ''}`} aria-label="Menú principal" hidden={!open}>
            <div className="menu-account">
              {isStudent && profile
                ? <a href="/skins" className="avatar-mini" title="Cambiar skin" onClick={() => setOpen(false)}><Avatar skin={profile.equippedSkin} size={30} /></a>
                : <span className="avatar-mini menu-account-initial" aria-hidden="true">{name.slice(0, 1) || '·'}</span>}
              <div className="menu-account-text">
                <strong>{name || 'Mi cuenta'}</strong>
                {isStudent && <span className="coin-badge" title="Monedas acumuladas">🪙 {profile?.coins ?? '···'}</span>}
              </div>
            </div>
            <ul className="menu-links">
              {menuLinks.map((link) => {
                const active = isStudent && path === link.href
                return (
                  <li key={link.href}>
                    <a href={link.href} className={active ? 'active' : ''} aria-current={active ? 'page' : undefined} onClick={() => setOpen(false)}>
                      {link.label}<span aria-hidden="true">›</span>
                    </a>
                  </li>
                )
              })}
            </ul>
            <button type="button" className="menu-logout" onClick={logout}>Salir</button>
          </nav>
        </>
      ) : (
        <div className="app-header-actions">
          <a className="header-link" href="/login">Iniciar sesión</a>
          <a className="header-cta" href="/registro">Registrarme</a>
        </div>
      )}
    </header>
  )
}
