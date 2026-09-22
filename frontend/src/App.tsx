import './App.css'
import { Header } from './components/Header'
import { TestPage } from './pages/TestPage'
import { AccessPage } from './pages/AccessPage'
import { RecommendationsPage } from './pages/RecommendationsPage'
import { HistoryPage } from './pages/HistoryPage'
import { AdminPage } from './pages/AdminPage'
import { ProfilePage } from './pages/ProfilePage'
import { SkinsPage } from './pages/SkinsPage'
import { getTokenRole } from './services/auth'

function App() {
  const path = window.location.pathname
  const token = localStorage.getItem('accessToken')
  const protectedPath = ['/perfil', '/test', '/recomendaciones', '/historial', '/skins', '/admin'].includes(path)

  return (
    <>
      <Header />
      {renderPage()}
    </>
  )

  function renderPage() {
    if (protectedPath && !token) return <main className="content-shell"><h1>Inicia sesión para continuar</h1><a className="start-link" href="/login">Ir al login</a></main>
    if (path === '/admin' && token && getTokenRole(token) !== 'ADMINISTRADOR') return <main className="content-shell"><h1>Acceso restringido</h1><p>Esta sección requiere el rol ADMINISTRADOR.</p></main>
    if (path === '/test') return <TestPage />
    if (path === '/login') return <AccessPage />
    if (path === '/registro') return <AccessPage register />
    if (path === '/recomendaciones') return <RecommendationsPage />
    if (path === '/historial') return <HistoryPage />
    if (path === '/perfil') return <ProfilePage />
    if (path === '/skins') return <SkinsPage />
    if (path === '/admin') return <AdminPage />

    return (
      <main className="hero-shell">
        <span className="eyebrow">Sistema híbrido de orientación vocacional</span>
        <h1>Descubre qué carrera se ajusta a ti.</h1>
        <p className="hero-subtitle">Test vocacional CHASIDE, recomendaciones por motor de reglas e interpretación con IA, pensado para bachilleres de Cochabamba.</p>
        <a className="start-link" href={token ? '/test' : '/registro'}>{token ? 'Ir al test vocacional' : 'Empezar ahora'}</a>
      </main>
    )
  }
}

export default App
