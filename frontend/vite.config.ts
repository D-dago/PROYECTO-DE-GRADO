import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  // host: true escucha en todas las interfaces, para abrir la web desde el celular
  // (http://IP-de-la-PC:5173) estando en la misma red Wi-Fi.
  server: { host: true, port: 5173, strictPort: true },
  preview: { host: true, port: 5173, strictPort: true },
  // La web compilada se guarda dentro de la API, que la sirve en su propio puerto.
  build: { outDir: '../backend/OrientacionVocacional.API/wwwroot', emptyOutDir: true },
})
