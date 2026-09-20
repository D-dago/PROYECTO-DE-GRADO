import type { ReactNode } from 'react'

type Outfit = { legs: string; torso: string; extra: ReactNode; headwear?: ReactNode; face?: ReactNode; longSkirt?: boolean }

const petal = (cx: number, cy: number, r = 3) => <circle key={`${cx}-${cy}`} cx={cx} cy={cy} r={r} fill="#ff5fa8" />

function outfitFor(skin: string): Outfit {
  switch (skin) {
    case 'hoodie':
      return {
        legs: '#2d2a44', torso: '#ec2f8f',
        extra: <><ellipse cx="60" cy="68" rx="24" ry="9" fill="#c91f78" /><rect x="46" y="102" width="28" height="14" rx="5" fill="#d02580" /><line x1="55" y1="72" x2="55" y2="88" stroke="#fff" strokeWidth="2" /><line x1="65" y1="72" x2="65" y2="88" stroke="#fff" strokeWidth="2" /></>,
      }
    case 'overol':
      return {
        legs: '#d9911f', torso: '#c9c9d6',
        extra: <><rect x="36" y="86" width="48" height="36" rx="8" fill="#f2a93b" /><rect x="46" y="74" width="28" height="16" rx="4" fill="#f2a93b" /><rect x="43" y="66" width="6" height="22" fill="#f2a93b" /><rect x="71" y="66" width="6" height="22" fill="#f2a93b" /><circle cx="46" cy="90" r="2.4" fill="#7a4a06" /><circle cx="74" cy="90" r="2.4" fill="#7a4a06" /></>,
        headwear: <><path d="M32,30 C32,6 88,6 88,30 Z" fill="#ffcc33" /><rect x="28" y="28" width="64" height="6" rx="3" fill="#e6b21f" /><rect x="56" y="10" width="8" height="12" rx="3" fill="#e6b21f" /></>,
      }
    case 'labcoat':
      return {
        legs: '#3b3f66', torso: '#f6f3fb',
        extra: <><rect x="32" y="68" width="56" height="66" rx="12" fill="#f6f3fb" /><path d="M52,68 L60,92 L68,68" fill="#8fc7ff" /><path d="M52,68 L58,100 M68,68 L62,100" stroke="#d4cde6" strokeWidth="2" fill="none" /><rect x="40" y="104" width="14" height="12" rx="3" fill="#e6e0f2" /></>,
        headwear: <><circle cx="50" cy="30" r="6.5" fill="rgba(120,190,255,.5)" stroke="#4aa3ff" strokeWidth="2" /><circle cx="70" cy="30" r="6.5" fill="rgba(120,190,255,.5)" stroke="#4aa3ff" strokeWidth="2" /><line x1="56" y1="30" x2="64" y2="30" stroke="#4aa3ff" strokeWidth="2" /></>,
      }
    case 'artist':
      return {
        legs: '#2b2b3d', torso: '#2c6fbb',
        extra: <><circle cx="46" cy="90" r="4" fill="#ff4d6d" /><circle cx="66" cy="104" r="4.5" fill="#ffd93d" /><circle cx="52" cy="112" r="3.5" fill="#4ade80" /><circle cx="72" cy="84" r="3" fill="#ff9f43" /></>,
        headwear: <><ellipse cx="56" cy="22" rx="30" ry="10" fill="#d62839" /><circle cx="52" cy="11" r="3.5" fill="#d62839" /></>,
      }
    case 'sweater':
      return {
        legs: '#3b3f66', torso: '#21b876',
        extra: <><path d="M48,68 Q60,82 72,68 L72,74 Q60,88 48,74 Z" fill="#eafff5" /><path d="M46,96 C40,88 48,84 52,90 C56,84 64,88 58,96 L52,104 Z" fill="#ff5fa8" transform="translate(-2 2)" /></>,
      }
    case 'suit':
      return {
        legs: '#1f2f5c', torso: '#1f2f5c',
        extra: <><path d="M52,68 L60,94 L68,68 Z" fill="#fff" /><path d="M58,72 L62,72 L64,96 L60,104 L56,96 Z" fill="#d62839" /><path d="M52,68 L46,100 M68,68 L74,100" stroke="#14203f" strokeWidth="2.5" fill="none" /></>,
      }
    case 'cardigan':
      return {
        legs: '#4b3f36', torso: '#b98a5e',
        extra: <><rect x="52" y="68" width="16" height="54" fill="#f3e6d3" /><circle cx="60" cy="82" r="2" fill="#5b3d1f" /><circle cx="60" cy="96" r="2" fill="#5b3d1f" /><circle cx="60" cy="110" r="2" fill="#5b3d1f" /></>,
        face: <><circle cx="51" cy="42" r="8" fill="none" stroke="#2b2b3d" strokeWidth="2" /><circle cx="69" cy="42" r="8" fill="none" stroke="#2b2b3d" strokeWidth="2" /><line x1="59" y1="42" x2="61" y2="42" stroke="#2b2b3d" strokeWidth="2" /></>,
      }
    case 'kimono':
      return {
        legs: '#ffc4de', torso: '#ffb3d4', longSkirt: true,
        extra: <><rect x="34" y="108" width="52" height="46" rx="8" fill="#ffc4de" /><rect x="34" y="94" width="52" height="13" fill="#c2185b" /><path d="M60,68 L50,94 M60,68 L70,94" stroke="#fff" strokeWidth="2.5" fill="none" />{[petal(44, 120), petal(70, 130), petal(52, 142), petal(76, 116, 2.5), petal(40, 80, 2.5), petal(80, 78, 2.5)]}</>,
        headwear: <circle cx="78" cy="22" r="6" fill="#ff5fa8" stroke="#fff" strokeWidth="1.5" />,
      }
    default:
      return { legs: '#3b3f66', torso: '#8fa0d6', extra: null }
  }
}

