/**
 * Petalos de sakura cayendo, sobre el fondo (que ya es una escena de cerezos). Es puro
 * CSS/SVG generado aqui mismo - no hace falta ningun GIF ni imagen externa: cada petalo es
 * un <svg> chiquito con una animacion CSS distinta (posicion, velocidad, retraso, giro).
 * position: fixed + pointer-events: none para que nunca tape ni bloquee un clic.
 */
const CANTIDAD = 16

// Fuera del componente: el azar se calcula una sola vez al cargar la pagina. Hacerlo durante el
// render (aunque sea con useMemo) vuelve impuro al componente y los petalos podrian "saltar".
const petalos = Array.from({ length: CANTIDAD }, (_, index) => ({
  id: index,
  izquierda: Math.random() * 100, // % del ancho de la pantalla
  duracion: 9 + Math.random() * 10, // segundos en caer de arriba a abajo
  retraso: -(Math.random() * 18), // negativo: algunos ya estan "a mitad de caida" al cargar la pagina
  tamano: 12 + Math.random() * 12, // px
  deriva: (Math.random() - 0.5) * 140, // px que se mueve a los lados mientras cae
  giro: 180 + Math.random() * 360, // grados que rota mientras cae
  opacidad: 0.45 + Math.random() * 0.4,
}))

export function FallingPetals() {
  return (
    <div className="petalos-capa" aria-hidden="true">
      {petalos.map((p) => (
        <span
          key={p.id}
          className="petalo"
          style={{
            left: `${p.izquierda}%`,
            width: p.tamano, height: p.tamano,
            opacity: p.opacidad,
            animationDuration: `${p.duracion}s`,
            animationDelay: `${p.retraso}s`,
            '--deriva': `${p.deriva}px`,
            '--giro': `${p.giro}deg`,
          } as React.CSSProperties}
        >
          <svg viewBox="0 0 32 32" fill="none">
            <path d="M16 2c4 4 6 9 6 13 0 5-3 9-6 15-3-6-6-10-6-15 0-4 2-9 6-13z" fill="currentColor" />
          </svg>
        </span>
      ))}
    </div>
  )
}
