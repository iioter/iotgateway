#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 518

RUN apt-get update
RUN apt-get install libgdiplus -y
RUN apt-get install nano -y

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["IoTGateway/IoTGateway.csproj", "IoTGateway/"]
COPY ["IoTGateway.ViewModel/IoTGateway.ViewModel.csproj", "IoTGateway.ViewModel/"]
COPY ["IoTGateway.Model/IoTGateway.Model.csproj", "IoTGateway.Model/"]
COPY ["IoTGateway.DataAccess/IoTGateway.DataAccess.csproj", "IoTGateway.DataAccess/"]
RUN dotnet restore "IoTGateway/IoTGateway.csproj"
COPY . .
WORKDIR "/src/IoTGateway"
RUN dotnet build "IoTGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IoTGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV TZ=Asia/Shanghai
ENTRYPOINT ["dotnet", "IoTGateway.dll"]