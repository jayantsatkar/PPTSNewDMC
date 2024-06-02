update mst_user set  First_Name='' where First_Name is null
update mst_user set  Last_Name='' where Last_Name is null
Update MST_USER SET Middle_Name ='' WHERE Middle_Name  IS NULL
Update MST_USER SET EmailId ='' WHERE EmailId  IS NULL
Update MST_USER SET Address ='' WHERE Address  IS NULL
Update MST_USER SET City ='' WHERE City  IS NULL
Update MST_USER SET State ='' WHERE State  IS NULL
Update MST_USER SET Country ='' WHERE Country  IS NULL
Update MST_USER SET Pincode ='' WHERE Pincode  IS NULL
Update MST_USER SET Mobile_No ='' WHERE Mobile_No  IS NULL
Update MST_USER SET EmployeeId ='' WHERE EmployeeId  IS NULL
Update MST_USER SET IsActive ='0' WHERE IsActive  IS NULL
Update MST_USER SET Created_By ='1' WHERE Created_By  IS NULL
Update MST_USER SET Created_On ='1 JAN 2024' WHERE Created_On  IS NULL

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Logs](
	[Id] [uniqueidentifier] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Level] [varchar](max) NOT NULL,
	[Logger] [varchar](max) NOT NULL,
	[Message] [varchar](max) NOT NULL,
	[Exception] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Logs] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Logs] ADD  CONSTRAINT [DEFAULT_Logs_Date]  DEFAULT (getdate()) FOR [Date]
GO