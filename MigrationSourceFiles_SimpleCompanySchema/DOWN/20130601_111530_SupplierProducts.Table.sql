
/****** Object:  Table [dbo].[SupplierProducts]    Script Date: 06/30/2013 20:15:28 ******/
ALTER TABLE [dbo].[SupplierProducts] DROP CONSTRAINT [FK_SupplierProducts_Products]
GO
ALTER TABLE [dbo].[SupplierProducts] DROP CONSTRAINT [FK_SupplierProducts_Suppliers]
GO
ALTER TABLE [dbo].[SupplierProducts] DROP CONSTRAINT [DF_SupplierProducts_Price]
GO
DROP TABLE [dbo].[SupplierProducts]
GO
