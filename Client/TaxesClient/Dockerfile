FROM node:13.3.0 AS compile-image


WORKDIR /opt/ng
COPY  ./Client/TaxesClient/package.json ./Client/TaxesClient/package-lock.json ./
RUN npm install

ENV PATH="./node_modules/.bin:$PATH" 

COPY ./Client/TaxesClient/ ./
RUN ng build 

FROM nginx
COPY docker/nginx/default.conf /etc/nginx/conf.d/default.conf
COPY --from=compile-image /opt/ng/dist/app-name /usr/share/nginx/html