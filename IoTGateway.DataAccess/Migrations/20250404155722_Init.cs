using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTGateway.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Elsa");

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
                    TenantCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
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
                name: "Bookmarks",
                schema: "Elsa",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Hash = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    ModelType = table.Column<string>(type: "TEXT", nullable: false),
                    ActivityType = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    ActivityId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    WorkflowInstanceId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    CorrelationId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookmarks", x => x.Id);
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
                    TenantCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
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
                    HandlerInfo = table.Column<string>(type: "TEXT", nullable: true),
                    TenantCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
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
                    GroupCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GroupName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GroupRemark = table.Column<string>(type: "TEXT", nullable: true),
                    Manager = table.Column<string>(type: "TEXT", nullable: true),
                    TenantCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ParentId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkGroups", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FrameworkGroups_FrameworkGroups_ParentId",
                        column: x => x.ParentId,
                        principalTable: "FrameworkGroups",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "FrameworkMenus",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    PageName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
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
                    TenantAllowed = table.Column<bool>(type: "INTEGER", nullable: true),
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
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "FrameworkRoles",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    RoleName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    RoleRemark = table.Column<string>(type: "TEXT", nullable: true),
                    TenantCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
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
                name: "FrameworkTenants",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    TCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TDb = table.Column<string>(type: "TEXT", nullable: true),
                    TDbType = table.Column<int>(type: "INTEGER", nullable: true),
                    DbContext = table.Column<string>(type: "TEXT", nullable: true),
                    TDomain = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    TenantCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    EnableSub = table.Column<bool>(type: "INTEGER", nullable: false),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkTenants", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FrameworkUserGroups",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GroupCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TenantCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
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
                    UserCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    RoleCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TenantCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
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
                name: "FrameworkWorkflows",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Tag = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    WorkflowName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    ModelType = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    ModelID = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Submitter = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    WorkflowId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ActivityId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TenantCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameworkWorkflows", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FunctionPrivileges",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleCode = table.Column<string>(type: "TEXT", nullable: true),
                    MenuItemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Allowed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TenantCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdateBy = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionPrivileges", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Triggers",
                schema: "Elsa",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Hash = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    ModelType = table.Column<string>(type: "TEXT", nullable: false),
                    ActivityType = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    ActivityId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    WorkflowDefinitionId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Triggers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowDefinitions",
                schema: "Elsa",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    DefinitionId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    IsSingleton = table.Column<bool>(type: "INTEGER", nullable: false),
                    PersistenceBehavior = table.Column<int>(type: "INTEGER", nullable: false),
                    DeleteCompletedInstances = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPublished = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsLatest = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tag = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Data = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowExecutionLogRecords",
                schema: "Elsa",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    WorkflowInstanceId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    ActivityType = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    ActivityId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    EventName = table.Column<string>(type: "TEXT", nullable: true),
                    Message = table.Column<string>(type: "TEXT", nullable: true),
                    Source = table.Column<string>(type: "TEXT", nullable: true),
                    Data = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowExecutionLogRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowInstances",
                schema: "Elsa",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    DefinitionId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    WorkflowStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    CorrelationId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    ContextType = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    ContextId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastExecutedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    FinishedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CancelledAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    FaultedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Data = table.Column<string>(type: "TEXT", nullable: true),
                    LastExecutedActivityId = table.Column<string>(type: "TEXT", nullable: true),
                    DefinitionVersionId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowInstances", x => x.Id);
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
                    TenantCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_FrameworkGroups_ParentId",
                table: "FrameworkGroups",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_FrameworkMenus_ParentId",
                table: "FrameworkMenus",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_FrameworkUsers_PhotoId",
                table: "FrameworkUsers",
                column: "PhotoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionLogs");

            migrationBuilder.DropTable(
                name: "Bookmarks",
                schema: "Elsa");

            migrationBuilder.DropTable(
                name: "DataPrivileges");

            migrationBuilder.DropTable(
                name: "FrameworkGroups");

            migrationBuilder.DropTable(
                name: "FrameworkMenus");

            migrationBuilder.DropTable(
                name: "FrameworkRoles");

            migrationBuilder.DropTable(
                name: "FrameworkTenants");

            migrationBuilder.DropTable(
                name: "FrameworkUserGroups");

            migrationBuilder.DropTable(
                name: "FrameworkUserRoles");

            migrationBuilder.DropTable(
                name: "FrameworkUsers");

            migrationBuilder.DropTable(
                name: "FrameworkWorkflows");

            migrationBuilder.DropTable(
                name: "FunctionPrivileges");

            migrationBuilder.DropTable(
                name: "Triggers",
                schema: "Elsa");

            migrationBuilder.DropTable(
                name: "WorkflowDefinitions",
                schema: "Elsa");

            migrationBuilder.DropTable(
                name: "WorkflowExecutionLogRecords",
                schema: "Elsa");

            migrationBuilder.DropTable(
                name: "WorkflowInstances",
                schema: "Elsa");

            migrationBuilder.DropTable(
                name: "FileAttachments");
        }
    }
}
