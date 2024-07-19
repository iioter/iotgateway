using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTGateway.DataAccess.Migrations
{
    public partial class CommentAndIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "SystemConfig",
                comment: "传输配置");

            migrationBuilder.AlterTable(
                name: "RpcLogs",
                comment: "RPC日志");

            migrationBuilder.AlterTable(
                name: "Drivers",
                comment: "驱动管理");

            migrationBuilder.AlterTable(
                name: "DeviceVariables",
                comment: "变量配置");

            migrationBuilder.AlterTable(
                name: "Devices",
                comment: "设备维护");

            migrationBuilder.AlterTable(
                name: "DeviceConfigs",
                comment: "通讯配置");

            migrationBuilder.AlterColumn<string>(
                name: "MqttUPwd",
                table: "SystemConfig",
                type: "TEXT",
                nullable: true,
                comment: "Mqtt密码",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MqttUName",
                table: "SystemConfig",
                type: "TEXT",
                nullable: true,
                comment: "Mqtt用户名",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MqttPort",
                table: "SystemConfig",
                type: "INTEGER",
                nullable: false,
                comment: "Mqtt端口",
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "MqttIp",
                table: "SystemConfig",
                type: "TEXT",
                nullable: true,
                comment: "Mqtt服务器",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IoTPlatformType",
                table: "SystemConfig",
                type: "INTEGER",
                nullable: false,
                comment: "输出平台",
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "GatewayName",
                table: "SystemConfig",
                type: "TEXT",
                nullable: true,
                comment: "网关名称",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "SystemConfig",
                type: "TEXT",
                nullable: true,
                comment: "ClientId",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "RpcLogs",
                type: "TEXT",
                nullable: false,
                comment: "开始时间",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "RpcSide",
                table: "RpcLogs",
                type: "INTEGER",
                nullable: false,
                comment: "发起方",
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Params",
                table: "RpcLogs",
                type: "TEXT",
                nullable: true,
                comment: "请求参数",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Method",
                table: "RpcLogs",
                type: "TEXT",
                nullable: true,
                comment: "方法",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsSuccess",
                table: "RpcLogs",
                type: "INTEGER",
                nullable: false,
                comment: "是否成功",
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "RpcLogs",
                type: "TEXT",
                nullable: false,
                comment: "结束时间",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "RpcLogs",
                type: "TEXT",
                nullable: true,
                comment: "所属设备",
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RpcLogs",
                type: "TEXT",
                nullable: true,
                comment: "描述",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Drivers",
                type: "TEXT",
                nullable: true,
                comment: "文件名",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DriverName",
                table: "Drivers",
                type: "TEXT",
                nullable: true,
                comment: "驱动名",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AuthorizesNum",
                table: "Drivers",
                type: "INTEGER",
                nullable: false,
                comment: "剩余授权数",
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "AssembleName",
                table: "Drivers",
                type: "TEXT",
                nullable: true,
                comment: "程序集名",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProtectType",
                table: "DeviceVariables",
                type: "INTEGER",
                nullable: false,
                comment: "权限",
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "DeviceVariables",
                type: "TEXT",
                nullable: true,
                comment: "变量名",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Method",
                table: "DeviceVariables",
                type: "TEXT",
                nullable: true,
                comment: "方法",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsUpload",
                table: "DeviceVariables",
                type: "INTEGER",
                nullable: false,
                comment: "上传",
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsTrigger",
                table: "DeviceVariables",
                type: "INTEGER",
                nullable: false,
                comment: "触发",
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<uint>(
                name: "Index",
                table: "DeviceVariables",
                type: "INTEGER",
                nullable: false,
                comment: "排序",
                oldClrType: typeof(uint),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Expressions",
                table: "DeviceVariables",
                type: "TEXT",
                nullable: true,
                comment: "表达式",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EndianType",
                table: "DeviceVariables",
                type: "INTEGER",
                nullable: false,
                comment: "大小端",
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "DeviceVariables",
                type: "TEXT",
                nullable: true,
                comment: "所属设备",
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeviceAddress",
                table: "DeviceVariables",
                type: "TEXT",
                nullable: true,
                comment: "地址",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "DeviceVariables",
                type: "TEXT",
                nullable: true,
                comment: "描述",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DataType",
                table: "DeviceVariables",
                type: "INTEGER",
                nullable: false,
                comment: "数据类型",
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Alias",
                table: "DeviceVariables",
                type: "TEXT",
                nullable: true,
                comment: "别名",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "Index",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                comment: "排序",
                oldClrType: typeof(uint),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<uint>(
                name: "EnforcePeriod",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                comment: "归档周期ms",
                oldClrType: typeof(uint),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<Guid>(
                name: "DriverId",
                table: "Devices",
                type: "TEXT",
                nullable: true,
                comment: "驱动",
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeviceTypeEnum",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                comment: "类型(组或设备)",
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceName",
                table: "Devices",
                type: "TEXT",
                nullable: true,
                comment: "名称",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Devices",
                type: "TEXT",
                nullable: true,
                comment: "描述",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "CmdPeriod",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                comment: "指令间隔ms",
                oldClrType: typeof(uint),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "CgUpload",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                comment: "变化上传",
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "AutoStart",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                comment: "启动",
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "DeviceConfigs",
                type: "TEXT",
                nullable: true,
                comment: "值",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EnumInfo",
                table: "DeviceConfigs",
                type: "TEXT",
                nullable: true,
                comment: "备注",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "DeviceConfigs",
                type: "TEXT",
                nullable: true,
                comment: "所属设备",
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeviceConfigName",
                table: "DeviceConfigs",
                type: "TEXT",
                nullable: true,
                comment: "名称",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "DeviceConfigs",
                type: "TEXT",
                nullable: true,
                comment: "描述",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DataSide",
                table: "DeviceConfigs",
                type: "INTEGER",
                nullable: false,
                comment: "属性侧",
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceVariables_DataType",
                table: "DeviceVariables",
                column: "DataType");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceVariables_DeviceAddress",
                table: "DeviceVariables",
                column: "DeviceAddress");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceVariables_Method",
                table: "DeviceVariables",
                column: "Method");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceVariables_Name",
                table: "DeviceVariables",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_AutoStart",
                table: "Devices",
                column: "AutoStart");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceName",
                table: "Devices",
                column: "DeviceName");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceTypeEnum",
                table: "Devices",
                column: "DeviceTypeEnum");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceConfigs_DeviceConfigName",
                table: "DeviceConfigs",
                column: "DeviceConfigName");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceConfigs_Value",
                table: "DeviceConfigs",
                column: "Value");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DeviceVariables_DataType",
                table: "DeviceVariables");

            migrationBuilder.DropIndex(
                name: "IX_DeviceVariables_DeviceAddress",
                table: "DeviceVariables");

            migrationBuilder.DropIndex(
                name: "IX_DeviceVariables_Method",
                table: "DeviceVariables");

            migrationBuilder.DropIndex(
                name: "IX_DeviceVariables_Name",
                table: "DeviceVariables");

            migrationBuilder.DropIndex(
                name: "IX_Devices_AutoStart",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_DeviceName",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_DeviceTypeEnum",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_DeviceConfigs_DeviceConfigName",
                table: "DeviceConfigs");

            migrationBuilder.DropIndex(
                name: "IX_DeviceConfigs_Value",
                table: "DeviceConfigs");

            migrationBuilder.AlterTable(
                name: "SystemConfig",
                oldComment: "传输配置");

            migrationBuilder.AlterTable(
                name: "RpcLogs",
                oldComment: "RPC日志");

            migrationBuilder.AlterTable(
                name: "Drivers",
                oldComment: "驱动管理");

            migrationBuilder.AlterTable(
                name: "DeviceVariables",
                oldComment: "变量配置");

            migrationBuilder.AlterTable(
                name: "Devices",
                oldComment: "设备维护");

            migrationBuilder.AlterTable(
                name: "DeviceConfigs",
                oldComment: "通讯配置");

            migrationBuilder.AlterColumn<string>(
                name: "MqttUPwd",
                table: "SystemConfig",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "Mqtt密码");

            migrationBuilder.AlterColumn<string>(
                name: "MqttUName",
                table: "SystemConfig",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "Mqtt用户名");

            migrationBuilder.AlterColumn<int>(
                name: "MqttPort",
                table: "SystemConfig",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldComment: "Mqtt端口");

            migrationBuilder.AlterColumn<string>(
                name: "MqttIp",
                table: "SystemConfig",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "Mqtt服务器");

            migrationBuilder.AlterColumn<int>(
                name: "IoTPlatformType",
                table: "SystemConfig",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldComment: "输出平台");

            migrationBuilder.AlterColumn<string>(
                name: "GatewayName",
                table: "SystemConfig",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "网关名称");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "SystemConfig",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "ClientId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "RpcLogs",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldComment: "开始时间");

            migrationBuilder.AlterColumn<int>(
                name: "RpcSide",
                table: "RpcLogs",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldComment: "发起方");

            migrationBuilder.AlterColumn<string>(
                name: "Params",
                table: "RpcLogs",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "请求参数");

            migrationBuilder.AlterColumn<string>(
                name: "Method",
                table: "RpcLogs",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "方法");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSuccess",
                table: "RpcLogs",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldComment: "是否成功");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "RpcLogs",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldComment: "结束时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "RpcLogs",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "所属设备");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RpcLogs",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "描述");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Drivers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "文件名");

            migrationBuilder.AlterColumn<string>(
                name: "DriverName",
                table: "Drivers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "驱动名");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorizesNum",
                table: "Drivers",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldComment: "剩余授权数");

            migrationBuilder.AlterColumn<string>(
                name: "AssembleName",
                table: "Drivers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "程序集名");

            migrationBuilder.AlterColumn<int>(
                name: "ProtectType",
                table: "DeviceVariables",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldComment: "权限");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "DeviceVariables",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "变量名");

            migrationBuilder.AlterColumn<string>(
                name: "Method",
                table: "DeviceVariables",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "方法");

            migrationBuilder.AlterColumn<bool>(
                name: "IsUpload",
                table: "DeviceVariables",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldComment: "上传");

            migrationBuilder.AlterColumn<bool>(
                name: "IsTrigger",
                table: "DeviceVariables",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldComment: "触发");

            migrationBuilder.AlterColumn<uint>(
                name: "Index",
                table: "DeviceVariables",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER",
                oldComment: "排序");

            migrationBuilder.AlterColumn<string>(
                name: "Expressions",
                table: "DeviceVariables",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "表达式");

            migrationBuilder.AlterColumn<int>(
                name: "EndianType",
                table: "DeviceVariables",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldComment: "大小端");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "DeviceVariables",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "所属设备");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceAddress",
                table: "DeviceVariables",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "地址");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "DeviceVariables",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "描述");

            migrationBuilder.AlterColumn<int>(
                name: "DataType",
                table: "DeviceVariables",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldComment: "数据类型");

            migrationBuilder.AlterColumn<string>(
                name: "Alias",
                table: "DeviceVariables",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "别名");

            migrationBuilder.AlterColumn<uint>(
                name: "Index",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER",
                oldComment: "排序");

            migrationBuilder.AlterColumn<uint>(
                name: "EnforcePeriod",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER",
                oldComment: "归档周期ms");

            migrationBuilder.AlterColumn<Guid>(
                name: "DriverId",
                table: "Devices",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "驱动");

            migrationBuilder.AlterColumn<int>(
                name: "DeviceTypeEnum",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldComment: "类型(组或设备)");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceName",
                table: "Devices",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "名称");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Devices",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "描述");

            migrationBuilder.AlterColumn<uint>(
                name: "CmdPeriod",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "INTEGER",
                oldComment: "指令间隔ms");

            migrationBuilder.AlterColumn<bool>(
                name: "CgUpload",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldComment: "变化上传");

            migrationBuilder.AlterColumn<bool>(
                name: "AutoStart",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldComment: "启动");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "DeviceConfigs",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "值");

            migrationBuilder.AlterColumn<string>(
                name: "EnumInfo",
                table: "DeviceConfigs",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "备注");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeviceId",
                table: "DeviceConfigs",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "所属设备");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceConfigName",
                table: "DeviceConfigs",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "名称");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "DeviceConfigs",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "描述");

            migrationBuilder.AlterColumn<int>(
                name: "DataSide",
                table: "DeviceConfigs",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldComment: "属性侧");
        }
    }
}
