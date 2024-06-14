TRUNCATE TRN_BOXtEMPO
TRUNCATE TRN_PALLETtEMPO
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



UPDATE MST_Customer SET Created_By = 1, Created_On= GETDATE() WHERE Created_By IS NULL
ALTER TABLE MST_Customer 
ALTER COLUMN [Created_By] [bigint] NOT NULL 

UPDATE MST_PartConfiguration SET Created_By = 1 WHERE Created_By IS NULL 

ALTER TABLE MST_PartConfiguration 
ALTER COLUMN [Created_By] [bigint] NOT NULL 

SET IDENTITY_INSERT MST_Form ON 
INSERT INTO MST_Form(Frm_Id,	Module_ID,	Sub_Module_ID,	Frm_Name,	Frm_URL,	Frm_Order,	Created_By,	Created_On)

SELECT 
11,	3,	0,	'Box Label Generation (Sticker)',	'/BoxLabel/BoxLabel',	3,	1,	'2024-06-14'

SET IDENTITY_INSERT MST_Form OFF 

INSERT INTO REL_UserForm(	User_Id,	Frm_Id,	Flag_Visible,	Created_By,	Created_On)
SELECT 	1,	11,	1	,1	,GETDATE()

usp_CreatePalletByUser
usp_CreateBoxByUser
usp_AddUpdatePartConfiguration
usp_GetPartDetails
usp_GetParts
usp_AddUpdateCustomer
usp_GetCustomer
usp_GetCustomers
usp_GetUserByUserName
usp_AddError
usp_AddUpdateUser
usp_GetUser
usp_GetUsers
usp_AddUpdateRole
usp_GetRole
usp_GetUserForms
usp_GetModules
usp_GetForms
usp_GetUserForm
usp_GetUserWithRole
usp_GetRoles