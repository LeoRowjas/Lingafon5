import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'
import path from 'path'
import { fileURLToPath } from 'url'

const __filename = fileURLToPath(import.meta.url)
const __dirname = path.dirname(__filename)


export default defineConfig({
  plugins: [tailwindcss(),react()],
  server: { port: 5173, },
  resolve: {
    alias: {
      '@assets': path.resolve(__dirname, 'src/assets'),
      '@entities': path.resolve(__dirname, 'src/App/entities'),
      '@features': path.resolve(__dirname, 'src/App/features'),
      '@pages': path.resolve(__dirname, 'src/App/pages'),
      '@ui': path.resolve(__dirname, 'src/App/shared/ui'),
      '@shared': path.resolve(__dirname, 'src/App/shared'),
      '@api': path.resolve(__dirname, 'src/api')
    },
  },
})
