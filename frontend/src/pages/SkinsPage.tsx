import { useEffect, useState } from 'react'
import { appApi } from '../services/appApi'
import type { AvatarStyle, Skin, SkinShop } from '../services/appApi'
import { Avatar } from '../components/Avatar'

export function SkinsPage() {
  const [shop, setShop] = useState<SkinShop | null>(null)
  const [message, setMessage] = useState<{ text: string; isError: boolean } | null>(null)
  const [busy, setBusy] = useState(false)

  useEffect(() => {
    appApi.skins().then(setShop).catch((error) => setMessage({ text: error instanceof Error ? error.message : 'No se pudieron cargar las skins.', isError: true }))
  }, [])

  // Hombre o mujer: gratis y se puede cambiar cuando se quiera; la ropa equipada se mantiene (ORION #38).
  async function chooseBody(style: AvatarStyle) {
    if (busy || shop?.avatarStyle === style) return
    setBusy(true)
    setMessage(null)
    try {
      setShop(await appApi.setAvatarStyle(style))
      window.dispatchEvent(new Event('profile-changed')) // el avatar del encabezado tambien cambia
    } catch (error) {
      setMessage({ text: error instanceof Error ? error.message : 'No se pudo cambiar el avatar.', isError: true })
    } finally { setBusy(false) }
  }

  async function act(kind: 'buy' | 'equip', skinId: string) {
    if (busy) return
    setBusy(true)
    setMessage(null)
    try {
      const updated = kind === 'buy' ? await appApi.buySkin(skinId) : await appApi.equipSkin(skinId)
      setShop(updated)
      setMessage({ text: kind === 'buy' ? '¡Skin comprada!' : 'Skin equipada.', isError: false })
      window.dispatchEvent(new Event('profile-changed'))
    } catch (error) {
      setMessage({ text: error instanceof Error ? error.message : 'No se pudo completar la acción.', isError: true })
    } finally { setBusy(false) }
  }

  function buy(skin: Skin) {
    if (shop && shop.coins < skin.price) {
      setMessage({ text: 'No tienes saldo suficiente. La única forma de conseguir monedas es haciendo el test.', isError: true })
      return Promise.resolve()
    }
    return act('buy', skin.id)
  }

  const equipped = shop?.skins.find((skin) => skin.equipped)
  const body = shop?.avatarStyle ?? 'hombre'

  return (
    <main className="content-shell">
      <span className="eyebrow">Tu avatar</span>
      <h1>Skins</h1>
      <p className="hero-subtitle">Elige si tu avatar es hombre o mujer (gratis) y usa las monedas del test para vestirlo.</p>
      {message && <p className={message.isError ? 'error-message' : 'success-message'}>{message.text}</p>}
      {!shop ? (
        !message && <p className="status-message">Cargando skins...</p>
      ) : (
        <div className="skins-layout">
          <aside className="skin-preview">
            <Avatar skin={shop.equippedSkin} body={body} size={170} />
            <strong>{equipped?.name}</strong>
            <span className="badge badge-active">En uso</span>
            <div className="avatar-body-switch" role="group" aria-label="Tu avatar">
              {(['hombre', 'mujer'] as const).map((style) => (
                <button key={style} type="button" aria-pressed={body === style} className={body === style ? 'is-active' : ''} disabled={busy} onClick={() => void chooseBody(style)}>
                  {style === 'hombre' ? 'Hombre' : 'Mujer'}
                </button>
              ))}
            </div>
          </aside>
          <div className="skin-grid">
            {shop.skins.map((skin) => (
              <article className={`skin-card${skin.equipped ? ' skin-card-equipped' : ''}`} key={skin.id}>
                <Avatar skin={skin.id} body={body} size={84} />
                <h3>{skin.name}</h3>
                <p>{skin.description}</p>
                <div className="skin-actions">
                  {skin.equipped ? <span className="badge badge-active">En uso</span>
                    : skin.owned ? <button type="button" disabled={busy} onClick={() => void act('equip', skin.id)}>Seleccionar</button>
                    : <button type="button" disabled={busy} onClick={() => void buy(skin)}>Comprar · 🪙 {skin.price}</button>}
                </div>
              </article>
            ))}
          </div>
        </div>
      )}
    </main>
  )
}
