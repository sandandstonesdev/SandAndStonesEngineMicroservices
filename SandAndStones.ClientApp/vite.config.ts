import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import mkcert from 'vite-plugin-mkcert'


export default defineConfig({
    base: "/",
    plugins: [react(), mkcert()],
    build: {
        minify: false,
    },
    preview: {
        port: 8080,
        strictPort: true,
        host: true,
        cors: true
    },
    server: {
        cors: true,
            proxy: {
                '/api': {
                    target: 'https://localhost:5000',
                    changeOrigin: true,
                    secure: false,
                    ws: true,
                },
                '/gateway-api': {
                    target: 'https://localhost:5000',
                    changeOrigin: true,
                    secure: false,
                    ws: true,
                }
            },
            port: 5173    
        }
    }
);