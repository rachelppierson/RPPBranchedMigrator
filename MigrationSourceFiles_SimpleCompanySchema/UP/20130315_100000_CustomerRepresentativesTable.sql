
/****** Object:  Table [dbo].[CustomerRepresentatives]    Script Date: 06/30/2013 20:11:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerRepresentatives](
	[Id] [int] NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[CustomerId] [int] NOT NULL,
 CONSTRAINT [PK_CustomerRepresentatives] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CustomerRepresentatives]  WITH CHECK ADD  CONSTRAINT [FK_CustomerRepresentatives_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([Id])
GO
ALTER TABLE [dbo].[CustomerRepresentatives] CHECK CONSTRAINT [FK_CustomerRepresentatives_Customers]
GO
