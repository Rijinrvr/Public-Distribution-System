USE [PersonDistributionSystem]
GO

/****** Object:  StoredProcedure [dbo].[FromCartTable]    Script Date: 8/8/2023 10:23:43 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[FromCartTable]
					(
						@Id INT =  NULL,
						@ProductId INT = NULL,
						@Item NVARCHAR(40) = NULL,
						@Quantity INT = NULL,
						@Price DECIMAL = NULL,
						@UserName NVARCHAR(40) = NULL
					)
					AS
					BEGIN
					 SELECT * FROM Products
					 WHERE Id = @Id
					END
GO

