#!/bin/bash
echo start
docker build -t iotgateway .
docker stop iotgateway
docker rm iotgateway
docker run -d --restart always --name iotgateway -p 518:518 -p 1888:1888 -p 62541:62541 -p 503:503 iotgateway
docker cp 3d iotgateway:app/wwwroot/
docker cp drivers iotgateway:app/
docker cp IoTGateway/iotgateway.db iotgateway:app/
docker cp IoTGateway/Quickstarts.ReferenceServer.Config.xml iotgateway:app/
docker restart iotgateway
docker commit iotgateway registry.cn-hangzhou.aliyuncs.com/wanghaidong/iotgateway
docker push registry.cn-hangzhou.aliyuncs.com/wanghaidong/iotgateway
docker tag registry.cn-hangzhou.aliyuncs.com/wanghaidong/iotgateway  15261671110/iotgateway
docker push 15261671110/iotgateway