FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# 将同级目录下的 app 文件夹拷贝到镜像内的 /app 目录
COPY app /app
# 暴露需要的端口
EXPOSE 518
EXPOSE 1888
EXPOSE 503

# 以 base 为基础构建最终镜像
FROM base AS final
# 设置时区为上海
ENV TZ=Asia/Shanghai
# 设置容器启动命令，启动 IoTGateway.dll
ENTRYPOINT ["dotnet", "IoTGateway.dll"]