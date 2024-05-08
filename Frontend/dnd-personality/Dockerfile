FROM node:18-alpine
WORKDIR /app
COPY package*.json ./
RUN npm install 
COPY . .
RUN npm ci 
RUN npm run build
ENV NODE_ENV production
ENV HTTPS true
EXPOSE 3000
CMD [ "npx", "serve", "build" ]
