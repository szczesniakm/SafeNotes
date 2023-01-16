FROM node:18.13-alpine3.16 as node
WORKDIR /app
RUN npm install -g @angular/cli@15.1.1
COPY ./package.json .
RUN npm install
COPY . .
RUN ng b --configuration=production

FROM nginx:alpine
COPY --from=node /app/dist/safe-notes /usr/share/nginx/html/
COPY nginx.conf /etc/nginx/conf.d/default.conf
