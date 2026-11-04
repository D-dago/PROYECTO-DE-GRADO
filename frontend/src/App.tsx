import { lazy, Suspense } from 'react'
import './App.css'
import { Header } from './components/Header'
import { FallingPetals } from './components/FallingPetals'
import { getTokenRole } from './services/auth'

// Cada pagina se descarga solo cuando se visita: un estudiante en el celular no paga por el panel de administrador.
const TestPage = lazy(() => import('./pages/TestPage').then((module) => ({ default: module.TestPage })))
const AccessPage = lazy(() => import('./pages/AccessPage').then((module) => ({ default: module.AccessPage })))
const RecommendationsPage = lazy(() => import('./pages/RecommendationsPage').then((module) => ({ default: module.RecommendationsPage })))
const HistoryPage = lazy(() => import('./pages/HistoryPage').then((module) => ({ default: module.HistoryPage })))
const AdminPage = lazy(() => import('./pages/AdminPage').then((module) => ({ default: module.AdminPage })))
const ProfilePage = lazy(() => import('./pages/ProfilePage').then((module) => ({ default: module.ProfilePage })))
const SkinsPage = lazy(() => import('./pages/SkinsPage').then((module) => ({ default: module.SkinsPage })))

function App() {
  const path = window.location.pathname
  const token = localStorage.getItem('accessToken')
  const protectedPath = ['/perfil', '/test', '/recomendaciones', '/historial', '/skins', '/admin'].includes(path)

  return (
    <>
      <FallingPetals />
      <Header />
      <Suspense fallback={<main className="content-shell"><p className="status-message">Cargando...</p></main>}>
        {renderPage()}
      </Suspense>
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
        <p className="hero-subtitle">Test vocacional RIASEC, recomendaciones por motor de reglas e interpretación con IA, pensado para bachilleres de Cochabamba.</p>
        <a className="start-link" href={token ? '/test' : '/registro'}>{token ? 'Ir al test vocacional' : 'Empezar ahora'}</a>
      </main>
    )
  }
}

export default App
