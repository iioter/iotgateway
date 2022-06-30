#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 518
EXPOSE 1888
EXPOSE 62541
EXPOSE 503

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src



COPY ["IoTGateway/IoTGateway.csproj", "IoTGateway/"]
COPY ["IoTGateway.ViewModel/IoTGateway.ViewModel.csproj", "IoTGateway.ViewModel/"]
COPY ["Plugins/Plugin/Plugin.csproj", "Plugins/Plugin/"]
COPY ["IoTGateway.Model/IoTGateway.Model.csproj", "IoTGateway.Model/"]
COPY ["WalkingTec.Mvvm/WalkingTec.Mvvm.Core/WalkingTec.Mvvm.Core.csproj", "WalkingTec.Mvvm/WalkingTec.Mvvm.Core/"]
COPY ["Plugins/PluginInterface/PluginInterface.csproj", "Plugins/PluginInterface/"]
COPY ["IoTGateway.DataAccess/IoTGateway.DataAccess.csproj", "IoTGateway.DataAccess/"]
COPY ["WalkingTec.Mvvm/WalkingTec.Mvvm.TagHelpers.LayUI/WalkingTec.Mvvm.TagHelpers.LayUI.csproj", "WalkingTec.Mvvm/WalkingTec.Mvvm.TagHelpers.LayUI/"]
COPY ["WalkingTec.Mvvm/WalkingTec.Mvvm.Mvc/WalkingTec.Mvvm.Mvc.csproj", "WalkingTec.Mvvm/WalkingTec.Mvvm.Mvc/"]




# 测试的内容
# ADD hello /usr/local # 将hello文件添加到容器的/usr/local下（支持解压）感觉比copy好有


COPY ["Plugins/Drivers/DriverAllenBradley/DriverAllenBradley.csproj", "Plugins/Drivers/DriverAllenBradley/"]
COPY ["Plugins/Drivers/DriverFanuc/DriverFanuc.csproj", "Plugins/Drivers/DriverFanuc/"]
COPY ["Plugins/Drivers/DriverFanucHsl/DriverFanucHsl.csproj", "Plugins/Drivers/DriverFanucHsl/"]
COPY ["Plugins/Drivers/DriverMitsubishi/DriverMitsubishi.csproj", "Plugins/Drivers/DriverMitsubishi/"]
COPY ["Plugins/Drivers/DriverModbusMaster/DriverModbusMaster.csproj", "Plugins/Drivers/DriverModbusMaster/"]
COPY ["Plugins/Drivers/DriverMTConnect/DriverMTConnect.csproj", "Plugins/Drivers/DriverMTConnect/"]
COPY ["Plugins/Drivers/DriverOmronFins/DriverOmronFins.csproj", "Plugins/Drivers/DriverOmronFins/"]
COPY ["Plugins/Drivers/DriverOPCUaClient/DriverOPCUaClient.csproj", "Plugins/Drivers/DriverOPCUaClient/"]
COPY ["Plugins/Drivers/DriverSiemensS7/DriverSiemensS7.csproj", "Plugins/Drivers/DriverSiemensS7/"]
COPY ["Plugins/Drivers/DriverSimTcpClient/DriverSimTcpClient.csproj", "Plugins/Drivers/DriverSimTcpClient/"]


#结束测试



RUN dotnet restore "IoTGateway/IoTGateway.csproj"
COPY . .
WORKDIR "/src/IoTGateway"
RUN dotnet build "IoTGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IoTGateway.csproj" -c Release -o /app/publish

 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# test
RUN dotnet restore "/src/Plugins/Drivers/DriverModbusMaster/DriverModbusMaster.csproj"
RUN dotnet build "/src/Plugins/Drivers/DriverModbusMaster/DriverModbusMaster.csproj" -c Release -o /app/drivers/net6.0

RUN ls /app/drivers/net6.0 -l
#end test

ENV TZ=Asia/Shanghai
ENTRYPOINT ["dotnet", "IoTGateway.dll"]
#ENTRYPOINT ["ls","-l"]    
#ENTRYPOINT ["pwd"," "]    