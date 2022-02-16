-- MySQL dump 10.13  Distrib 8.0.23, for Win64 (x86_64)
--
-- Host: localhost    Database: remote_desktop
-- ------------------------------------------------------
-- Server version	8.0.23

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `admin_accounts`
--

DROP TABLE IF EXISTS `admin_accounts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `admin_accounts` (
  `admin_account_id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL,
  `password` varchar(50) NOT NULL,
  PRIMARY KEY (`admin_account_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `admin_accounts`
--

LOCK TABLES `admin_accounts` WRITE;
/*!40000 ALTER TABLE `admin_accounts` DISABLE KEYS */;
INSERT INTO `admin_accounts` VALUES (1,'admin','admin');
/*!40000 ALTER TABLE `admin_accounts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `admin_configs`
--

DROP TABLE IF EXISTS `admin_configs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `admin_configs` (
  `admin_config_id` int NOT NULL AUTO_INCREMENT,
  `psexec_directory` varchar(50) NOT NULL,
  `psshutdown_directory` varchar(50) NOT NULL,
  `screenshot_directory` varchar(255) NOT NULL COMMENT 'from the server machine, this is where all the screenshots of lab desktops are pointed by desktops.screenshot_directory. So, make sure that all fields of desktops.screenshot_directory are pointing correctly.',
  PRIMARY KEY (`admin_config_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `admin_configs`
--

LOCK TABLES `admin_configs` WRITE;
/*!40000 ALTER TABLE `admin_configs` DISABLE KEYS */;
INSERT INTO `admin_configs` VALUES (1,'C:\\Windows\\System32\\PSTools\\psexec64','C:\\Windows\\System32\\PSTools\\psshutdown','C:\\Users\\Earth Jan\\Documents\\csharp_files\\csharp_codes\\RemoteScreenshot\\Screenshots\\');
/*!40000 ALTER TABLE `admin_configs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `attendance_students`
--

DROP TABLE IF EXISTS `attendance_students`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `attendance_students` (
  `attendance_student_id` int NOT NULL AUTO_INCREMENT,
  `attendance_id` int NOT NULL,
  `student_id` int NOT NULL,
  `timestamp` datetime NOT NULL,
  PRIMARY KEY (`attendance_student_id`),
  KEY `attendance_id_attendance_students_idx` (`attendance_id`),
  KEY `student_id_attendance_students_idx` (`student_id`),
  CONSTRAINT `attendance_id_attendance_students` FOREIGN KEY (`attendance_id`) REFERENCES `attendances` (`attendance_id`),
  CONSTRAINT `student_id_attendance_students` FOREIGN KEY (`student_id`) REFERENCES `students` (`student_id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `attendance_students`
--

LOCK TABLES `attendance_students` WRITE;
/*!40000 ALTER TABLE `attendance_students` DISABLE KEYS */;
/*!40000 ALTER TABLE `attendance_students` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `attendances`
--

DROP TABLE IF EXISTS `attendances`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `attendances` (
  `attendance_id` int NOT NULL AUTO_INCREMENT,
  `class` varchar(10) NOT NULL,
  `session` datetime NOT NULL,
  `status` tinyint NOT NULL DEFAULT '1',
  PRIMARY KEY (`attendance_id`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `attendances`
--

LOCK TABLES `attendances` WRITE;
/*!40000 ALTER TABLE `attendances` DISABLE KEYS */;
/*!40000 ALTER TABLE `attendances` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `desktops`
--

DROP TABLE IF EXISTS `desktops`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `desktops` (
  `desktop_id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `username` varchar(50) NOT NULL,
  `password` varchar(50) NOT NULL,
  `screenshot_directory` varchar(255) NOT NULL,
  `nircmd_directory` varchar(255) NOT NULL,
  `user_session` int NOT NULL,
  `tasklist_output_directory` varchar(255) NOT NULL,
  `status` tinyint NOT NULL DEFAULT '1',
  `ip_address` varchar(50) NOT NULL,
  `output_device_status` varchar(255) NOT NULL,
  `output_device_monitoring_script_directory` varchar(255) NOT NULL,
  `currently_running_apps` longtext NOT NULL,
  `app_monitoring_script_directory` varchar(255) NOT NULL,
  PRIMARY KEY (`desktop_id`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=1000 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `desktops`
--

LOCK TABLES `desktops` WRITE;
/*!40000 ALTER TABLE `desktops` DISABLE KEYS */;
INSERT INTO `desktops` VALUES (1,'vm1','virtualmachine1v2','virtualmachine1v2','\\\\vmware-host\\Shared Folders\\Users\\Earth Jan\\Documents\\csharp_files\\csharp_codes\\RemoteScreenshot\\Screenshots\\','\\\\vmware-host\\Shared Folders\\Windows\\System32\\nircmd64\\nircmd',45,'C:\\Users\\Earth Jan\\Documents\\csharp_files\\csharp_codes\\RemoteScreenshot\\TasklistOutputs\\',0,'192.168.43.55','Operation failed: couldn\'t get the status of vm1\'s output devices.','\\\\vmware-host\\Shared Folders\\Users\\Earth Jan\\Documents\\Git\\Repos\\computer-lab-management\\Scripts\\IODeviceStatuses.ps1','\nPsExec v2.33 - Execute processes remotely\nCopyright (C) 2001-2021 Mark Russinovich\nSysinternals - www.sysinternals.com\nConnecting to vm1...\nStarting PSEXESVC service on vm1...\nCopying authentication key to vm1...\nConnecting with PsExec service on vm1...\nStarting cmd on vm1...','\\\\vmware-host\\Shared Folders\\Users\\Earth Jan\\Documents\\Git\\Repos\\computer-lab-management\\Scripts\\CurrentRunningApps.ps1'),(999,'testForPowerStatus','testForPowerStatus','testForPowerStatus','testForPowerStatus','testForPowerStatus',999,'testForPowerStatus',0,'100.96.23.13','Operation failed: couldn\'t get the status of testForPowerStatus\'s output devices.','C:\\Users\\Earth Jan\\Documents\\csharp_files\\csharp_codes\\RemoteScreenshot\\Scripts\\IODeviceStatuses.ps1','\nPsExec v2.33 - Execute processes remotely\nCopyright (C) 2001-2021 Mark Russinovich\nSysinternals - www.sysinternals.com\nConnecting to testForPowerStatus...\nConnecting to testForPowerStatus...','');
/*!40000 ALTER TABLE `desktops` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `laboratories`
--

DROP TABLE IF EXISTS `laboratories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `laboratories` (
  `laboratory_id` int NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`laboratory_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `laboratories`
--

LOCK TABLES `laboratories` WRITE;
/*!40000 ALTER TABLE `laboratories` DISABLE KEYS */;
INSERT INTO `laboratories` VALUES (1),(2);
/*!40000 ALTER TABLE `laboratories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `laboratory_desktops`
--

DROP TABLE IF EXISTS `laboratory_desktops`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `laboratory_desktops` (
  `laboratory_desktop_id` int NOT NULL AUTO_INCREMENT,
  `laboratory_id` int NOT NULL,
  `desktop_id` int NOT NULL,
  PRIMARY KEY (`laboratory_desktop_id`),
  KEY `desktop_id_idx` (`desktop_id`),
  KEY `laboratory_id_laboratory_desktops_idx` (`laboratory_id`),
  CONSTRAINT `desktop_id_laboratory_desktops` FOREIGN KEY (`desktop_id`) REFERENCES `desktops` (`desktop_id`),
  CONSTRAINT `laboratory_id_laboratory_desktops` FOREIGN KEY (`laboratory_id`) REFERENCES `laboratories` (`laboratory_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `laboratory_desktops`
--

LOCK TABLES `laboratory_desktops` WRITE;
/*!40000 ALTER TABLE `laboratory_desktops` DISABLE KEYS */;
INSERT INTO `laboratory_desktops` VALUES (1,1,1),(2,2,999);
/*!40000 ALTER TABLE `laboratory_desktops` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `screenshots`
--

DROP TABLE IF EXISTS `screenshots`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `screenshots` (
  `screenshot_id` int NOT NULL AUTO_INCREMENT,
  `desktop_id` int NOT NULL,
  `image` mediumblob NOT NULL,
  `timestamp` datetime NOT NULL,
  PRIMARY KEY (`screenshot_id`),
  KEY `desktop_id_idx` (`desktop_id`),
  CONSTRAINT `desktop_id_screenshots` FOREIGN KEY (`desktop_id`) REFERENCES `desktops` (`desktop_id`)
) ENGINE=InnoDB AUTO_INCREMENT=326 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `screenshots`
--

LOCK TABLES `screenshots` WRITE;
/*!40000 ALTER TABLE `screenshots` DISABLE KEYS */;
/*!40000 ALTER TABLE `screenshots` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `students`
--

DROP TABLE IF EXISTS `students`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `students` (
  `student_id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL,
  `password` varchar(50) NOT NULL,
  `ip_address` varchar(50) NOT NULL,
  PRIMARY KEY (`student_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `students`
--

LOCK TABLES `students` WRITE;
/*!40000 ALTER TABLE `students` DISABLE KEYS */;
INSERT INTO `students` VALUES (1,'student1','student1','192.168.43.55'),(2,'student2','student2','100.96.23.13');
/*!40000 ALTER TABLE `students` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-02-16 23:56:44
