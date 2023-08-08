USE [PersonDistributionSystem]
GO

/****** Object:  StoredProcedure [dbo].[ProductSCRUD]    Script Date: 8/8/2023 10:24:28 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ProductSCRUD]
(
	@Action VARCHAR(10) = NULL,
	@Id INT =NULL,
    @Item NVARCHAR(20) = NULL,
    @Quantity INT = NULL,
    @Price DECIMAL(10, 2) = NULL
)
AS
BEGIN
	IF @Action	=	'Create'
	BEGIN
		INSERT INTO Products 
			(
				Item,
				Quantity,
				Price
			)
		 VALUES 
			(
				@Item,
				@Quantity,
				@Price
			);
	END
	ELSE IF @Action	=	'Delete'
	BEGIN
			DELETE FROM Products
			WHERE Id = @Id
	END
	ELSE IF @Action =	'Update'
	BEGIN
				UPDATE Products
				SET
				Item		=	@Item,
				Quantity	=	@Quantity,
				Price		=	@Price	
				WHERE Id = @Id
	END

END
GO

