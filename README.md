# iotgateway 
## [iotgateway在线体验](http://42.193.160.84:518/)访问：http://42.193.160.84:518/
## [iotgateway教程文档](http://42.193.160.84/)访问：http://iotgateway.net/
## github地址:[iotgateway](https://github.com/iioter/iotgateway/) https://github.com/iioter/iotgateway
## gitee地址:[iotgateway](https://gitee.com/iioter/iotgateway/) https://gitee.com/iioter/iotgateway
基于.net6的跨平台物联网网关。通过可视化配置，轻松的连接到你的任何设备和系统(如PLC、扫码枪、CNC、数据库、串口设备、上位机、OPC Server、OPC UA Server、Mqtt Server等)，从而与 Thingsboard、IoTSharp或您自己的物联网平台进行双向数据通讯。提供简单的驱动开发接口；当然也可以进行边缘计算。

* 抛砖引玉，共同进步
* 基于.net6的开源物联网网关
* 浏览器可视化的配置方式实现数据采集(使用wtm开发)
* 物联网网关mqtt+opcua双通道实时输出，支持thingsboard、iotsharp等第三方平台
* 内置Mqtt服务端,支持websocket，进行标准mqtt输出。本地端口1888 admin 000000
* 内置OPCUA服务端,数据实时更新。匿名本地访问:opc.tcp://localhost:62541/Quickstarts/ReferenceServer
* 内置三菱PLC、Modbus驱动全协议支持、MT机床、欧姆龙PLC、OPCUA客户端、西门子PLC、AB(罗克韦尔)PLC、
* 增支持计算表达式，数据边缘预处理
* 支持驱动二次开发
* 目前只支持遥测数据上传，后续支持属性的双向通信
* 简单集成了web组态项目
* 3D可视化展示Demo


# 免责声明
## 生产环境使用请做好评估；
## 项目中OPCUA相关功能仅用作学习及测试，如使用OPCUA协议请联系OPC基金会进行授权，产生一切纠纷与本项目无关

