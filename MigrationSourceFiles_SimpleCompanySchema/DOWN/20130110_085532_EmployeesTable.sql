
/****** Object:  Table [dbo].[Employees]    Script Date: 06/30/2013 20:15:28 ******/
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_Employees_Departments]
GO
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_Employees_Employees]
GO
DROP TABLE [dbo].[Employees]
GO
