### 📚 数据库表目录
 | 序号 | 表名 | 表说明 | 
 | :---: | :--- | :--- | 
 | 1 | [actionlogs](#actionlogs) |  | 
 | 2 | [dataprivileges](#dataprivileges) |  | 
 | 3 | [deviceconfigs](#deviceconfigs) | 通讯配置 | 
 | 4 | [devices](#devices) | 设备维护 | 
 | 5 | [devicevariables](#devicevariables) | 变量配置 | 
 | 6 | [drivers](#drivers) | 驱动管理 | 
 | 7 | [fileattachments](#fileattachments) |  | 
 | 8 | [frameworkgroups](#frameworkgroups) |  | 
 | 9 | [frameworkmenus](#frameworkmenus) |  | 
 | 10 | [frameworkroles](#frameworkroles) |  | 
 | 11 | [frameworkusergroups](#frameworkusergroups) |  | 
 | 12 | [frameworkuserroles](#frameworkuserroles) |  | 
 | 13 | [frameworkusers](#frameworkusers) |  | 
 | 14 | [functionprivileges](#functionprivileges) |  | 
 | 15 | [persistedgrants](#persistedgrants) |  | 
 | 16 | [rpclogs](#rpclogs) | RPC日志 | 
 | 17 | [systemconfig](#systemconfig) | 传输配置 | 
### 📒 表结构
#### 表名： actionlogs
说明： 
 | 序号 | 列名 | 数据类型 | 长度 | 主键 | 自增 | 允许空 | 默认值 | 列说明 | 
 | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :--- | 
 | 1 | ID | char | (36) | √ |  |  |  |  | 
 | 2 | ModuleName | varchar | (255) |  |  | √ |  |  | 
 | 3 | ActionName | varchar | (255) |  |  | √ |  |  | 
 | 4 | ITCode | varchar | (50) |  |  | √ |  |  | 
 | 5 | ActionUrl | varchar | (250) |  |  | √ |  |  | 
 | 6 | ActionTime | datetime |  |  |  |  |  |  | 
 | 7 | Duration | double |  |  |  |  |  |  | 
 | 8 | Remark | longtext |  |  |  | √ |  |  | 
 | 9 | IP | varchar | (50) |  |  | √ |  |  | 
 | 10 | LogType | int |  |  |  |  |  |  | 
 | 11 | CreateTime | datetime |  |  |  | √ |  |  | 
 | 12 | CreateBy | varchar | (50) |  |  | √ |  |  | 
 | 13 | UpdateTime | datetime |  |  |  | √ |  |  | 
 | 14 | UpdateBy | varchar | (50) |  |  | √ |  |  | 

#### 表名： dataprivileges
说明： 
 | 序号 | 列名 | 数据类型 | 长度 | 主键 | 自增 | 允许空 | 默认值 | 列说明 | 
 | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :--- | 
 | 1 | ID | char | (36) | √ |  |  |  |  | 
 | 2 | UserCode | longtext |  |  |  | √ |  |  | 
 | 3 | GroupCode | longtext |  |  |  | √ |  |  | 
 | 4 | TableName | varchar | (50) |  |  |  |  |  | 
 | 5 | RelateId | longtext |  |  |  | √ |  |  | 
 | 6 | Domain | longtext |  |  |  | √ |  |  | 
 | 7 | CreateTime | datetime |  |  |  | √ |  |  | 
 | 8 | CreateBy | varchar | (50) |  |  | √ |  |  | 
 | 9 | UpdateTime | datetime |  |  |  | √ |  |  | 
 | 10 | UpdateBy | varchar | (50) |  |  | √ |  |  | 

#### 表名： deviceconfigs
说明： 通讯配置
 | 序号 | 列名 | 数据类型 | 长度 | 主键 | 自增 | 允许空 | 默认值 | 列说明 | 
 | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :--- | 
 | 1 | ID | char | (36) | √ |  |  |  |  | 
 | 2 | DeviceConfigName | varchar | (255) |  |  | √ |  | 名称 | 
 | 3 | DataSide | int |  |  |  |  |  | 属性侧 | 
 | 4 | Description | longtext |  |  |  | √ |  | 描述 | 
 | 5 | Value | varchar | (255) |  |  | √ |  | 值 | 
 | 6 | EnumInfo | longtext |  |  |  | √ |  | 备注 | 
 | 7 | DeviceId | char | (36) |  |  | √ |  | 所属设备 | 
 | 8 | CreateTime | datetime |  |  |  | √ |  |  | 
 | 9 | CreateBy | varchar | (50) |  |  | √ |  |  | 
 | 10 | UpdateTime | datetime |  |  |  | √ |  |  | 
 | 11 | UpdateBy | varchar | (50) |  |  | √ |  |  | 

#### 表名： devices
说明： 设备维护
 | 序号 | 列名 | 数据类型 | 长度 | 主键 | 自增 | 允许空 | 默认值 | 列说明 | 
 | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :--- | 
 | 1 | ID | char | (36) | √ |  |  |  |  | 
 | 2 | DeviceName | varchar | (255) |  |  | √ |  | 名称 | 
 | 3 | Index | int unsigned |  |  |  |  |  | 排序 | 
 | 4 | Description | longtext |  |  |  | √ |  | 描述 | 
 | 5 | DriverId | char | (36) |  |  | √ |  | 驱动 | 
 | 6 | AutoStart | tinyint |  |  |  |  |  | 启动 | 
 | 7 | CgUpload | tinyint |  |  |  |  |  | 变化上传 | 
 | 8 | EnforcePeriod | int unsigned |  |  |  |  |  | 归档周期ms | 
 | 9 | CmdPeriod | int unsigned |  |  |  |  |  | 指令间隔ms | 
 | 10 | DeviceTypeEnum | int |  |  |  |  |  | 类型(组或设备) | 
 | 11 | CreateTime | datetime |  |  |  | √ |  |  | 
 | 12 | CreateBy | longtext |  |  |  | √ |  |  | 
 | 13 | UpdateTime | datetime |  |  |  | √ |  |  | 
 | 14 | UpdateBy | longtext |  |  |  | √ |  |  | 
 | 15 | ParentId | char | (36) |  |  | √ |  |  | 

#### 表名： devicevariables
说明： 变量配置
 | 序号 | 列名 | 数据类型 | 长度 | 主键 | 自增 | 允许空 | 默认值 | 列说明 | 
 | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :--- | 
 | 1 | ID | char | (36) | √ |  |  |  |  | 
 | 2 | Name | varchar | (255) |  |  | √ |  | 变量名 | 
 | 3 | Description | longtext |  |  |  | √ |  | 描述 | 
 | 4 | Method | varchar | (255) |  |  | √ |  | 方法 | 
 | 5 | DeviceAddress | varchar | (255) |  |  | √ |  | 地址 | 
 | 6 | DataType | int |  |  |  |  |  | 数据类型 | 
 | 7 | IsTrigger | tinyint |  |  |  |  |  | 触发 | 
 | 8 | EndianType | int |  |  |  |  |  | 大小端 | 
 | 9 | Expressions | longtext |  |  |  | √ |  | 表达式 | 
 | 10 | IsUpload | tinyint |  |  |  |  |  | 上传 | 
 | 11 | ProtectType | int |  |  |  |  |  | 权限 | 
 | 12 | Index | int unsigned |  |  |  |  |  | 排序 | 
 | 13 | DeviceId | char | (36) |  |  | √ |  | 所属设备 | 
 | 14 | Alias | longtext |  |  |  | √ |  | 别名 | 

#### 表名： drivers
说明： 驱动管理
 | 序号 | 列名 | 数据类型 | 长度 | 主键 | 自增 | 允许空 | 默认值 | 列说明 | 
 | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :--- | 
 | 1 | ID | char | (36) | √ |  |  |  |  | 
 | 2 | DriverName | longtext |  |  |  | √ |  | 驱动名 | 
 | 3 | FileName | longtext |  |  |  | √ |  | 文件名 | 
 | 4 | AssembleName | longtext |  |  |  | √ |  | 程序集名 | 
 | 5 | AuthorizesNum | int |  |  |  |  |  | 剩余授权数 | 
 | 6 | CreateTime | datetime |  |  |  | √ |  |  | 
 | 7 | CreateBy | varchar | (50) |  |  | √ |  |  | 
 | 8 | UpdateTime | datetime |  |  |  | √ |  |  | 
 | 9 | UpdateBy | varchar | (50) |  |  | √ |  |  | 

#### 表名： fileattachments
说明： 
 | 序号 | 列名 | 数据类型 | 长度 | 主键 | 自增 | 允许空 | 默认值 | 列说明 | 
 | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :--- | 
 | 1 | ID | char | (36) | √ |  |  |  |  | 
 | 2 | FileName | longtext |  |  |  |  |  |  | 
 | 3 | FileExt | varchar | (10) |  |  |  |  |  | 
 | 4 | Path | longtext |  |  |  | √ |  |  | 
 | 5 | Length | bigint |  |  |  |  |  |  | 
 | 6 | UploadTime | datetime |  |  |  |  |  |  | 
 | 7 | SaveMode | longtext |  |  |  | √ |  |  | 
 | 8 | FileData | longblob |  |  |  | √ |  |  | 
 | 9 | ExtraInfo | longtext |  |  |  | √ |  |  | 
 | 10 | HandlerInfo | longtext |  |  |  | √ |  |  | 

#### 表名： frameworkgroups
说明： 
 | 序号 | 列名 | 数据类型 | 长度 | 主键 | 自增 | 允许空 | 默认值 | 列说明 | 
 | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :--- | 
 | 1 | ID | char | (36) | √ |  |  |  |  | 
 | 2 | GroupCode | varchar | (100) |  |  |  |  |  | 
 | 3 | GroupName | varchar | (50) |  |  |  |  |  | 
 | 4 | GroupRemark | longtext |  |  |  | √ |  |  | 
 | 5 | TenantCode | longtext |  |  |  | √ |  |  | 
 | 6 | CreateTime | datetime |  |  |  | √ |  |  | 
 | 7 | CreateBy | varchar | (50) |  |  | √ |  |  | 
 | 8 | UpdateTime | datetime |  |  |  | √ |  |  | 
 | 9 | UpdateBy | varchar | (50) |  |  | √ |  |  | 

#### 表名： frameworkmenus
说明： 
 | 序号 | 列名 | 数据类型 | 长度 | 主键 | 自增 | 允许空 | 默认值 | 列说明 | 
 | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :--- | 
 | 1 | ID | char | (36) | √ |  |  |  |  | 
 | 2 | PageName | varchar | (50) |  |  |  |  |  | 
 | 3 | ActionName | longtext |  |  |  | √ |  |  | 
 | 4 | ModuleName | longtext |  |  |  | √ |  |  | 
 | 5 | FolderOnly | tinyint |  |  |  |  |  |  | 
 | 6 | IsInherit | tinyint |  |  |  |  |  |  | 
 | 7 | ClassName | longtext |  |  |  | √ |  |  | 
 | 8 | MethodName | longtext |  |  |  | √ |  |  | 
 | 9 | Domain | longtext |  |  |  | √ |  |  | 
 | 10 | ShowOnMenu | tinyint |  |  |  |  |  |  | 
 | 11 | IsPublic | tinyint |  |  |  |  |  |  | 
 | 12 | DisplayOrder | int |  |  |  |  |  |  | 
 | 13 | IsInside | tinyint |  |  |  |  |  |  | 
 | 14 | Url | longtext |  |  |  | √ |  |  | 
 | 15 | Icon | varchar | (50) |  |  | √ |  |  | 
 | 16 | ParentId | char | (36) |  |  | √ |  |  | 

#### 表名： frameworkroles
说明： 
 | 序号 | 列名 | 数据类型 | 长度 | 主键 | 自增 | 允许空 | 默认值 | 列说明 | 
 | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :--- | 
 | 1 | ID | char | (36) | √ |  |  |  |  | 
 | 2 | RoleCode | varchar | (100) |  |  |  |  |  | 
 | 3 | RoleName | varchar | (50) |  |  |  |  |  | 
 | 4 | RoleRemark | longtext |  |  |  | √ |  |  | 
 | 5 | TenantCode | longtext |  |  |  | √ |  |  | 
 | 6 | CreateTime | datetime |  |  |  | √ |  |  | 
 | 7 | CreateBy | varchar | (50) |  |  | √ |  |  | 
 | 8 | UpdateTime | datetime |  |  |  | √ |  |  | 
 | 9 | UpdateBy | varchar | (50) |  |  | √ |  |  | 

#### 表名： frameworkusergroups
说明： 
 | 序号 | 列名 | 数据类型 | 长度 | 主键 | 自增 | 允许空 | 默认值 | 列说明 | 
 | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :--- | 
 | 1 | ID | char | (36) | √ |  |  |  |  | 
 | 2 | UserCode | longtext |  |  |  |  |  |  | 
 | 3 | GroupCode | longtext |  |  |  |  |  |  | 
 | 4 | CreateTime | datetime |  |  |  | √ |  |  | 
 | 5 | CreateBy | varchar | (50) |  |  | √ |  |  | 
 | 6 | UpdateTime | datetime |  |  |  | √ |  |  | 
 | 7 | UpdateBy | varchar | (50) |  |  | √ |  |  | 

#### 表名： frameworkuserroles
说明： 
 | 序号 | 列名 | 数据类型 | 长度 | 主键 | 自增 | 允许空 | 默认值 | 列说明 | 
 | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :--- | 
 | 1 | ID | char | (36) | √ |  |  |  |  | 
 | 2 | UserCode | longtext |  |  |  |  |  |  | 
 | 3 | RoleCode | longtext |  |  |  |  |  |  | 
 | 4 | CreateTime | datetime |  |  |  | √ |  |  | 
 | 5 | CreateBy | varchar | (50) |  |  | √ |  |  | 
 | 6 | UpdateTime | datetime |  |  |  | √ |  |  | 
 | 7 | UpdateBy | varchar | (50) |  |  | √ |  |  | 

#### 表名： frameworkusers
说明： 
 | 序号 | 列名 | 数据类型 | 长度 | 主键 | 自增 | 允许空 | 默认值 | 列说明 | 
 | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :--- | 
 | 1 | ID | char | (36) | √ |  |  |  |  | 
 | 2 | Email | varchar | (50) |  |  | √ |  |  | 
 | 3 | Gender | int |  |  |  | √ |  |  | 
 | 4 | CellPhone | longtext |  |  |  | √ |  |  | 
 | 5 | HomePhone | varchar | (30) |  |  | √ |  |  | 
 | 6 | Address | varchar | (200) |  |  | √ |  |  | 
 | 7 | ZipCode | longtext |  |  |  | √ |  |  | 
 | 8 | CreateTime | datetime |  |  |  | √ |  |  | 
 | 9 | CreateBy | varchar | (50) |  |  | √ |  |  | 
 | 10 | UpdateTime | datetime |  |  |  | √ |  |  | 
 | 11 | UpdateBy | varchar | (50) |  |  | √ |  |  | 
 | 12 | ITCode | varchar | (50) |  |  |  |  |  | 
 | 13 | Password | varchar | (32) |  |  |  |  |  | 
 | 14 | Name | varchar | (50) |  |  |  |  |  | 
 | 15 | IsValid | tinyint |  |  |  |  |  |  | 
 | 16 | PhotoId | char | (36) |  |  | √ |  |  | 
 | 17 | TenantCode | longtext |  |  |  | √ |  |  | 

#### 表名： functionprivileges
说明： 
 | 序号 | 列名 | 数据类型 | 长度 | 主键 | 自增 | 允许空 | 默认值 | 列说明 | 
 | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :--- | 
 | 1 | ID | char | (36) | √ |  |  |  |  | 
 | 2 | RoleCode | longtext |  |  |  | √ |  |  | 
 | 3 | MenuItemId | char | (36) |  |  |  |  |  | 
 | 4 | Allowed | tinyint |  |  |  |  |  |  | 
 | 5 | CreateTime | datetime |  |  |  | √ |  |  | 
 | 6 | CreateBy | varchar | (50) |  |  | √ |  |  | 
 | 7 | UpdateTime | datetime |  |  |  | √ |  |  | 
 | 8 | UpdateBy | varchar | (50) |  |  | √ |  |  | 

#### 表名： persistedgrants
说明： 
 | 序号 | 列名 | 数据类型 | 长度 | 主键 | 自增 | 允许空 | 默认值 | 列说明 | 
 | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :--- | 
 | 1 | ID | char | (36) | √ |  |  |  |  | 
 | 2 | Type | varchar | (50) |  |  | √ |  |  | 
 | 3 | UserCode | longtext |  |  |  | √ |  |  | 
 | 4 | CreationTime | datetime |  |  |  |  |  |  | 
 | 5 | Expiration | datetime |  |  |  |  |  |  | 
 | 6 | RefreshToken | varchar | (50) |  |  | √ |  |  | 

#### 表名： rpclogs
说明： RPC日志
 | 序号 | 列名 | 数据类型 | 长度 | 主键 | 自增 | 允许空 | 默认值 | 列说明 | 
 | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :--- | 
 | 1 | ID | char | (36) | √ |  |  |  |  | 
 | 2 | RpcSide | int |  |  |  |  |  | 发起方 | 
 | 3 | StartTime | datetime |  |  |  |  |  | 开始时间 | 
 | 4 | DeviceId | char | (36) |  |  | √ |  | 所属设备 | 
 | 5 | Method | longtext |  |  |  | √ |  | 方法 | 
 | 6 | Params | longtext |  |  |  | √ |  | 请求参数 | 
 | 7 | EndTime | datetime |  |  |  |  |  | 结束时间 | 
 | 8 | IsSuccess | tinyint |  |  |  |  |  | 是否成功 | 
 | 9 | Description | longtext |  |  |  | √ |  | 描述 | 

#### 表名： systemconfig
说明： 传输配置
 | 序号 | 列名 | 数据类型 | 长度 | 主键 | 自增 | 允许空 | 默认值 | 列说明 | 
 | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :--- | 
 | 1 | ID | char | (36) | √ |  |  |  |  | 
 | 2 | GatewayName | longtext |  |  |  | √ |  | 网关名称 | 
 | 3 | ClientId | longtext |  |  |  | √ |  | ClientId | 
 | 4 | MqttIp | longtext |  |  |  | √ |  | Mqtt服务器 | 
 | 5 | MqttPort | int |  |  |  |  |  | Mqtt端口 | 
 | 6 | MqttUName | longtext |  |  |  | √ |  | Mqtt用户名 | 
 | 7 | MqttUPwd | longtext |  |  |  | √ |  | Mqtt密码 | 
 | 8 | IoTPlatformType | int |  |  |  |  |  | 输出平台 | 
 | 9 | CreateTime | datetime |  |  |  | √ |  |  | 
 | 10 | CreateBy | varchar | (50) |  |  | √ |  |  | 
 | 11 | UpdateTime | datetime |  |  |  | √ |  |  | 
 | 12 | UpdateBy | varchar | (50) |  |  | √ |  |  | 
