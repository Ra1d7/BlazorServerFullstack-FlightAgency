USE [master]
GO
/****** Object:  Database [TravelAgencyDB]    Script Date: 4/30/2023 1:26:58 PM ******/
CREATE DATABASE [TravelAgencyDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TravelAgencyDB', FILENAME = N'D:\SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\TravelAgencyDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TravelAgencyDB_log', FILENAME = N'D:\SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\TravelAgencyDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [TravelAgencyDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TravelAgencyDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TravelAgencyDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TravelAgencyDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TravelAgencyDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TravelAgencyDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TravelAgencyDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [TravelAgencyDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TravelAgencyDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TravelAgencyDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TravelAgencyDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TravelAgencyDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TravelAgencyDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TravelAgencyDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TravelAgencyDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TravelAgencyDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TravelAgencyDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [TravelAgencyDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TravelAgencyDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TravelAgencyDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TravelAgencyDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TravelAgencyDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TravelAgencyDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TravelAgencyDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TravelAgencyDB] SET RECOVERY FULL 
GO
ALTER DATABASE [TravelAgencyDB] SET  MULTI_USER 
GO
ALTER DATABASE [TravelAgencyDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TravelAgencyDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TravelAgencyDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TravelAgencyDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [TravelAgencyDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [TravelAgencyDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'TravelAgencyDB', N'ON'
GO
ALTER DATABASE [TravelAgencyDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [TravelAgencyDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [TravelAgencyDB]
GO
/****** Object:  UserDefinedFunction [dbo].[fnIsValidEmail]    Script Date: 4/30/2023 1:26:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fnIsValidEmail]
(
    @email varchar(255)
)   
--Returns true if the string is a valid email address.  
RETURNS bit  
As  
BEGIN
    RETURN CASE WHEN ISNULL(@email, '') <> '' AND @email LIKE '%_@%_.__%' THEN 1 ELSE 0 END
END
GO
/****** Object:  Table [dbo].[BookedFlights]    Script Date: 4/30/2023 1:26:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookedFlights](
	[flightId] [int] NOT NULL,
	[userId] [int] NOT NULL,
	[book_time] [datetime] NOT NULL,
 CONSTRAINT [PK_BookedFlights] PRIMARY KEY CLUSTERED 
(
	[flightId] ASC,
	[userId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CancelledFlights]    Script Date: 4/30/2023 1:26:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CancelledFlights](
	[flightid] [int] NOT NULL,
	[userid] [int] NOT NULL,
	[cancellation_date] [datetime] NOT NULL,
	[reason] [nvarchar](350) NOT NULL,
 CONSTRAINT [PK_CancelledFlights] PRIMARY KEY CLUSTERED 
(
	[flightid] ASC,
	[userid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Flights]    Script Date: 4/30/2023 1:26:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Flights](
	[flightId] [int] IDENTITY(1,1) NOT NULL,
	[from] [nvarchar](80) NOT NULL,
	[to] [nvarchar](80) NOT NULL,
	[num_of_seats] [int] NOT NULL,
	[flight_time] [datetime] NOT NULL,
	[cost] [money] NOT NULL,
 CONSTRAINT [PK_Flights] PRIMARY KEY CLUSTERED 
(
	[flightId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 4/30/2023 1:26:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[roleId] [int] IDENTITY(1,1) NOT NULL,
	[role] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[roleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 4/30/2023 1:26:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[userId] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](50) NOT NULL,
	[hashedPass] [nvarchar](250) NOT NULL,
	[roleId] [int] NOT NULL,
	[email] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[userId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BookedFlights]  WITH CHECK ADD  CONSTRAINT [FK_BookedFlights_Flights] FOREIGN KEY([flightId])
REFERENCES [dbo].[Flights] ([flightId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BookedFlights] CHECK CONSTRAINT [FK_BookedFlights_Flights]
GO
ALTER TABLE [dbo].[BookedFlights]  WITH CHECK ADD  CONSTRAINT [FK_BookedFlights_Users] FOREIGN KEY([userId])
REFERENCES [dbo].[Users] ([userId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BookedFlights] CHECK CONSTRAINT [FK_BookedFlights_Users]
GO
ALTER TABLE [dbo].[CancelledFlights]  WITH CHECK ADD  CONSTRAINT [FK_CancelledFlights_Flights] FOREIGN KEY([flightid])
REFERENCES [dbo].[Flights] ([flightId])
GO
ALTER TABLE [dbo].[CancelledFlights] CHECK CONSTRAINT [FK_CancelledFlights_Flights]
GO
ALTER TABLE [dbo].[CancelledFlights]  WITH CHECK ADD  CONSTRAINT [FK_CancelledFlights_Users] FOREIGN KEY([userid])
REFERENCES [dbo].[Users] ([userId])
GO
ALTER TABLE [dbo].[CancelledFlights] CHECK CONSTRAINT [FK_CancelledFlights_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([roleId])
REFERENCES [dbo].[Roles] ([roleId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [valid_email] CHECK  (([email] like '%_@%_.__%'))
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [valid_email]
GO
/****** Object:  StoredProcedure [dbo].[Admin_AddFlight]    Script Date: 4/30/2023 1:26:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Admin_AddFlight]
@from nvarchar(80),
@to nvarchar(80),
@seats INT,
@time datetime,
@cost money
AS
BEGIN
INSERT INTO FLIGHTS ([from],[to],num_of_seats,flight_time,cost) VALUES (@from,@to,@seats,@time,@cost);
END
GO
/****** Object:  StoredProcedure [dbo].[Admin_CreateUser]    Script Date: 4/30/2023 1:26:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Admin_CreateUser]
@username varchar(50),
@password nvarchar(250),
@role int,
@email varchar(50)
AS
BEGIN
INSERT INTO Users (username,hashedPass,roleId,email) VALUES (@username,@password,@role,@email);
END
GO
/****** Object:  StoredProcedure [dbo].[Admin_DeleteFlight]    Script Date: 4/30/2023 1:26:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Admin_DeleteFlight]
@flightId INT
AS
BEGIN
DELETE FROM Flights WHERE flightId = @flightId
END
GO
/****** Object:  StoredProcedure [dbo].[Admin_UpdateFlight]    Script Date: 4/30/2023 1:26:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Admin_UpdateFlight]
@flightid int,
@from varchar(80),
@to varchar(80),
@seats int,
@time datetime,
@cost money
AS
BEGIN
UPDATE Flights SET [from]=@from,[to]=@to,num_of_seats=@seats,flight_time=@time,cost=@cost WHERE flightId = @flightid
END
GO
/****** Object:  StoredProcedure [dbo].[Admin_UpdateUser]    Script Date: 4/30/2023 1:26:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Admin_UpdateUser]
@userid int,
@username varchar(50),
@password nvarchar(250),
@role int,
@email varchar(50)
AS
BEGIN
UPDATE Users SET username=@username,hashedPass=@password,roleId=@role,email=@email WHERE userId = @userid
END
GO
/****** Object:  StoredProcedure [dbo].[BookFlight]    Script Date: 4/30/2023 1:26:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BookFlight]
@userId INT,
@flightId INT
AS
BEGIN
IF(
(SELECT num_of_seats FROM Flights WHERE flightId = @flightId) > 0)
BEGIN
INSERT INTO BookedFlights(userId,flightId,book_time) VALUES (@userId,@flightId,GETDATE());
UPDATE Flights SET num_of_seats = num_of_seats-1 WHERE flightId = @flightId
SELECT 1
RETURN
END
ELSE
SELECT 0
RETURN
END
GO
/****** Object:  StoredProcedure [dbo].[CancelFlight]    Script Date: 4/30/2023 1:26:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CancelFlight]
@flightId INT,
@userId INT,
@reason NVARCHAR(350)
AS
IF EXISTS (SELECT 1 FROM BookedFlights WHERE flightId = @flightId AND userId = @userId)
BEGIN
    UPDATE Flights SET num_of_seats = num_of_seats+1 WHERE flightId = @flightId
	DELETE FROM BookedFlights WHERE flightId = @flightId AND userId = @userId
	INSERT INTO TravelAgencyDB.CancelledFlights () VALUES (@flightId,@userId,GETDATE(),@reason)
END
GO
/****** Object:  StoredProcedure [dbo].[DowngradeUser]    Script Date: 4/30/2023 1:26:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DowngradeUser]
@userId INT,
@roleId INT = 2
AS
BEGIN
UPDATE Users SET roleId = @roleId WHERE userId = @userId
END
GO
/****** Object:  StoredProcedure [dbo].[GetFlights]    Script Date: 4/30/2023 1:26:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetFlights]
@userId INT
AS
BEGIN
SELECT Flights.flightId,[from],[to],num_of_seats,flight_time,cost,book_time,IIF(book_time > GETDATE(),'flown','upcoming') [status] FROM Flights INNER JOIN BookedFlights ON Flights.flightId = BookedFlights.flightId WHERE Flights.flightId IN(SELECT flightId FROM BookedFlights WHERE userId = @userId)
END
GO
/****** Object:  StoredProcedure [dbo].[GetUser]    Script Date: 4/30/2023 1:26:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUser]
@term VARCHAR(80)
AS
BEGIN
SELECT userId,username,roleId FROM Users WHERE username LIKE '%'+@term+'%' OR email LIKE '%'+@term+'%';
END
GO
/****** Object:  StoredProcedure [dbo].[LoginUser]    Script Date: 4/30/2023 1:26:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LoginUser]
@email_or_username varchar(50) = null,
@hashedPassword nvarchar(250) = null
AS
BEGIN
SELECT
CASE 
WHEN @hashedPassword = (SELECT hashedPass FROM Users WHERE userName = @email_or_username OR email = @email_or_username) THEN 1 
ELSE 0 END AS result
END
GO
/****** Object:  StoredProcedure [dbo].[RegisterUser]    Script Date: 4/30/2023 1:26:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RegisterUser]
@username varchar(50),
@hashedPassword nvarchar(250),
@email varchar(50)
AS
BEGIN
INSERT INTO Users(userName,hashedPass,roleId,email) VALUES (@username,@hashedPassword,1,@email)
END
GO
/****** Object:  StoredProcedure [dbo].[UpgradeUser]    Script Date: 4/30/2023 1:26:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpgradeUser]
@userId INT,
@roleId INT = 2
AS
BEGIN
UPDATE Users SET roleId = @roleId WHERE userId = @userId
END
GO
USE [master]
GO
ALTER DATABASE [TravelAgencyDB] SET  READ_WRITE 
GO
