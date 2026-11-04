import { useEffect, useState } from 'react'
import { appApi } from '../services/appApi'
import type { Recommendation } from '../services/appApi'
import { UniversityLinks } from '../components/UniversityLinks'
import { useLiveCareers } from '../services/liveData'

// Antes esta pagina pedia municipio, costo y lugar, pero esos datos eran estimaciones sin confirmar.
// Ahora muestra lo que si se puede sostener: la compatibilidad con el perfil y el sitio oficial.
export function RecommendationsPage() {
  const [recommendation, setRecommendation] = useState<Recommendation | null>(null)
  const [error, setError] = useState<string | null>(null)
  useLiveCareers(recommendation, setRecommendation)

  useEffect(() => {
    appApi.recommendations({})
      .then(setRecommendation)
      .catch((loadError) => setError(loadError instanceof Error ? loadError.message : 'No se pudieron cargar las universidades.'))
  }, [])

  return (
    <main className="content-shell">
      <span className="eyebrow">Según tu último test</span>
      <h1>Universidades de Cochabamba</h1>
      {error && (
        <>
          <p className="empty-state">{error}</p>
          <a className="start-link" href="/test">Ir al test vocacional</a>
        </>
      )}
      {!error && !recommendation && <p className="status-message">Buscando universidades compatibles...</p>}
      {recommendation && <UniversityLinks careers={recommendation.items} />}
    </main>
  )
}
