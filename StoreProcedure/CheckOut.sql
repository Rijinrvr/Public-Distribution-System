USE [PersonDistributionSystem]
GO

/****** Object:  StoredProcedure [dbo].[CheckOut]    Script Date: 8/8/2023 10:23:20 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CheckOut]
		(
			@UserName NVARCHAR(20)		
		)
		AS
		BEGIN
			DELETE FROM Carts
			WHERE UserName = @UserName;
		END
GO

