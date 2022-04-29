# STEP 1: Build
FROM node:10 as builder

LABEL authors="iotgateway"

COPY package.json package-lock.json ./

RUN npm set progress=false && npm config set depth 0 && npm cache clean --force
RUN npm i && mkdir /iotgateway && cp -R ./node_modules ./iotgateway

WORKDIR /iotgateway

COPY . .

RUN npm run build

# STEP 2: Setup
FROM nginx:alpine

COPY --from=builder /iotgateway/_nginx/default.conf /etc/nginx/conf.d/default.conf
# COPY --from=builder /iotgateway/_nginx/ssl/* /etc/nginx/ssl/

RUN rm -rf /usr/share/nginx/html/*

COPY --from=builder /iotgateway/dist /usr/share/nginx/html

CMD [ "nginx", "-g", "daemon off;"]
