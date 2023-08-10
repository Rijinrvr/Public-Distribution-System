USE [PersonDistributionSystem]
GO

/****** Object:  StoredProcedure [dbo].[LoginDetails]    Script Date: 8/10/2023 10:33:03 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[LoginDetails]
	@Username				NVARCHAR(100),
    @Password				NVARCHAR(100),
    @Action					NVARCHAR(10)	=	NULL,
    @Isadmin				BIT				=	0,
    @IsAuthenticOutput		BIT				=	NULL OUT,
    @IsAdminOutput			NVARCHAR(10)	=	NULL OUT,
    @ReturnMessage			BIT				=	NULL OUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'Insert'
		BEGIN
			SET @ReturnMessage = 0;
			IF EXISTS 
				(
					 SELECT 1 FROM [Login] 
					 WHERE Username = @Username
				)
			BEGIN
				SET @ReturnMessage = 1;
			END

			ELSE
				BEGIN
				 SET @ReturnMessage =0;
				    DECLARE @EncryptedPassword VARBINARY(64);
					SET @EncryptedPassword = HASHBYTES('SHA2_256', @Password);

					INSERT INTO [Login] 
						(
							[Username], 
							[Password], 
							[Isadmin]
						)
					VALUES 
						(
							@Username, 
							@EncryptedPassword, 
							@Isadmin
						);
				END;
			END;

			ELSE IF @Action = 'Check'
				BEGIN
					SET @IsAuthenticOutput = 0;
						IF EXISTS 
							(	
							    SELECT [Username]
							    FROM [Login]
							    WHERE [Username] = @Username AND 
								[Password] = HASHBYTES('SHA2_256', @Password)
							)
						BEGIN
							SET @IsAuthenticOutput = 1;
								IF EXISTS
									(
									    SELECT [Username]
									    FROM [Login]
									    WHERE [Username] = @Username AND [Isadmin] = 1 
									)
								BEGIN
							SET @IsAdminOutput ='Admin';
						END
					ELSE
						BEGIN
							SET @IsAdminOutput = 'NotAdmin';
						END
				END;
			END;
		END;
GO

