# iotgateway
## github地址:[iotgateway](https://github.com/iioter/iotgateway/) https://github.com/iioter/iotgateway
## gitee地址:[iotgateway](https://gitee.com/wang_haidong/iotgateway/) https://gitee.com/wang_haidong/iotgateway
基于.net5的跨平台物联网网关。通过可视化配置，轻松的连接到你的任何设备和系统(如PLC、扫码枪、CNC、数据库、串口设备、上位机、OPC Server、OPC UA Server、Mqtt Server等)，从而与 Thingsboard、IoTSharp或您自己的物联网平台进行双向数据通讯。提供简单的驱动开发接口；当然也可以进行边缘计算。
* 物联网网关mqtt输出，支持thingsboard
* 抛砖引玉，共同进步
* 可视化的配置方式实现数据采集(使用wtm开发)
* 基于.net5的开源物联网网关
* 内置Mqtt服务端,支持websocket，端口1888,/mqtt，可查看客户端列表
* 内置Modbus驱动全协议支持
* 内置西门子PLC驱动
* 增加计算表达式
* 支持驱动二次开发（短期内会提供三菱、fanuc通讯）
* 数据通过mqtt推送，支持thingsboard
* 目前只支持遥测数据上传，后续支持属性的双向通信
* 简单集成了web组态项目
# 体验
1. 在线体验[iotgateway](http://wanghaidong.cloud:518/)后台：http://wanghaidong.cloud:518/
2. 用户名 admin 密码 000000
3. 内置Modbustcp模拟设备 ip 172.17.0.1 port 16051 不要修改，否则连不上
4. 其中modbus地址0-1为固定值，2-9为随机值，10-19为0
5. 想要外网访问modbus设备，请连接:wanghaidong.cloud:16051，进行标准modbus协议读写
6. 想要通过mqtt接收数据，请连接mqttserver:wanghaidong.cloud,1888 admin 000000；订阅topic: v1/gateway/telemetry
![image](https://user-images.githubusercontent.com/29589505/145837715-c0529db4-f2aa-47f7-aca6-db101642f820.png)
![image](https://user-images.githubusercontent.com/29589505/146733538-d9a83a0e-b2af-40f9-acb5-4cb9fa5aa3c8.png)
![image](https://user-images.githubusercontent.com/29589505/146880197-7dc76b67-71c0-47d1-80a1-74ca898da8dd.png)
![image](https://user-images.githubusercontent.com/29589505/146880219-454ffa90-a153-47a9-9b54-962bf95bfa7f.png)


# 运行
## windows运行：
1. [下载Releasev0.02](https://github.com/iioter/iotgateway/releases/download/v0.01/iotgateway-winx64-v0.02.zip)发布版本
2. [下载.net5](https://dotnet.microsoft.com/en-us/download/dotnet/5.0) sdk或runtime
3. 安装.net5 
4. 解压release包，运行IoTGateway.exe
5. 访问[iotgateway](http://localhost:518/)后台：http://localhost:518
## linux docker运行
1. docker pull registry.cn-hangzhou.aliyuncs.com/wanghaidong/iotgateway
2. docker tag registry.cn-hangzhou.aliyuncs.com/wanghaidong/iotgateway 15261671110/iotgateway
3. docker run -d -p 518:518 -p 1888:1888 --name iotgateway --restart always 15261671110/iotgateway
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


# 声明
## 君子性非异也，善假于物也
1. [WTM(MIT)](https://github.com/dotnetcore/WTM)
2. [NModbus4(MIT)](https://github.com/NModbus4/NModbus4)
2. [S7NetPlus(MIT)](https://github.com/S7NetPlus/s7netplus)
2. [MQTTnet(MIT)](https://github.com/chkr1011/MQTTnet)
4. [DynamicExpresso(MIT)](https://github.com/dynamicexpresso/DynamicExpresso)
3. [EFCore(MIT)](https://github.com/dotnet/efcore)
4. [LayUI(MIT)](https://github.com/sentsin/layui)
5. [SQLite](https://github.com/sqlite/sqlite)
