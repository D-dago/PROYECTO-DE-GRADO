import { useEffect, useRef } from 'react'
import type { Dispatch, SetStateAction } from 'react'
import { appApi } from './appApi'
import type { Recommendation } from './appApi'

const INTERVALO_MS = 5000

type LiveArea = 'catalog' | 'activity'

/**
 * Ejecuta onChange cuando cambian los datos en el servidor. Pregunta cada 5 s si cambiaron y, si la
 * pestana esta en segundo plano, no pregunta: al volver a ella se pone al dia de inmediato.
 * Se pregunta en vez de recibir un aviso porque los tuneles rapidos de Cloudflare no admiten SSE.
 */
export function useLiveData(areas: LiveArea[], onChange: () => void) {
  const callback = useRef(onChange)
  useEffect(() => { callback.current = onChange })
  const areasKey = areas.join(',')

  useEffect(() => {
    const watched = areasKey.split(',') as LiveArea[]
    let last: string | null = null
    let checking = false

    async function check() {
      if (document.hidden || checking) return
      checking = true
      try {
        const version = await appApi.liveVersion()
        const current = watched.map((area) => version[area]).join(',')
        if (last !== null && last !== current) callback.current()
        last = current
      } catch {
        // Un corte de red no debe molestar a quien esta leyendo: se reintenta en la siguiente vuelta.
      } finally {
        checking = false
      }
    }

    void check()
    const interval = setInterval(() => void check(), INTERVALO_MS)
    const onVisible = () => void check()
    document.addEventListener('visibilitychange', onVisible)
    return () => {
      clearInterval(interval)
      document.removeEventListener('visibilitychange', onVisible)
    }
  }, [areasKey])
}

/** Mantiene al dia las universidades de un resultado, sin volver a pedir la explicacion de la IA. */
export function useLiveCareers(setRecommendation: Dispatch<SetStateAction<Recommendation | null>>) {
  useLiveData(['catalog'], () => {
    appApi.myCareers()
      .then((items) => setRecommendation((current) => current && { ...current, items }))
      .catch(() => { /* se reintenta con el siguiente cambio */ })
  })
}
