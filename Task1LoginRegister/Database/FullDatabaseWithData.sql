USE [master]
GO
/****** Object:  Database [WebMobiTask1DB]    Script Date: 08-04-2025 11:33:04 ******/
CREATE DATABASE [WebMobiTask1DB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WebMobiTask1DB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\WebMobiTask1DB.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'WebMobiTask1DB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\WebMobiTask1DB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [WebMobiTask1DB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [WebMobiTask1DB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [WebMobiTask1DB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [WebMobiTask1DB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [WebMobiTask1DB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [WebMobiTask1DB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [WebMobiTask1DB] SET ARITHABORT OFF 
GO
ALTER DATABASE [WebMobiTask1DB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [WebMobiTask1DB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [WebMobiTask1DB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [WebMobiTask1DB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [WebMobiTask1DB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [WebMobiTask1DB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [WebMobiTask1DB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [WebMobiTask1DB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [WebMobiTask1DB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [WebMobiTask1DB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [WebMobiTask1DB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [WebMobiTask1DB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [WebMobiTask1DB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [WebMobiTask1DB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [WebMobiTask1DB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [WebMobiTask1DB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [WebMobiTask1DB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [WebMobiTask1DB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [WebMobiTask1DB] SET  MULTI_USER 
GO
ALTER DATABASE [WebMobiTask1DB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [WebMobiTask1DB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [WebMobiTask1DB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [WebMobiTask1DB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [WebMobiTask1DB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [WebMobiTask1DB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [WebMobiTask1DB] SET QUERY_STORE = ON
GO
ALTER DATABASE [WebMobiTask1DB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [WebMobiTask1DB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 08-04-2025 11:33:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Carts]    Script Date: 08-04-2025 11:33:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Carts](
	[CartId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Carts] PRIMARY KEY CLUSTERED 
(
	[CartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 08-04-2025 11:33:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeliveryAddresses]    Script Date: 08-04-2025 11:33:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeliveryAddresses](
	[AddressId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Street] [nvarchar](max) NOT NULL,
	[City] [nvarchar](100) NOT NULL,
	[State] [nvarchar](100) NOT NULL,
	[ZipCode] [nvarchar](6) NOT NULL,
	[Country] [nvarchar](max) NOT NULL,
	[PhoneNumber] [nvarchar](10) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_DeliveryAddresses] PRIMARY KEY CLUSTERED 
(
	[AddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GstTax]    Script Date: 08-04-2025 11:33:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GstTax](
	[GSTId] [int] IDENTITY(1,1) NOT NULL,
	[SubcategoryId] [int] NOT NULL,
	[CGST] [decimal](18, 2) NOT NULL,
	[SGST] [decimal](18, 2) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_GstTax] PRIMARY KEY CLUSTERED 
(
	[GSTId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItems]    Script Date: 08-04-2025 11:33:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItems](
	[OrderItemId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[SnapshotPrice] [decimal](18, 2) NOT NULL,
	[ProductName] [nvarchar](max) NOT NULL,
	[ProductSKU] [nvarchar](max) NOT NULL,
	[TotalPrice]  AS ([SnapshotPrice]*[Quantity]) PERSISTED,
	[BasePrice] [decimal](18, 2) NOT NULL,
	[DeliveryCharge] [decimal](18, 2) NOT NULL,
	[SnapshotCGSTPercentage] [decimal](18, 2) NOT NULL,
	[SnapshotCostPrice] [decimal](18, 2) NOT NULL,
	[SnapshotDiscountPercentage] [decimal](18, 2) NOT NULL,
	[SnapshotGSTAmount] [decimal](18, 2) NOT NULL,
	[SnapshotProfitPercentage] [decimal](18, 2) NOT NULL,
	[SnapshotSGSTPercentage] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_OrderItems] PRIMARY KEY CLUSTERED 
(
	[OrderItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 08-04-2025 11:33:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[OrderNumber] [nvarchar](450) NOT NULL,
	[UserId] [int] NOT NULL,
	[OrderDate] [datetime2](7) NOT NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[OrderStatus] [nvarchar](max) NOT NULL,
	[DeliveryAddressId] [int] NOT NULL,
	[PaymentId] [nvarchar](max) NULL,
	[PaymentMethod] [nvarchar](max) NULL,
	[PaymentStatus] [nvarchar](max) NULL,
	[RazorpayOrderId] [nvarchar](max) NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductImages]    Script Date: 08-04-2025 11:33:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductImages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ImageUrl] [varchar](max) NOT NULL,
	[ProductId] [int] NOT NULL,
	[IsPrimaryImage] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_ProductImages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 08-04-2025 11:33:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[ShortDescription] [nvarchar](200) NOT NULL,
	[SKU] [nvarchar](50) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[SellingPricePercent] [decimal](18, 2) NOT NULL,
	[CalculatedSellingPrice] [decimal](18, 2) NOT NULL,
	[Status] [bit] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[SubcategoryId] [int] NOT NULL,
	[DeliveryCharge] [decimal](18, 2) NOT NULL,
	[MinimumStockLevel] [int] NOT NULL,
	[StockQuantity] [int] NOT NULL,
	[CostPrice] [decimal](18, 2) NOT NULL,
	[ProfitPercentage] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RazorpayOrders]    Script Date: 08-04-2025 11:33:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RazorpayOrders](
	[OrderId] [nvarchar](450) NOT NULL,
	[Razorpaykey] [nvarchar](max) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Currency] [nvarchar](max) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[PhoneNumber] [nvarchar](max) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[ApplicationOrderId] [int] NOT NULL,
 CONSTRAINT [PK_RazorpayOrders] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RefundDetails]    Script Date: 08-04-2025 11:33:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefundDetails](
	[RefundId] [nvarchar](450) NOT NULL,
	[PaymentId] [nvarchar](max) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Status] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[OrderId] [int] NOT NULL,
	[Notes] [nvarchar](max) NOT NULL,
	[SpeedCode] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_RefundDetails] PRIMARY KEY CLUSTERED 
(
	[RefundId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reviews]    Script Date: 08-04-2025 11:33:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reviews](
	[ReviewId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Rating] [int] NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[IsApproved] [bit] NOT NULL,
	[ApprovedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Reviews] PRIMARY KEY CLUSTERED 
(
	[ReviewId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subcategories]    Script Date: 08-04-2025 11:33:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subcategories](
	[SubcategoryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_Subcategories] PRIMARY KEY CLUSTERED 
(
	[SubcategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[userlogin]    Script Date: 08-04-2025 11:33:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[userlogin](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Phone] [nchar](10) NOT NULL,
	[Photo] [varchar](max) NOT NULL,
	[Gender] [varchar](6) NOT NULL,
	[Password] [varchar](max) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Role] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_userlogin] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250113104340_AddIsActiveColumn', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250127072640_Products-Tables-Migration', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250127082710_AddUniqueNames', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250127103739_AddProductImagesTable', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250221082023_AddRoleColumnINUser', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250221091949_AddRoleColumnINUser', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250223130324_AddedCartTable', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250224045531_AddedeDeliveryChargeInProductTable', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250225052912_AddedGSTtable', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250225053807_UpdateGstTable', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250225110530_AddedOrdersTables', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250226060104_AddedOaymentMethodin_Order_Tabke', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250227065031_remove_unique_productId', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250313110259_AddRazorPayColumns', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250313110653_AddRazorpayOrderRelationship', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250317093739_AddRefundDetailsTable', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250324112036_AddTotalPriceComputedColumn', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250326061433_AddedStock-In-Products', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250328044252_AddedCostPrice-And-ProfitMarginINProduct', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250328064308_AddedSnapshot-OrderItems-Price-Changes', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250328091540_OrderItems-HistoryData', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250403101832_ReviewTable', N'8.0.11')
GO
SET IDENTITY_INSERT [dbo].[Carts] ON 

INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (42, 9, 1003, 1, CAST(6960.00 AS Decimal(18, 2)), CAST(N'2025-02-26T16:32:19.7401256' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (44, 4, 1003, 1, CAST(8100.00 AS Decimal(18, 2)), CAST(N'2025-02-26T16:42:31.3610584' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (45, 12, 1003, 2, CAST(4499.10 AS Decimal(18, 2)), CAST(N'2025-02-27T11:26:51.0001848' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (56, 8, 3, 1, CAST(4500.00 AS Decimal(18, 2)), CAST(N'2025-02-27T12:24:47.9520373' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (57, 8, 3, 3, CAST(4500.00 AS Decimal(18, 2)), CAST(N'2025-02-27T12:25:03.6046187' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (65, 12, 1005, 3, CAST(4499.10 AS Decimal(18, 2)), CAST(N'2025-03-04T10:10:29.2391157' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (67, 7, 1005, 1, CAST(3600.00 AS Decimal(18, 2)), CAST(N'2025-03-04T10:10:58.9662007' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (68, 8, 1005, 1, CAST(4500.00 AS Decimal(18, 2)), CAST(N'2025-03-04T10:11:06.0666147' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (69, 9, 1005, 1, CAST(6960.00 AS Decimal(18, 2)), CAST(N'2025-03-04T10:11:20.9452712' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (74, 5, 1005, 1, CAST(5400.00 AS Decimal(18, 2)), CAST(N'2025-03-04T10:12:36.4596576' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (75, 3, 1005, 1, CAST(110371.18 AS Decimal(18, 2)), CAST(N'2025-03-04T10:15:02.9686695' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (76, 10, 1005, 1, CAST(2700.00 AS Decimal(18, 2)), CAST(N'2025-03-04T10:15:10.4332613' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (77, 1, 1005, 1, CAST(115853.01 AS Decimal(18, 2)), CAST(N'2025-03-04T10:15:24.0928856' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (78, 11, 1005, 1, CAST(8265.00 AS Decimal(18, 2)), CAST(N'2025-03-04T10:15:45.0080012' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (79, 4, 1005, 1, CAST(8100.00 AS Decimal(18, 2)), CAST(N'2025-03-04T10:15:54.0828633' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (83, 4, 7, 1, CAST(8100.00 AS Decimal(18, 2)), CAST(N'2025-03-04T10:59:51.7819179' AS DateTime2), 1)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (86, 1, 1003, 1, CAST(115853.01 AS Decimal(18, 2)), CAST(N'2025-03-06T12:23:28.7179531' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (87, 4, 1003, 2, CAST(8100.00 AS Decimal(18, 2)), CAST(N'2025-03-06T12:23:43.1963538' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (88, 11, 3, 2, CAST(8265.00 AS Decimal(18, 2)), CAST(N'2025-03-06T12:51:20.5487042' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (89, 8, 8, 1, CAST(4500.00 AS Decimal(18, 2)), CAST(N'2025-03-13T16:40:28.8125579' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (90, 11, 9, 1, CAST(8265.00 AS Decimal(18, 2)), CAST(N'2025-03-13T17:09:38.9877862' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (91, 1, 9, 1, CAST(115853.01 AS Decimal(18, 2)), CAST(N'2025-03-13T17:09:46.6730449' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (92, 4, 9, 1, CAST(8100.00 AS Decimal(18, 2)), CAST(N'2025-03-13T17:18:22.6066659' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (93, 12, 1003, 1, CAST(4499.10 AS Decimal(18, 2)), CAST(N'2025-03-17T10:14:40.1981158' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (95, 11, 1003, 1, CAST(8265.00 AS Decimal(18, 2)), CAST(N'2025-03-17T10:15:11.5218591' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (96, 4, 8, 1, CAST(8100.00 AS Decimal(18, 2)), CAST(N'2025-03-17T10:23:31.2050662' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (97, 12, 8, 1, CAST(4499.10 AS Decimal(18, 2)), CAST(N'2025-03-17T10:25:33.5808829' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (98, 7, 9, 1, CAST(3600.00 AS Decimal(18, 2)), CAST(N'2025-03-17T10:34:47.8520447' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (100, 10, 8, 1, CAST(2700.00 AS Decimal(18, 2)), CAST(N'2025-03-18T16:29:38.7933839' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (101, 13, 3, 1, CAST(216000.00 AS Decimal(18, 2)), CAST(N'2025-03-18T16:56:41.4538735' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (1102, 14, 8, 1, CAST(1173602.97 AS Decimal(18, 2)), CAST(N'2025-03-20T11:44:30.2809151' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (1103, 13, 8, 1, CAST(216000.00 AS Decimal(18, 2)), CAST(N'2025-03-20T11:44:53.7558934' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (1104, 19, 8, 1, CAST(107466.29 AS Decimal(18, 2)), CAST(N'2025-03-20T11:45:14.6243049' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (1108, 13, 8, 1, CAST(27000.00 AS Decimal(18, 2)), CAST(N'2025-03-20T11:55:38.5623877' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (1109, 14, 8, 1, CAST(40102.97 AS Decimal(18, 2)), CAST(N'2025-03-20T11:56:09.8115438' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (1110, 14, 8, 1, CAST(40102.97 AS Decimal(18, 2)), CAST(N'2025-03-20T11:59:18.7866560' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (1111, 19, 1003, 1, CAST(107466.29 AS Decimal(18, 2)), CAST(N'2025-03-21T10:39:57.9120965' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (1112, 20, 9, 1, CAST(41491.84 AS Decimal(18, 2)), CAST(N'2025-03-21T14:32:20.7342025' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (1113, 19, 8, 1, CAST(51466.29 AS Decimal(18, 2)), CAST(N'2025-03-25T12:58:24.4141152' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (1114, 19, 9, 1, CAST(51466.29 AS Decimal(18, 2)), CAST(N'2025-03-25T12:59:35.7959596' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (1115, 14, 10, 1, CAST(40102.97 AS Decimal(18, 2)), CAST(N'2025-03-27T14:19:21.8329437' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (1118, 18, 10, 1, CAST(28517.75 AS Decimal(18, 2)), CAST(N'2025-03-27T14:23:26.7792574' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (1119, 19, 11, 1, CAST(49955.05 AS Decimal(18, 2)), CAST(N'2025-04-03T11:21:31.5421473' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (1121, 13, 11, 1, CAST(27000.00 AS Decimal(18, 2)), CAST(N'2025-04-03T11:22:19.7133313' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (1124, 14, 11, 1, CAST(80205.94 AS Decimal(18, 2)), CAST(N'2025-04-03T11:26:00.7125441' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (1130, 17, 12, 1, CAST(78512.60 AS Decimal(18, 2)), CAST(N'2025-04-07T13:14:05.1389334' AS DateTime2), 0)
INSERT [dbo].[Carts] ([CartId], [ProductId], [UserId], [Quantity], [Price], [CreatedAt], [IsActive]) VALUES (1131, 19, 1003, 1, CAST(49955.05 AS Decimal(18, 2)), CAST(N'2025-04-08T10:38:09.1853092' AS DateTime2), 0)
SET IDENTITY_INSERT [dbo].[Carts] OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([CategoryId], [Name]) VALUES (10, N'Books')
INSERT [dbo].[Categories] ([CategoryId], [Name]) VALUES (11, N'Cabinet and Cupboard')
INSERT [dbo].[Categories] ([CategoryId], [Name]) VALUES (9, N'Drama')
INSERT [dbo].[Categories] ([CategoryId], [Name]) VALUES (2, N'Electronics')
INSERT [dbo].[Categories] ([CategoryId], [Name]) VALUES (5, N'Fashion')
INSERT [dbo].[Categories] ([CategoryId], [Name]) VALUES (6, N'Games')
INSERT [dbo].[Categories] ([CategoryId], [Name]) VALUES (1, N'Living Room Furniture')
INSERT [dbo].[Categories] ([CategoryId], [Name]) VALUES (3, N'Sports')
INSERT [dbo].[Categories] ([CategoryId], [Name]) VALUES (12, N'Table')
INSERT [dbo].[Categories] ([CategoryId], [Name]) VALUES (4, N'Toys')
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[DeliveryAddresses] ON 

INSERT [dbo].[DeliveryAddresses] ([AddressId], [UserId], [FullName], [Street], [City], [State], [ZipCode], [Country], [PhoneNumber], [Email], [IsDefault], [CreatedAt]) VALUES (1, 1003, N'Shayan Vahora', N'Ismailnagar, Bhalej Road', N'Anand', N'Gujarat', N'388001', N'India', N'9987451225', N'shayan2@gmail.com', 1, CAST(N'2025-02-26T12:12:39.0000000' AS DateTime2))
INSERT [dbo].[DeliveryAddresses] ([AddressId], [UserId], [FullName], [Street], [City], [State], [ZipCode], [Country], [PhoneNumber], [Email], [IsDefault], [CreatedAt]) VALUES (3, 3, N'Demo User', N'Demo street', N'Anand', N'Gujarat', N'388001', N'India', N'7891234567', N'demo2@gmail.com', 1, CAST(N'2025-02-26T13:30:46.0295933' AS DateTime2))
INSERT [dbo].[DeliveryAddresses] ([AddressId], [UserId], [FullName], [Street], [City], [State], [ZipCode], [Country], [PhoneNumber], [Email], [IsDefault], [CreatedAt]) VALUES (5, 3, N'Demo User Second', N'Demo Street, S. G. Highway', N'Ahmedabad', N'Gujarat', N'388110', N'India', N'7896547894', N'demo2@gmail.com', 0, CAST(N'2025-02-27T12:26:17.6651415' AS DateTime2))
INSERT [dbo].[DeliveryAddresses] ([AddressId], [UserId], [FullName], [Street], [City], [State], [ZipCode], [Country], [PhoneNumber], [Email], [IsDefault], [CreatedAt]) VALUES (7, 1003, N'Siraj Vahora', N'Royal Plaza, 100 feet road', N'Anand', N'Gujarat', N'388001', N'India', N'7814253698', N'shayan2@gmail.com', 0, CAST(N'2025-03-03T14:35:22.0000000' AS DateTime2))
INSERT [dbo].[DeliveryAddresses] ([AddressId], [UserId], [FullName], [Street], [City], [State], [ZipCode], [Country], [PhoneNumber], [Email], [IsDefault], [CreatedAt]) VALUES (10, 1005, N'Arsil Malek', N'Shalimar Society, Bhalej Road', N'Anand', N'Gujarat', N'388001', N'India', N'9978124536', N'arshil2@gmail.com', 1, CAST(N'2025-03-04T10:18:01.0768486' AS DateTime2))
INSERT [dbo].[DeliveryAddresses] ([AddressId], [UserId], [FullName], [Street], [City], [State], [ZipCode], [Country], [PhoneNumber], [Email], [IsDefault], [CreatedAt]) VALUES (12, 8, N'Jamila Lokhadwala', N'Royal City', N'Umreth', N'Gujarat', N'381201', N'India', N'8200746058', N'jamila15@gmail.com', 1, CAST(N'2025-03-13T16:42:05.8187752' AS DateTime2))
INSERT [dbo].[DeliveryAddresses] ([AddressId], [UserId], [FullName], [Street], [City], [State], [ZipCode], [Country], [PhoneNumber], [Email], [IsDefault], [CreatedAt]) VALUES (13, 9, N'Shaniya Vahora', N'Royal Garden, APC Circle', N'Anand', N'Gujarat', N'388001', N'India', N'8791234569', N'shaniya37@gmail.com', 1, CAST(N'2025-03-13T17:11:21.0000000' AS DateTime2))
INSERT [dbo].[DeliveryAddresses] ([AddressId], [UserId], [FullName], [Street], [City], [State], [ZipCode], [Country], [PhoneNumber], [Email], [IsDefault], [CreatedAt]) VALUES (15, 10, N'Ashfiya Vahora', N'Ismailnagar, Bhalej Road', N'Anand', N'Gujarat', N'388001', N'India', N'7897845214', N'ashfiya09@gmail.com', 1, CAST(N'2025-03-27T14:24:38.0900701' AS DateTime2))
INSERT [dbo].[DeliveryAddresses] ([AddressId], [UserId], [FullName], [Street], [City], [State], [ZipCode], [Country], [PhoneNumber], [Email], [IsDefault], [CreatedAt]) VALUES (16, 11, N'Hiba Nawab', N'Marin Drive', N'Mumbai', N'Maharashtra', N'400020', N'India', N'7574887412', N'hiba2@gmail.com', 1, CAST(N'2025-04-03T11:24:05.7460778' AS DateTime2))
INSERT [dbo].[DeliveryAddresses] ([AddressId], [UserId], [FullName], [Street], [City], [State], [ZipCode], [Country], [PhoneNumber], [Email], [IsDefault], [CreatedAt]) VALUES (17, 12, N'Robert Downey', N'Royal Park', N'Surat', N'Surat', N'388785', N'India', N'7890789014', N'robertd2@gmail.com', 0, CAST(N'2025-04-07T13:15:02.1541123' AS DateTime2))
SET IDENTITY_INSERT [dbo].[DeliveryAddresses] OFF
GO
SET IDENTITY_INSERT [dbo].[GstTax] ON 

INSERT [dbo].[GstTax] ([GSTId], [SubcategoryId], [CGST], [SGST], [CreatedAt], [UpdatedAt]) VALUES (1, 3, CAST(6.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)), CAST(N'2025-02-25T11:18:01.4671210' AS DateTime2), CAST(N'2025-02-25T11:18:01.4672483' AS DateTime2))
INSERT [dbo].[GstTax] ([GSTId], [SubcategoryId], [CGST], [SGST], [CreatedAt], [UpdatedAt]) VALUES (2, 13, CAST(3.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(N'2025-02-25T11:39:12.3637318' AS DateTime2), NULL)
INSERT [dbo].[GstTax] ([GSTId], [SubcategoryId], [CGST], [SGST], [CreatedAt], [UpdatedAt]) VALUES (3, 14, CAST(3.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(N'2025-02-25T12:14:29.3246904' AS DateTime2), NULL)
INSERT [dbo].[GstTax] ([GSTId], [SubcategoryId], [CGST], [SGST], [CreatedAt], [UpdatedAt]) VALUES (4, 26, CAST(9.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(N'2025-03-20T11:33:30.9733742' AS DateTime2), NULL)
INSERT [dbo].[GstTax] ([GSTId], [SubcategoryId], [CGST], [SGST], [CreatedAt], [UpdatedAt]) VALUES (5, 27, CAST(9.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(N'2025-03-20T11:33:38.3402045' AS DateTime2), NULL)
INSERT [dbo].[GstTax] ([GSTId], [SubcategoryId], [CGST], [SGST], [CreatedAt], [UpdatedAt]) VALUES (6, 1, CAST(9.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(N'2025-03-20T11:34:18.5674482' AS DateTime2), NULL)
INSERT [dbo].[GstTax] ([GSTId], [SubcategoryId], [CGST], [SGST], [CreatedAt], [UpdatedAt]) VALUES (7, 2, CAST(9.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(N'2025-03-20T11:34:26.2567889' AS DateTime2), NULL)
INSERT [dbo].[GstTax] ([GSTId], [SubcategoryId], [CGST], [SGST], [CreatedAt], [UpdatedAt]) VALUES (8, 30, CAST(9.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(N'2025-03-20T11:34:33.5565596' AS DateTime2), NULL)
INSERT [dbo].[GstTax] ([GSTId], [SubcategoryId], [CGST], [SGST], [CreatedAt], [UpdatedAt]) VALUES (9, 28, CAST(9.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(N'2025-03-20T11:34:40.7251988' AS DateTime2), NULL)
INSERT [dbo].[GstTax] ([GSTId], [SubcategoryId], [CGST], [SGST], [CreatedAt], [UpdatedAt]) VALUES (10, 29, CAST(9.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(N'2025-03-20T11:34:49.6618660' AS DateTime2), NULL)
SET IDENTITY_INSERT [dbo].[GstTax] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderItems] ON 

INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (1, 1, 1, 3, CAST(N'2025-02-26T12:12:39.7386435' AS DateTime2), CAST(115853.01 AS Decimal(18, 2)), N'Samsung S25 Ultra', N'C2S3SA2F01', CAST(130171.92 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)), CAST(86781.28 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), CAST(41707.08 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (2, 1, 11, 1, CAST(N'2025-02-26T12:12:39.7521670' AS DateTime2), CAST(8265.00 AS Decimal(18, 2)), N'Call of Duty: Black Ops 6', N'C6S13CACB62', CAST(9500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(6333.33 AS Decimal(18, 2)), CAST(13.00 AS Decimal(18, 2)), CAST(495.90 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (3, 1, 6, 1, CAST(N'2025-02-26T12:12:39.7532162' AS DateTime2), CAST(4950.00 AS Decimal(18, 2)), N'Grand Theft Auto V', N'C6S13GRF2DC', CAST(5500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(3666.67 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(297.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (4, 1, 10, 1, CAST(N'2025-02-26T12:12:39.7532628' AS DateTime2), CAST(2700.00 AS Decimal(18, 2)), N'PlayerUnknown''s BattleGrounds', N'C6S13PL59C9', CAST(3000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(2000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(162.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (5, 2, 3, 2, CAST(N'2025-02-26T12:47:14.4842826' AS DateTime2), CAST(110371.18 AS Decimal(18, 2)), N'Samsung Galaxy Z Fold6', N'C2S3SA6BE9', CAST(134599.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)), CAST(89732.67 AS Decimal(18, 2)), CAST(18.00 AS Decimal(18, 2)), CAST(26489.08 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (7, 4, 5, 1, CAST(N'2025-02-26T13:24:36.7894268' AS DateTime2), CAST(5400.00 AS Decimal(18, 2)), N'sfdsd', N'C4S9SFB469', CAST(6000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (8, 5, 1, 9, CAST(N'2025-02-26T13:30:46.1117673' AS DateTime2), CAST(115853.01 AS Decimal(18, 2)), N'Samsung S25 Ultra', N'C2S3SA2F01', CAST(130171.92 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)), CAST(86781.28 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), CAST(125121.25 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (9, 5, 11, 1, CAST(N'2025-02-26T13:30:46.1119680' AS DateTime2), CAST(8265.00 AS Decimal(18, 2)), N'Call of Duty: Black Ops 6', N'C6S13CACB62', CAST(9500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(6333.33 AS Decimal(18, 2)), CAST(13.00 AS Decimal(18, 2)), CAST(495.90 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (10, 6, 9, 1, CAST(N'2025-02-27T11:36:09.0946859' AS DateTime2), CAST(6960.00 AS Decimal(18, 2)), N'Uncharted 4: A Thief’s End', N'C6S14UNFE1E', CAST(8000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(5333.33 AS Decimal(18, 2)), CAST(13.00 AS Decimal(18, 2)), CAST(417.60 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (11, 6, 4, 1, CAST(N'2025-02-27T11:36:09.1127460' AS DateTime2), CAST(8100.00 AS Decimal(18, 2)), N'Wooden Door', N'C1S1WO12E7', CAST(9000.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(6000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(1458.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (12, 6, 12, 2, CAST(N'2025-02-27T11:36:09.1137162' AS DateTime2), CAST(4499.10 AS Decimal(18, 2)), N'ThunderBolt X1 RC Car', N'C4S8TH7202', CAST(4999.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3332.67 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (13, 7, 3, 2, CAST(N'2025-02-27T11:41:29.9936344' AS DateTime2), CAST(110371.18 AS Decimal(18, 2)), N'Samsung Galaxy Z Fold6', N'C2S3SA6BE9', CAST(134599.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)), CAST(89732.67 AS Decimal(18, 2)), CAST(18.00 AS Decimal(18, 2)), CAST(26489.08 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (14, 7, 1, 1, CAST(N'2025-02-27T11:41:29.9938524' AS DateTime2), CAST(115853.01 AS Decimal(18, 2)), N'Samsung S25 Ultra', N'C2S3SA2F01', CAST(130171.92 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)), CAST(86781.28 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), CAST(13902.36 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (15, 7, 11, 1, CAST(N'2025-02-27T11:41:29.9939032' AS DateTime2), CAST(8265.00 AS Decimal(18, 2)), N'Call of Duty: Black Ops 6', N'C6S13CACB62', CAST(9500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(6333.33 AS Decimal(18, 2)), CAST(13.00 AS Decimal(18, 2)), CAST(495.90 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (16, 7, 12, 2, CAST(N'2025-02-27T11:41:29.9939509' AS DateTime2), CAST(4499.10 AS Decimal(18, 2)), N'ThunderBolt X1 RC Car', N'C4S8TH7202', CAST(4999.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3332.67 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (17, 7, 4, 1, CAST(N'2025-02-27T11:41:29.9940197' AS DateTime2), CAST(8100.00 AS Decimal(18, 2)), N'Wooden Door', N'C1S1WO12E7', CAST(9000.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(6000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(1458.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (18, 8, 8, 1, CAST(N'2025-02-27T12:24:53.8963576' AS DateTime2), CAST(4500.00 AS Decimal(18, 2)), N'The Witcher 3: Wild Hunt', N'C6S14TH4098', CAST(6000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)), CAST(25.00 AS Decimal(18, 2)), CAST(270.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (19, 9, 8, 3, CAST(N'2025-02-27T12:26:17.6934892' AS DateTime2), CAST(4500.00 AS Decimal(18, 2)), N'The Witcher 3: Wild Hunt', N'C6S14TH4098', CAST(6000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)), CAST(25.00 AS Decimal(18, 2)), CAST(810.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (20, 10, 12, 3, CAST(N'2025-03-04T10:18:01.1602400' AS DateTime2), CAST(4499.10 AS Decimal(18, 2)), N'ThunderBolt X1 RC Car', N'C4S8TH7202', CAST(4999.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3332.67 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (21, 10, 7, 1, CAST(N'2025-03-04T10:18:01.1635237' AS DateTime2), CAST(3600.00 AS Decimal(18, 2)), N'Red Dead Redemption 2', N'C6S13RE59DD', CAST(4500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(3000.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), CAST(216.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (22, 10, 8, 1, CAST(N'2025-03-04T10:18:01.1635602' AS DateTime2), CAST(4500.00 AS Decimal(18, 2)), N'The Witcher 3: Wild Hunt', N'C6S14TH4098', CAST(6000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)), CAST(25.00 AS Decimal(18, 2)), CAST(270.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (23, 10, 9, 1, CAST(N'2025-03-04T10:18:01.1636129' AS DateTime2), CAST(6960.00 AS Decimal(18, 2)), N'Uncharted 4: A Thief’s End', N'C6S14UNFE1E', CAST(8000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(5333.33 AS Decimal(18, 2)), CAST(13.00 AS Decimal(18, 2)), CAST(417.60 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (24, 10, 5, 1, CAST(N'2025-03-04T10:18:01.1636467' AS DateTime2), CAST(5400.00 AS Decimal(18, 2)), N'sfdsd', N'C4S9SFB469', CAST(6000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (25, 10, 3, 1, CAST(N'2025-03-04T10:18:01.1636598' AS DateTime2), CAST(110371.18 AS Decimal(18, 2)), N'Samsung Galaxy Z Fold6', N'C2S3SA6BE9', CAST(134599.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)), CAST(89732.67 AS Decimal(18, 2)), CAST(18.00 AS Decimal(18, 2)), CAST(13244.54 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (26, 10, 10, 1, CAST(N'2025-03-04T10:18:01.1636700' AS DateTime2), CAST(2700.00 AS Decimal(18, 2)), N'PlayerUnknown''s BattleGrounds', N'C6S13PL59C9', CAST(3000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(2000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(162.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (27, 10, 1, 1, CAST(N'2025-03-04T10:18:01.1636810' AS DateTime2), CAST(115853.01 AS Decimal(18, 2)), N'Samsung S25 Ultra', N'C2S3SA2F01', CAST(130171.92 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)), CAST(86781.28 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), CAST(13902.36 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (28, 10, 11, 1, CAST(N'2025-03-04T10:18:01.1636913' AS DateTime2), CAST(8265.00 AS Decimal(18, 2)), N'Call of Duty: Black Ops 6', N'C6S13CACB62', CAST(9500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(6333.33 AS Decimal(18, 2)), CAST(13.00 AS Decimal(18, 2)), CAST(495.90 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (29, 10, 4, 1, CAST(N'2025-03-04T10:18:01.1637012' AS DateTime2), CAST(8100.00 AS Decimal(18, 2)), N'Wooden Door', N'C1S1WO12E7', CAST(9000.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(6000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(1458.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (30, 11, 1, 1, CAST(N'2025-03-06T12:33:01.3737953' AS DateTime2), CAST(115853.01 AS Decimal(18, 2)), N'Samsung S25 Ultra', N'C2S3SA2F01', CAST(130171.92 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)), CAST(86781.28 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), CAST(13902.36 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(6.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (31, 11, 4, 2, CAST(N'2025-03-06T12:33:01.3903210' AS DateTime2), CAST(8100.00 AS Decimal(18, 2)), N'Wooden Door', N'C1S1WO12E7', CAST(9000.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(6000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(2916.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (32, 12, 11, 2, CAST(N'2025-03-06T12:52:24.7147822' AS DateTime2), CAST(8265.00 AS Decimal(18, 2)), N'Call of Duty: Black Ops 6', N'C6S13CACB62', CAST(9500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(6333.33 AS Decimal(18, 2)), CAST(13.00 AS Decimal(18, 2)), CAST(991.80 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (33, 13, 8, 1, CAST(N'2025-03-13T16:42:05.9394943' AS DateTime2), CAST(4500.00 AS Decimal(18, 2)), N'The Witcher 3: Wild Hunt', N'C6S14TH4098', CAST(6000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)), CAST(25.00 AS Decimal(18, 2)), CAST(270.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (36, 15, 4, 1, CAST(N'2025-03-13T17:18:28.9406441' AS DateTime2), CAST(8100.00 AS Decimal(18, 2)), N'Wooden Door', N'C1S1WO12E7', CAST(9000.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(6000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(1458.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (37, 16, 12, 1, CAST(N'2025-03-17T10:15:34.8464043' AS DateTime2), CAST(4499.10 AS Decimal(18, 2)), N'ThunderBolt X1 RC Car', N'C4S8TH7202', CAST(4999.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3332.67 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (38, 16, 11, 1, CAST(N'2025-03-17T10:15:34.8658711' AS DateTime2), CAST(8265.00 AS Decimal(18, 2)), N'Call of Duty: Black Ops 6', N'C6S13CACB62', CAST(9500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(6333.33 AS Decimal(18, 2)), CAST(13.00 AS Decimal(18, 2)), CAST(495.90 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (39, 17, 4, 1, CAST(N'2025-03-17T10:23:39.3787680' AS DateTime2), CAST(8100.00 AS Decimal(18, 2)), N'Wooden Door', N'C1S1WO12E7', CAST(9000.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(6000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(1458.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (40, 18, 12, 1, CAST(N'2025-03-17T10:25:37.6714422' AS DateTime2), CAST(4499.10 AS Decimal(18, 2)), N'ThunderBolt X1 RC Car', N'C4S8TH7202', CAST(4999.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3332.67 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (41, 19, 7, 1, CAST(N'2025-03-17T10:34:52.5330958' AS DateTime2), CAST(3600.00 AS Decimal(18, 2)), N'Red Dead Redemption 2', N'C6S13RE59DD', CAST(4500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(3000.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), CAST(216.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (42, 20, 10, 1, CAST(N'2025-03-18T16:29:46.5927272' AS DateTime2), CAST(2700.00 AS Decimal(18, 2)), N'PlayerUnknown''s BattleGrounds', N'C6S13PL59C9', CAST(3000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(2000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(162.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (43, 21, 13, 1, CAST(N'2025-03-18T16:56:47.1427774' AS DateTime2), CAST(27000.00 AS Decimal(18, 2)), N'Luxurious Round Entrance Dining Table Black Marqui', N'C12S29LUE2D5', CAST(30000.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(20000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(4860.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (48, 23, 19, 1, CAST(N'2025-04-03T11:24:05.9817875' AS DateTime2), CAST(49955.05 AS Decimal(18, 2)), N'Luxurious Black Marquina Console 160x35x80', N'C12S28LU8EEE', CAST(71364.36 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(35682.18 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), CAST(8991.91 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (49, 23, 13, 1, CAST(N'2025-04-03T11:24:06.1005215' AS DateTime2), CAST(27000.00 AS Decimal(18, 2)), N'Luxurious Round Entrance Dining Table Black Marqui', N'C12S29LUE2D5', CAST(30000.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(20000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(4860.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (50, 24, 14, 1, CAST(N'2025-04-03T11:26:17.2731932' AS DateTime2), CAST(80205.94 AS Decimal(18, 2)), N'Customized Built-in Storage', N'C11S27CU80B4', CAST(160411.89 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(53470.63 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(14437.07 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (51, 25, 17, 1, CAST(N'2025-04-07T13:15:02.3306228' AS DateTime2), CAST(78512.60 AS Decimal(18, 2)), N'Trevi 230M 3-Seater Sofa Bed', N'C1S1TRC2BF', CAST(157025.21 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(65427.17 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(14132.27 AS Decimal(18, 2)), CAST(140.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (52, 26, 19, 1, CAST(N'2025-04-08T10:38:33.1521531' AS DateTime2), CAST(49955.05 AS Decimal(18, 2)), N'Luxurious Black Marquina Console 160x35x80', N'C12S28LU8EEE', CAST(71364.36 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(35682.18 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), CAST(8991.91 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (1043, 1021, 14, 1, CAST(N'2025-03-20T11:49:15.7889320' AS DateTime2), CAST(35205.94 AS Decimal(18, 2)), N'Customized Built-in Storage', N'C11S27CU80B4', CAST(70411.89 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(23470.63 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(6337.07 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (1044, 1021, 13, 1, CAST(N'2025-03-20T11:49:15.7946655' AS DateTime2), CAST(27000.00 AS Decimal(18, 2)), N'Luxurious Round Entrance Dining Table Black Marqui', N'C12S29LUE2D5', CAST(30000.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(20000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(4860.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (1045, 1021, 19, 1, CAST(N'2025-03-20T11:49:15.7947026' AS DateTime2), CAST(49955.05 AS Decimal(18, 2)), N'Luxurious Black Marquina Console 160x35x80', N'C12S28LU8EEE', CAST(71364.36 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(35682.18 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), CAST(8991.91 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (1046, 1022, 13, 1, CAST(N'2025-03-20T11:56:35.7490805' AS DateTime2), CAST(27000.00 AS Decimal(18, 2)), N'Luxurious Round Entrance Dining Table Black Marqui', N'C12S29LUE2D5', CAST(30000.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(20000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(4860.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (1047, 1022, 14, 1, CAST(N'2025-03-20T11:56:35.7491997' AS DateTime2), CAST(35205.94 AS Decimal(18, 2)), N'Customized Built-in Storage', N'C11S27CU80B4', CAST(70411.89 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(23470.63 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(6337.07 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (1048, 1023, 14, 1, CAST(N'2025-03-20T11:59:23.8077253' AS DateTime2), CAST(35205.94 AS Decimal(18, 2)), N'Customized Built-in Storage', N'C11S27CU80B4', CAST(70411.89 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(23470.63 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(6337.07 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (1049, 1024, 19, 1, CAST(N'2025-03-21T10:40:06.6637801' AS DateTime2), CAST(49955.05 AS Decimal(18, 2)), N'Luxurious Black Marquina Console 160x35x80', N'C12S28LU8EEE', CAST(71364.36 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(35682.18 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), CAST(8991.91 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (1050, 1025, 20, 1, CAST(N'2025-03-21T14:32:27.0555462' AS DateTime2), CAST(41491.84 AS Decimal(18, 2)), N'Mocha Bookcase With Drawers', N'C11S26MODEF0', CAST(142035.74 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(33734.26 AS Decimal(18, 2)), CAST(60.00 AS Decimal(18, 2)), CAST(7468.52 AS Decimal(18, 2)), CAST(250.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (1051, 1026, 19, 1, CAST(N'2025-03-25T12:58:30.6126210' AS DateTime2), CAST(49955.05 AS Decimal(18, 2)), N'Luxurious Black Marquina Console 160x35x80', N'C12S28LU8EEE', CAST(71364.36 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(35682.18 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), CAST(8991.91 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (1052, 1027, 19, 1, CAST(N'2025-03-25T12:59:39.9470355' AS DateTime2), CAST(49955.05 AS Decimal(18, 2)), N'Luxurious Black Marquina Console 160x35x80', N'C12S28LU8EEE', CAST(71364.36 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(35682.18 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), CAST(8991.91 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (1053, 1028, 14, 1, CAST(N'2025-03-27T14:24:38.2317205' AS DateTime2), CAST(35205.94 AS Decimal(18, 2)), N'Customized Built-in Storage', N'C11S27CU80B4', CAST(70411.89 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(23470.63 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(6337.07 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [Quantity], [CreatedAt], [SnapshotPrice], [ProductName], [ProductSKU], [BasePrice], [DeliveryCharge], [SnapshotCGSTPercentage], [SnapshotCostPrice], [SnapshotDiscountPercentage], [SnapshotGSTAmount], [SnapshotProfitPercentage], [SnapshotSGSTPercentage]) VALUES (1054, 1028, 18, 1, CAST(N'2025-03-27T14:24:38.2593522' AS DateTime2), CAST(45628.40 AS Decimal(18, 2)), N'Oval Dining Table Light Travertine 180x90x75', N'C12S29OV98C0', CAST(57035.50 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)), CAST(23764.79 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), CAST(8213.11 AS Decimal(18, 2)), CAST(140.00 AS Decimal(18, 2)), CAST(9.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[OrderItems] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (1, N'ORD-20250226121239-1EAE7', 1003, CAST(N'2025-02-26T12:12:39.6866347' AS DateTime2), CAST(406176.01 AS Decimal(18, 2)), N'Delivered', 1, NULL, N'Cash on Delivery', N'Paid', NULL)
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (2, N'ORD-20250226124714-0D1A7', 1003, CAST(N'2025-02-26T12:47:14.4359863' AS DateTime2), CAST(247231.44 AS Decimal(18, 2)), N'Delivered', 1, NULL, N'Cash on Delivery', N'Paid', NULL)
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (4, N'ORD-20250226132436-1281A', 1003, CAST(N'2025-02-26T13:24:36.5216397' AS DateTime2), CAST(5400.00 AS Decimal(18, 2)), N'Delivered', 1, NULL, N'Cash on Delivery', N'Paid', NULL)
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (5, N'ORD-20250226133046-A5449', 3, CAST(N'2025-02-26T13:30:46.1015455' AS DateTime2), CAST(1176599.24 AS Decimal(18, 2)), N'Delivered', 3, NULL, N'Cash on Delivery', N'Paid', NULL)
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (6, N'ORD20250227113608A0AA8', 1003, CAST(N'2025-02-27T11:36:08.8019347' AS DateTime2), CAST(24545.80 AS Decimal(18, 2)), N'Delivered', 1, N'pay_Q7kP9fyyM4rlpI', N'Online Payment', N'Paid', N'order_Q7kOYuwm4MtTqq')
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (7, N'ORD2025022711412918A25', 3, CAST(N'2025-02-27T11:41:29.9829169' AS DateTime2), CAST(402955.91 AS Decimal(18, 2)), N'Cancelled', 3, NULL, N'Cash on Delivery', N'Failed', N'order_Q7q82IMyFyhygE')
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (8, N'ORD20250227122453F608A', 3, CAST(N'2025-02-27T12:24:53.8637305' AS DateTime2), CAST(4770.00 AS Decimal(18, 2)), N'Delivered', 3, N'pay_Q7q6rpYmkVKsch', N'Online Payment', N'Paid', N'order_Q7q6ffc88gZhIm')
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (9, N'ORD2025022712261700C01', 3, CAST(N'2025-02-27T12:26:17.6889993' AS DateTime2), CAST(14310.00 AS Decimal(18, 2)), N'Delivered', 5, NULL, N'Cash on Delivery', N'Paid', NULL)
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (10, N'ORD20250304101801D39E9', 1005, CAST(N'2025-03-04T10:18:01.1394610' AS DateTime2), CAST(308064.89 AS Decimal(18, 2)), N'Delivered', 10, NULL, N'Cash on Delivery', N'Paid', NULL)
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (11, N'ORD20250306123301D160C', 1003, CAST(N'2025-03-06T12:33:01.3071432' AS DateTime2), CAST(132053.01 AS Decimal(18, 2)), N'Delivered', 7, NULL, N'Cash on Delivery', N'Paid', NULL)
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (12, N'ORD2025030612522463DE2', 3, CAST(N'2025-03-06T12:52:24.7040721' AS DateTime2), CAST(16530.00 AS Decimal(18, 2)), N'Delivered', 5, NULL, N'Cash on Delivery', N'Paid', NULL)
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (13, N'ORD20250313164205C92C2', 8, CAST(N'2025-03-13T16:42:05.8708654' AS DateTime2), CAST(4770.00 AS Decimal(18, 2)), N'Delivered', 12, N'pay_Q6G05RbziRz6Hp', N'Online Payment', N'Paid', N'order_Q6Fzj48OGTu6H8')
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (15, N'ORD20250313171828774CF', 9, CAST(N'2025-03-13T17:18:28.9253241' AS DateTime2), CAST(8140.00 AS Decimal(18, 2)), N'Delivered', 13, N'pay_Q6GQRrrmuj9tFL', N'Online Payment', N'Paid', N'order_Q6GPvNB1xcPAcR')
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (16, N'ORD20250317101534351A9', 1003, CAST(N'2025-03-17T10:15:34.7556405' AS DateTime2), CAST(13290.00 AS Decimal(18, 2)), N'Cancelled', 7, N'pay_Q7jMdw0cxqkZRl', N'Online Payment', N'Refunded', N'order_Q7jLjRDBIYOeXw')
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (17, N'ORD202503171023391EDD7', 8, CAST(N'2025-03-17T10:23:39.3676308' AS DateTime2), CAST(8140.00 AS Decimal(18, 2)), N'Delivered', 12, N'pay_Q7jVVSrCGqZdM4', N'Online Payment', N'Paid', N'order_Q7jfSXfks9JPFc')
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (18, N'ORD20250317102537ACE29', 8, CAST(N'2025-03-17T10:25:37.6616573' AS DateTime2), CAST(4529.10 AS Decimal(18, 2)), N'Delivered', 12, N'pay_Q7jt7104ObOoqk', N'Online Payment', N'Paid', N'order_Q7jsTkV04Mdanb')
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (19, N'ORD202503171034521FE98', 9, CAST(N'2025-03-17T10:34:52.5193954' AS DateTime2), CAST(3816.00 AS Decimal(18, 2)), N'Delivered', 13, N'pay_Q7jkRiHEfMVyfL', N'Online Payment', N'Paid', N'order_Q7jkI07ACQxs93')
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (20, N'ORD20250318162946FF553', 8, CAST(N'2025-03-18T16:29:46.5249988' AS DateTime2), CAST(2862.00 AS Decimal(18, 2)), N'Cancelled', 12, N'pay_Q8EGKd4jOPmp1Z', N'Online Payment', N'Refunded', N'order_Q8EG76mNgRgVQ4')
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (21, N'ORD20250318165647703CA', 3, CAST(N'2025-03-18T16:56:47.1136457' AS DateTime2), CAST(31900.00 AS Decimal(18, 2)), N'Delivered', 3, NULL, N'Cash on Delivery', N'Paid', NULL)
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (23, N'ORD20250403112405D4D29', 11, CAST(N'2025-04-03T11:24:05.9026109' AS DateTime2), CAST(90846.96 AS Decimal(18, 2)), N'Delivered', 16, N'pay_QETbOeOOrFOyRU', N'Online Payment', N'Paid', N'order_QETbAwcZuMUXm7')
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (24, N'ORD20250403112617C74DF', 11, CAST(N'2025-04-03T11:26:17.2319368' AS DateTime2), CAST(94643.01 AS Decimal(18, 2)), N'Delivered', 16, N'pay_QETdfDHPdmYnLU', N'Online Payment', N'Paid', N'order_QETdS8U00qwOdM')
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (25, N'ORD2025040713150289978', 12, CAST(N'2025-04-07T13:15:02.2625009' AS DateTime2), CAST(92684.87 AS Decimal(18, 2)), N'Delivered', 17, N'pay_QG5dVRQSCWG7ZR', N'Online Payment', N'Paid', N'order_QG5cxsDptYkT6r')
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (26, N'ORD20250408103833D536D', 1003, CAST(N'2025-04-08T10:38:33.0346654' AS DateTime2), CAST(58986.96 AS Decimal(18, 2)), N'Cancellation Requested', 1, N'pay_QGRUu1o6oTY8KV', N'Online Payment', N'Paid', N'order_QGRUfUqYYrqPNE')
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (1021, N'ORD202503201149154A8BD', 8, CAST(N'2025-03-20T11:49:15.6942619' AS DateTime2), CAST(132389.97 AS Decimal(18, 2)), N'Cancelled', 12, NULL, N'Online Payment', N'Failed', NULL)
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (1022, N'ORD20250320115635B3F3D', 8, CAST(N'2025-03-20T11:56:35.7299267' AS DateTime2), CAST(79221.50 AS Decimal(18, 2)), N'Delivered', 12, N'pay_Q8wkEBPGIbteg8', N'Online Payment', N'Paid', N'order_Q8wk4ZW0VXKACT')
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (1023, N'ORD20250320115923C13E0', 8, CAST(N'2025-03-20T11:59:23.7884694' AS DateTime2), CAST(47321.50 AS Decimal(18, 2)), N'Cancelled', 12, N'pay_Q8wjOKR5exYK8R', N'Online Payment', N'Refunded', N'order_Q8wij63xLuXRpy')
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (1024, N'ORD2025032110400693190', 1003, CAST(N'2025-03-21T10:40:06.5621179' AS DateTime2), CAST(126850.22 AS Decimal(18, 2)), N'Cancelled', 1, NULL, N'Online Payment', N'Failed', N'order_Q9Ju7zYEt5kuc3')
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (1025, N'ORD202503211432261C3D4', 9, CAST(N'2025-03-21T14:32:26.9560387' AS DateTime2), CAST(49000.37 AS Decimal(18, 2)), N'Delivered', 13, N'pay_Q9NrjdtP2BIRPt', N'Online Payment', N'Paid', N'order_Q9NrYaWAyJfRWY')
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (1026, N'ORD20250325125830DECDC', 8, CAST(N'2025-03-25T12:58:30.5523500' AS DateTime2), CAST(60770.22 AS Decimal(18, 2)), N'Cancelled', 12, NULL, N'Online Payment', N'Failed', N'order_QAwOmZrL7dAXsu')
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (1027, N'ORD20250325125939E9130', 9, CAST(N'2025-03-25T12:59:39.9210686' AS DateTime2), CAST(60770.22 AS Decimal(18, 2)), N'Delivered', 13, N'pay_QAwQ8K6dlqCuBI', N'Online Payment', N'Paid', N'order_QAwPxsjg3OlqWb')
INSERT [dbo].[Orders] ([OrderId], [OrderNumber], [UserId], [OrderDate], [TotalAmount], [OrderStatus], [DeliveryAddressId], [PaymentId], [PaymentMethod], [PaymentStatus], [RazorpayOrderId]) VALUES (1028, N'ORD20250327142438F55F6', 10, CAST(N'2025-03-27T14:24:38.2028356' AS DateTime2), CAST(81012.45 AS Decimal(18, 2)), N'Delivered', 15, N'pay_QBkwFX2AuaDcwL', N'Online Payment', N'Paid', N'order_QBkvyvwlSmh43m')
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductImages] ON 

INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (21, N'/Images/ProductsImage/88af2503-dcb6-473e-89a5-704ed5a93e3c_S25_Ultra_PDP_exclusive_desktop_black.jpg', 1, 1, CAST(N'2025-01-28T13:25:13.637' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (22, N'/Images/ProductsImage/2881d74b-1093-43ef-8c30-dcb8c002c514_kv_IN_03_PC.jpg', 1, 0, CAST(N'2025-01-28T13:25:13.637' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (23, N'/Images/ProductsImage/191dde0d-015b-4b9f-8e94-3397d3ccfcc3_kv_IN_02_PC.jpg', 1, 0, CAST(N'2025-01-28T13:25:13.637' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (24, N'/Images/ProductsImage/5394639a-be60-419b-87c2-1a3d83d24970_kv_IN_01_PC.jpg', 1, 0, CAST(N'2025-01-28T13:25:13.637' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (25, N'/Images/ProductsImage/bfe2a179-dce3-4618-bf3b-9c2eb62ec2b5_kv_global_PC_v2.jpg', 1, 0, CAST(N'2025-01-28T13:25:13.637' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (32, N'/Images/ProductsImage/abdb0ffb-8283-49b8-a83f-b8f8b72c707a_wd1.jpg', 4, 1, CAST(N'2025-01-28T14:46:42.857' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (33, N'/Images/ProductsImage/af7c0d6a-2321-4fe6-864b-9f63abf0653f_wd3.jpg', 4, 0, CAST(N'2025-01-28T14:46:42.867' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (34, N'/Images/ProductsImage/0fbe676c-6ac9-4df4-87b8-6ba27f5d139a_wd2.jpg', 4, 0, CAST(N'2025-01-28T14:46:42.870' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (35, N'/Images/ProductsImage/021182c3-9327-4b20-9d7e-885ab1c20583_dice2.jpg', 5, 1, CAST(N'2025-01-28T16:10:33.180' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (41, N'/Images/ProductsImage/369adaf1-c25b-4e03-8148-4a4a8e1e8c0b_dice4.jpg', 5, 0, CAST(N'2025-01-28T16:11:38.457' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (42, N'/Images/ProductsImage/d0f105a4-06c2-4e8c-a386-b0ac6a7b0d13_dice5.jpg', 5, 0, CAST(N'2025-01-28T16:11:38.463' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (44, N'/Images/ProductsImage/9b843820-8ee6-4eaa-8cc7-b0d4b5e546e7_SM-F956_ZFold6_Silver-Shadow_1600x864.webm', 3, 1, CAST(N'2025-01-30T16:28:08.727' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (45, N'/Images/ProductsImage/17b56f4f-7579-483e-92c7-e824ce4e78ed_61Ut4ymAzGL._SX679_.jpg', 3, 0, CAST(N'2025-01-30T16:28:08.763' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (46, N'/Images/ProductsImage/ccfb449a-6e3c-42f7-b150-2a79b8c5984b_617Ecr1MNDL._SX679_.jpg', 3, 0, CAST(N'2025-01-30T16:28:08.767' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (47, N'/Images/ProductsImage/94c0acfd-a26b-439e-85ce-33c323dbddd0_Fold6_1600x8641.jpg', 3, 0, CAST(N'2025-01-30T16:28:08.770' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (48, N'/Images/ProductsImage/54ef84ed-995c-4a32-a887-e328f1d03e21_Fold6_1600x864-1.jpg', 3, 0, CAST(N'2025-01-30T16:28:08.773' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (49, N'/Images/ProductsImage/e29ec569-7991-417c-bf05-49f431cf3de7_Fold6_1600x864.jpg', 3, 0, CAST(N'2025-01-30T16:28:08.773' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (50, N'/Images/ProductsImage/17fa4359-d9b9-4a02-beb2-827a6c01e9d1_kv_2exclusive_PC_v4.jpg', 3, 0, CAST(N'2025-01-30T16:28:08.777' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (51, N'/Images/ProductsImage/6791d4db-e955-4fb7-ba2e-aa9d68802430_gtamin.jpg', 6, 1, CAST(N'2025-02-13T13:37:16.527' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (52, N'/Images/ProductsImage/2f90b026-ce38-4177-bd62-a0b835692654_gta2.jpg', 6, 0, CAST(N'2025-02-13T13:37:16.563' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (53, N'/Images/ProductsImage/8e5501ae-46ad-4014-8b2d-350ceb714537_gta1.jpg', 6, 0, CAST(N'2025-02-13T13:37:16.570' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (54, N'/Images/ProductsImage/d03eb0bc-99fe-4b27-84f3-5516ba1a2575_redgamemain.jpg', 7, 1, CAST(N'2025-02-13T13:39:36.563' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (55, N'/Images/ProductsImage/e6241f91-e26e-43d8-9f4f-8a1a460ad2e4_rg2.jpg', 7, 0, CAST(N'2025-02-13T13:39:36.583' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (56, N'/Images/ProductsImage/ffb73c09-fad8-4fb1-8beb-43991c7417bf_rg1.jpg', 7, 0, CAST(N'2025-02-13T13:39:36.587' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (57, N'/Images/ProductsImage/d1a2678b-0aa4-44d3-8339-e0d96f14c318_withcer.jpg', 8, 1, CAST(N'2025-02-13T13:42:15.513' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (58, N'/Images/ProductsImage/a751683b-485a-4da6-8ae1-e7e6e410e658_w2.jpg', 8, 0, CAST(N'2025-02-13T13:42:15.533' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (59, N'/Images/ProductsImage/61610e5c-0147-4e72-842c-2b55946a296b_w1.jpg', 8, 0, CAST(N'2025-02-13T13:42:15.550' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (60, N'/Images/ProductsImage/16372872-08fc-45a5-b308-a2d0055fbe1c_unmain.jpg', 9, 1, CAST(N'2025-02-13T13:44:27.657' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (61, N'/Images/ProductsImage/fb487bda-54db-4926-8788-bf5e24a37ace_un2.jpg', 9, 0, CAST(N'2025-02-13T13:44:27.677' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (62, N'/Images/ProductsImage/9cc02471-972b-4089-b3ef-8c55a44beb23_u1.jpg', 9, 0, CAST(N'2025-02-13T13:44:27.677' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (63, N'/Images/ProductsImage/bbcd5e01-3461-45ba-a8aa-2c9ee1aabbd6_pgmain.jpg', 10, 1, CAST(N'2025-02-13T13:47:13.497' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (65, N'/Images/ProductsImage/fd827113-5536-4b9a-95e0-3af2da71c05b_pg2.jpg', 10, 0, CAST(N'2025-02-13T13:47:13.523' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (66, N'/Images/ProductsImage/cdf228de-14c0-4008-8183-bed98be68022_pg1.jpg', 10, 0, CAST(N'2025-02-13T13:47:13.527' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (67, N'/Images/ProductsImage/9ed9fb8f-682b-4619-a13b-87cea34ca452_codmain.jpg', 11, 1, CAST(N'2025-02-13T13:49:49.080' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (68, N'/Images/ProductsImage/a459f700-48f7-46cc-84c4-0bb8829e4294_cod3.jpg', 11, 0, CAST(N'2025-02-13T13:49:49.107' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (69, N'/Images/ProductsImage/b8ccc175-5557-4afb-979f-dba7ea8c74df_cod2.jpg', 11, 0, CAST(N'2025-02-13T13:49:49.113' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (70, N'/Images/ProductsImage/ab0a524a-fd58-4259-8c01-c8b668c82fca_cod1.jpg', 11, 0, CAST(N'2025-02-13T13:49:49.123' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (71, N'/Images/ProductsImage/33cf0f78-2412-460f-8131-0bf64d743768_71hkOw2NkoL._SY450_.jpg', 12, 1, CAST(N'2025-02-27T10:20:37.790' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (72, N'/Images/ProductsImage/5d0b35c9-9f07-4d9b-aad3-ad2b9001e64d_514kfvkd2EL._SX450_.jpg', 12, 0, CAST(N'2025-02-27T10:20:37.813' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (73, N'/Images/ProductsImage/d81e1c74-0ac1-45be-93e5-291976fcbabe_71A2cplES-L._SX450_.jpg', 12, 0, CAST(N'2025-02-27T10:20:37.817' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (74, N'/Images/ProductsImage/7526f5db-d1cb-46a4-acd9-11deab931132_81LinqbkqIL._SY450_.jpg', 12, 0, CAST(N'2025-02-27T10:20:37.820' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (75, N'/Images/ProductsImage/da4247c0-7b56-4ab8-96a4-a87323061dc0_41xO5vqAofL._SX450_.jpg', 12, 0, CAST(N'2025-02-27T10:20:37.827' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (76, N'/Images/ProductsImage/36e4c9c2-fd72-455b-b2c9-ad9f40b351be_81yia4yEuoL._SY606_.jpg', 12, 0, CAST(N'2025-02-27T10:20:37.830' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (78, N'/Images/ProductsImage/3c5a59c0-7353-4579-b525-1f6baec18cc9_t3.webp', 13, 0, CAST(N'2025-03-17T17:24:39.550' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (79, N'/Images/ProductsImage/7eb24a91-8e79-43c2-9779-2600c9572bd2_t2.webp', 13, 0, CAST(N'2025-03-17T17:24:39.550' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (80, N'/Images/ProductsImage/c4e90b2e-8a80-43c4-bad1-62ee5ad1d093_r1.jpg', 13, 1, CAST(N'2025-03-17T17:25:21.210' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (81, N'/Images/ProductsImage/5a59228f-3f00-414d-a528-0e6057f575ae_cabinate1.jpg', 14, 1, CAST(N'2025-03-20T11:10:14.370' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (82, N'/Images/ProductsImage/9aa4c57b-e43c-4070-a2f6-031dce77016c_cab4.jpg', 14, 0, CAST(N'2025-03-20T11:10:14.397' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (83, N'/Images/ProductsImage/33304e14-7b6a-4fdf-a74b-60583773e989_cab3.jpg', 14, 0, CAST(N'2025-03-20T11:10:14.403' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (84, N'/Images/ProductsImage/76d2cd75-c40e-40e6-a189-ed2f03f69d82_cab2.jpg', 14, 0, CAST(N'2025-03-20T11:10:14.413' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (85, N'/Images/ProductsImage/704b7aa7-cab6-4cc4-aef8-e65f4473fca9_sbench1.jpg', 15, 1, CAST(N'2025-03-20T11:14:28.963' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (86, N'/Images/ProductsImage/1d2ef90e-31f0-4309-aca3-8372697428ef_sbench3.jpg', 15, 0, CAST(N'2025-03-20T11:14:28.973' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (87, N'/Images/ProductsImage/d676ba84-c46b-4987-86b7-5b8b8746b36c_sbench2.jpg', 15, 0, CAST(N'2025-03-20T11:14:28.977' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (88, N'/Images/ProductsImage/f8c6eae3-625d-4245-8c48-ed80ad898abe_beeds.jpg', 16, 1, CAST(N'2025-03-20T11:18:25.513' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (89, N'/Images/ProductsImage/1eb845e2-bf14-4243-9aba-e290f68d71e5_beed2.jpg', 16, 0, CAST(N'2025-03-20T11:18:25.527' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (90, N'/Images/ProductsImage/9a20effe-3fd2-4a15-92eb-f74252b0c711_sofa1.jpg', 17, 1, CAST(N'2025-03-20T11:20:37.823' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (91, N'/Images/ProductsImage/0c9403bd-e730-43de-938f-ef4260527442_sofa4.jpg', 17, 0, CAST(N'2025-03-20T11:20:37.840' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (92, N'/Images/ProductsImage/82efb520-7bc0-43b6-bfa9-a33a97454fd3_sofa3.jpg', 17, 0, CAST(N'2025-03-20T11:20:37.843' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (93, N'/Images/ProductsImage/2c37e050-5449-4c21-a227-1af7bd3fc3bf_sofa2.jpg', 17, 0, CAST(N'2025-03-20T11:20:37.847' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (94, N'/Images/ProductsImage/b6d0bae4-c46a-4e55-9f2c-b117a60efd9a_str1.jpg', 18, 1, CAST(N'2025-03-20T11:23:33.723' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (95, N'/Images/ProductsImage/dd4bacd8-cd67-4aa7-947c-5914237728bd_str2.jpg', 18, 0, CAST(N'2025-03-20T11:23:33.730' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (96, N'/Images/ProductsImage/7ab21fa6-f3e3-47e9-a0dc-9ef3af6a3e70_consolo1.jpg', 19, 1, CAST(N'2025-03-20T11:25:21.973' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (97, N'/Images/ProductsImage/693bfbc5-f025-4f44-b41d-089ed835079d_console2.webp', 19, 0, CAST(N'2025-03-20T11:25:21.983' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (98, N'/Images/ProductsImage/3214d30f-663e-4a47-812b-8065913f884f_bookcase1.jpg', 20, 1, CAST(N'2025-03-20T11:27:18.807' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (99, N'/Images/ProductsImage/5362840b-bfb3-4412-9a47-0364fe3fd60a_book2.jpg', 20, 0, CAST(N'2025-03-20T11:27:18.817' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (100, N'/Images/ProductsImage/29fe3678-4d17-4d1a-9341-d8bdefc1e529_s1.jpg', 21, 1, CAST(N'2025-03-20T11:30:14.300' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (101, N'/Images/ProductsImage/aad26ef4-e459-4f00-aea7-c627667247db_s3.jpg', 21, 0, CAST(N'2025-03-20T11:30:14.310' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (102, N'/Images/ProductsImage/8aad9b44-48f4-4d09-8b01-ddd6eda8640a_s2.jpeg', 21, 0, CAST(N'2025-03-20T11:30:14.313' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (103, N'/Images/ProductsImage/01334b1b-3def-45f2-94b2-eb31509b35f6_tb1.webp', 22, 1, CAST(N'2025-03-28T10:56:22.260' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (104, N'/Images/ProductsImage/760b1f6c-e3e4-40c9-a859-6162efad269c_tb5.webp', 22, 0, CAST(N'2025-03-28T10:56:22.300' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (105, N'/Images/ProductsImage/ff3c6a2d-e64b-40f0-9c4c-88fe2658f635_tb4.webp', 22, 0, CAST(N'2025-03-28T10:56:22.307' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (106, N'/Images/ProductsImage/9b90e1f8-2457-439b-8852-a8b0f2355842_tb3.webp', 22, 0, CAST(N'2025-03-28T10:56:22.313' AS DateTime))
INSERT [dbo].[ProductImages] ([Id], [ImageUrl], [ProductId], [IsPrimaryImage], [CreatedAt]) VALUES (107, N'/Images/ProductsImage/822e6231-a02b-4126-93ea-23e5e14b07c2_tb2.webp', 22, 0, CAST(N'2025-03-28T10:56:22.317' AS DateTime))
SET IDENTITY_INSERT [dbo].[ProductImages] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (1, N'Samsung S25 Ultra', N'The Samsung Galaxy S25 Ultra redefines premium smartphones with its cutting-edge technology and stunning design. Equipped with a powerful 200MP quad-camera system, the S25 Ultra captures ultra-high-definition photos and videos, while its expansive 6.9-inch AMOLED display offers a mesmerizing viewing experience. Powered by the latest Snapdragon processor and a 5,000mAh battery, this flagship device ensures smooth performance, fast charging, and long-lasting power. Whether you''re gaming, streaming, or working, the Galaxy S25 Ultra provides top-tier performance, all wrapped in a sleek and durable glass and metal body.', N'The Samsung Galaxy S25 Ultra is a flagship smartphone featuring a 200MP camera, 6.9-inch AMOLED display, and the latest Snapdragon chipset for an unmatched mobile experience.', N'C2S3SA2F01', CAST(130171.92 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), CAST(115853.01 AS Decimal(18, 2)), 1, 2, 3, CAST(40.00 AS Decimal(18, 2)), 5, 35, CAST(86781.28 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (3, N'Samsung Galaxy Z Fold6', N'Galaxy AI is Here - Put PC-like power in your pocket, Galaxy Z Fold6. More powerful than ever with its super-slim, productive screen. Now super-charged with Galaxy AI on foldables. Fold open a mobile gaming beast with a massive screen made better with the Vision Booster''s powerful brightness and clarity even in broad daylight. Enjoy silky-smooth gaming with Vulkan, even in AAA games. Then, Snapdragon® 8 Gen 3 for Galaxy renders graphics that are absolutely fire. Topped with an upgraded NPU, mind-blowing specs and ProVisual Engine, it''ll transform your multimedia experience. Zoom way, way, way in while keeping noise down and resolution clear with ProVisual Engine.', N'Samsung Galaxy Z Fold6 5G AI Smartphone (Navy, 12GB RAM, 256GB Storage) Without Offer', N'C2S3SA6BE9', CAST(134599.00 AS Decimal(18, 2)), CAST(18.00 AS Decimal(18, 2)), CAST(110371.18 AS Decimal(18, 2)), 1, 2, 3, CAST(0.00 AS Decimal(18, 2)), 10, 28, CAST(89732.67 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (4, N'Wooden Door', N'A high-quality wooden door perfect for both modern and traditional interiors.', N'Premium solid wood door with elegant design.', N'C1S1WO12E7', CAST(9000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(8100.00 AS Decimal(18, 2)), 1, 1, 1, CAST(40.00 AS Decimal(18, 2)), 15, 50, CAST(6000.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (5, N'sfdsd', N'dsfdasf', N'fdsddddddddddd', N'C4S9SFB469', CAST(6000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(5400.00 AS Decimal(18, 2)), 1, 4, 9, CAST(0.00 AS Decimal(18, 2)), 5, 24, CAST(4000.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (6, N'Grand Theft Auto V', N'A groundbreaking open-world action-adventure game that lets you live the life of criminals in the bustling city of Los Santos.', N'Open-world action game with heists, car chases, and combat.', N'C6S13GRF2DC', CAST(5500.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(4950.00 AS Decimal(18, 2)), 1, 6, 13, CAST(0.00 AS Decimal(18, 2)), 5, 80, CAST(3666.67 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (7, N'Red Dead Redemption 2', N'A beautiful and immersive action-adventure game set in the late 1800s, featuring an outlaw’s journey through the American frontier.', N'Epic western action game with a vast open world and gripping story.', N'C6S13RE59DD', CAST(4500.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), CAST(3600.00 AS Decimal(18, 2)), 1, 6, 13, CAST(0.00 AS Decimal(18, 2)), 5, 80, CAST(3000.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (8, N'The Witcher 3: Wild Hunt', N'A deep RPG set in a massive world filled with monsters, magic, and intriguing quests, where you play as Geralt of Rivia, a monster hunter.', N'Epic fantasy RPG with monster hunting and exploration.', N'C6S14TH4098', CAST(6000.00 AS Decimal(18, 2)), CAST(25.00 AS Decimal(18, 2)), CAST(4500.00 AS Decimal(18, 2)), 1, 6, 14, CAST(0.00 AS Decimal(18, 2)), 5, 60, CAST(4000.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (9, N'Uncharted 4: A Thief’s End', N'A thrilling action-adventure game where Nathan Drake embarks on a final journey filled with treasure hunts, shootouts, and dangerous encounters.', N'Action-adventure with treasure hunts and cinematic storytelling.', N'C6S14UNFE1E', CAST(8000.00 AS Decimal(18, 2)), CAST(13.00 AS Decimal(18, 2)), CAST(6960.00 AS Decimal(18, 2)), 1, 6, 14, CAST(0.00 AS Decimal(18, 2)), 10, 40, CAST(5333.33 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (10, N'PlayerUnknown''s BattleGrounds', N'PUBG is an online multiplayer battle royale game where players fight to be the last one standing in an ever-shrinking play area.', N'Battle royale game where 100 players fight to survive.', N'C6S13PL59C9', CAST(3000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(2700.00 AS Decimal(18, 2)), 1, 6, 13, CAST(0.00 AS Decimal(18, 2)), 20, 30, CAST(2000.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (11, N'Call of Duty: Black Ops 6', N'Call of Duty: Black Ops is a first-person shooter that takes players through covert operations during the Cold War. The game features an engaging story and multiplayer modes.', N'First-person shooter with a gripping Cold War-era story and intense multiplayer action.', N'C6S13CACB62', CAST(9500.00 AS Decimal(18, 2)), CAST(13.00 AS Decimal(18, 2)), CAST(8265.00 AS Decimal(18, 2)), 1, 6, 13, CAST(0.00 AS Decimal(18, 2)), 10, 80, CAST(6333.33 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (12, N'ThunderBolt X1 RC Car', N'A high-speed, all-terrain remote-controlled car with durable shock absorbers and a powerful motor.', N'High-speed RC car with durable design.', N'C4S8TH7202', CAST(4999.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(4499.10 AS Decimal(18, 2)), 1, 4, 8, CAST(30.00 AS Decimal(18, 2)), 5, 68, CAST(3332.67 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (13, N'Luxurious Round Entrance Dining Table Black Marqui', N'This luxurious dining table fits best at the entrance of your hall. Its bold look commands attention. It stands on a sturdy 49cm anti shake pillar. The table is made from high-grade marble. Size Dimensions: 130 Diameter x 75 Height', N'Luxurious round dining table crafted from high-grade Black Marquina marble, featuring a bold design and a sturdy 49cm anti-shake pillar. Perfect for entrance halls. Dimensions: 130cm (D) x 75cm (H)', N'C12S29LUE2D5', CAST(30000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(27000.00 AS Decimal(18, 2)), 1, 12, 29, CAST(40.00 AS Decimal(18, 2)), 5, 59, CAST(20000.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (14, N'Customized Built-in Storage', N'Reimagine the limitations of your space by turning the challenge of a column in the room, into an opportunity for creativity and innovation, resulting in a storage system that is uniquely yours and reflects your personality and style. Transform your home into a canvas that embodies your imagination and artistic expression.', N'Turn a column into stylish, personalized built-in storage.', N'C11S27CU80B4', CAST(160411.89 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(80205.94 AS Decimal(18, 2)), 1, 11, 27, CAST(0.00 AS Decimal(18, 2)), 20, 68, CAST(53470.63 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (15, N'Sorrento(K)-230M 3-Seater Sofa Bed', N'The Sorrento sofa bed with Puma transformation mechanism, with edging, upholstered in luxurious microvelor is simply created for sybarites and aristocrats. Soft quilted seat and back cushions made from latex-like, highly elastic HR brand polyurethane provide maximum comfort and hold their shape for years. The frame of the sofa is made of calibrated wooden beams of category 1; The base of the sofa is made of high-quality natural birch plywood and elastic straps.

This sofa bed includes 5 back cushions 57x65 cm
In the standard version: Upholstery in fabric from the Garda Decor collection - matting Santo1902-SECRU, piping - artificial leather Nevada 83, embroide', N'Sorrento 3-Seater Sofa Bed offers comfortable seating and a convenient bed option for any living space.', N'C1S30SOEABF', CAST(797026.53 AS Decimal(18, 2)), CAST(60.00 AS Decimal(18, 2)), CAST(318810.61 AS Decimal(18, 2)), 1, 1, 30, CAST(50.00 AS Decimal(18, 2)), 15, 65, CAST(295195.01 AS Decimal(18, 2)), CAST(170.00 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (16, N'Dido Lounge 92x90x77', N'Why settle for anything less than the best when it comes to your comfort? Our Beanbags are FUN, COMFORTABLE, and of the highest QUALITY. With our innovative design, these beanbags provide maximum comfort and support while still being soft enough to relax in.
Size Dimensions: 92Wx90Dx77H cm', N'Experience ultimate comfort with our fun, high-quality beanbags!', N'C1S2DI9907', CAST(61616.42 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(55454.78 AS Decimal(18, 2)), 1, 1, 2, CAST(40.00 AS Decimal(18, 2)), 10, 34, CAST(41077.61 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (17, N'Trevi 230M 3-Seater Sofa Bed', N'Trevi 3-seater Sofa bed 230M, developed by Garda Studio. Transformation mechanism - PUMA. The Trevi Sofa is made in the fabrics of the Garda Decor collection - Velv Orang (main) and Lattice Orang (rear back and decorative pillows), front supports gold color. The set is delivered unassembled; additional assembly is required.

Overall Dimensions: 232Wx103Dx88H cm
Seat Dimensions: 179Wx60Dx40H cm
Bed Dimensions: 198Wx140D cm
Wooden Support: 8 cm

This product is currently unavailable. Custom-made orders with the client’s fabric of choice are accepted within a production time of 3 months. Cancellations are not accepted.', N'Trevi 3-seater Sofa bed 230M, developed by Garda Studio. Transformation mechanism - PUMA.', N'C1S1TRC2BF', CAST(157025.21 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(78512.60 AS Decimal(18, 2)), 1, 1, 1, CAST(40.00 AS Decimal(18, 2)), 5, 23, CAST(65427.17 AS Decimal(18, 2)), CAST(140.00 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (18, N'Oval Dining Table Light Travertine 180x90x75', N'This luxurious light travertine oval dining table fits best in your kitchen or dining room. Its simple yet elegant look will grab your guests'' attention. This table is made from high-grade marble. This table can accomodate 2-4 people.', N'This oval dining table is elegant and spacious, perfect for family meals.', N'C12S29OV98C0', CAST(57035.50 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), CAST(45628.40 AS Decimal(18, 2)), 1, 12, 29, CAST(40.00 AS Decimal(18, 2)), 10, 17, CAST(23764.79 AS Decimal(18, 2)), CAST(140.00 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (19, N'Luxurious Black Marquina Console 160x35x80', N'This console is crafted through black marquina marble. A piece of accent furniture intended for an entrance, living room, the back of a couch, or a hallway. It is a beautiful addition to any room’s design where you can add a vase, flowers, a table lamp, or any of those little touches that make your home’s design look grand. You may also top it with a mirror to make the space look larger and more welcoming. Size Dimensions: 160Lx35Wx80H', N'The Black Marquina Console is pure luxury! Its sleek, high-gloss finish and stunning marble design instantly elevate the room’s aesthetic.', N'C12S28LU8EEE', CAST(71364.36 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), CAST(49955.05 AS Decimal(18, 2)), 1, 12, 28, CAST(40.00 AS Decimal(18, 2)), 10, 8, CAST(35682.18 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (20, N'Mocha Bookcase With Drawers', N'Mocha Bookcase with Drawers, a complementary and functional member of the Mocha series ; With its design consisting of a door wardrobe, two drawers and open shelves, it offers a stylish and functional solution for a teenager''s room.
Thanks to its narrow and long structure, the Mocha bookcase with drawers can be easily used even in small spaces. Books of all sizes can be easily placed thanks to its wide-sized shelves.', N'Mocha Bookcase with Drawers, a complementary and functional member of the Mocha series', N'C11S26MODEF0', CAST(242035.74 AS Decimal(18, 2)), CAST(60.00 AS Decimal(18, 2)), CAST(96814.30 AS Decimal(18, 2)), 1, 11, 26, CAST(40.00 AS Decimal(18, 2)), 5, 60, CAST(69153.07 AS Decimal(18, 2)), CAST(250.00 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (21, N'Malai 3-Seater Sofa Bed', N'Create a comfortable corner in your space with our Malai Sofa Bed. This versatile piece can be easily converted into a bed, providing the perfect accommodation for your guests.

Size Dimensions: 208Wx103Dx75H cm', N'Create a comfortable corner in your space with our Malai Sofa Bed.', N'C1S1MAF420', CAST(139514.79 AS Decimal(18, 2)), CAST(60.00 AS Decimal(18, 2)), CAST(55805.92 AS Decimal(18, 2)), 1, 1, 1, CAST(40.00 AS Decimal(18, 2)), 0, 0, CAST(45004.77 AS Decimal(18, 2)), CAST(210.00 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([ProductId], [Name], [Description], [ShortDescription], [SKU], [Price], [SellingPricePercent], [CalculatedSellingPrice], [Status], [CategoryId], [SubcategoryId], [DeliveryCharge], [MinimumStockLevel], [StockQuantity], [CostPrice], [ProfitPercentage]) VALUES (22, N'Luxury Director L-Shape Office Table Desk', N'Luxury Director L-Shape Office Table Desk Made in MDF with Wire Manager Drawers Lockable and Storage for a Modern Workspace - White
Product Dimension

Size 1st: 5x2.5 Ft: L60 x W30 x H30 IN / Side Unit: L66 x W18 x H26 IN

Size 2nd: 6x3 Ft: L72 x W36 x H30 IN / Side Unit: L72 x W18 x H26 IN

Size 3rd: 8x3 Ft: L96 x W36 x H30 IN / Side Unit: L72 x W18 x H26 IN

Top Thickness- 34MM

Material: Made in High-quality PLY and Solid Wood with Veneer and PU Polish

Colour:  White

Feature: Office Desk - Side unit, Drawers, Storage CPU Space and Wire manager

Specs Details:-

Action Tesa /Merino/ Prelaminated Particle Board/ MDF/Plywood
High-Quality Fittings Hardware- Hinges, Channels, Handles, Locks ( EBCO / OZONE / HETTICH )
Greenlam / Centuryply Veneer Featuring A Variety Of Grain Patterns
High-Quality Seasoned Teak & Pine Solid Wood
Five-Coat Polish Using Asian / Sirca Paints
Precision Engineering Through Machine-Driven Manufacturing
Environment-Friendly Furniture
Five-Step Quality Check
ISO/ BIFMA Certified
Material Offering Excellent Resistance To Moisture
Fire Retardant Properties', N'Luxury Director L-Shape Office Table Desk Made in Plywood with Veneer Wire Manager Drawers Lockable and Storage for a Modern Workspace - White', N'C12S28LU4067', CAST(210873.75 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(189786.38 AS Decimal(18, 2)), 1, 12, 28, CAST(0.00 AS Decimal(18, 2)), 5, 25, CAST(140582.50 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
INSERT [dbo].[RazorpayOrders] ([OrderId], [Razorpaykey], [Amount], [Currency], [Name], [Email], [PhoneNumber], [Address], [Description], [ApplicationOrderId]) VALUES (N'order_Q9NrYaWAyJfRWY', N'rzp_test_bGMrj0FqAz3fWQ', CAST(49000.37 AS Decimal(18, 2)), N'INR', N'Shaniya Vahora', N'shalini37@gmail.com', N'8791234569', N'Royal Garden, APC Circle, Anand, Gujarat - 388001 , India', N'Order #ORD202503211432261C3D4', 1025)
INSERT [dbo].[RazorpayOrders] ([OrderId], [Razorpaykey], [Amount], [Currency], [Name], [Email], [PhoneNumber], [Address], [Description], [ApplicationOrderId]) VALUES (N'order_QAwOmZrL7dAXsu', N'rzp_test_bGMrj0FqAz3fWQ', CAST(60770.22 AS Decimal(18, 2)), N'INR', N'Jamila Lokhadwala', N'jamila15@gmail.com', N'8200746058', N'Royal City, Umreth, Gujarat - 381201 , India', N'Order #ORD20250325125830DECDC', 1026)
INSERT [dbo].[RazorpayOrders] ([OrderId], [Razorpaykey], [Amount], [Currency], [Name], [Email], [PhoneNumber], [Address], [Description], [ApplicationOrderId]) VALUES (N'order_QAwPxsjg3OlqWb', N'rzp_test_bGMrj0FqAz3fWQ', CAST(60770.22 AS Decimal(18, 2)), N'INR', N'Shaniya Vahora', N'shaniya37@gmail.com', N'8791234569', N'Royal Garden, APC Circle, Anand, Gujarat - 388001 , India', N'Order #ORD20250325125939E9130', 1027)
INSERT [dbo].[RazorpayOrders] ([OrderId], [Razorpaykey], [Amount], [Currency], [Name], [Email], [PhoneNumber], [Address], [Description], [ApplicationOrderId]) VALUES (N'order_QBkvyvwlSmh43m', N'rzp_test_bGMrj0FqAz3fWQ', CAST(81012.45 AS Decimal(18, 2)), N'INR', N'Ashfiya Vahora', N'ashfiya09@gmail.com', N'7897845214', N'Ismailnagar, Bhalej Road, Anand, Gujarat - 388001 , India', N'Order #ORD20250327142438F55F6', 1028)
INSERT [dbo].[RazorpayOrders] ([OrderId], [Razorpaykey], [Amount], [Currency], [Name], [Email], [PhoneNumber], [Address], [Description], [ApplicationOrderId]) VALUES (N'order_QETbAwcZuMUXm7', N'rzp_test_bGMrj0FqAz3fWQ', CAST(90846.96 AS Decimal(18, 2)), N'INR', N'Hiba Nawab', N'hiba2@gmail.com', N'7574887412', N'Marin Drive, Mumbai, Maharashtra - 400020 , India', N'Order #ORD20250403112405D4D29', 23)
INSERT [dbo].[RazorpayOrders] ([OrderId], [Razorpaykey], [Amount], [Currency], [Name], [Email], [PhoneNumber], [Address], [Description], [ApplicationOrderId]) VALUES (N'order_QETdS8U00qwOdM', N'rzp_test_bGMrj0FqAz3fWQ', CAST(94643.01 AS Decimal(18, 2)), N'INR', N'Hiba Nawab', N'hiba2@gmail.com', N'7574887412', N'Marin Drive, Mumbai, Maharashtra - 400020 , India', N'Order #ORD20250403112617C74DF', 24)
INSERT [dbo].[RazorpayOrders] ([OrderId], [Razorpaykey], [Amount], [Currency], [Name], [Email], [PhoneNumber], [Address], [Description], [ApplicationOrderId]) VALUES (N'order_QG5cxsDptYkT6r', N'rzp_test_bGMrj0FqAz3fWQ', CAST(92684.87 AS Decimal(18, 2)), N'INR', N'Robert Downey', N'robertd2@gmail.com', N'7890789014', N'Royal Park, Surat, Surat - 388785 , India', N'Order #ORD2025040713150289978', 25)
INSERT [dbo].[RazorpayOrders] ([OrderId], [Razorpaykey], [Amount], [Currency], [Name], [Email], [PhoneNumber], [Address], [Description], [ApplicationOrderId]) VALUES (N'order_QGRUfUqYYrqPNE', N'rzp_test_bGMrj0FqAz3fWQ', CAST(58986.96 AS Decimal(18, 2)), N'INR', N'Shayan Vahora', N'shayan2@gmail.com', N'9987451225', N'Ismailnagar, Bhalej Road, Anand, Gujarat - 388001 , India', N'Order #ORD20250408103833D536D', 26)
GO
INSERT [dbo].[RefundDetails] ([RefundId], [PaymentId], [Amount], [Status], [CreatedAt], [OrderId], [Notes], [SpeedCode]) VALUES (N'0e163d4a48bb4861ada175c7b24e228c', N'pay_Q7jMdw0cxqkZRl', CAST(13290.00 AS Decimal(18, 2)), N'Completed', CAST(N'2025-03-18T13:25:06.7197375' AS DateTime2), 16, N'Order Cancellation by Customer', N'HNA3HO')
INSERT [dbo].[RefundDetails] ([RefundId], [PaymentId], [Amount], [Status], [CreatedAt], [OrderId], [Notes], [SpeedCode]) VALUES (N'285caff8c7704772859ef78abd2f6188', N'pay_Q8EGKd4jOPmp1Z', CAST(2862.00 AS Decimal(18, 2)), N'Completed', CAST(N'2025-03-18T16:46:22.7968781' AS DateTime2), 20, N'Order Cancellation by Customer', N'AAAJ5A')
INSERT [dbo].[RefundDetails] ([RefundId], [PaymentId], [Amount], [Status], [CreatedAt], [OrderId], [Notes], [SpeedCode]) VALUES (N'8bda52859fe7424cab1fa7a5d3e1c7cf', N'pay_Q8wjOKR5exYK8R', CAST(47321.50 AS Decimal(18, 2)), N'Completed', CAST(N'2025-03-20T12:02:35.8122041' AS DateTime2), 1023, N'Order Cancellation by Customer', N'VVVVJ3')
INSERT [dbo].[RefundDetails] ([RefundId], [PaymentId], [Amount], [Status], [CreatedAt], [OrderId], [Notes], [SpeedCode]) VALUES (N'e305a3f30a844044989d5ed23ecf2e90', N'pay_QGRUu1o6oTY8KV', CAST(58986.96 AS Decimal(18, 2)), N'Pending', CAST(N'2025-04-08T11:01:02.3717567' AS DateTime2), 26, N'Order Cancellation by Customer', N'EF25S3')
GO
SET IDENTITY_INSERT [dbo].[Reviews] ON 

INSERT [dbo].[Reviews] ([ReviewId], [ProductId], [UserId], [Rating], [Description], [CreatedDate], [IsApproved], [ApprovedDate]) VALUES (1, 22, 1003, 4, N'Good', CAST(N'2025-04-03T16:24:33.6869211' AS DateTime2), 1, NULL)
INSERT [dbo].[Reviews] ([ReviewId], [ProductId], [UserId], [Rating], [Description], [CreatedDate], [IsApproved], [ApprovedDate]) VALUES (2, 13, 8, 4, N'Good', CAST(N'2025-04-04T12:14:17.8905374' AS DateTime2), 1, NULL)
INSERT [dbo].[Reviews] ([ReviewId], [ProductId], [UserId], [Rating], [Description], [CreatedDate], [IsApproved], [ApprovedDate]) VALUES (3, 14, 8, 4, N'Good', CAST(N'2025-04-06T10:01:35.5048835' AS DateTime2), 1, NULL)
INSERT [dbo].[Reviews] ([ReviewId], [ProductId], [UserId], [Rating], [Description], [CreatedDate], [IsApproved], [ApprovedDate]) VALUES (4, 1, 1003, 4, N'Good', CAST(N'2025-04-07T11:20:21.5936724' AS DateTime2), 1, NULL)
INSERT [dbo].[Reviews] ([ReviewId], [ProductId], [UserId], [Rating], [Description], [CreatedDate], [IsApproved], [ApprovedDate]) VALUES (5, 5, 1003, 2, N'gg', CAST(N'2025-04-07T12:43:55.1941499' AS DateTime2), 1, NULL)
INSERT [dbo].[Reviews] ([ReviewId], [ProductId], [UserId], [Rating], [Description], [CreatedDate], [IsApproved], [ApprovedDate]) VALUES (6, 11, 1003, 5, N'Good Game for action', CAST(N'2025-04-07T12:48:06.0375569' AS DateTime2), 1, NULL)
INSERT [dbo].[Reviews] ([ReviewId], [ProductId], [UserId], [Rating], [Description], [CreatedDate], [IsApproved], [ApprovedDate]) VALUES (7, 11, 3, 4, N'Fantastic game and graphics', CAST(N'2025-04-07T12:55:08.8806052' AS DateTime2), 1, NULL)
SET IDENTITY_INSERT [dbo].[Reviews] OFF
GO
SET IDENTITY_INSERT [dbo].[Subcategories] ON 

INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (1, N'Sofa Bed', 1)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (2, N'Bean Bag', 1)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (3, N'Mobiles', 2)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (5, N'Bat', 3)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (8, N'Remote Car', 4)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (9, N'Doll', 4)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (11, N'Dress', 5)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (12, N'Kurta', 5)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (13, N'Action', 6)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (14, N'Adventure', 6)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (15, N'Simulation', 6)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (17, N'Sports', 6)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (21, N'Movie', 9)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (22, N'Music', 9)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (23, N'Fiction', 10)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (24, N'Non-Fiction', 10)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (25, N'Educational', 10)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (26, N'Bookcases', 11)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (27, N'Cabinates', 11)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (28, N'Console', 12)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (29, N'Dining Table', 12)
INSERT [dbo].[Subcategories] ([SubcategoryId], [Name], [CategoryId]) VALUES (30, N'Bench', 1)
SET IDENTITY_INSERT [dbo].[Subcategories] OFF
GO
SET IDENTITY_INSERT [dbo].[userlogin] ON 

INSERT [dbo].[userlogin] ([Id], [FirstName], [LastName], [Email], [Phone], [Photo], [Gender], [Password], [IsActive], [Role]) VALUES (1, N'Aamin', N'Vahora', N'ak22@gmail.com', N'7525095865', N'a443c1e4-7d41-4410-be33-7f5f80bc11e6_candy.jpeg', N'Male', N'123', 1, N'Admin')
INSERT [dbo].[userlogin] ([Id], [FirstName], [LastName], [Email], [Phone], [Photo], [Gender], [Password], [IsActive], [Role]) VALUES (2, N'Akil', N'Vahora', N'akil22@gmail.com', N'7896541235', N'c678d08f-fd92-4482-8280-9057bb4a9fde_g7.jpeg', N'Male', N'123', 1, N'Admin')
INSERT [dbo].[userlogin] ([Id], [FirstName], [LastName], [Email], [Phone], [Photo], [Gender], [Password], [IsActive], [Role]) VALUES (3, N'Demo', N'Demo', N'demo2@gmail.com', N'7896541235', N'c49f29c6-9d00-49bc-bfe6-88bfc00c2242_email2.jpeg', N'Male', N'456', 1, N'User')
INSERT [dbo].[userlogin] ([Id], [FirstName], [LastName], [Email], [Phone], [Photo], [Gender], [Password], [IsActive], [Role]) VALUES (7, N'Chandresh', N'Panchal', N'chandresh23@gmail.com', N'8798792044', N'88286dfe-5679-443e-b270-bd9953cfb1d0_security1.jpeg', N'Male', N'456', 1, N'User')
INSERT [dbo].[userlogin] ([Id], [FirstName], [LastName], [Email], [Phone], [Photo], [Gender], [Password], [IsActive], [Role]) VALUES (8, N'Jamila', N'Lokhadwala', N'jamila15@gmail.com', N'8200746058', N'28a9f0a8-215d-47ab-ad4a-219e06c0f7f8_fpilot1.jpg', N'Female', N'123', 1, N'User')
INSERT [dbo].[userlogin] ([Id], [FirstName], [LastName], [Email], [Phone], [Photo], [Gender], [Password], [IsActive], [Role]) VALUES (9, N'Shaniya', N'Vahora', N'shaniya37@gmail.com', N'8791234569', N'9f16f917-d51c-4968-805e-dd20e0ba66b2_s2.jpg', N'Female', N'456', 1, N'User')
INSERT [dbo].[userlogin] ([Id], [FirstName], [LastName], [Email], [Phone], [Photo], [Gender], [Password], [IsActive], [Role]) VALUES (10, N'Ashfiya', N'Vahora', N'ashfiya09@gmail.com', N'7897845214', N'517aa31b-9573-47f1-a050-2d34cbc1691e_gettyimages-1403935505-612x612.jpg', N'Female', N'123', 1, N'User')
INSERT [dbo].[userlogin] ([Id], [FirstName], [LastName], [Email], [Phone], [Photo], [Gender], [Password], [IsActive], [Role]) VALUES (11, N'Hiba', N'Nawab', N'hiba3@gmail.com', N'7574887412', N'7ec394d0-e76e-480c-9d20-3962861bb6b2_h1.jpg', N'Female', N'123', 1, N'User')
INSERT [dbo].[userlogin] ([Id], [FirstName], [LastName], [Email], [Phone], [Photo], [Gender], [Password], [IsActive], [Role]) VALUES (12, N'Robert', N'Downey', N'robertd2@gmail.com', N'7890789014', N'e0549499-3ce0-461f-a36f-67c75b9b3590_author2.png', N'Male', N'123', 1, N'User')
INSERT [dbo].[userlogin] ([Id], [FirstName], [LastName], [Email], [Phone], [Photo], [Gender], [Password], [IsActive], [Role]) VALUES (1003, N'Shayan', N'Vahora', N'shayan2@gmail.com', N'7896541254', N'862b29bd-6ff3-4c8d-83e3-892c904e0e31_home3.jpeg', N'Male', N'123', 1, N'User')
INSERT [dbo].[userlogin] ([Id], [FirstName], [LastName], [Email], [Phone], [Photo], [Gender], [Password], [IsActive], [Role]) VALUES (1004, N'Demo', N'Role', N'demorole2@gmail.com', N'7896541238', N'cf12c85e-87cf-47a9-9b25-ce8f0dd105e9_81uu3CcTfFL._SX679_.jpg', N'Male', N'123', 1, N'User')
INSERT [dbo].[userlogin] ([Id], [FirstName], [LastName], [Email], [Phone], [Photo], [Gender], [Password], [IsActive], [Role]) VALUES (1005, N'Arsil', N'Malek', N'arsil2@gmail.com', N'9978124536', N'10e81458-aee3-4caa-8696-9ad63b87944a_pilot1.jpg', N'Male', N'123', 1, N'User')
SET IDENTITY_INSERT [dbo].[userlogin] OFF
GO
/****** Object:  Index [IX_Carts_ProductId]    Script Date: 08-04-2025 11:33:04 ******/
CREATE NONCLUSTERED INDEX [IX_Carts_ProductId] ON [dbo].[Carts]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Category_Name_Unique]    Script Date: 08-04-2025 11:33:04 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Category_Name_Unique] ON [dbo].[Categories]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DeliveryAddresses_UserId]    Script Date: 08-04-2025 11:33:04 ******/
CREATE NONCLUSTERED INDEX [IX_DeliveryAddresses_UserId] ON [dbo].[DeliveryAddresses]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_GstTax_SubcategoryId]    Script Date: 08-04-2025 11:33:04 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_GstTax_SubcategoryId] ON [dbo].[GstTax]
(
	[SubcategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderItems_OrderId]    Script Date: 08-04-2025 11:33:04 ******/
CREATE NONCLUSTERED INDEX [IX_OrderItems_OrderId] ON [dbo].[OrderItems]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderItems_ProductId]    Script Date: 08-04-2025 11:33:04 ******/
CREATE NONCLUSTERED INDEX [IX_OrderItems_ProductId] ON [dbo].[OrderItems]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_DeliveryAddressId]    Script Date: 08-04-2025 11:33:04 ******/
CREATE NONCLUSTERED INDEX [IX_Orders_DeliveryAddressId] ON [dbo].[Orders]
(
	[DeliveryAddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Orders_OrderNumber]    Script Date: 08-04-2025 11:33:04 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Orders_OrderNumber] ON [dbo].[Orders]
(
	[OrderNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_UserId]    Script Date: 08-04-2025 11:33:04 ******/
CREATE NONCLUSTERED INDEX [IX_Orders_UserId] ON [dbo].[Orders]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductImages_ProductId]    Script Date: 08-04-2025 11:33:04 ******/
CREATE NONCLUSTERED INDEX [IX_ProductImages_ProductId] ON [dbo].[ProductImages]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Products_CategoryId]    Script Date: 08-04-2025 11:33:04 ******/
CREATE NONCLUSTERED INDEX [IX_Products_CategoryId] ON [dbo].[Products]
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Products_SubcategoryId]    Script Date: 08-04-2025 11:33:04 ******/
CREATE NONCLUSTERED INDEX [IX_Products_SubcategoryId] ON [dbo].[Products]
(
	[SubcategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Product_Name_Unique]    Script Date: 08-04-2025 11:33:04 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Product_Name_Unique] ON [dbo].[Products]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Product_SKU_Unique]    Script Date: 08-04-2025 11:33:04 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Product_SKU_Unique] ON [dbo].[Products]
(
	[SKU] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RazorpayOrders_ApplicationOrderId]    Script Date: 08-04-2025 11:33:04 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_RazorpayOrders_ApplicationOrderId] ON [dbo].[RazorpayOrders]
(
	[ApplicationOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RefundDetails_OrderId]    Script Date: 08-04-2025 11:33:04 ******/
CREATE NONCLUSTERED INDEX [IX_RefundDetails_OrderId] ON [dbo].[RefundDetails]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Reviews_ProductId]    Script Date: 08-04-2025 11:33:04 ******/
CREATE NONCLUSTERED INDEX [IX_Reviews_ProductId] ON [dbo].[Reviews]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Reviews_UserId]    Script Date: 08-04-2025 11:33:04 ******/
CREATE NONCLUSTERED INDEX [IX_Reviews_UserId] ON [dbo].[Reviews]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Subcategories_CategoryId]    Script Date: 08-04-2025 11:33:04 ******/
CREATE NONCLUSTERED INDEX [IX_Subcategories_CategoryId] ON [dbo].[Subcategories]
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Subcategory_Name_Unique]    Script Date: 08-04-2025 11:33:04 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Subcategory_Name_Unique] ON [dbo].[Subcategories]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [email_unique_userlogin]    Script Date: 08-04-2025 11:33:04 ******/
CREATE UNIQUE NONCLUSTERED INDEX [email_unique_userlogin] ON [dbo].[userlogin]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OrderItems] ADD  DEFAULT ((0.0)) FOR [BasePrice]
GO
ALTER TABLE [dbo].[OrderItems] ADD  DEFAULT ((0.0)) FOR [DeliveryCharge]
GO
ALTER TABLE [dbo].[OrderItems] ADD  DEFAULT ((0.0)) FOR [SnapshotCGSTPercentage]
GO
ALTER TABLE [dbo].[OrderItems] ADD  DEFAULT ((0.0)) FOR [SnapshotCostPrice]
GO
ALTER TABLE [dbo].[OrderItems] ADD  DEFAULT ((0.0)) FOR [SnapshotDiscountPercentage]
GO
ALTER TABLE [dbo].[OrderItems] ADD  DEFAULT ((0.0)) FOR [SnapshotGSTAmount]
GO
ALTER TABLE [dbo].[OrderItems] ADD  DEFAULT ((0.0)) FOR [SnapshotProfitPercentage]
GO
ALTER TABLE [dbo].[OrderItems] ADD  DEFAULT ((0.0)) FOR [SnapshotSGSTPercentage]
GO
ALTER TABLE [dbo].[ProductImages] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsPrimaryImage]
GO
ALTER TABLE [dbo].[ProductImages] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0.0)) FOR [DeliveryCharge]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0)) FOR [MinimumStockLevel]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0)) FOR [StockQuantity]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0.0)) FOR [CostPrice]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((50.0)) FOR [ProfitPercentage]
GO
ALTER TABLE [dbo].[userlogin] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsActive]
GO
ALTER TABLE [dbo].[userlogin] ADD  DEFAULT (N'User') FOR [Role]
GO
ALTER TABLE [dbo].[Carts]  WITH CHECK ADD  CONSTRAINT [FK_Carts_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Carts] CHECK CONSTRAINT [FK_Carts_Products_ProductId]
GO
ALTER TABLE [dbo].[Carts]  WITH CHECK ADD  CONSTRAINT [FK_Carts_userlogin_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[userlogin] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Carts] CHECK CONSTRAINT [FK_Carts_userlogin_UserId]
GO
ALTER TABLE [dbo].[DeliveryAddresses]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryAddresses_userlogin_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[userlogin] ([Id])
GO
ALTER TABLE [dbo].[DeliveryAddresses] CHECK CONSTRAINT [FK_DeliveryAddresses_userlogin_UserId]
GO
ALTER TABLE [dbo].[GstTax]  WITH CHECK ADD  CONSTRAINT [FK_GstTax_Subcategories_SubcategoryId] FOREIGN KEY([SubcategoryId])
REFERENCES [dbo].[Subcategories] ([SubcategoryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GstTax] CHECK CONSTRAINT [FK_GstTax_Subcategories_SubcategoryId]
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_OrderItems_Orders_OrderId]
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_OrderItems_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_OrderItems_Products_ProductId]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_DeliveryAddresses_DeliveryAddressId] FOREIGN KEY([DeliveryAddressId])
REFERENCES [dbo].[DeliveryAddresses] ([AddressId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_DeliveryAddresses_DeliveryAddressId]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_userlogin_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[userlogin] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_userlogin_UserId]
GO
ALTER TABLE [dbo].[ProductImages]  WITH CHECK ADD  CONSTRAINT [FK_ProductImages_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductImages] CHECK CONSTRAINT [FK_ProductImages_Products_ProductId]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([CategoryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Categories_CategoryId]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Subcategories_SubcategoryId] FOREIGN KEY([SubcategoryId])
REFERENCES [dbo].[Subcategories] ([SubcategoryId])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Subcategories_SubcategoryId]
GO
ALTER TABLE [dbo].[RazorpayOrders]  WITH CHECK ADD  CONSTRAINT [FK_RazorpayOrders_Orders_ApplicationOrderId] FOREIGN KEY([ApplicationOrderId])
REFERENCES [dbo].[Orders] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RazorpayOrders] CHECK CONSTRAINT [FK_RazorpayOrders_Orders_ApplicationOrderId]
GO
ALTER TABLE [dbo].[RefundDetails]  WITH CHECK ADD  CONSTRAINT [FK_RefundDetails_Orders_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RefundDetails] CHECK CONSTRAINT [FK_RefundDetails_Orders_OrderId]
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD  CONSTRAINT [FK_Reviews_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Reviews] CHECK CONSTRAINT [FK_Reviews_Products_ProductId]
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD  CONSTRAINT [FK_Reviews_userlogin_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[userlogin] ([Id])
GO
ALTER TABLE [dbo].[Reviews] CHECK CONSTRAINT [FK_Reviews_userlogin_UserId]
GO
ALTER TABLE [dbo].[Subcategories]  WITH CHECK ADD  CONSTRAINT [FK_Subcategories_Categories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([CategoryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Subcategories] CHECK CONSTRAINT [FK_Subcategories_Categories_CategoryId]
GO
USE [master]
GO
ALTER DATABASE [WebMobiTask1DB] SET  READ_WRITE 
GO
