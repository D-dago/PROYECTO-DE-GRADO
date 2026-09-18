import { useState } from 'react'

type Props = { name: string; minLength?: number; required?: boolean; autoComplete?: string }

export function PasswordInput(props: Props) {
  const [visible, setVisible] = useState(false)
  const label = visible ? 'Ocultar contraseña' : 'Mostrar contraseña'

  return (
    <span className="password-field">
      <input {...props} type={visible ? 'text' : 'password'} />
      <button type="button" className="password-toggle" onClick={() => setVisible((current) => !current)} aria-label={label} title={label}>
        <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" aria-hidden="true">
          <path d="M1 12s4-7 11-7 11 7 11 7-4 7-11 7S1 12 1 12z" />
          <circle cx="12" cy="12" r="3" />
          {visible && <line x1="3" y1="3" x2="21" y2="21" />}
        </svg>
      </button>
    </span>
  )
}
