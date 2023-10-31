import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import { resolve } from 'path'

const projectRootDir = resolve(__dirname);

export default defineConfig({
  plugins: [vue()],
  server: {
    open: true,
    port: 2025,
  },
  preview: {
    port: 2025,
  },
  resolve: {
    alias: {
      "@": resolve(projectRootDir, "src"),
      "@components": resolve(projectRootDir, "src/components"),
    },
  }
})
