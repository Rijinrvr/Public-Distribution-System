USE [PersonDistributionSystem]
GO

/****** Object:  StoredProcedure [dbo].[AddingProductsInCarts]    Script Date: 8/8/2023 10:22:52 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddingProductsInCarts]
		( 
			@Action VARCHAR(10) = NULL,
			@Id INT =  NULL,
			@ProductId INT = NULL,
			@Item NVARCHAR(40) = NULL,
			@Quantity INT = NULL,
			@Price DECIMAL = NULL,
			@UserName NVARCHAR(40) = NULL,
			@ReturnQuantityMessage NVARCHAR(10) = NULL OUT,
			@Result INT = NULL,
			@AddedQuantity INT = NULL
			
		)
			AS
	BEGIN
		SET NOCOUNT ON;
		DECLARE @AvailableQuantity INT;

		IF @Action = 'Insert'
		BEGIN
		    SELECT @AvailableQuantity = Quantity
		    FROM Products
		    WHERE Id = @ProductId;

		    IF @AvailableQuantity < @Quantity
		    BEGIN
		        SET @ReturnQuantityMessage = 'Greater';
		        RETURN; 
		    END

			  IF EXISTS 
				(
					SELECT 1
					FROM Carts
					WHERE ProductId = @ProductId AND UserName = @UserName
				)
		
			BEGIN
				UPDATE Carts
				SET Quantity = Quantity + @Quantity,
				Price = Price + @Price
				WHERE ProductId = @ProductId AND UserName = @UserName;

				SET @ReturnQuantityMessage = 'Added';

				SELECT @Result = Quantity
				FROM Products 
				WHERE Id = @ProductId;
				SET @AddedQuantity = @Result - @Quantity;

				UPDATE Products
				SET Quantity = @AddedQuantity
				WHERE Id = @ProductId;
			END
			ELSE
			BEGIN
				INSERT INTO Carts 
					(
						ProductId,
						Item,
						Quantity,
						Price,
						UserName
					)
				VALUES 
					(
						@ProductId,
						@Item,
						@Quantity,
						@Price,
						@UserName
					);

				SELECT @Result = Quantity
				FROM Products 
				WHERE Id = @ProductId;
				SET @AddedQuantity = @Result - @Quantity;

				UPDATE Products
				SET Quantity = @AddedQuantity
				WHERE Id = @ProductId;
			END
		END

		ELSE IF @Action = 'Delete'
		BEGIN
			SELECT @ProductId = ProductId,
			       @Result = Quantity
			FROM Carts
			WHERE Id = @Id;

			UPDATE Products
			SET Quantity = Quantity + @Result
			WHERE Id = @ProductId;

			DELETE FROM Carts
			WHERE Id = @Id;
		END
		ELSE IF @Action = 'CheckOut'
		BEGIN
			DELETE FROM Carts
			WHERE UserName = @UserName;
		END
	END;


 
GO

