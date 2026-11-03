import { useEffect, useRef } from 'react'

/**
 * En el panel del administrador el formulario de edicion esta debajo de la lista: al apretar
 * "Editar" se llenaba, pero sin moverse de la lista, y parecia que no pasaba nada. Esto lleva la
 * pantalla hasta el formulario y pone el cursor en su primer campo.
 */
export function useFormularioDeEdicion(editando: unknown) {
  const formulario = useRef<HTMLFormElement>(null)
  useEffect(() => {
    if (!editando || !formulario.current) return
    formulario.current.scrollIntoView({ behavior: 'smooth', block: 'start' })
    formulario.current.querySelector<HTMLElement>('input:not([disabled]), select:not([disabled])')?.focus({ preventScroll: true })
  }, [editando])
  return formulario
}
