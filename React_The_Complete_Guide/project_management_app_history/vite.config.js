import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import base64Loader from './base64FileLoader'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react(), base64Loader()],
})
