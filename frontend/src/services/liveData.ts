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
 * Si onChange falla, se reintenta en la siguiente vuelta: un corte o el limite anti-abuso, que un salon
 * comparte por tener la misma IP, no deben dejar la pantalla desactualizada hasta el proximo cambio.
 */
export function useLiveData(areas: LiveArea[], onChange: () => unknown) {
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
        if (last !== null && last !== current) await callback.current()
        last = current
      } catch {
        // No se avisa a quien esta leyendo y "last" no avanza, asi que se reintenta en la siguiente vuelta.
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
export function useLiveCareers(recommendation: Recommendation | null, setRecommendation: Dispatch<SetStateAction<Recommendation | null>>) {
  useLiveData(['catalog'], () => {
    // Sin resultado cargado (por ejemplo, sin test hecho) no hay nada que refrescar ni que reintentar.
    if (!recommendation) return undefined
    return appApi.myCareers().then((items) => setRecommendation((current) => current && { ...current, items }))
  })
}
