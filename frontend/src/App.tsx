import './App.css'
import { TestPage } from './pages/TestPage'
import { AccessPage } from './pages/AccessPage'
import { RecommendationsPage } from './pages/RecommendationsPage'
import { HistoryPage } from './pages/HistoryPage'
import { AdminPage } from './pages/AdminPage'
import { ProfilePage } from './pages/ProfilePage'

function App() {
  const path = window.location.pathname
  const token = localStorage.getItem('accessToken')
  const protectedPath = ['/perfil', '/test', '/recomendaciones', '/historial', '/admin'].includes(path)
  if (protectedPath && !token) return <main><h1>Inicia sesión para continuar</h1><a className="start-link" href="/login">Ir al login</a></main>
  if (path === '/admin' && token && getTokenRole(token) !== 'ADMINISTRADOR') return <main><h1>Acceso restringido</h1><p>Esta sección requiere el rol ADMINISTRADOR.</p></main>
  if (path === '/test') return <TestPage />
  if (window.location.pathname === '/login') return <AccessPage />
  if (window.location.pathname === '/registro') return <AccessPage register />
  if (window.location.pathname === '/recomendaciones') return <RecommendationsPage />
  if (window.location.pathname === '/historial') return <HistoryPage />
  if (window.location.pathname === '/perfil') return <ProfilePage />
  if (window.location.pathname === '/admin') return <AdminPage />

  return (
    <main>
      <h1>Orientacion vocacional</h1>
      <p>Base inicial del sistema web hibrido.</p>
      <a className="start-link" href="/test">Comenzar test RIASEC</a>
      <nav className="app-nav"><a href="/registro">Registro</a><a href="/login">Login</a><a href="/perfil">Perfil</a><a href="/recomendaciones">Recomendaciones</a><a href="/historial">Historial</a><a href="/admin">Administración</a>{token && <button type="button" onClick={() => { localStorage.removeItem('accessToken'); window.location.href = '/' }}>Cerrar sesión</button>}</nav>
    </main>
  )
}

function getTokenRole(token: string) {
  try { const payload = JSON.parse(atob(token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/'))); return (payload.role ?? payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']) as string } catch { return '' }
}

export default App