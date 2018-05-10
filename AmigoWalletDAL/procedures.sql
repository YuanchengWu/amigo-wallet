--TODO: convert points to wallet money
--•	Customer should be able to redeem points only when the unredeemed reward points are above 100
--•	Customer's balance should be increased when the rewards points are redeemed
--•	Partial redemption of points should not be allowed
--•	Only 10 % of the redeemed points should get credited as amount to the wallet
--•	On successful redemption, a success message along with the updated balance should be displayed
--•	In case of any error or exception, appropriate error message should be displayed

CREATE PROCEDURE [dbo].[usp_RedeemRewardPoints]
(
	@EmailId VARCHAR(255)
)
AS
BEGIN
	DECLARE @PointBalance INT, @Amount MONEY
	SET @PointBalance = (SELECT SUM(PointsEarned) FROM UserTransaction WHERE @EmailId = EmailId)
	BEGIN TRY
		IF (@PointBalance <= 100)
			RETURN -1
		SET @Amount = @PointBalance * 0.1
		INSERT INTO PaymentType VALUES ('W', 'W', 1)
		INSERT INTO UserTransaction VALUES (@EmailId, @Amount, GETDATE(), IDENT_CURRENT('PaymentType'), NULL, 'OK', 1, 0, 1)
		RETURN 1
	END TRY
	BEGIN CATCH
		RETURN -99
	END CATCH
END
GO

CREATE PROCEDURE [dbo].[usp_GetFullBalance]
(
	@EmailId VARCHAR(255)
)
AS
BEGIN
	RETURN (SELECT SUM(Amount) FROM UserTransaction WHERE EmailId = @EmailId)
END
GO


CREATE PROCEDURE [dbo].[usp_RegisterUser]
(
	@EmailId VARCHAR(255),
	@Name VARCHAR(100),
	@Password VARCHAR(128),
	@MobileNumber VARCHAR(10)
)
AS
BEGIN
	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [User] WHERE  EmailId=@EmailId)
		BEGIN
			IF NOT EXISTS (SELECT * FROM [User] WHERE MobileNumber=@MobileNumber)
			BEGIN
				INSERT INTO [User]([EmailId], [MobileNumber], [Password], [StatusId], [CreatedTimestamp],[ModifiedTimestamp])
				VALUES (@EmailId, @MobileNumber, @Password, 1, GETDATE(), NULL)
				RETURN 1
			END
			ELSE
			BEGIN
				RETURN -2 -- MOBILE NUMBER ALREADY EXISTS
			END
		END
		ELSE
		BEGIN
			RETURN -3 -- EMAIL ALREADY EXISTS
		END
	END TRY
	BEGIN CATCH
		RETURN -99 -- SOME OTHER ERROR
	END CATCH
END
GO

CREATE FUNCTION [dbo].[ufn_validateCredentials]
(
	@EmailId VARCHAR(255),
	@Password VARCHAR(128)
)
RETURNS BIT 
AS
BEGIN
	BEGIN TRY
		IF EXISTS (SELECT UserId FROM [User] WHERE EmailId = @EmailId AND Password = @Password)
		BEGIN
			RETURN 1
		END
		IF EXISTS (SELECT UserId FROM [Merchant] WHERE EmailId = @EmailId AND Password = @Password)
		BEGIN
			RETURN 1
		END
	END TRY
	BEGIN CATCH
		RETURN 0
	END CATCH
	RETURN 0
END
GO

CREATE PROCEDURE [dbo].[usp_changePassword]
(
	@EmailId VARCHAR(255),
	@OldPassword VARCHAR(128),
	@NewPassword VARCHAR(128)
)
AS
BEGIN
	BEGIN TRY
		IF EXISTS (SELECT EmailId FROM [User] WHERE EmailId = @EmailId AND Password = @OldPassword)
		BEGIN
			UPDATE [User] SET [Password] = @NewPassword 
			WHERE [EmailId] = @EmailId
			RETURN 1
		END
		IF EXISTS (SELECT EmailId FROM [Merchant] WHERE EmailId = @EmailId AND Password = @OldPassword)
		BEGIN
			UPDATE [Merchant] SET [Password] = @NewPassword 
			WHERE [EmailId] = @EmailId
			RETURN 1
		END
	END TRY
	BEGIN CATCH
		RETURN 0;
	END CATCH
	RETURN 0;
END
GO

CREATE PROCEDURE [dbo].[usp_AddNewCard]
(
	@EmailId VARCHAR(255),
	@CardNumber VARCHAR(16),
	@BankId TINYINT,
	@ExpiryDate SMALLDATETIME,
	@StatusId TINYINT 
)
AS
BEGIN
	BEGIN TRY
		IF EXISTS (SELECT * FROM UserCard WHERE EmailId=@EmailId AND CardNumber=@CardNumber)
		BEGIN
			RETURN 0;
		END
		ELSE
		BEGIN
			INSERT INTO UserCard(EmailId, CardNumber, BankId, ExpiryDate, StatusId)
			VALUES (@EmailId, @CardNumber, @BankId, @ExpiryDate, 1)
			RETURN 1
		END
	END TRY
	BEGIN CATCH
		RETURN 0
	END CATCH
	RETURN 0
