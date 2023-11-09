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

RUN dotnet restore "IoTGateway/IoTGateway.csproj"
COPY . .
WORKDIR "/src/IoTGateway"
RUN dotnet build "IoTGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IoTGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY drivers /app/drivers/
COPY 3d /app/wwwroot/3d/
COPY --from=publish /app/publish .

ENV TZ=Asia/Shanghai
ENTRYPOINT ["dotnet", "IoTGateway.dll"]
