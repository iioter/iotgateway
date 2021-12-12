using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IoTGateway.DataAccess.Migrations
{
    public partial class ini : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActionLogs",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    ModuleName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    ActionName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    ITCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ActionUrl = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    ActionTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Duration = table.Column<double>(type: "REAL", nullable: false),
                    Remark = table.Column<string>(type: "TEXT", nullable: true),
                    IP = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    LogType = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionLogs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DataPrivileges",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserCode = table.Column<string>(type: "TEXT", nullable: true),
                    GroupCode = table.Column<string>(type: "TEXT", nullable: true),
                    TableName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    RelateId = table.Column<string>(type: "TEXT", nullable: true),
                    Domain = table.Column<string>(type: "TEXT", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataPrivileges", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    DriverName = table.Column<string>(type: "TEXT", nullable: true),
                    FileName = table.Column<string>(type: "TEXT", nullable: true),
                    AssembleName = table.Column<string>(type: "TEXT", nullable: true),
                    AuthorizesNum = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FileAttachments",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    FileExt = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: true),
                    Length = table.Column<long>(type: "INTEGER", nullable: false),
                    UploadTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SaveMode = table.Column<string>(type: "TEXT", nullable: true),
                    FileData = table.Column<byte[]>(type: "BLOB", nullable: true),
                    ExtraInfo = table.Column<string>(type: "TEXT", nullable: true),
                    HandlerInfo = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileAttachments", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FrameworkGroups",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    GroupCode = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    GroupName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GroupRemark = table.Column<string>(type: "TEXT", nullable: true),
                    TenantCode = table.Column<string>(type: "TEXT", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkGroups", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FrameworkMenus",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    PageName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ActionName = table.Column<string>(type: "TEXT", nullable: true),
                    ModuleName = table.Column<string>(type: "TEXT", nullable: true),
                    FolderOnly = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsInherit = table.Column<bool>(type: "INTEGER", nullable: false),
                    ClassName = table.Column<string>(type: "TEXT", nullable: true),
                    MethodName = table.Column<string>(type: "TEXT", nullable: true),
                    Domain = table.Column<string>(type: "TEXT", nullable: true),
                    ShowOnMenu = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsInside = table.Column<bool>(type: "INTEGER", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    Icon = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ParentId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkMenus", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FrameworkMenus_FrameworkMenus_ParentId",
                        column: x => x.ParentId,
                        principalTable: "FrameworkMenus",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FrameworkRoles",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleCode = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    RoleName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    RoleRemark = table.Column<string>(type: "TEXT", nullable: true),
                    TenantCode = table.Column<string>(type: "TEXT", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkRoles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FrameworkUserGroups",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserCode = table.Column<string>(type: "TEXT", nullable: false),
                    GroupCode = table.Column<string>(type: "TEXT", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkUserGroups", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FrameworkUserRoles",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserCode = table.Column<string>(type: "TEXT", nullable: false),
                    RoleCode = table.Column<string>(type: "TEXT", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkUserRoles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PersistedGrants",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UserCode = table.Column<string>(type: "TEXT", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Expiration = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RefreshToken = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedGrants", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SystemConfig",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    GatewayName = table.Column<string>(type: "TEXT", nullable: true),
                    MqttIp = table.Column<string>(type: "TEXT", nullable: true),
                    MqttPort = table.Column<int>(type: "INTEGER", nullable: false),
                    MqttUName = table.Column<string>(type: "TEXT", nullable: true),
                    MqttUPwd = table.Column<string>(type: "TEXT", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemConfig", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    DeviceName = table.Column<string>(type: "TEXT", nullable: true),
                    Index = table.Column<uint>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DriverId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AutoStart = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeviceTypeEnum = table.Column<int>(type: "INTEGER", nullable: false),
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
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Devices_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FrameworkUsers",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Gender = table.Column<int>(type: "INTEGER", nullable: true),
                    CellPhone = table.Column<string>(type: "TEXT", nullable: true),
                    HomePhone = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    ZipCode = table.Column<string>(type: "TEXT", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ITCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IsValid = table.Column<bool>(type: "INTEGER", nullable: false),
                    PhotoId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TenantCode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkUsers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FrameworkUsers_FileAttachments_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "FileAttachments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FunctionPrivileges",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleCode = table.Column<string>(type: "TEXT", nullable: true),
                    MenuItemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Allowed = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionPrivileges", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FunctionPrivileges_FrameworkMenus_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "FrameworkMenus",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceConfigs",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    DeviceConfigName = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: true),
                    EnumInfo = table.Column<string>(type: "TEXT", nullable: true),
                    DeviceId = table.Column<Guid>(type: "TEXT", nullable: true),
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
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeviceVariables",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Method = table.Column<string>(type: "TEXT", nullable: true),
                    DeviceAddress = table.Column<string>(type: "TEXT", nullable: true),
                    DataType = table.Column<int>(type: "INTEGER", nullable: false),
                    ValueFactor = table.Column<double>(type: "REAL", nullable: false),
                    ProtectType = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceVariables", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DeviceVariables_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceConfigs_DeviceId",
                table: "DeviceConfigs",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DriverId",
                table: "Devices",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ParentId",
                table: "Devices",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceVariables_DeviceId",
                table: "DeviceVariables",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_FrameworkMenus_ParentId",
                table: "FrameworkMenus",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_FrameworkUsers_PhotoId",
                table: "FrameworkUsers",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionPrivileges_MenuItemId",
                table: "FunctionPrivileges",
                column: "MenuItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionLogs");

            migrationBuilder.DropTable(
                name: "DataPrivileges");

            migrationBuilder.DropTable(
                name: "DeviceConfigs");

            migrationBuilder.DropTable(
                name: "DeviceVariables");

            migrationBuilder.DropTable(
                name: "FrameworkGroups");

            migrationBuilder.DropTable(
                name: "FrameworkRoles");

            migrationBuilder.DropTable(
                name: "FrameworkUserGroups");

            migrationBuilder.DropTable(
                name: "FrameworkUserRoles");

            migrationBuilder.DropTable(
                name: "FrameworkUsers");

            migrationBuilder.DropTable(
                name: "FunctionPrivileges");

            migrationBuilder.DropTable(
                name: "PersistedGrants");

            migrationBuilder.DropTable(
                name: "SystemConfig");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "FileAttachments");

            migrationBuilder.DropTable(
                name: "FrameworkMenus");

            migrationBuilder.DropTable(
                name: "Drivers");
        }
    }
}
