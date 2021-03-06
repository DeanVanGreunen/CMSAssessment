USE [master]
GO

/****** Object:  Database [cms_test]    Script Date: 2021/05/13 18:13:22 ******/
DROP DATABASE [cms_test]
GO

/****** Object:  Database [cms_test]    Script Date: 2021/05/13 18:13:22 ******/
CREATE DATABASE [cms_test]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'cms_test', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\cms_test.mdf' , SIZE = 139264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'cms_test_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\cms_test_log.ldf' , SIZE = 139264KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [cms_test].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [cms_test] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [cms_test] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [cms_test] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [cms_test] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [cms_test] SET ARITHABORT OFF 
GO

ALTER DATABASE [cms_test] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [cms_test] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [cms_test] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [cms_test] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [cms_test] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [cms_test] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [cms_test] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [cms_test] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [cms_test] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [cms_test] SET  DISABLE_BROKER 
GO

ALTER DATABASE [cms_test] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [cms_test] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [cms_test] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [cms_test] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [cms_test] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [cms_test] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [cms_test] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [cms_test] SET RECOVERY FULL 
GO

ALTER DATABASE [cms_test] SET  MULTI_USER 
GO

ALTER DATABASE [cms_test] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [cms_test] SET DB_CHAINING OFF 
GO

ALTER DATABASE [cms_test] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [cms_test] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [cms_test] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [cms_test] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [cms_test] SET QUERY_STORE = OFF
GO

ALTER DATABASE [cms_test] SET  READ_WRITE 
GO