export function Avatar({ skin, size = 120 }: { skin: string; size?: number }) {
  const outfit = outfitFor(skin)
  const tone = '#f2c6a0'

  return (
    <svg width={size} height={(size * 4) / 3} viewBox="0 0 120 160" role="img" aria-label={`Avatar con la skin ${skin}`}>
      {!outfit.longSkirt && <><rect x="38" y="116" width="20" height="36" rx="7" fill={outfit.legs} /><rect x="62" y="116" width="20" height="36" rx="7" fill={outfit.legs} /></>}
      <ellipse cx="48" cy="153" rx="12" ry="5" fill="#241733" />
      <ellipse cx="72" cy="153" rx="12" ry="5" fill="#241733" />
      <rect x="20" y="70" width="18" height="46" rx="9" fill={outfit.torso} />
      <rect x="82" y="70" width="18" height="46" rx="9" fill={outfit.torso} />
      <rect x="34" y="66" width="52" height="56" rx="14" fill={outfit.torso} />
      {outfit.extra}
      <rect x="53" y="58" width="14" height="12" fill={tone} />
      <circle cx="29" cy="118" r="8" fill={tone} />
      <circle cx="91" cy="118" r="8" fill={tone} />
      <circle cx="60" cy="40" r="25" fill={tone} />
      <path d="M35,38 C35,12 85,12 85,38 C78,28 42,28 35,38 Z" fill="#2b1b33" />
      <circle cx="51" cy="43" r="2.6" fill="#2b1b33" />
      <circle cx="69" cy="43" r="2.6" fill="#2b1b33" />
      <circle cx="45" cy="50" r="4" fill="#ff8fb5" opacity=".35" />
      <circle cx="75" cy="50" r="4" fill="#ff8fb5" opacity=".35" />
      <path d="M52,52 Q60,59 68,52" stroke="#2b1b33" strokeWidth="2" fill="none" strokeLinecap="round" />
      {outfit.face}
      {outfit.headwear}
    </svg>
  )
}
