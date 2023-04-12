#!/bin/bash
echo start
tag="2.0.0"
echo $tag
docker build -t iotgateway .

docker stop iotgateway
docker rm iotgateway

docker run -d --restart always --name iotgateway -p 518:518 -p 1888:1888 -p 62541:62541 -p 503:503 iotgateway

docker cp 3d iotgateway:app/wwwroot/
docker cp drivers iotgateway:app/
docker cp IoTGateway/iotgateway.db iotgateway:app/
docker cp IoTGateway/Quickstarts.ReferenceServer.Config.xml iotgateway:app/

docker restart iotgateway

docker commit iotgateway registry.cn-hangzhou.aliyuncs.com/iotgateway/iotgateway:$tag
docker push registry.cn-hangzhou.aliyuncs.com/iotgateway/iotgateway:$tag

docker tag registry.cn-hangzhou.aliyuncs.com/iotgateway/iotgateway:$tag registry.cn-hangzhou.aliyuncs.com/iotgateway/iotgateway:latest
docker push registry.cn-hangzhou.aliyuncs.com/iotgateway/iotgateway:latest

docker tag registry.cn-hangzhou.aliyuncs.com/iotgateway/iotgateway:latest  15261671110/iotgateway:$tag
docker push 15261671110/iotgateway:$tag

docker tag 15261671110/iotgateway:$tag 15261671110/iotgateway:latest
docker push 15261671110/iotgateway:latest
