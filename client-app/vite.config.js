import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()],
  server: {
    proxy: {
      '/hubs': {
        target: 'http://localhost:5146',
        changeOrigin: true,
        secure: false,
        ws: true,
      },
      '/api': {
        target: 'http://localhost:5146',
        changeOrigin: true,
        secure: false,
      },
      '/chat': {
        target: 'http://localhost:5146',
        changeOrigin: true,
        secure: false,
        ws: false,
      },
    },
  },
  build: {
    outDir: '../MyMvcApp/wwwroot',
    emptyOutDir: true,
  },
})
