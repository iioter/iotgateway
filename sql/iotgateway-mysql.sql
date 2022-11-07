/*
 Navicat Premium Data Transfer

 Source Server         : MySql8
 Source Server Type    : MySQL
 Source Server Version : 80030
 Source Host           : localhost:3306
 Source Schema         : iotgateway

 Target Server Type    : MySQL
 Target Server Version : 80030
 File Encoding         : 65001

 Date: 07/11/2022 12:51:16
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for __efmigrationshistory
-- ----------------------------
DROP TABLE IF EXISTS `__efmigrationshistory`;
CREATE TABLE `__efmigrationshistory`  (
  `MigrationId` varchar(95) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of __efmigrationshistory
-- ----------------------------
INSERT INTO `__efmigrationshistory` VALUES ('20221107035009_Init', '6.0.0');
INSERT INTO `__efmigrationshistory` VALUES ('20221107035318_CmdPeriod', '6.0.10');

-- ----------------------------
-- Table structure for actionlogs
-- ----------------------------
DROP TABLE IF EXISTS `actionlogs`;
CREATE TABLE `actionlogs`  (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ModuleName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `ActionName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `ITCode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `ActionUrl` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `ActionTime` datetime(6) NOT NULL,
  `Duration` double NOT NULL,
  `Remark` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `IP` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `LogType` int NOT NULL,
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `CreateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `UpdateTime` datetime(6) NULL DEFAULT NULL,
  `UpdateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of actionlogs
-- ----------------------------

-- ----------------------------
-- Table structure for dataprivileges
-- ----------------------------
DROP TABLE IF EXISTS `dataprivileges`;
CREATE TABLE `dataprivileges`  (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `UserCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `GroupCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `TableName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RelateId` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Domain` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `CreateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `UpdateTime` datetime(6) NULL DEFAULT NULL,
  `UpdateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of dataprivileges
-- ----------------------------

-- ----------------------------
-- Table structure for deviceconfigs
-- ----------------------------
DROP TABLE IF EXISTS `deviceconfigs`;
CREATE TABLE `deviceconfigs`  (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeviceConfigName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `DataSide` int NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `EnumInfo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `DeviceId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NULL DEFAULT NULL,
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `CreateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `UpdateTime` datetime(6) NULL DEFAULT NULL,
  `UpdateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  INDEX `IX_DeviceConfigs_DeviceId`(`DeviceId`) USING BTREE,
  CONSTRAINT `FK_DeviceConfigs_Devices_DeviceId` FOREIGN KEY (`DeviceId`) REFERENCES `devices` (`ID`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of deviceconfigs
-- ----------------------------

-- ----------------------------
-- Table structure for devices
-- ----------------------------
DROP TABLE IF EXISTS `devices`;
CREATE TABLE `devices`  (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DeviceName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Index` int UNSIGNED NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `DriverId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NULL DEFAULT NULL,
  `AutoStart` tinyint(1) NOT NULL,
  `CgUpload` tinyint(1) NOT NULL,
  `EnforcePeriod` int UNSIGNED NOT NULL,
  `CmdPeriod` int UNSIGNED NOT NULL,
  `DeviceTypeEnum` int NOT NULL,
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `CreateBy` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `UpdateTime` datetime(6) NULL DEFAULT NULL,
  `UpdateBy` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `ParentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  INDEX `IX_Devices_DriverId`(`DriverId`) USING BTREE,
  INDEX `IX_Devices_ParentId`(`ParentId`) USING BTREE,
  CONSTRAINT `FK_Devices_Devices_ParentId` FOREIGN KEY (`ParentId`) REFERENCES `devices` (`ID`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_Devices_Drivers_DriverId` FOREIGN KEY (`DriverId`) REFERENCES `drivers` (`ID`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of devices
-- ----------------------------

-- ----------------------------
-- Table structure for devicevariables
-- ----------------------------
DROP TABLE IF EXISTS `devicevariables`;
CREATE TABLE `devicevariables`  (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Method` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `DeviceAddress` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `DataType` int NOT NULL,
  `Expressions` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `ProtectType` int NOT NULL,
  `Index` int UNSIGNED NOT NULL,
  `DeviceId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  INDEX `IX_DeviceVariables_DeviceId`(`DeviceId`) USING BTREE,
  CONSTRAINT `FK_DeviceVariables_Devices_DeviceId` FOREIGN KEY (`DeviceId`) REFERENCES `devices` (`ID`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of devicevariables
-- ----------------------------

-- ----------------------------
-- Table structure for drivers
-- ----------------------------
DROP TABLE IF EXISTS `drivers`;
CREATE TABLE `drivers`  (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DriverName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `FileName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `AssembleName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `AuthorizesNum` int NOT NULL,
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `CreateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `UpdateTime` datetime(6) NULL DEFAULT NULL,
  `UpdateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of drivers
-- ----------------------------
INSERT INTO `drivers` VALUES ('08d9c081-9003-41e6-8621-2ba04b477ef3', '西门子PLC驱动', 'DriverSiemensS7.dll', 'DriverSiemensS7.SiemensS7', 100, '2021-12-16 18:48:14.905984', 'admin', '2022-10-28 21:18:02.851063', 'admin');
INSERT INTO `drivers` VALUES ('08d9c6b1-219c-4471-8c97-a8e93f9fb494', 'AllenBradley驱动', 'DriverAllenBradley.dll', 'DriverAllenBradley.AllenBradley', 100, '2021-12-24 15:43:52.499485', 'admin', '2022-10-28 20:59:02.045243', 'admin');
INSERT INTO `drivers` VALUES ('08d9d8b8-9c1d-4d79-8877-6dfb072f9c52', '三菱PLC驱动', 'DriverMitsubishi.dll', 'DriverMitsubishi.Mitsubishi', 100, '2022-01-16 14:22:45.438091', 'admin', '2022-10-28 20:59:14.786774', 'admin');
INSERT INTO `drivers` VALUES ('08d9d8b8-aca4-4c13-8959-d26515431bff', 'ModbusMaster驱动', 'DriverModbusMaster.dll', 'DriverModbusMaster.ModbusMaster', 100, '2022-01-16 14:23:13.168346', 'admin', '2022-10-28 20:59:28.874963', 'admin');
INSERT INTO `drivers` VALUES ('08d9d8b8-ba08-4d8c-803c-e37889d4ff5d', '欧姆龙PLC驱动', 'DriverOmronFins.dll', 'DriverOmronFins.OmronFins', 100, '2022-01-16 14:23:35.635234', 'admin', '2022-10-28 20:59:41.385442', 'admin');
INSERT INTO `drivers` VALUES ('08da149e-8c9b-4bf2-8abe-9ff22bacfcfd', 'Fanuc-Hsl驱动', 'DriverFanucHsl.dll', 'DriverFanucHsl.FanucHsl', 100, '2022-04-02 19:47:22.274405', 'admin', '2022-10-28 20:59:54.880311', 'admin');
INSERT INTO `drivers` VALUES ('08da6254-ed87-44cf-8cb1-e1c9326a3d7b', 'OPCUClient驱动', 'DriverOPCUaClient.dll', 'DriverOPCUaClient.OPCUaClient', 100, '2022-07-10 17:16:52.818011', 'admin', '2022-10-28 21:00:10.164914', 'admin');
INSERT INTO `drivers` VALUES ('08da6957-aef5-4537-8127-35ee0aa0817c', 'MTConnect驱动', 'DriverMTConnect.dll', 'DriverMTConnect.MTConnectClient', 100, '2022-07-19 15:24:14.472078', 'admin', '2022-10-28 21:00:23.670881', 'admin');
INSERT INTO `drivers` VALUES ('08dab8e6-b383-441b-8ac9-0314a7e1dbd5', 'OPCDA驱动', 'DriverOPCDaClient.dll', 'DriverOPCDaClient.OPCDaClient', 100, '2022-10-28 21:17:02.004055', 'admin', '2022-10-28 21:17:57.642559', 'admin');
INSERT INTO `drivers` VALUES ('08dab8e6-d1e6-4d8a-8468-6e51538d3ee4', 'Demo模拟驱动', 'DriverSimTcpClient.dll', 'DriverSimTcpClient.SimTcpClient', 100, '2022-10-28 21:17:52.989232', 'admin', NULL, NULL);

-- ----------------------------
-- Table structure for fileattachments
-- ----------------------------
DROP TABLE IF EXISTS `fileattachments`;
CREATE TABLE `fileattachments`  (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `FileName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `FileExt` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Path` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Length` bigint NOT NULL,
  `UploadTime` datetime(6) NOT NULL,
  `SaveMode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `FileData` longblob NULL,
  `ExtraInfo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `HandlerInfo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of fileattachments
-- ----------------------------

-- ----------------------------
-- Table structure for frameworkgroups
-- ----------------------------
DROP TABLE IF EXISTS `frameworkgroups`;
CREATE TABLE `frameworkgroups`  (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `GroupCode` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `GroupName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `GroupRemark` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `TenantCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `CreateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `UpdateTime` datetime(6) NULL DEFAULT NULL,
  `UpdateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of frameworkgroups
-- ----------------------------

-- ----------------------------
-- Table structure for frameworkmenus
-- ----------------------------
DROP TABLE IF EXISTS `frameworkmenus`;
CREATE TABLE `frameworkmenus`  (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `PageName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ActionName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `ModuleName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `FolderOnly` tinyint(1) NOT NULL,
  `IsInherit` tinyint(1) NOT NULL,
  `ClassName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `MethodName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Domain` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `ShowOnMenu` tinyint(1) NOT NULL,
  `IsPublic` tinyint(1) NOT NULL,
  `DisplayOrder` int NOT NULL,
  `IsInside` tinyint(1) NOT NULL,
  `Url` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Icon` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `ParentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  INDEX `IX_FrameworkMenus_ParentId`(`ParentId`) USING BTREE,
  CONSTRAINT `FK_FrameworkMenus_FrameworkMenus_ParentId` FOREIGN KEY (`ParentId`) REFERENCES `frameworkmenus` (`ID`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of frameworkmenus
-- ----------------------------
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c00-4da9-8730-d0f58b175bf7', 'MenuKey.SystemManagement', NULL, NULL, 1, 0, NULL, NULL, NULL, 1, 0, 1, 1, NULL, 'layui-icon layui-icon-set', NULL);
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0a-4931-8269-f0015556d3d1', 'MenuKey.ActionLog', 'Sys.Search', '日志', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,ActionLog', NULL, NULL, 1, 0, 1, 1, '/_Admin/ActionLog', 'layui-icon layui-icon-form', '08dac074-8c00-4da9-8730-d0f58b175bf7');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0c-4ccc-81f2-3c059bfac431', 'MenuKey.ActionLog', 'Sys.Details', '日志', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,ActionLog', 'Details', NULL, 0, 0, 1, 1, '/_Admin/ActionLog/Details', NULL, '08dac074-8c0a-4931-8269-f0015556d3d1');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-47d0-8db2-cc04fc965208', 'MenuKey.ActionLog', 'Sys.Search', '日志', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,ActionLog', 'Search', NULL, 0, 0, 2, 1, '/_Admin/ActionLog/Search', NULL, '08dac074-8c0a-4931-8269-f0015556d3d1');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-483f-8e52-40be8a3ec51e', 'MenuKey.ActionLog', 'Sys.Export', '日志', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,ActionLog', 'ExportExcel', NULL, 0, 0, 3, 1, '/_Admin/ActionLog/ExportExcel', NULL, '08dac074-8c0a-4931-8269-f0015556d3d1');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4874-823c-2b03010c6d05', 'MenuKey.ActionLog', 'Sys.BatchDelete', '日志', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,ActionLog', 'BatchDelete', NULL, 0, 0, 4, 1, '/_Admin/ActionLog/BatchDelete', NULL, '08dac074-8c0a-4931-8269-f0015556d3d1');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-48be-83d7-d3f34457b47f', 'MenuKey.UserManagement', 'Sys.Search', '用户管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkUser', NULL, NULL, 1, 0, 2, 1, '/_Admin/FrameworkUser', 'layui-icon layui-icon-friends', '08dac074-8c00-4da9-8730-d0f58b175bf7');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-48de-8b87-aa160027abc1', 'MenuKey.UserManagement', 'Sys.Create', '用户管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkUser', 'Create', NULL, 0, 0, 1, 1, '/_Admin/FrameworkUser/Create', NULL, '08dac074-8c0f-48be-83d7-d3f34457b47f');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-490a-84ae-023e6a4c48bc', 'MenuKey.UserManagement', 'Sys.Edit', '用户管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkUser', 'Edit', NULL, 0, 0, 2, 1, '/_Admin/FrameworkUser/Edit', NULL, '08dac074-8c0f-48be-83d7-d3f34457b47f');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4932-8ca7-db0c29ecc225', 'MenuKey.UserManagement', 'Login.ChangePassword', '用户管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkUser', 'Password', NULL, 0, 0, 3, 1, '/_Admin/FrameworkUser/Password', NULL, '08dac074-8c0f-48be-83d7-d3f34457b47f');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4960-8553-7cebcf6aa89f', 'MenuKey.UserManagement', 'Sys.Delete', '用户管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkUser', 'Delete', NULL, 0, 0, 4, 1, '/_Admin/FrameworkUser/Delete', NULL, '08dac074-8c0f-48be-83d7-d3f34457b47f');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4988-8489-1a56e0ef323d', 'MenuKey.UserManagement', 'Sys.Details', '用户管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkUser', 'Details', NULL, 0, 0, 5, 1, '/_Admin/FrameworkUser/Details', NULL, '08dac074-8c0f-48be-83d7-d3f34457b47f');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-49b0-8dfa-f0665fd41ce3', 'MenuKey.UserManagement', 'Sys.Import', '用户管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkUser', 'Import', NULL, 0, 0, 6, 1, '/_Admin/FrameworkUser/Import', NULL, '08dac074-8c0f-48be-83d7-d3f34457b47f');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-49d7-8691-2f00252881cb', 'MenuKey.UserManagement', 'Sys.Enable', '用户管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkUser', 'Enable', NULL, 0, 0, 7, 1, '/_Admin/FrameworkUser/Enable', NULL, '08dac074-8c0f-48be-83d7-d3f34457b47f');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4a04-8501-20ab39da605b', 'MenuKey.UserManagement', 'Sys.Search', '用户管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkUser', 'Search', NULL, 0, 0, 8, 1, '/_Admin/FrameworkUser/Search', NULL, '08dac074-8c0f-48be-83d7-d3f34457b47f');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4a30-820f-d7ffed4055e8', 'MenuKey.UserManagement', 'Sys.BatchEdit', '用户管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkUser', 'BatchEdit', NULL, 0, 0, 9, 1, '/_Admin/FrameworkUser/BatchEdit', NULL, '08dac074-8c0f-48be-83d7-d3f34457b47f');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4a56-894c-186f19c8a304', 'MenuKey.UserManagement', 'Sys.BatchDelete', '用户管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkUser', 'BatchDelete', NULL, 0, 0, 10, 1, '/_Admin/FrameworkUser/BatchDelete', NULL, '08dac074-8c0f-48be-83d7-d3f34457b47f');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4a7f-83dc-a14da1c284dd', 'MenuKey.UserManagement', 'Sys.Export', '用户管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkUser', 'ExportExcel', NULL, 0, 0, 11, 1, '/_Admin/FrameworkUser/ExportExcel', NULL, '08dac074-8c0f-48be-83d7-d3f34457b47f');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4ab9-8bba-6167eaeea132', 'MenuKey.RoleManagement', 'Sys.Search', '角色管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkRole', NULL, NULL, 1, 0, 3, 1, '/_Admin/FrameworkRole', 'layui-icon layui-icon-user', '08dac074-8c00-4da9-8730-d0f58b175bf7');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4ad1-8f82-1c2a0e2365ab', 'MenuKey.RoleManagement', 'Sys.Create', '角色管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkRole', 'Create', NULL, 0, 0, 1, 1, '/_Admin/FrameworkRole/Create', NULL, '08dac074-8c0f-4ab9-8bba-6167eaeea132');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4afe-8205-db7f3fe4086b', 'MenuKey.RoleManagement', 'Sys.Edit', '角色管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkRole', 'Edit', NULL, 0, 0, 2, 1, '/_Admin/FrameworkRole/Edit', NULL, '08dac074-8c0f-4ab9-8bba-6167eaeea132');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4b24-8a5c-b027a2d77ddc', 'MenuKey.RoleManagement', 'Sys.Delete', '角色管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkRole', 'Delete', NULL, 0, 0, 3, 1, '/_Admin/FrameworkRole/Delete', NULL, '08dac074-8c0f-4ab9-8bba-6167eaeea132');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4b4e-82ae-cf707ec14aed', 'MenuKey.RoleManagement', 'Sys.Import', '角色管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkRole', 'Import', NULL, 0, 0, 4, 1, '/_Admin/FrameworkRole/Import', NULL, '08dac074-8c0f-4ab9-8bba-6167eaeea132');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4b74-8e0e-8a146569f410', 'MenuKey.RoleManagement', 'Sys.Details', '角色管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkRole', 'Details', NULL, 0, 0, 5, 1, '/_Admin/FrameworkRole/Details', NULL, '08dac074-8c0f-4ab9-8bba-6167eaeea132');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4b9b-8dcf-66a4eb47e532', 'MenuKey.RoleManagement', '_Admin.PageFunction', '角色管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkRole', 'PageFunction', NULL, 0, 0, 6, 1, '/_Admin/FrameworkRole/PageFunction', NULL, '08dac074-8c0f-4ab9-8bba-6167eaeea132');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4bc4-851a-b2c5b7dec17d', 'MenuKey.RoleManagement', 'Sys.Search', '角色管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkRole', 'Search', NULL, 0, 0, 7, 1, '/_Admin/FrameworkRole/Search', NULL, '08dac074-8c0f-4ab9-8bba-6167eaeea132');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4beb-8a01-de6ad1d43988', 'MenuKey.RoleManagement', 'Sys.BatchDelete', '角色管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkRole', 'BatchDelete', NULL, 0, 0, 8, 1, '/_Admin/FrameworkRole/BatchDelete', NULL, '08dac074-8c0f-4ab9-8bba-6167eaeea132');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4c15-8a6b-b626f34d7085', 'MenuKey.RoleManagement', 'Sys.Export', '角色管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkRole', 'ExportExcel', NULL, 0, 0, 9, 1, '/_Admin/FrameworkRole/ExportExcel', NULL, '08dac074-8c0f-4ab9-8bba-6167eaeea132');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4c4c-8e9a-74ea6f0d5f17', 'MenuKey.GroupManagement', 'Sys.Search', '用户组管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkGroup', NULL, NULL, 1, 0, 4, 1, '/_Admin/FrameworkGroup', 'layui-icon layui-icon-group', '08dac074-8c00-4da9-8730-d0f58b175bf7');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4c65-8827-d3eacfc3b014', 'MenuKey.GroupManagement', 'Sys.Create', '用户组管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkGroup', 'Create', NULL, 0, 0, 1, 1, '/_Admin/FrameworkGroup/Create', NULL, '08dac074-8c0f-4c4c-8e9a-74ea6f0d5f17');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4c8c-8138-156dd7afd52b', 'MenuKey.GroupManagement', 'Sys.Edit', '用户组管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkGroup', 'Edit', NULL, 0, 0, 2, 1, '/_Admin/FrameworkGroup/Edit', NULL, '08dac074-8c0f-4c4c-8e9a-74ea6f0d5f17');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4cb4-805f-408d47477ccf', 'MenuKey.GroupManagement', 'Sys.Delete', '用户组管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkGroup', 'Delete', NULL, 0, 0, 3, 1, '/_Admin/FrameworkGroup/Delete', NULL, '08dac074-8c0f-4c4c-8e9a-74ea6f0d5f17');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4cda-898e-f45aef6af445', 'MenuKey.GroupManagement', 'Sys.Import', '用户组管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkGroup', 'Import', NULL, 0, 0, 4, 1, '/_Admin/FrameworkGroup/Import', NULL, '08dac074-8c0f-4c4c-8e9a-74ea6f0d5f17');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4d02-82ee-83aaca8a0eb3', 'MenuKey.GroupManagement', '_Admin.DataPrivilege', '用户组管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkGroup', 'DataFunction', NULL, 0, 0, 5, 1, '/_Admin/FrameworkGroup/DataFunction', NULL, '08dac074-8c0f-4c4c-8e9a-74ea6f0d5f17');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4d2b-8c6d-5a70d94f57cb', 'MenuKey.GroupManagement', 'Sys.Search', '用户组管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkGroup', 'Search', NULL, 0, 0, 6, 1, '/_Admin/FrameworkGroup/Search', NULL, '08dac074-8c0f-4c4c-8e9a-74ea6f0d5f17');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4d53-8054-dc2a6b1ca4f4', 'MenuKey.GroupManagement', 'Sys.BatchDelete', '用户组管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkGroup', 'BatchDelete', NULL, 0, 0, 7, 1, '/_Admin/FrameworkGroup/BatchDelete', NULL, '08dac074-8c0f-4c4c-8e9a-74ea6f0d5f17');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4d7f-8041-4dd643264814', 'MenuKey.GroupManagement', 'Sys.Export', '用户组管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkGroup', 'ExportExcel', NULL, 0, 0, 8, 1, '/_Admin/FrameworkGroup/ExportExcel', NULL, '08dac074-8c0f-4c4c-8e9a-74ea6f0d5f17');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4ddf-87c8-196854fbd220', 'MenuKey.MenuMangement', 'Sys.Search', '菜单管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkMenu', NULL, NULL, 1, 0, 5, 1, '/_Admin/FrameworkMenu', 'layui-icon layui-icon-menu-fill', '08dac074-8c00-4da9-8730-d0f58b175bf7');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4e05-8737-8506526db3ac', 'MenuKey.MenuMangement', 'Sys.Create', '菜单管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkMenu', 'Create', NULL, 0, 0, 1, 1, '/_Admin/FrameworkMenu/Create', NULL, '08dac074-8c0f-4ddf-87c8-196854fbd220');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4e2e-8969-2d75f9961e48', 'MenuKey.MenuMangement', 'Sys.Edit', '菜单管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkMenu', 'Edit', NULL, 0, 0, 2, 1, '/_Admin/FrameworkMenu/Edit', NULL, '08dac074-8c0f-4ddf-87c8-196854fbd220');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4e5a-8606-318172cac185', 'MenuKey.MenuMangement', 'Sys.Delete', '菜单管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkMenu', 'Delete', NULL, 0, 0, 3, 1, '/_Admin/FrameworkMenu/Delete', NULL, '08dac074-8c0f-4ddf-87c8-196854fbd220');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4e89-876c-24e71aaa70b5', 'MenuKey.MenuMangement', 'Sys.Details', '菜单管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkMenu', 'Details', NULL, 0, 0, 4, 1, '/_Admin/FrameworkMenu/Details', NULL, '08dac074-8c0f-4ddf-87c8-196854fbd220');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4eb6-89e4-8f7880ebade9', 'MenuKey.MenuMangement', '_Admin.UnsetPages', '菜单管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkMenu', 'UnsetPages', NULL, 0, 0, 5, 1, '/_Admin/FrameworkMenu/UnsetPages', NULL, '08dac074-8c0f-4ddf-87c8-196854fbd220');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4edd-8ed9-a10a4bc8ec89', 'MenuKey.MenuMangement', '_Admin.RefreshMenu', '菜单管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkMenu', 'RefreshMenu', NULL, 0, 0, 6, 1, '/_Admin/FrameworkMenu/RefreshMenu', NULL, '08dac074-8c0f-4ddf-87c8-196854fbd220');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4f05-8947-77c8ac78f8f1', 'MenuKey.MenuMangement', 'Sys.Search', '菜单管理', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,FrameworkMenu', 'Search', NULL, 0, 0, 7, 1, '/_Admin/FrameworkMenu/Search', NULL, '08dac074-8c0f-4ddf-87c8-196854fbd220');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4f40-8edc-315bb28d028a', 'MenuKey.DataPrivilege', 'Sys.Search', '数据权限', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,DataPrivilege', NULL, NULL, 1, 0, 6, 1, '/_Admin/DataPrivilege', 'layui-icon layui-icon-auz', '08dac074-8c00-4da9-8730-d0f58b175bf7');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4f58-86df-c7d6387de8e0', 'MenuKey.DataPrivilege', 'Sys.Create', '数据权限', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,DataPrivilege', 'Create', NULL, 0, 0, 1, 1, '/_Admin/DataPrivilege/Create', NULL, '08dac074-8c0f-4f40-8edc-315bb28d028a');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4f81-8e0a-f7deba609f59', 'MenuKey.DataPrivilege', 'Sys.Edit', '数据权限', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,DataPrivilege', 'Edit', NULL, 0, 0, 2, 1, '/_Admin/DataPrivilege/Edit', NULL, '08dac074-8c0f-4f40-8edc-315bb28d028a');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4fad-881b-4814008067a3', 'MenuKey.DataPrivilege', 'Sys.Delete', '数据权限', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,DataPrivilege', 'Delete', NULL, 0, 0, 3, 1, '/_Admin/DataPrivilege/Delete', NULL, '08dac074-8c0f-4f40-8edc-315bb28d028a');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4fd3-8aac-6981ce8bca69', 'MenuKey.DataPrivilege', 'Sys.Search', '数据权限', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,DataPrivilege', 'Search', NULL, 0, 0, 4, 1, '/_Admin/DataPrivilege/Search', NULL, '08dac074-8c0f-4f40-8edc-315bb28d028a');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c0f-4ffa-81e5-915452e433c6', 'MenuKey.DataPrivilege', 'Sys.Export', '数据权限', 0, 0, 'WalkingTec.Mvvm.Mvc.Admin.Controllers,DataPrivilege', 'ExportExcel', NULL, 0, 0, 5, 1, '/_Admin/DataPrivilege/ExportExcel', NULL, '08dac074-8c0f-4f40-8edc-315bb28d028a');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4c63-820e-2aa4e0a3e6d3', 'MenuKey.Api', NULL, NULL, 1, 0, NULL, NULL, NULL, 0, 0, 100, 1, NULL, NULL, NULL);
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4c87-8e86-131cf8c56371', 'MenuKey.ActionLog', 'MainPage', '日志管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,ActionLog', NULL, NULL, 1, 0, 1, 1, '/actionlog', NULL, '08dac074-8c10-4c63-820e-2aa4e0a3e6d3');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4ca6-883f-beded738238f', 'MenuKey.ActionLog', 'Sys.Get', '日志管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,ActionLog', 'Get', NULL, 0, 0, 1, 1, '/api/_ActionLog/{id}', NULL, '08dac074-8c10-4c87-8e86-131cf8c56371');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4cd2-84b6-1eb698329bdd', 'MenuKey.ActionLog', 'Sys.Search', '日志管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,ActionLog', 'Search', NULL, 0, 0, 2, 1, '/api/_ActionLog/Search', NULL, '08dac074-8c10-4c87-8e86-131cf8c56371');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4cfa-8f9e-adcc49e2cd87', 'MenuKey.ActionLog', 'Sys.Delete', '日志管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,ActionLog', 'BatchDelete', NULL, 0, 0, 3, 1, '/api/_ActionLog/BatchDelete', NULL, '08dac074-8c10-4c87-8e86-131cf8c56371');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4d25-8610-b2713c0d7957', 'MenuKey.ActionLog', 'Sys.Export', '日志管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,ActionLog', 'ExportExcel', NULL, 0, 0, 4, 1, '/api/_ActionLog/ExportExcel', NULL, '08dac074-8c10-4c87-8e86-131cf8c56371');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4d4d-8618-42cf073be9d2', 'MenuKey.ActionLog', 'Sys.ExportByIds', '日志管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,ActionLog', 'ExportExcelByIds', NULL, 0, 0, 5, 1, '/api/_ActionLog/ExportExcelByIds', NULL, '08dac074-8c10-4c87-8e86-131cf8c56371');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4d87-827b-4a16cc6efb4b', 'MenuKey.UserManagement', 'MainPage', '用户管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkUser', NULL, NULL, 1, 0, 2, 1, '/frameworkuser', NULL, '08dac074-8c10-4c63-820e-2aa4e0a3e6d3');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4da0-874a-ec66ce841446', 'MenuKey.UserManagement', 'Sys.Get', '用户管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkUser', 'Get', NULL, 0, 0, 1, 1, '/api/_FrameworkUser/{id}', NULL, '08dac074-8c10-4d87-827b-4a16cc6efb4b');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4dc7-87ba-b7b2867b665b', 'MenuKey.UserManagement', 'Sys.Edit', '用户管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkUser', 'Edit', NULL, 0, 0, 2, 1, '/api/_FrameworkUser/Edit', NULL, '08dac074-8c10-4d87-827b-4a16cc6efb4b');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4dee-80da-111f44418eee', 'MenuKey.UserManagement', 'Sys.DownloadTemplate', '用户管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkUser', 'GetExcelTemplate', NULL, 0, 0, 3, 1, '/api/_FrameworkUser/GetExcelTemplate', NULL, '08dac074-8c10-4d87-827b-4a16cc6efb4b');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4e14-82d2-e6c548e6f371', 'MenuKey.UserManagement', 'Sys.Search', '用户管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkUser', 'Search', NULL, 0, 0, 4, 1, '/api/_FrameworkUser/Search', NULL, '08dac074-8c10-4d87-827b-4a16cc6efb4b');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4e3a-8c0f-cf98c7d4e299', 'MenuKey.UserManagement', 'Sys.Create', '用户管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkUser', 'Add', NULL, 0, 0, 5, 1, '/api/_FrameworkUser/Add', NULL, '08dac074-8c10-4d87-827b-4a16cc6efb4b');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4e61-8486-73eb6655b783', 'MenuKey.UserManagement', 'Sys.Delete', '用户管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkUser', 'BatchDelete', NULL, 0, 0, 6, 1, '/api/_FrameworkUser/BatchDelete', NULL, '08dac074-8c10-4d87-827b-4a16cc6efb4b');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4e89-8af8-02915b022c71', 'MenuKey.UserManagement', 'Sys.Export', '用户管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkUser', 'ExportExcel', NULL, 0, 0, 7, 1, '/api/_FrameworkUser/ExportExcel', NULL, '08dac074-8c10-4d87-827b-4a16cc6efb4b');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4eb1-89d0-47a5b29130e3', 'MenuKey.UserManagement', 'Sys.ExportByIds', '用户管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkUser', 'ExportExcelByIds', NULL, 0, 0, 8, 1, '/api/_FrameworkUser/ExportExcelByIds', NULL, '08dac074-8c10-4d87-827b-4a16cc6efb4b');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4edb-826b-c09049bac70a', 'MenuKey.UserManagement', 'Sys.Import', '用户管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkUser', 'Import', NULL, 0, 0, 9, 1, '/api/_FrameworkUser/Import', NULL, '08dac074-8c10-4d87-827b-4a16cc6efb4b');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4f12-86cb-a3e43c04f048', 'MenuKey.RoleManagement', 'MainPage', '角色管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkRole', NULL, NULL, 1, 0, 3, 1, '/frameworkrole', NULL, '08dac074-8c10-4c63-820e-2aa4e0a3e6d3');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4f29-8c90-d1959626e7cd', 'MenuKey.RoleManagement', 'Sys.Get', '角色管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkRole', 'Get', NULL, 0, 0, 1, 1, '/api/_FrameworkRole/{id}', NULL, '08dac074-8c10-4f12-86cb-a3e43c04f048');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4f50-89ec-8689612ddd46', 'MenuKey.RoleManagement', '_Admin.PageFunction', '角色管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkRole', 'EditPrivilege', NULL, 0, 0, 2, 1, '/api/_FrameworkRole/EditPrivilege', NULL, '08dac074-8c10-4f12-86cb-a3e43c04f048');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4f77-8cf3-ae1a959f7e7f', 'MenuKey.RoleManagement', 'Sys.Edit', '角色管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkRole', 'Edit', NULL, 0, 0, 3, 1, '/api/_FrameworkRole/Edit', NULL, '08dac074-8c10-4f12-86cb-a3e43c04f048');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4fa2-848f-5982f68acfe3', 'MenuKey.RoleManagement', 'Sys.DownloadTemplate', '角色管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkRole', 'GetExcelTemplate', NULL, 0, 0, 4, 1, '/api/_FrameworkRole/GetExcelTemplate', NULL, '08dac074-8c10-4f12-86cb-a3e43c04f048');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4fca-85c6-55d535ea4b23', 'MenuKey.RoleManagement', 'Sys.Search', '角色管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkRole', 'Search', NULL, 0, 0, 5, 1, '/api/_FrameworkRole/Search', NULL, '08dac074-8c10-4f12-86cb-a3e43c04f048');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c10-4ff0-8fc4-5816e2cb8c9f', 'MenuKey.RoleManagement', 'Sys.Create', '角色管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkRole', 'Add', NULL, 0, 0, 6, 1, '/api/_FrameworkRole/Add', NULL, '08dac074-8c10-4f12-86cb-a3e43c04f048');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-4017-88b9-7ea8a7961335', 'MenuKey.RoleManagement', 'Sys.Delete', '角色管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkRole', 'BatchDelete', NULL, 0, 0, 7, 1, '/api/_FrameworkRole/BatchDelete', NULL, '08dac074-8c10-4f12-86cb-a3e43c04f048');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-403d-8b2d-5d1b75ac6054', 'MenuKey.RoleManagement', 'Sys.Export', '角色管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkRole', 'ExportExcel', NULL, 0, 0, 8, 1, '/api/_FrameworkRole/ExportExcel', NULL, '08dac074-8c10-4f12-86cb-a3e43c04f048');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-4065-847b-6406c6096125', 'MenuKey.RoleManagement', 'Sys.ExportByIds', '角色管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkRole', 'ExportExcelByIds', NULL, 0, 0, 9, 1, '/api/_FrameworkRole/ExportExcelByIds', NULL, '08dac074-8c10-4f12-86cb-a3e43c04f048');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-408b-8bb9-466170059b4f', 'MenuKey.RoleManagement', 'Sys.Import', '角色管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkRole', 'Import', NULL, 0, 0, 10, 1, '/api/_FrameworkRole/Import', NULL, '08dac074-8c10-4f12-86cb-a3e43c04f048');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-40c4-86ba-87814bf7024e', 'MenuKey.GroupManagement', 'MainPage', '用户组管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkGroup', NULL, NULL, 1, 0, 4, 1, '/frameworkgroup', NULL, '08dac074-8c10-4c63-820e-2aa4e0a3e6d3');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-40dd-809b-dcca3f32f9f0', 'MenuKey.GroupManagement', 'Sys.Get', '用户组管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkGroup', 'Get', NULL, 0, 0, 1, 1, '/api/_FrameworkGroup/{id}', NULL, '08dac074-8c11-40c4-86ba-87814bf7024e');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-4103-8e14-e65100162966', 'MenuKey.GroupManagement', 'Sys.Edit', '用户组管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkGroup', 'Edit', NULL, 0, 0, 2, 1, '/api/_FrameworkGroup/Edit', NULL, '08dac074-8c11-40c4-86ba-87814bf7024e');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-4129-8e61-32227ad151f0', 'MenuKey.GroupManagement', 'Sys.DownloadTemplate', '用户组管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkGroup', 'GetExcelTemplate', NULL, 0, 0, 3, 1, '/api/_FrameworkGroup/GetExcelTemplate', NULL, '08dac074-8c11-40c4-86ba-87814bf7024e');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-4151-8985-3fe242c34826', 'MenuKey.GroupManagement', 'Sys.Search', '用户组管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkGroup', 'Search', NULL, 0, 0, 4, 1, '/api/_FrameworkGroup/Search', NULL, '08dac074-8c11-40c4-86ba-87814bf7024e');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-4177-89b8-1c2baf9b05ec', 'MenuKey.GroupManagement', 'Sys.Create', '用户组管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkGroup', 'Add', NULL, 0, 0, 5, 1, '/api/_FrameworkGroup/Add', NULL, '08dac074-8c11-40c4-86ba-87814bf7024e');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-419e-8c44-51dd2d664d6e', 'MenuKey.GroupManagement', 'Sys.Delete', '用户组管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkGroup', 'BatchDelete', NULL, 0, 0, 6, 1, '/api/_FrameworkGroup/BatchDelete', NULL, '08dac074-8c11-40c4-86ba-87814bf7024e');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-41c4-8b4e-abe00a00e2ed', 'MenuKey.GroupManagement', 'Sys.Export', '用户组管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkGroup', 'ExportExcel', NULL, 0, 0, 7, 1, '/api/_FrameworkGroup/ExportExcel', NULL, '08dac074-8c11-40c4-86ba-87814bf7024e');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-41ef-869d-45ff4c3640d9', 'MenuKey.GroupManagement', 'Sys.ExportByIds', '用户组管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkGroup', 'ExportExcelByIds', NULL, 0, 0, 8, 1, '/api/_FrameworkGroup/ExportExcelByIds', NULL, '08dac074-8c11-40c4-86ba-87814bf7024e');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-4217-87fa-d6d6a4268db9', 'MenuKey.GroupManagement', 'Sys.Import', '用户组管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkGroup', 'Import', NULL, 0, 0, 9, 1, '/api/_FrameworkGroup/Import', NULL, '08dac074-8c11-40c4-86ba-87814bf7024e');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-424f-8e77-7dd602feb709', 'MenuKey.MenuMangement', 'MainPage', '菜单管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkMenu', NULL, NULL, 1, 0, 5, 1, '/frameworkmenu', NULL, '08dac074-8c10-4c63-820e-2aa4e0a3e6d3');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-4269-8b28-923d54464b92', 'MenuKey.MenuMangement', 'Sys.Get', '菜单管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkMenu', 'Get', NULL, 0, 0, 1, 1, '/api/_FrameworkMenu/{id}', NULL, '08dac074-8c11-424f-8e77-7dd602feb709');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-428f-8bfb-e50a5ada61f5', 'MenuKey.MenuMangement', 'Sys.Edit', '菜单管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkMenu', 'Edit', NULL, 0, 0, 2, 1, '/api/_FrameworkMenu/Edit', NULL, '08dac074-8c11-424f-8e77-7dd602feb709');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-42bc-866b-818801c32e57', 'MenuKey.MenuMangement', '_Admin.UnsetPages', '菜单管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkMenu', 'UnsetPages', NULL, 0, 0, 3, 1, '/api/_FrameworkMenu/UnsetPages', NULL, '08dac074-8c11-424f-8e77-7dd602feb709');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-42e2-8bd8-036ecf221632', 'MenuKey.MenuMangement', '_Admin.RefreshMenu', '菜单管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkMenu', 'RefreshMenu', NULL, 0, 0, 4, 1, '/api/_FrameworkMenu/RefreshMenu', NULL, '08dac074-8c11-424f-8e77-7dd602feb709');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-4309-80da-28eea8689f04', 'MenuKey.MenuMangement', 'Sys.Search', '菜单管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkMenu', 'Search', NULL, 0, 0, 5, 1, '/api/_FrameworkMenu/Search', NULL, '08dac074-8c11-424f-8e77-7dd602feb709');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-432f-862f-bc94d27669dc', 'MenuKey.MenuMangement', 'Sys.Create', '菜单管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkMenu', 'Add', NULL, 0, 0, 6, 1, '/api/_FrameworkMenu/Add', NULL, '08dac074-8c11-424f-8e77-7dd602feb709');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-4355-8693-e22721fad949', 'MenuKey.MenuMangement', 'Sys.Delete', '菜单管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkMenu', 'BatchDelete', NULL, 0, 0, 7, 1, '/api/_FrameworkMenu/BatchDelete', NULL, '08dac074-8c11-424f-8e77-7dd602feb709');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-437d-8eaf-68765ad05a52', 'MenuKey.MenuMangement', 'Sys.Export', '菜单管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkMenu', 'ExportExcel', NULL, 0, 0, 8, 1, '/api/_FrameworkMenu/ExportExcel', NULL, '08dac074-8c11-424f-8e77-7dd602feb709');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-43a4-8085-46b83847da5a', 'MenuKey.MenuMangement', 'Sys.ExportByIds', '菜单管理Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,FrameworkMenu', 'ExportExcelByIds', NULL, 0, 0, 9, 1, '/api/_FrameworkMenu/ExportExcelByIds', NULL, '08dac074-8c11-424f-8e77-7dd602feb709');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-43dc-8020-4c729d7bc00e', 'MenuKey.DataPrivilege', 'MainPage', '数据权限Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,DataPrivilege', NULL, NULL, 1, 0, 6, 1, '/dataprivilege', NULL, '08dac074-8c10-4c63-820e-2aa4e0a3e6d3');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-43f5-8b7f-0371a76841b4', 'MenuKey.DataPrivilege', 'Sys.Get', '数据权限Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,DataPrivilege', 'Get', NULL, 0, 0, 1, 1, '/api/_DataPrivilege/Get', NULL, '08dac074-8c11-43dc-8020-4c729d7bc00e');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-441c-83f9-1ca16c607e8f', 'MenuKey.DataPrivilege', 'Sys.Edit', '数据权限Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,DataPrivilege', 'Edit', NULL, 0, 0, 2, 1, '/api/_DataPrivilege/Edit', NULL, '08dac074-8c11-43dc-8020-4c729d7bc00e');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-4441-8f2d-6e4ded48d941', 'MenuKey.DataPrivilege', 'Sys.Search', '数据权限Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,DataPrivilege', 'Search', NULL, 0, 0, 3, 1, '/api/_DataPrivilege/Search', NULL, '08dac074-8c11-43dc-8020-4c729d7bc00e');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-446d-898d-9722a3c94e48', 'MenuKey.DataPrivilege', 'Sys.Create', '数据权限Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,DataPrivilege', 'Add', NULL, 0, 0, 4, 1, '/api/_DataPrivilege/Add', NULL, '08dac074-8c11-43dc-8020-4c729d7bc00e');
INSERT INTO `frameworkmenus` VALUES ('08dac074-8c11-4494-8e69-d6f0b5da3cd9', 'MenuKey.DataPrivilege', 'Sys.Delete', '数据权限Api', 0, 0, 'WalkingTec.Mvvm.Admin.Api,DataPrivilege', 'Delete', NULL, 0, 0, 5, 1, '/api/_DataPrivilege/Delete', NULL, '08dac074-8c11-43dc-8020-4c729d7bc00e');

-- ----------------------------
-- Table structure for frameworkroles
-- ----------------------------
DROP TABLE IF EXISTS `frameworkroles`;
CREATE TABLE `frameworkroles`  (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `RoleCode` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RoleName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RoleRemark` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `TenantCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `CreateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `UpdateTime` datetime(6) NULL DEFAULT NULL,
  `UpdateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of frameworkroles
-- ----------------------------
INSERT INTO `frameworkroles` VALUES ('6cd73c09-2ac4-45b1-b41b-eeb37bbafa5b', '002', '用户', NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `frameworkroles` VALUES ('94304e88-7654-4545-8064-9e85944c9ea6', '001', '超级管理员', NULL, NULL, NULL, 'Admin', NULL, NULL);

-- ----------------------------
-- Table structure for frameworkusergroups
-- ----------------------------
DROP TABLE IF EXISTS `frameworkusergroups`;
CREATE TABLE `frameworkusergroups`  (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `UserCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `GroupCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `CreateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `UpdateTime` datetime(6) NULL DEFAULT NULL,
  `UpdateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of frameworkusergroups
-- ----------------------------

-- ----------------------------
-- Table structure for frameworkuserroles
-- ----------------------------
DROP TABLE IF EXISTS `frameworkuserroles`;
CREATE TABLE `frameworkuserroles`  (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `UserCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RoleCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `CreateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `UpdateTime` datetime(6) NULL DEFAULT NULL,
  `UpdateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of frameworkuserroles
-- ----------------------------
INSERT INTO `frameworkuserroles` VALUES ('08dac074-8c3a-410b-8bc5-204cda20b30b', 'admin', '001', NULL, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for frameworkusers
-- ----------------------------
DROP TABLE IF EXISTS `frameworkusers`;
CREATE TABLE `frameworkusers`  (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Email` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Gender` int NULL DEFAULT NULL,
  `CellPhone` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `HomePhone` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Address` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `ZipCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `CreateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `UpdateTime` datetime(6) NULL DEFAULT NULL,
  `UpdateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `ITCode` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Password` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IsValid` tinyint(1) NOT NULL,
  `PhotoId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NULL DEFAULT NULL,
  `TenantCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  INDEX `IX_FrameworkUsers_PhotoId`(`PhotoId`) USING BTREE,
  CONSTRAINT `FK_FrameworkUsers_FileAttachments_PhotoId` FOREIGN KEY (`PhotoId`) REFERENCES `fileattachments` (`ID`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of frameworkusers
-- ----------------------------
INSERT INTO `frameworkusers` VALUES ('08dac074-8c35-4739-89cb-927e4528ae9a', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'admin', '670B14728AD9902AECBA32E22FA4F6BD', 'Admin', 1, NULL, NULL);

-- ----------------------------
-- Table structure for functionprivileges
-- ----------------------------
DROP TABLE IF EXISTS `functionprivileges`;
CREATE TABLE `functionprivileges`  (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `RoleCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `MenuItemId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Allowed` tinyint(1) NOT NULL,
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `CreateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `UpdateTime` datetime(6) NULL DEFAULT NULL,
  `UpdateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  INDEX `IX_FunctionPrivileges_MenuItemId`(`MenuItemId`) USING BTREE,
  CONSTRAINT `FK_FunctionPrivileges_FrameworkMenus_MenuItemId` FOREIGN KEY (`MenuItemId`) REFERENCES `frameworkmenus` (`ID`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of functionprivileges
-- ----------------------------
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0d-4079-8009-4893c2bf8205', '001', '08dac074-8c0c-4ccc-81f2-3c059bfac431', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-482a-8bc0-f62541b3ca04', '001', '08dac074-8c0f-47d0-8db2-cc04fc965208', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4859-8f4a-9b74a8382509', '001', '08dac074-8c0f-483f-8e52-40be8a3ec51e', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4893-8ba7-23fddf07699b', '001', '08dac074-8c0f-4874-823c-2b03010c6d05', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-48ab-8928-14208ae23050', '001', '08dac074-8c0a-4931-8269-f0015556d3d1', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-48f6-8817-aad860510777', '001', '08dac074-8c0f-48de-8b87-aa160027abc1', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4920-8fd5-cbaa7eece0cf', '001', '08dac074-8c0f-490a-84ae-023e6a4c48bc', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4949-8795-54415f595474', '001', '08dac074-8c0f-4932-8ca7-db0c29ecc225', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4975-81ff-6c6406226595', '001', '08dac074-8c0f-4960-8553-7cebcf6aa89f', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-499f-8d09-0e3383f9c83f', '001', '08dac074-8c0f-4988-8489-1a56e0ef323d', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-49c6-8b1d-c72b784b7041', '001', '08dac074-8c0f-49b0-8dfa-f0665fd41ce3', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-49ee-876b-0c5a91e28eb6', '001', '08dac074-8c0f-49d7-8691-2f00252881cb', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4a1f-840f-e3916a8fad77', '001', '08dac074-8c0f-4a04-8501-20ab39da605b', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4a46-80ed-e4522ccafbf3', '001', '08dac074-8c0f-4a30-820f-d7ffed4055e8', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4a6c-8982-dcc4f0e90bcd', '001', '08dac074-8c0f-4a56-894c-186f19c8a304', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4a94-8222-00cb4edf4687', '001', '08dac074-8c0f-4a7f-83dc-a14da1c284dd', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4aa9-8084-2fa30e3a2611', '001', '08dac074-8c0f-48be-83d7-d3f34457b47f', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4ae9-85b6-bda022f61464', '001', '08dac074-8c0f-4ad1-8f82-1c2a0e2365ab', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4b14-805b-9b651d1ff516', '001', '08dac074-8c0f-4afe-8205-db7f3fe4086b', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4b3b-8d03-6b315d725e3f', '001', '08dac074-8c0f-4b24-8a5c-b027a2d77ddc', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4b64-833b-221c60cde509', '001', '08dac074-8c0f-4b4e-82ae-cf707ec14aed', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4b8a-8583-d24dcb9977ed', '001', '08dac074-8c0f-4b74-8e0e-8a146569f410', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4bb1-8a7e-6cdb1e4920a6', '001', '08dac074-8c0f-4b9b-8dcf-66a4eb47e532', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4bd9-82aa-d2fee0b7dfff', '001', '08dac074-8c0f-4bc4-851a-b2c5b7dec17d', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4c01-8b49-8ab59ea09ba8', '001', '08dac074-8c0f-4beb-8a01-de6ad1d43988', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4c29-8bf8-24bd159dbb2c', '001', '08dac074-8c0f-4c15-8a6b-b626f34d7085', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4c3c-8d75-2cb384e8f4db', '001', '08dac074-8c0f-4ab9-8bba-6167eaeea132', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4c7b-893a-87f110f54c5e', '001', '08dac074-8c0f-4c65-8827-d3eacfc3b014', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4ca1-8c50-556738505513', '001', '08dac074-8c0f-4c8c-8138-156dd7afd52b', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4cc7-8f35-6df6bcec6256', '001', '08dac074-8c0f-4cb4-805f-408d47477ccf', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4cee-88e4-c9736d5782d7', '001', '08dac074-8c0f-4cda-898e-f45aef6af445', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4d17-8e78-ce17dbe35d90', '001', '08dac074-8c0f-4d02-82ee-83aaca8a0eb3', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4d40-8731-9b179650165a', '001', '08dac074-8c0f-4d2b-8c6d-5a70d94f57cb', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4d68-8a2d-170c9c2e659d', '001', '08dac074-8c0f-4d53-8054-dc2a6b1ca4f4', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4da8-8147-bd606de40e65', '001', '08dac074-8c0f-4d7f-8041-4dd643264814', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4dc7-8632-0c944cfc6198', '001', '08dac074-8c0f-4c4c-8e9a-74ea6f0d5f17', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4e1b-8cf0-39e6cecd1d71', '001', '08dac074-8c0f-4e05-8737-8506526db3ac', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4e44-8bae-a5a0f6ddc9d5', '001', '08dac074-8c0f-4e2e-8969-2d75f9961e48', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4e75-8629-c5f03a9587bc', '001', '08dac074-8c0f-4e5a-8606-318172cac185', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4e9f-8f66-cd3be65e07a1', '001', '08dac074-8c0f-4e89-876c-24e71aaa70b5', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4ecc-8f0c-63a4feff9ec9', '001', '08dac074-8c0f-4eb6-89e4-8f7880ebade9', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4ef3-8b2d-6a3f320ee7a1', '001', '08dac074-8c0f-4edd-8ed9-a10a4bc8ec89', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4f1c-8f64-6fc3c11b9e27', '001', '08dac074-8c0f-4f05-8947-77c8ac78f8f1', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4f2d-8f13-0ddf0588a5db', '001', '08dac074-8c0f-4ddf-87c8-196854fbd220', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4f70-81d7-47e2d11eda97', '001', '08dac074-8c0f-4f58-86df-c7d6387de8e0', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4f9a-8c1c-859664cec818', '001', '08dac074-8c0f-4f81-8e0a-f7deba609f59', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4fc1-8162-3fdc72aa65ca', '001', '08dac074-8c0f-4fad-881b-4814008067a3', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c0f-4fe7-8264-6d993de7995a', '001', '08dac074-8c0f-4fd3-8aac-6981ce8bca69', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-400e-88d3-331d9de9a041', '001', '08dac074-8c0f-4ffa-81e5-915452e433c6', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4021-8afd-a9623c1be99c', '001', '08dac074-8c0f-4f40-8edc-315bb28d028a', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4031-8201-59accb8c8b84', '001', '08dac074-8c00-4da9-8730-d0f58b175bf7', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4cbf-8285-2e2cb43d67cd', '001', '08dac074-8c10-4ca6-883f-beded738238f', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4ce8-8da6-1d4ea9248e96', '001', '08dac074-8c10-4cd2-84b6-1eb698329bdd', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4d13-8493-fbc9bdeea9c6', '001', '08dac074-8c10-4cfa-8f9e-adcc49e2cd87', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4d3c-8588-87c3474fde0e', '001', '08dac074-8c10-4d25-8610-b2713c0d7957', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4d63-8867-b004b8c16295', '001', '08dac074-8c10-4d4d-8618-42cf073be9d2', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4d76-89e3-bc1b343314a3', '001', '08dac074-8c10-4c87-8e86-131cf8c56371', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4db4-892a-9251d8017ed9', '001', '08dac074-8c10-4da0-874a-ec66ce841446', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4ddd-87e4-1ba7eb8a3bac', '001', '08dac074-8c10-4dc7-87ba-b7b2867b665b', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4e03-8b03-fe73b838f4d8', '001', '08dac074-8c10-4dee-80da-111f44418eee', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4e2a-8430-fb35a3b0cce0', '001', '08dac074-8c10-4e14-82d2-e6c548e6f371', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4e50-89e9-1c29de17b81c', '001', '08dac074-8c10-4e3a-8c0f-cf98c7d4e299', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4e77-8569-0fbffbc20187', '001', '08dac074-8c10-4e61-8486-73eb6655b783', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4e9d-8c6e-f065ed9c6032', '001', '08dac074-8c10-4e89-8af8-02915b022c71', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4ec7-8f3c-5cbb7154ebb6', '001', '08dac074-8c10-4eb1-89d0-47a5b29130e3', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4eee-8f0e-33ec8bed1913', '001', '08dac074-8c10-4edb-826b-c09049bac70a', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4f02-84d8-618450d2f3d5', '001', '08dac074-8c10-4d87-827b-4a16cc6efb4b', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4f40-80c6-53d61652abfb', '001', '08dac074-8c10-4f29-8c90-d1959626e7cd', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4f66-852c-aa79ef4b0c30', '001', '08dac074-8c10-4f50-89ec-8689612ddd46', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4f8f-8c0d-5ff3345c99e8', '001', '08dac074-8c10-4f77-8cf3-ae1a959f7e7f', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4fb6-82c8-7d3b1d6b62f6', '001', '08dac074-8c10-4fa2-848f-5982f68acfe3', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c10-4fde-85d8-1136676636f1', '001', '08dac074-8c10-4fca-85c6-55d535ea4b23', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-4004-8b44-2fa78546711c', '001', '08dac074-8c10-4ff0-8fc4-5816e2cb8c9f', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-402d-823a-5c89a32868fd', '001', '08dac074-8c11-4017-88b9-7ea8a7961335', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-4053-872d-e6313eeedb60', '001', '08dac074-8c11-403d-8b2d-5d1b75ac6054', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-407b-84ef-a57e6b719778', '001', '08dac074-8c11-4065-847b-6406c6096125', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-40a1-884a-bcb04560537b', '001', '08dac074-8c11-408b-8bb9-466170059b4f', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-40b2-845d-40f28099a929', '001', '08dac074-8c10-4f12-86cb-a3e43c04f048', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-40f1-8004-91857e7cc1c5', '001', '08dac074-8c11-40dd-809b-dcca3f32f9f0', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-4117-8826-08a0d4936b9c', '001', '08dac074-8c11-4103-8e14-e65100162966', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-4141-8447-b4edade4b448', '001', '08dac074-8c11-4129-8e61-32227ad151f0', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-4167-840e-f6ad4b54d0b5', '001', '08dac074-8c11-4151-8985-3fe242c34826', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-418e-879d-e66a261280ff', '001', '08dac074-8c11-4177-89b8-1c2baf9b05ec', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-41b4-8822-1b4e765326c3', '001', '08dac074-8c11-419e-8c44-51dd2d664d6e', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-41db-8d08-234c72afc1b8', '001', '08dac074-8c11-41c4-8b4e-abe00a00e2ed', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-4203-8bb2-7b4a584fe5d9', '001', '08dac074-8c11-41ef-869d-45ff4c3640d9', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-422b-84a6-7bffafa5af09', '001', '08dac074-8c11-4217-87fa-d6d6a4268db9', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-423f-8b13-9a781ac20b1e', '001', '08dac074-8c11-40c4-86ba-87814bf7024e', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-427f-8652-ff4b3d75a255', '001', '08dac074-8c11-4269-8b28-923d54464b92', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-42a5-8b0c-48f762ed2a6a', '001', '08dac074-8c11-428f-8bfb-e50a5ada61f5', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-42d2-81ec-18929dae3e33', '001', '08dac074-8c11-42bc-866b-818801c32e57', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-42f8-8976-5b0f9287fbbe', '001', '08dac074-8c11-42e2-8bd8-036ecf221632', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-431f-807b-762e672beb46', '001', '08dac074-8c11-4309-80da-28eea8689f04', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-4345-817f-ccbe24492820', '001', '08dac074-8c11-432f-862f-bc94d27669dc', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-436b-8646-4bd1eae2b83a', '001', '08dac074-8c11-4355-8693-e22721fad949', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-4391-8a24-a285657f05a5', '001', '08dac074-8c11-437d-8eaf-68765ad05a52', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-43b7-896e-888c8614b29e', '001', '08dac074-8c11-43a4-8085-46b83847da5a', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-43cb-8e38-ccee649ee6c3', '001', '08dac074-8c11-424f-8e77-7dd602feb709', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-440c-804e-fd1b7e7e747a', '001', '08dac074-8c11-43f5-8b7f-0371a76841b4', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-4431-8bb1-28d637415c13', '001', '08dac074-8c11-441c-83f9-1ca16c607e8f', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-4457-8acc-869db17848d6', '001', '08dac074-8c11-4441-8f2d-6e4ded48d941', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-4483-8412-f0bb218d99d3', '001', '08dac074-8c11-446d-898d-9722a3c94e48', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-44ac-81de-7205ea7bad0e', '001', '08dac074-8c11-4494-8e69-d6f0b5da3cd9', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-44bd-86a6-d284cdd43a23', '001', '08dac074-8c11-43dc-8020-4c729d7bc00e', 1, NULL, NULL, NULL, NULL);
INSERT INTO `functionprivileges` VALUES ('08dac074-8c11-44cf-81a9-5269ab550b4e', '001', '08dac074-8c10-4c63-820e-2aa4e0a3e6d3', 1, NULL, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for persistedgrants
-- ----------------------------
DROP TABLE IF EXISTS `persistedgrants`;
CREATE TABLE `persistedgrants`  (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `UserCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `CreationTime` datetime(6) NOT NULL,
  `Expiration` datetime(6) NOT NULL,
  `RefreshToken` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of persistedgrants
-- ----------------------------

-- ----------------------------
-- Table structure for rpclogs
-- ----------------------------
DROP TABLE IF EXISTS `rpclogs`;
CREATE TABLE `rpclogs`  (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `RpcSide` int NOT NULL,
  `StartTime` datetime(6) NOT NULL,
  `DeviceId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NULL DEFAULT NULL,
  `Method` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Params` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `EndTime` datetime(6) NOT NULL,
  `IsSuccess` tinyint(1) NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  INDEX `IX_RpcLogs_DeviceId`(`DeviceId`) USING BTREE,
  CONSTRAINT `FK_RpcLogs_Devices_DeviceId` FOREIGN KEY (`DeviceId`) REFERENCES `devices` (`ID`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of rpclogs
-- ----------------------------

-- ----------------------------
-- Table structure for systemconfig
-- ----------------------------
DROP TABLE IF EXISTS `systemconfig`;
CREATE TABLE `systemconfig`  (
  `ID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `GatewayName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `ClientId` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `MqttIp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `MqttPort` int NOT NULL,
  `MqttUName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `MqttUPwd` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `IoTPlatformType` int NOT NULL,
  `CreateTime` datetime(6) NULL DEFAULT NULL,
  `CreateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `UpdateTime` datetime(6) NULL DEFAULT NULL,
  `UpdateBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of systemconfig
-- ----------------------------
INSERT INTO `systemconfig` VALUES ('BF014E65-6A03-471B-807B-294B3AFB2773', 'IoT网关', 'iotgateway', '127.0.0.1', 1888, 'eagls2JfHNFoXdj4dnUT', 'pwd', 1, '1900-01-20 16:19:30.591479', 'admin', '2022-10-28 21:34:27.263539', 'admin');

SET FOREIGN_KEY_CHECKS = 1;
