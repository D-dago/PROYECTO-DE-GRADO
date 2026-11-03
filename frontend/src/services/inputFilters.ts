import type { FormEvent } from 'react'

// Filtran lo que el usuario escribe: los caracteres no permitidos ni llegan a aparecer.
// El backend valida lo mismo, asi que esto es solo comodidad (y no se puede saltar el servidor).

/** Solo letras (con tildes/ñ), espacio, apostrofe y guion. Para nombre y apellido. */
export function lettersOnly(event: FormEvent<HTMLInputElement>) {
  event.currentTarget.value = event.currentTarget.value.replace(/[^\p{L} '-]/gu, '').replace(/\s{2,}/g, ' ')
}

/** Igual que lettersOnly pero admite punto. Para ciudad y colegio. */
export function placeOnly(event: FormEvent<HTMLInputElement>) {
  event.currentTarget.value = event.currentTarget.value.replace(/[^\p{L} '\-.]/gu, '').replace(/\s{2,}/g, ' ')
}

/** Solo digitos. Para el telefono. */
export function digitsOnly(event: FormEvent<HTMLInputElement>) {
  event.currentTarget.value = event.currentTarget.value.replace(/\D/g, '')
}

// \s* a los costados: tolera un espacio sobrante al principio o al final (lo agrega solo el
// autocompletado o el teclado del celular), igual que el mismo patron del lado del servidor.
export const inputRules = {
  name: { maxLength: 50, minLength: 2, pattern: "\\s*\\p{L}+([ '\\-]\\p{L}+)*\\s*", title: 'Solo letras (sin números ni símbolos).', autoComplete: 'off' },
  place: { maxLength: 60, pattern: "\\s*\\p{L}+([ '\\-.]\\p{L}+)*\\.?\\s*", title: 'Solo letras (sin números ni símbolos).' },
  phone: { maxLength: 15, minLength: 7, pattern: '\\d{7,15}', title: 'Solo dígitos, de 7 a 15.', inputMode: 'numeric' as const },
}
