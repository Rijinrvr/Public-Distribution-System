USE [PersonDistributionSystem]
GO

/****** Object:  StoredProcedure [dbo].[ViewProducts]    Script Date: 8/10/2023 10:32:23 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ViewProducts]
			(
				@Id					INT				= NULL,
				@Item				NVARCHAR(50)	= NULL,
				@Quantity			NVARCHAR(10)	= NULL,
				@Price				DECIMAL(18,0)	= NULL,
				@Action				NVARCHAR(10)	= NULL,
				@SearchKey			NVARCHAR(100)	= NULL,
				@ReturnMessage		NVARCHAR(10)	= NULL	OUT
				
			)
			AS
			BEGIN
			    IF @Action = 'View'
			    BEGIN
			        IF @SearchKey IS NULL OR @SearchKey = ''
			        BEGIN
			            SELECT * FROM Products;
			        END
			        ELSE
			        BEGIN
			            IF EXISTS
			            (
			                SELECT * FROM Products As Product
			                WHERE 
			                (
			                    Product.Item LIKE '%' + @SearchKey + '%' OR
			                    CAST(Product.Price AS NVARCHAR(50)) LIKE '%' + @SearchKey + '%'
			                )
			            )
			            BEGIN
			                SET @ReturnMessage = 'existing';
								SELECT * FROM Products AS Product
								WHERE 
									(
										Product.Item LIKE '%' + @SearchKey + '%' OR
										CAST(Product.Price AS NVARCHAR(50)) LIKE '%' + @SearchKey + '%'
									)
			            END
			            ELSE
			            BEGIN
			                SET @ReturnMessage = 'notExisting';
			            END
			        END
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

