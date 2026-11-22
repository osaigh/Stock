import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// The origin http://localhost:44602 is registered with the identity server,
// so the port must not fall back to another one.
export default defineConfig({
  plugins: [react()],
  server: {
    port: 44602,
    strictPort: true,
    open: false,
  },
  preview: {
    port: 44602,
    strictPort: true,
    open: false,
  },
  build: {
    rolldownOptions: {
      output: {
        // Put the big libraries in their own chunks so that no chunk gets too large
        // and they stay cached in the browser when only the application code changes.
        codeSplitting: {
          groups: [
            { name: 'react', test: /node_modules[\\/](react|react-dom|react-router|react-router-dom|scheduler)[\\/]/, priority: 30 },
            { name: 'mui-x', test: /node_modules[\\/]@mui[\\/]x-/, priority: 20, includeDependenciesRecursively: false },
            { name: 'recharts', test: /node_modules[\\/](recharts|d3-[^\\/]+|victory-vendor)[\\/]/, priority: 15 },
            { name: 'mui', test: /node_modules[\\/](@mui|@emotion)[\\/]/, priority: 10 },
          ],
        },
      },
    },
  },
})


