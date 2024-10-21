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
        cors: false
    },
    server: {
        proxy: {
            '^/home': {
                target: 'https://localhost:7232/',
                secure: false
            },
            '^/assetBatch': {
                target: 'https://localhost:7232/',
                secure: false
            },
            '^/textureFile': {
                target: 'https://localhost:7232/',
                secure: false
            },
            '^/register': {
                target: 'https://localhost:7232/',
                secure: false
            },
            '^/login': {
                target: 'https://localhost:7232/',
                secure: false
            },
            '^/logout': {
                target: 'https://localhost:7232/',
                secure: false
            },
            '^/profile': {
                target: 'https://localhost:7232/',
                secure: false
            }
        },
        port: 5173
    }
});