USE master
GO

-- Drop the database if it already exists
IF  EXISTS (
	SELECT name 
		FROM sys.databases 
		WHERE name = N'MailManager'
)
DROP DATABASE MailManager
GO

CREATE DATABASE MailManager
GO

USE MailManager
GO

/****** Object:  Table [dbo].[tblSubscriptionStatus]    Script Date: 02/10/2007 21:49:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSubscriptionStatus](
	[SubscriptionStatusID] [int] NOT NULL,
	[SubscriptionStatus] [nvarchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_tblSubscrptionStatus] PRIMARY KEY CLUSTERED 
(
	[SubscriptionStatusID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[tblSubscriptions]    Script Date: 02/10/2007 21:48:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSubscriptions](
	[SubscriptionID] [uniqueidentifier] NOT NULL,
	[EmailAdress] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SubscriptionStatus] [int] NOT NULL CONSTRAINT [DF_tblSubscriptions_SubscriptionStatus]  DEFAULT ((1)),
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_tblSubscriptions_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_tblSubscriptions] PRIMARY KEY CLUSTERED 
(
	[SubscriptionID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[tblSubscriptions]  WITH CHECK ADD  CONSTRAINT [FK_tblSubscriptions_tblSubscriptionStatus] FOREIGN KEY([SubscriptionStatus])
REFERENCES [dbo].[tblSubscriptionStatus] ([SubscriptionStatusID])
GO
ALTER TABLE [dbo].[tblSubscriptions] CHECK CONSTRAINT [FK_tblSubscriptions_tblSubscriptionStatus]
GO

/* Fill in initial values */
INSERT INTO tblSubscriptionStatus (SubscriptionStatusID, SubscriptionStatus)
VALUES     (1, N'Pending')
GO
INSERT INTO tblSubscriptionStatus (SubscriptionStatusID, SubscriptionStatus)
VALUES     (2, N'Confirmed')
GO
