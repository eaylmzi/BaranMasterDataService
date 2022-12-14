USE [master]
GO
/****** Object:  Database [BaranMasterDataDB]    Script Date: 19.08.2022 12:14:22 ******/
CREATE DATABASE [BaranMasterDataDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BaranMasterDataDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\BaranMasterDataDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BaranMasterDataDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\BaranMasterDataDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [BaranMasterDataDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BaranMasterDataDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BaranMasterDataDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BaranMasterDataDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BaranMasterDataDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BaranMasterDataDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BaranMasterDataDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [BaranMasterDataDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BaranMasterDataDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BaranMasterDataDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BaranMasterDataDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BaranMasterDataDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BaranMasterDataDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BaranMasterDataDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BaranMasterDataDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BaranMasterDataDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BaranMasterDataDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [BaranMasterDataDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BaranMasterDataDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BaranMasterDataDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BaranMasterDataDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BaranMasterDataDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BaranMasterDataDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BaranMasterDataDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BaranMasterDataDB] SET RECOVERY FULL 
GO
ALTER DATABASE [BaranMasterDataDB] SET  MULTI_USER 
GO
ALTER DATABASE [BaranMasterDataDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BaranMasterDataDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BaranMasterDataDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BaranMasterDataDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BaranMasterDataDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BaranMasterDataDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'BaranMasterDataDB', N'ON'
GO
ALTER DATABASE [BaranMasterDataDB] SET QUERY_STORE = OFF
GO
USE [BaranMasterDataDB]
GO
/****** Object:  Table [dbo].[CNMaterials]    Script Date: 19.08.2022 12:14:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CNMaterials](
	[material] [nvarchar](12) NOT NULL,
	[stext] [nvarchar](100) NULL,
	[qunit] [nvarchar](3) NULL,
	[cnwdate] [datetime] NULL,
	[ddwdate] [datetime] NULL,
	[fswdate] [datetime] NULL,
	[cnrdate] [datetime] NULL,
	[ddrdate] [datetime] NULL,
	[fsrdate] [datetime] NULL,
 CONSTRAINT [PK_CNMaterials] PRIMARY KEY CLUSTERED 
(
	[material] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ErrorLog]    Script Date: 19.08.2022 12:14:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ErrorLog](
	[log_id] [int] IDENTITY(1,1) NOT NULL,
	[error_date] [datetime] NULL,
	[error_material_code] [nvarchar](12) NULL,
	[error_message] [nvarchar](max) NULL,
 CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED 
(
	[log_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionLog]    Script Date: 19.08.2022 12:14:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionLog](
	[log_id] [int] IDENTITY(1,1) NOT NULL,
	[material] [nvarchar](12) NULL,
	[transaction_result] [int] NULL,
	[transaction_message] [nvarchar](100) NULL,
 CONSTRAINT [PK_TransactionLog] PRIMARY KEY CLUSTERED 
(
	[log_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[findNullFSRDate]    Script Date: 19.08.2022 12:14:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[findNullFSRDate]
as
SELECT * from dbo.CNMaterials where fsrdate IS NULL
GO
/****** Object:  StoredProcedure [dbo].[insertToErrorLog]    Script Date: 19.08.2022 12:14:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[insertToErrorLog](@error_date datetime,@error_material_code nvarchar(12),@error_message nvarchar(max))
as
INSERT INTO dbo.ErrorLog (error_date,error_material_code,error_message) VALUES (@error_date,@error_material_code,@error_message)
GO
/****** Object:  StoredProcedure [dbo].[insertToTransactionLog]    Script Date: 19.08.2022 12:14:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[insertToTransactionLog](@material nvarchar(12),@transaction_result int,@transaction_message nvarchar(max))
as
INSERT INTO dbo.TransactionLog (material,transaction_result,transaction_message)VALUES (@material,@transaction_result,@transaction_message)
GO
/****** Object:  StoredProcedure [dbo].[updateFSRDate]    Script Date: 19.08.2022 12:14:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[updateFSRDate](@material nvarchar(12),@fsr_date datetime)
as
UPDATE dbo.CNMaterials set fsrdate=@fsr_date WHERE material=@material
GO
USE [master]
GO
ALTER DATABASE [BaranMasterDataDB] SET  READ_WRITE 
GO