END
GO

CREATE PROCEDURE [dbo].[usp_AddMoneyWithCard]
(
	@EmailId VARCHAR(255),
	@CardId INT,
	@Amount DECIMAL,
	@Remarks VARCHAR(255)
)
AS
BEGIN
	BEGIN TRY
		-- new payment type row
		DECLARE @type INT;
		DECLARE @id INT
		IF (@Amount > 0)
			SET @type = 1
		ELSE
			SET @type = 0
		INSERT INTO PaymentType(PaymentFrom, PaymentTo,PaymentType)
		VALUES ('B','W',@type)
		SET @id = IDENT_CURRENT('PaymentType')
		-- new transaction row
		INSERT INTO UserTransaction(EmailId,Amount,PaymentTypeId,Remarks,Info,StatusId)
		VALUES (@EmailId, @Amount, @id, @Remarks, 'OK', 1)
		RETURN 1
	END TRY
	BEGIN CATCH
		RETURN 0;
	END CATCH
END
GO

CREATE PROCEDURE [dbo].[usp_PayBills]
(
	@UserEmailId VARCHAR(255),
	@ServiceType VARCHAR(50),
	@MerchantEmailId VARCHAR(255),
	@Amount MONEY
)
AS
BEGIN
	DECLARE @PointsEarned INT, @Balance MONEY
	BEGIN TRY
		SET @Balance = (SELECT SUM(Amount) FROM UserTransaction WHERE EmailId = @UserEmailId)
		IF (@Balance < @Amount)
			RETURN -1
		-- check if merchant exists for this service type
		IF NOT EXISTS (SELECT * FROM MerchantServiceMapping MSM JOIN MerchantServiceType MST ON MSM.ServiceId = MST.ServiceId WHERE @MerchantEmailId = MSM.EmailId)
			RETURN -2
		IF (@Amount >= 10)
			SET @PointsEarned = CAST(CEILING(@Amount) AS INT)
		ELSE
			SET @PointsEarned = 0
		INSERT INTO PaymentType VALUES('W', 'M', 1)
		INSERT INTO UserTransaction VALUES(@UserEmailId, -@Amount, GETDATE(), IDENT_CURRENT('PaymentType'), NULL, 'PAID', 1, @PointsEarned, 0)
		INSERT INTO MerchantTransaction VALUES(@MerchantEmailId, @Amount, GETDATE(), IDENT_CURRENT('PaymentType'), NULL, 'RECEIVED', 1)
		RETURN 1
	END TRY
	BEGIN CATCH
		RETURN -99
	END CATCH
END
GO

CREATE PROCEDURE [dbo].[usp_WalletToWallet]
(
	@FromEmailId VARCHAR(255),
	@ToEmailId VARCHAR(255),
	@Amount MONEY
)
AS
BEGIN
	DECLARE @Balance MONEY
	BEGIN TRY
		SET @Balance = (SELECT SUM(Amount) FROM UserTransaction WHERE EmailId = @FromEmailId)
		IF (@Amount <= 0)
			RETURN -1
		IF (@Balance < @Amount)
			RETURN -2
		INSERT INTO PaymentType VALUES('W', 'W', 1)
		INSERT INTO UserTransaction VALUES(@FromEmailId, -@Amount, GETDATE(), IDENT_CURRENT('PaymentType'), NULL, 'PAID', 1, 0, 0)
		INSERT INTO UserTransaction VALUES(@ToEmailId, @Amount, GETDATE(), IDENT_CURRENT('PaymentType'), NULL, 'PAID', 1, 0, 0)
		RETURN 1
	END TRY
	BEGIN CATCH
		RETURN -99
	END CATCH
END
GO

CREATE PROCEDURE [dbo].[usp_WalletToBank]
(
	@EmailId VARCHAR(255),
	@Amount MONEY,
	@Info VARCHAR(MAX),
	@Remarks VARCHAR(MAX)
)
AS 
BEGIN
	DECLARE @Balance MONEY
	BEGIN TRY
		SET @Balance = (SELECT SUM(Amount) FROM UserTransaction WHERE EmailId = @EmailId)
		IF (@Amount <= 0)
			RETURN -1
		IF (@Balance < @Amount)
			RETURN -2
		INSERT INTO PaymentType VALUES('W', 'B', 1)
		INSERT INTO UserTransaction VALUES(@EmailId, -@Amount, GETDATE(), IDENT_CURRENT('PaymentType'), @Remarks, @Info, 1, 0, 0)
		RETURN 1
	END TRY
	BEGIN CATCH
		RETURN -99
	END CATCH
END
GO