# IoTGateway

## [教程文档:http://iotgateway.net](http://iotgateway.net/)
## [在线体验:http://online.iotgateway.net](http://online.iotgateway.net/)


> 基于.NET6的跨平台物联网网关
> 
> B/S架构，可视化配置
> 
> 南向连接到你的任何设备和系统(如PLC、扫码枪、CNC、数据库、串口设备、上位机、OPC Server、OPC UA Server、Mqtt Server等)
> 
> 北向连接Thingsboard、IoTSharp、ThingsCloud、IoTDB或您自己的物联网平台进行双向数据通讯
> 
> 当然也可以进行边缘计算
>
## 交流

| 公众号:工业物联网网关 |    [QQ群:712105424](https://qm.qq.com/cgi-bin/qm/qr?k=e3Y8biyVdhDxx3LPbjvNY3TSNOEAmjp7&jump_from=webapi)  |
| ------ | ---- |
| ![wx](./images/wx.jpg) | ![qq](./images/qq.png) |

## 运行
- [直接运行:http://iotgateway.net/docs/iotgateway/run/release-run](http://iotgateway.net/docs/iotgateway/run/release-run)
- [Docker运行:http://iotgateway.net/docs/iotgateway/run/docker-run](http://iotgateway.net/docs/iotgateway/run/docker-run)
- [源码运行:http://iotgateway.net/docs/iotgateway/run/build-run](http://iotgateway.net/docs/iotgateway/run/build-run)

## 南向
- 支持三菱PLC、Modbus驱动全协议支持、欧姆龙PLC、OPCUA客户端、西门子PLC、ABPLC、MT机床、Fanuc CNC
- 驱动支持二次开发
- [驱动简介](http://iotgateway.net/docs/iotgateway/driver/drvier)
- [驱动二次开发实战之TcpClient](http://iotgateway.net/docs/iotgateway/driver/tcpclient)
- 支持设备数据写入
  ![set-variabl](./images/set-variable.png)  
- 支持计算表达式  
  ![express](./images/express.png)
- 支持变化上传和定时归档
  ![change-uploa](./images/change-upload.png)
  

## 北向
- thingsboard、iotsharp、thingscloud、IoTDB第三方平台
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

## 免责声明
- ## 生产环境使用请做好评估
- ## 项目中OPCUA相关功能仅用作学习及测试
- ## 如使用OPCUA协议请联系OPC基金会进行授权，产生一切纠纷与本项目无关

## 打赏请留微信或QQ
|  微信 | 支付宝 |
| ----- | ---- |
| ![wx-pay](./images/wx-pay.jpg) | ![ali-pay](./images/ali-pay.png) |