# 体验
1. 在线体验[iotgateway](http://42.193.160.84:518/)后台：http://42.193.160.84:518/
2. 用户名 admin 密码 000000
3. 内置Modbustcp模拟设备 ip 172.17.0.1 port 503 不要修改，否则连不上
4. 其中modbus地址0-1为固定值，2-9为随机值，10-19为0
5. 外网访问测试modbus设备，请连接:42.193.160.84:503，进行标准modbus协议读写
6. 外网访问测试mqtt服务器，42.193.160.84:1888 admin 000000
7. 外网访问测试opcua服务，opc.tcp://42.193.160.84:62541/Quickstarts/ReferenceServer 匿名访问
8. 想要通过mqtt接收数据，请连接mqttserver:42.193.160.84,1888 admin 000000；订阅topic: v1/gateway/telemetry
## 3D可视化(数字孪生?)
![easteregg](https://user-images.githubusercontent.com/29589505/147798707-cf4de713-9bb6-48c1-88a6-ac9f703f89d2.gif)
## 数据实时更新
![iotgateway](https://user-images.githubusercontent.com/29589505/147055534-3954039c-2723-4fc3-8981-c9ce3bb0163e.gif)
## RPC反向控制
![4 rpc](https://user-images.githubusercontent.com/29589505/163549468-33ef7017-391c-4c80-833e-fab08f4bb569.gif)
## 设备数据更新OPCUA服务端
![795d56161e78c770a2ca4d32f8e6b73](https://user-images.githubusercontent.com/29589505/147349299-f1fc1152-c758-47a4-a0c1-85da1895db9c.png)
## 接入组态项目
![iotgateway](https://user-images.githubusercontent.com/29589505/147056511-14611d19-8498-4a3c-bd67-3749ab75462f.gif)
## 组态配置
![image](https://user-images.githubusercontent.com/29589505/146880219-454ffa90-a153-47a9-9b54-962bf95bfa7f.png)
## 通过MQTT订阅数据
![image](https://user-images.githubusercontent.com/29589505/145837715-c0529db4-f2aa-47f7-aca6-db101642f820.png)


# 运行
## windows主机运行：
1. [下载Releasev0.4.0](https://github.com/iioter/iotgateway/releases/download/v0.4.0/iotgateway-v0.4.0.zip)发布版本
2. [下载.net6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) sdk或runtime
3. 安装.net6 
4. 解压release包，运行IoTGateway.exe
5. 访问[iotgateway](http://localhost:518/)后台：http://localhost:518

## linux/amd64,win/amd64 docker运行(官方仓)
1. docker run -d -p 518:518 -p 1888:1888 -p 62541:62541 -p 503:503 --name iotgateway --restart always 15261671110/iotgateway
## linux/amd64,win/amd64 docker运行(阿里仓)
1. docker pull registry.cn-hangzhou.aliyuncs.com/wanghaidong/iotgateway 
2. docker tag registry.cn-hangzhou.aliyuncs.com/wanghaidong/iotgateway 15261671110/iotgateway
3. docker run -d -p 518:518 -p 1888:1888 -p 62541:62541 -p 503:503 --name iotgateway --restart always 15261671110/iotgateway
 
## linux/arm64 docker运行(官方仓)
1. docker run -d -p 518:518 -p 1888:1888 -p 62541:62541 -p 503:503 --name iotgateway --restart always 15261671110/iotgateway:arm
## linux/arm64 docker运行(阿里仓)
1. docker pull registry.cn-hangzhou.aliyuncs.com/wanghaidong/iotgateway:arm 
2. docker tag registry.cn-hangzhou.aliyuncs.com/wanghaidong/iotgateway:arm 15261671110/iotgateway
3. docker run -d -p 518:518 -p 1888:1888 -p 62541:62541 -p 503:503 --name iotgateway --restart always 15261671110/iotgateway

## 登入系统
1. 用户名 admin,密码 000000
2. 打开发布文件路径下的ReadMe文件夹中的手摸手，按照顺序添加设备进行采集
# 采集配置
![1 登录](https://user-images.githubusercontent.com/29589505/145705166-d5818557-4ba1-4e7b-b8d8-8f5f4b28868f.png)
![2 首页](https://user-images.githubusercontent.com/29589505/145705168-94b3ff0c-2f5c-4a31-8e83-c2ed799320ce.png)
![3 网关配置](https://user-images.githubusercontent.com/29589505/145705172-2a10d11b-436d-4a2c-86bf-cf6aa5dade07.png)
![4 设备维护](https://user-images.githubusercontent.com/29589505/145705173-7c9ccc0e-e1ab-49ba-af28-0e5c654a57e3.png)
![5 设备参数配置](https://user-images.githubusercontent.com/29589505/145705174-95a14642-dd30-4e73-803d-5f998f297842.png)
![6 设备变量配置](https://user-images.githubusercontent.com/29589505/145705175-fb11013f-55f8-4123-b770-aaf41706a7aa.png)
![7 设备变量配置-新建](https://user-images.githubusercontent.com/29589505/145705178-52c7580f-1793-4c9a-935b-658d21946aa1.png)
# thingsboard接入教程
![0 准备一个modsim 或者modbus tcp设备](https://user-images.githubusercontent.com/29589505/145705255-18e8d649-6a08-4dc7-96b3-6bdf8fc23cb4.png)
![1 thingsboard  新建网关](https://user-images.githubusercontent.com/29589505/145705256-1e01030f-2ccb-464e-a3cc-784d5a7c1c91.png)
![2 gateway 修改网关传输配置](https://user-images.githubusercontent.com/29589505/145705260-3f9f36c9-c163-4853-9787-677926989af3.png)
![3 gateway 创建组](https://user-images.githubusercontent.com/29589505/145705261-c605c825-d463-491d-ad32-1a3c245e2ee3.png)
![4 gateway 创建设备](https://user-images.githubusercontent.com/29589505/145705263-d3ea2a6b-7ea3-491a-bcd1-20e8dc770922.png)
![5 gateway 配置设备参数](https://user-images.githubusercontent.com/29589505/145705264-6db386ba-e68e-4a7f-acff-44f55ce25e73.png)
![6 gateway 新建变量](https://user-images.githubusercontent.com/29589505/145705265-44a0da5d-6d16-4463-a755-626d93dc121c.png)
![6 gateway 修改设备为自启动](https://user-images.githubusercontent.com/29589505/145705269-c816789c-cd67-4c01-973f-ae4f10eb41d9.png)
![7 thingsboard 查看到设备和数据](https://user-images.githubusercontent.com/29589505/145705270-31d8884f-7f6f-4ff5-a6bb-1d57a97012f4.png)
![8 gateway 查看到数据](https://user-images.githubusercontent.com/29589505/145705271-cb80b80e-006e-4312-8843-6d0ae9457cb1.png)




# 善假于物
1. [WTM(MIT)](https://github.com/dotnetcore/WTM)
2. [OPCUA(OPCUA)](https://github.com/OPCFoundation/UA-.NETStandard)
3. [NModbus4(MIT)](https://github.com/NModbus4/NModbus4)
4. [S7NetPlus(MIT)](https://github.com/S7NetPlus/s7netplus)
5. [MQTTnet(MIT)](https://github.com/chkr1011/MQTTnet)
6. [DynamicExpresso(MIT)](https://github.com/dynamicexpresso/DynamicExpresso)
7. [EFCore(MIT)](https://github.com/dotnet/efcore)
8. [LayUI(MIT)](https://github.com/sentsin/layui)
9. [SQLite](https://github.com/sqlite/sqlite)
10. [mtconnect ](https://github.com/ctacke/mtconnect)
