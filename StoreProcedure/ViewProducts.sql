USE [PersonDistributionSystem]
GO

/****** Object:  StoredProcedure [dbo].[ViewProducts]    Script Date: 8/8/2023 10:25:08 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ViewProducts]
			(
				@Id INT=NULL,
				@Item NVARCHAR(50) = NULL,
				@Quantity NVARCHAR(10) = NULL,
				@Price DECIMAL(18,0) = NULL,
				@Action NVARCHAR(10) = NULL
			)
			AS
			BEGIN
				IF @Action = 'View'
					BEGIN
						SELECT * FROM Products;

					END

				ELSE IF @Action = 'Product'
					 BEGIN
						SELECT * FROM Products
						WHERE Id = @Id;
					END
				ELSE IF @Action = 'CartProduct'
					 BEGIN
						SELECT * FROM Carts
						WHERE Id = @Id;
					END
				
			END
GO

