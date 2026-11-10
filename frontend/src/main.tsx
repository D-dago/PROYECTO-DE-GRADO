import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { startSession } from './services/auth'

// Antes de dibujar: descarta una sesion vieja (de localStorage o sin uso hace mas de 30 minutos).
startSession()

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>,
)
