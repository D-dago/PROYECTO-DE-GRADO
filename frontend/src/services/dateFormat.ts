// Fecha y hora sin segundos ("15/11/2026, 08:59"): los segundos no le dicen nada al estudiante ni al
// administrador (ORION #40). Usa el idioma del navegador, como antes.
export function formatDateTime(value: string | number | Date) {
  return new Date(value).toLocaleString(undefined, { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' })
}
