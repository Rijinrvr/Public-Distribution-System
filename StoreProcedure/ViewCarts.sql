USE [PersonDistributionSystem]
GO

/****** Object:  StoredProcedure [dbo].[ViewCarts]    Script Date: 8/8/2023 10:24:45 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ViewCarts]
	(
		@Id INT =  NULL,
		@ProductId INT = NULL,
		@Item NVARCHAR(40) = NULL,
		@Quantity INT = NULL,
		@Price DECIMAL = NULL,
		@UserName NVARCHAR(40)
	)
	AS
		BEGIN
			SELECT * FROM Carts 
			WHERE  UserName = @UserName
		END
GO

