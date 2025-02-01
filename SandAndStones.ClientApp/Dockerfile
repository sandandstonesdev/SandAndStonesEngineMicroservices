FROM node:22-alpine AS build

WORKDIR /app
COPY ./SandAndStones.ClientApp/package*.json ./
RUN npm install
COPY ./SandAndStones.ClientApp ./

EXPOSE 5173

CMD ["npm", "run", "dev", "--https"]