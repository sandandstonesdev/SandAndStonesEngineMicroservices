import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import fs from 'fs';
import path from 'path';
import { env } from 'process';
import child_process from 'child_process';

const baseFolder =
    env.APPDATA !== undefined && env.APPDATA !== ''
        ? `${env.APPDATA}/ASP.NET/https`
        : `${env.HOME}/.aspnet/https`;

const certificateName = "securewebsite.client";
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    if (0 !== child_process.spawnSync('dotnet', [
        'dev-certs',
        'https',
        '--export-path',
        certFilePath,
        '--format',
        'Pem',
        '--no-password',
    ], { stdio: 'inherit', }).status) {
        throw new Error("Could not create certificate.");
    }
}

export default defineConfig({
    plugins: [react()],
    build: {
        minify: false,
    },
    preview: {
        port: 8080,
        strictPort: true,
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
        port: 5173,
        https: {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath),
        }
    }
});