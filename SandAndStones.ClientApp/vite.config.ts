import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import mkcert from 'vite-plugin-mkcert'
import fs from 'fs';
import path from 'path';

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
        host: true,
        https: {
            key: fs.readFileSync(path.resolve(__dirname, 'key.pem')),
            cert: fs.readFileSync(path.resolve(__dirname, 'cert.pem')),
        },
        cors: true,
            proxy: {
                '/asset-api': {
                    target: 'https://localhost:5000',
                    changeOrigin: true,
                    secure: false,
                    ws: true,
                },
                '/texture-api': {
                    target: 'https://localhost:5000',
                    changeOrigin: true,
                    secure: false,
                    ws: true,
                },
                '/eventlog-api': {
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