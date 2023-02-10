# IoTGateway

## [在线体验](http://online.iotgateway.net/)

> 基于.NET6的跨平台工业物联网网关
> 
> B/S架构，可视化配置
> 
> 南向连接到你的任何设备和系统(如PLC、扫码枪、CNC、数据库、串口设备、上位机、非标设备、OPC Server、OPC UA Server、Mqtt Server等)
> 
> 北向连接Thingsboard、IoTSharp、华为云或您自己的物联网平台(MES、SCADA)等进行双向数据通讯
> 
> 当然也可以进行边缘计算
>

## [教程文档](http://iotgateway.net/)
## [硬件网关](http://iotgateway.net/docs/hardware/selection/)
## [企业版](http://iotgateway.net/docs/enterprise/intro)

## 运行部署

| [发行包运行](http://iotgateway.net/docs/iotgateway-beginner/run/release-run) 
| [docker运行](http://iotgateway.net/docs/iotgateway-beginner/run/docker-run) 
| [源码运行](http://iotgateway.net/docs/iotgateway-beginner/run/source-run) 
| [发布部署](http://iotgateway.net/docs/iotgateway-beginner/run/publish-run) 


## 社区

| 微信扫我进群 | 公众号 |    [QQ群:895199932](https://jq.qq.com/?_wv=1027&k=mus0CV0W)  |
| ---- | ------ | ---- |
| ![qq](./images/wxgroup.png) | ![wx](./images/wx.jpg) | ![qq](./images/qq.png) |


## 南向
- 支持**西门子PLC**、**三菱PLC**、**Modbus**、**欧姆龙PLC**、**OPCUA**、**OPCDA**、**ABPLC**、**MT机床**、**Fanuc CNC**
- [驱动支持扩展](http://iotgateway.net/docs/iotgateway/driver/tcpclient)
- 支持设备数据写入
  ![set-variabl](./images/set-variable.png)  
- 支持计算表达式  
  ![express](./images/express.png)
- 支持变化上传和定时归档
  ![change-uploa](./images/change-upload.png)
  

## 北向
- thingsboard、iotsharp、华为云等第三方平台
- 遥测、属性上传
- RPC反向控制
  ![rpc](./images/rpc.gif)

## 服务
- 内置Mqtt服务(1888,1888/mqtt),支持websocker-mqtt，直连你的MES、SCADA等
  ![mqtt](./images/mqtt.png)
- 内置OpcUA(opc.tcp://localhost:62541/Quickstarts/ReferenceServer)，你的设备也可以通过OPCUA和其他设备通信
  ![opcua](./images/opcua.png)
- 内置ModbusSlave(模拟设备)，端口503

## 展示
- Websocker免刷新
![variables](./images/variables.gif)

- 3D数字孪生Demo
  ![3d](./images/3d.gif)
  
- 支持接入web组态项目
![scada](./images/scada.gif)
![scada-config](./images/scada-config.png)

## 声明

- 使用OPCUA协议**请联系OPC基金会进行授权**，产生一切**纠纷与本项目无关**
- 我们**接受并感谢**资金以及任何方式的的**赞助**，但并**不意味着我们会为您承诺或担保任何事情**
- 若你使用IoTGateway**获利**，我们希望你对IoTGateway**是有贡献的**(不限于代码、文档、意见建议或力所能及的赞助)
- 请*严格*遵循**MIT**协议
- [企业版介绍点我](http://iotgateway.net/docs/enterprise/intro)

## 友情链接

### IoTClient

开源地址：
https://github.com/zhaopeiym/IoTClient
概述：
这是一个基于.NET Standard 2.0物联网设备通讯协议实现客户端，包括主流PLC、Bacnet等。

## 获得奖项(部分)

### .NET20周年云原生开发挑战赛一等奖
### Gitee 2022 GVP
### OSC 2022 最火热中国开源项目社区

## 企业客户(部分)

国家电网(电力)、歌尔股份(3C)、经纬纺机(央企上市)、BOSCH(汽车零部件)、凌坤智能(AGV)、容恒、卧晨、惠斯通、益信......

## 打赏请留微信或QQ

|  微信 | 支付宝 |
| ----- | ---- |
| ![wx-pay](./images/wx-pay.jpg) | ![ali-pay](./images/ali-pay.png) |
