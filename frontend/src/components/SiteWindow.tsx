import { useEffect, useRef, useState } from 'react'
import { appApi } from '../services/appApi'

type WindowState = 'checking' | 'embedded' | 'blocked' | 'unverified'

/**
 * Ventana dentro de la misma pagina con el sitio oficial de una institucion.
 * <dialog> ya resuelve cerrar con Esc, retener el foco y tapar el resto de la pagina.
 * Algunos sitios prohiben mostrarse dentro de otra pagina: el servidor lo comprueba antes, y en ese
 * caso se ofrece abrirlo aparte en vez de dejar al estudiante frente a un recuadro de error.
 */
export function SiteWindow({ institutionId, name, website, onClose }: { institutionId: string; name: string; website: string; onClose: () => void }) {
  const dialog = useRef<HTMLDialogElement>(null)
  const [state, setState] = useState<WindowState>('checking')
  const [loaded, setLoaded] = useState(false)

  useEffect(() => {
    const element = dialog.current
    element?.showModal()
    return () => element?.close()
  }, [])

  useEffect(() => {
    let active = true
    // Una pagina https no puede mostrar dentro un sitio http: el navegador lo bloquea siempre.
    const mixedContent = window.location.protocol === 'https:' && website.toLowerCase().startsWith('http:')
    if (mixedContent) {
      void Promise.resolve().then(() => { if (active) setState('blocked') })
    } else {
      void appApi.siteCheck(institutionId)
        .then((result) => { if (active) setState(result.embeddable ? 'embedded' : result.verified ? 'blocked' : 'unverified') })
        // Sin comprobar no se intenta mostrar: si el sitio lo prohibe, solo se veria un recuadro de error.
        .catch(() => { if (active) setState('unverified') })
    }
    return () => { active = false }
  }, [institutionId, website])

  return (
    <dialog ref={dialog} className="site-window" aria-label={`Sitio oficial de ${name}`} onClose={onClose}>
      <header className="site-window-bar">
        <strong>{name}</strong>
        <a href={website} target="_blank" rel="noopener noreferrer">Abrir en otra pestaña ↗</a>
        <button type="button" onClick={() => dialog.current?.close()} aria-label="Cerrar ventana">✕</button>
      </header>
      <div className="site-window-body">
        {state === 'checking' && <p className="site-window-message">Preparando el sitio...</p>}
        {(state === 'blocked' || state === 'unverified') && (
          <div className="site-window-message">
            <p>{state === 'blocked' ? 'Esta institución no permite mostrar su sitio dentro de otra página.' : 'Este sitio está tardando en responder. Puedes abrirlo en otra pestaña.'}</p>
            <a className="start-link" href={website} target="_blank" rel="noopener noreferrer">Abrir su sitio oficial ↗</a>
          </div>
        )}
        {state === 'embedded' && (
          <>
            {!loaded && <p className="site-window-message">Cargando el sitio oficial...</p>}
            {/* Sin allow-top-navigation: el sitio no puede sacar al estudiante de esta pagina.
                allow-same-origin aqui es seguro porque el sitio es de otro origen y conserva el suyo. */}
            <iframe
              title={`Sitio oficial de ${name}`}
              src={website}
              sandbox="allow-scripts allow-same-origin allow-forms allow-popups allow-popups-to-escape-sandbox"
              referrerPolicy="no-referrer"
              onLoad={() => setLoaded(true)}
            />
          </>
        )}
      </div>
    </dialog>
  )
}
