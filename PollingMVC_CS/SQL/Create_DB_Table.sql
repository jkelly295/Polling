/****** Object:  Table [dbo].[PLP_Assemble]    Script Date: 4/9/2018 8:48:10 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PLP_Assemble](
	[PLP_ID] [int] NOT NULL,
	[Assembler] [varchar](50) NOT NULL,
	[Assemble_Started] [datetime] NULL,
	[Assemble_Ended] [datetime] NULL,
	[Status] [varchar](50) NOT NULL,
	[Percent_Complete] [int] NOT NULL,
	[LastStepNote] [varchar](max) NULL,
 CONSTRAINT [PK_PLP_Assemble] PRIMARY KEY CLUSTERED 
(
	[PLP_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO