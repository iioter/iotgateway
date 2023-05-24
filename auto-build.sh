#!/bin/bash
echo start
tag="2.0.2"
echo $tag
docker build -t registry.cn-hangzhou.aliyuncs.com/iotgateway/iotgateway:$tag .
docker push registry.cn-hangzhou.aliyuncs.com/iotgateway/iotgateway:$tag

docker tag registry.cn-hangzhou.aliyuncs.com/iotgateway/iotgateway:$tag registry.cn-hangzhou.aliyuncs.com/iotgateway/iotgateway:latest
docker push registry.cn-hangzhou.aliyuncs.com/iotgateway/iotgateway:latest

docker tag registry.cn-hangzhou.aliyuncs.com/iotgateway/iotgateway:latest  15261671110/iotgateway:$tag
docker push 15261671110/iotgateway:$tag

docker tag 15261671110/iotgateway:$tag 15261671110/iotgateway:latest
docker push 15261671110/iotgateway:latest
