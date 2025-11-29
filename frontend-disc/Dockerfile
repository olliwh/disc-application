FROM node:20-alpine

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

EXPOSE 3000

ENV VITE_HMR_HOST=localhost
ENV VITE_HMR_PORT=3000

CMD ["npm","run", "dev"]