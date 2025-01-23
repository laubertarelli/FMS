import { defineConfig } from 'vite';
import { fileURLToPath, URL } from 'node:url'
import plugin from '@vitejs/plugin-vue';

// https://vitejs.dev/config/
// port 5174
export default defineConfig({
    plugins: [plugin()],
    resolve: {
        alias: {
          '@': fileURLToPath(new URL('./src', import.meta.url))
        }
      }
})
