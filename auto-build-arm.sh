#!/bin/bash
echo start
tag="2.0.2"
echo $tag
docker build -t registry.cn-hangzhou.aliyuncs.com/iotgateway/iotgateway:arm-$tag .
docker push registry.cn-hangzhou.aliyuncs.com/iotgateway/iotgateway:arm-$tag

docker tag registry.cn-hangzhou.aliyuncs.com/iotgateway/iotgateway:arm-$tag registry.cn-hangzhou.aliyuncs.com/iotgateway/iotgateway:arm
docker push registry.cn-hangzhou.aliyuncs.com/iotgateway/iotgateway:arm

docker tag registry.cn-hangzhou.aliyuncs.com/iotgateway/iotgateway:latest  15261671110/iotgateway:arm-$tag
docker push 15261671110/iotgateway:arm-$tag

docker tag 15261671110/iotgateway:$tag 15261671110/iotgateway:arm
docker push 15261671110/iotgateway:arm





