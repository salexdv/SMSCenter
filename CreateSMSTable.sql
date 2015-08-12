USE [SMS]
GO

/****** Object:  Table [dbo].[Messages]    Script Date: 05/22/2014 11:58:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Messages](
	[id] [numeric](11, 0) NOT NULL,
	[sender] [nchar](20) NOT NULL,
	[text] [text] NOT NULL,
	[number] [numeric](11, 0) NOT NULL,
	[create_date] [datetime] NOT NULL,
	[send_date] [datetime] NOT NULL,
	[send_status] [numeric](5, 0) NOT NULL,
	[delivery_date] [datetime] NOT NULL,
	[delivery_status] [numeric](5, 0) NOT NULL,
	[delivery_text] [text] NOT NULL,
	[uin] [numeric](11, 0) NOT NULL,
 CONSTRAINT [PK_id] UNIQUE NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


