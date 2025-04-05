using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTGateway.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CommentAndIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    DriverName = table.Column<string>(type: "TEXT", nullable: true, comment: "驱动名"),
                    FileName = table.Column<string>(type: "TEXT", nullable: true, comment: "文件名"),
                    AssembleName = table.Column<string>(type: "TEXT", nullable: true, comment: "程序集名"),
                    AuthorizesNum = table.Column<int>(type: "INTEGER", nullable: false, comment: "剩余授权数"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.ID);
                },
                comment: "驱动管理");

            migrationBuilder.CreateTable(
                name: "SystemConfig",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    GatewayName = table.Column<string>(type: "TEXT", nullable: true, comment: "网关名称"),
                    ClientId = table.Column<string>(type: "TEXT", nullable: true, comment: "ClientId"),
                    MqttIp = table.Column<string>(type: "TEXT", nullable: true, comment: "Mqtt服务器"),
                    MqttPort = table.Column<int>(type: "INTEGER", nullable: false, comment: "Mqtt端口"),
                    MqttUName = table.Column<string>(type: "TEXT", nullable: true, comment: "Mqtt用户名"),
                    MqttUPwd = table.Column<string>(type: "TEXT", nullable: true, comment: "Mqtt密码"),
                    IoTPlatformType = table.Column<int>(type: "INTEGER", nullable: false, comment: "输出平台"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemConfig", x => x.ID);
                },
                comment: "传输配置");

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    DeviceName = table.Column<string>(type: "TEXT", nullable: true, comment: "名称"),
                    Index = table.Column<uint>(type: "INTEGER", nullable: false, comment: "排序"),
                    Description = table.Column<string>(type: "TEXT", nullable: true, comment: "描述"),
                    DriverId = table.Column<Guid>(type: "TEXT", nullable: true, comment: "驱动"),
                    AutoStart = table.Column<bool>(type: "INTEGER", nullable: false, comment: "启动"),
                    CgUpload = table.Column<bool>(type: "INTEGER", nullable: false, comment: "变化上传"),
                    EnforcePeriod = table.Column<uint>(type: "INTEGER", nullable: false, comment: "归档周期ms"),
                    CmdPeriod = table.Column<uint>(type: "INTEGER", nullable: false, comment: "指令间隔ms"),
                    DeviceTypeEnum = table.Column<int>(type: "INTEGER", nullable: false, comment: "类型(组或设备)"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", nullable: true),
                    ParentId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Devices_Devices_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Devices",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Devices_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "ID");
                },
                comment: "设备维护");

            migrationBuilder.CreateTable(
                name: "DeviceConfigs",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    DeviceConfigName = table.Column<string>(type: "TEXT", nullable: true, comment: "名称"),
                    DataSide = table.Column<int>(type: "INTEGER", nullable: false, comment: "属性侧"),
                    Description = table.Column<string>(type: "TEXT", nullable: true, comment: "描述"),
                    Value = table.Column<string>(type: "TEXT", nullable: true, comment: "值"),
                    EnumInfo = table.Column<string>(type: "TEXT", nullable: true, comment: "备注"),
                    DeviceId = table.Column<Guid>(type: "TEXT", nullable: true, comment: "所属设备"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceConfigs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DeviceConfigs_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "ID");
                },
                comment: "通讯配置");

            migrationBuilder.CreateTable(
                name: "DeviceVariables",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true, comment: "变量名"),
                    Description = table.Column<string>(type: "TEXT", nullable: true, comment: "描述"),
                    Method = table.Column<string>(type: "TEXT", nullable: true, comment: "方法"),
                    DeviceAddress = table.Column<string>(type: "TEXT", nullable: true, comment: "地址"),
                    DataType = table.Column<int>(type: "INTEGER", nullable: false, comment: "数据类型"),
                    IsTrigger = table.Column<bool>(type: "INTEGER", nullable: false, comment: "触发"),
                    EndianType = table.Column<int>(type: "INTEGER", nullable: false, comment: "大小端"),
                    Expressions = table.Column<string>(type: "TEXT", nullable: true, comment: "表达式"),
                    IsUpload = table.Column<bool>(type: "INTEGER", nullable: false, comment: "上传"),
                    ProtectType = table.Column<int>(type: "INTEGER", nullable: false, comment: "权限"),
                    Index = table.Column<uint>(type: "INTEGER", nullable: false, comment: "排序"),
                    DeviceId = table.Column<Guid>(type: "TEXT", nullable: true, comment: "所属设备"),
                    Alias = table.Column<string>(type: "TEXT", nullable: true, comment: "别名")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceVariables", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DeviceVariables_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "ID");
                },
                comment: "变量配置");

            migrationBuilder.CreateTable(
                name: "RpcLogs",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    RpcSide = table.Column<int>(type: "INTEGER", nullable: false, comment: "发起方"),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "开始时间"),
                    DeviceId = table.Column<Guid>(type: "TEXT", nullable: true, comment: "所属设备"),
                    Method = table.Column<string>(type: "TEXT", nullable: true, comment: "方法"),
                    Params = table.Column<string>(type: "TEXT", nullable: true, comment: "请求参数"),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "结束时间"),
                    IsSuccess = table.Column<bool>(type: "INTEGER", nullable: false, comment: "是否成功"),
                    Description = table.Column<string>(type: "TEXT", nullable: true, comment: "描述")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RpcLogs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RpcLogs_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "ID");
                },
                comment: "RPC日志");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceConfigs_DeviceConfigName",
                table: "DeviceConfigs",
                column: "DeviceConfigName");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceConfigs_DeviceId",
                table: "DeviceConfigs",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceConfigs_Value",
                table: "DeviceConfigs",
                column: "Value");

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
                name: "IX_Devices_DriverId",
                table: "Devices",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ParentId",
                table: "Devices",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceVariables_DataType",
                table: "DeviceVariables",
                column: "DataType");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceVariables_DeviceAddress",
                table: "DeviceVariables",
                column: "DeviceAddress");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceVariables_DeviceId",
                table: "DeviceVariables",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceVariables_Method",
                table: "DeviceVariables",
                column: "Method");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceVariables_Name",
                table: "DeviceVariables",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_RpcLogs_DeviceId",
                table: "RpcLogs",
                column: "DeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceConfigs");

            migrationBuilder.DropTable(
                name: "DeviceVariables");

            migrationBuilder.DropTable(
                name: "RpcLogs");

            migrationBuilder.DropTable(
                name: "SystemConfig");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Drivers");
        }
    }
}
