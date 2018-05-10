Use [USTRPUMSJUL17BATCH1Group2DB]

IF OBJECT_ID('Status')  IS NOT NULL
DROP TABLE [Status]
GO

IF OBJECT_ID('User')  IS NOT NULL
DROP TABLE [User]
GO

IF OBJECT_ID('Bank')  IS NOT NULL
DROP TABLE Bank
GO

IF OBJECT_ID('UserCard')  IS NOT NULL
DROP TABLE UserCard
GO

IF OBJECT_ID('Merchant')  IS NOT NULL
DROP TABLE Merchant
GO

IF OBJECT_ID('MerchantServiceType')  IS NOT NULL
DROP TABLE MerchantServiceType
GO

IF OBJECT_ID('MerchantServiceMapping')  IS NOT NULL
DROP TABLE MerchantServiceMapping
GO

IF OBJECT_ID('PaymentType')  IS NOT NULL
DROP TABLE PaymentType
GO

IF OBJECT_ID('UserTransaction')  IS NOT NULL
DROP TABLE UserTransaction
GO

IF OBJECT_ID('MerchantTransactions')  IS NOT NULL
DROP TABLE MerchantTransactions
GO


IF OBJECT_ID('OTPPurpose')  IS NOT NULL
DROP TABLE MerchantTransactions
GO

IF OBJECT_ID('OTP')  IS NOT NULL
DROP TABLE OTP
GO

CREATE TABLE [Status]
(
	[StatusId] TINYINT CONSTRAINT pk_StatusId PRIMARY KEY NOT NULL IDENTITY,
	[StatusValue] VARCHAR(20) NOT NULL
)
GO

CREATE TABLE [User]
(
	[UserId] INT CONSTRAINT pk_UserId PRIMARY KEY NOT NULL IDENTITY(1000, 1),
	[EmailId] VARCHAR(255) CONSTRAINT uq_EmailId UNIQUE NOT NULL,
	[MobileNumber] VARCHAR(10) CONSTRAINT uq_MobileNumber UNIQUE NOT NULL,
	[Name] VARCHAR(100) NOT NULL,
	[Password] VARCHAR(300) NOT NULL,
	[StatusId] TINYINT FOREIGN KEY REFERENCES [Status](StatusId) NOT NULL,
	[CreatedTimestamp] DATETIME2(7) DEFAULT GETDATE() NOT NULL,
	[ModifiedTimestamp] DATETIME2(7)
)
GO

CREATE TABLE [Bank]
(
	[BankId] TINYINT CONSTRAINT pk_BankId PRIMARY KEY NOT NULL IDENTITY(100, 1),
	[BankName] VARCHAR(50) NOT NULL
)
GO

CREATE TABLE UserCard
(
	[UserCardMappingId] INT CONSTRAINT pk_UserCardMappingId PRIMARY KEY IDENTITY(1000, 1),
	[EmailId] VARCHAR(255) NOT NULL CONSTRAINT fk_UserCard_EmailId REFERENCES [User](EmailId),
	[CardNumber] VARCHAR(16) NOT NULL,
	[BankId] TINYINT NOT NULL CONSTRAINT fk_UserCard_BankId REFERENCES [Bank](BankId),
	[ExpiryDate] SMALLDATETIME NOT NULL,
	[StatusId] TINYINT NOT NULL,
	[CreatedTimestamp] DATETIME2(7) DEFAULT GETDATE() NOT NULL, 
	[ModifiedTimestamp] DATETIME2(7),
	CONSTRAINT unq_Email_Card UNIQUE(EmailId, CardNumber)
)
GO

CREATE TABLE Merchant
(
	[MerchantId] SMALLINT CONSTRAINT pk_MerchantId PRIMARY KEY IDENTITY(100, 1),
	[EmailId] VARCHAR(255) UNIQUE NOT NULL,
	[Name] VARCHAR(100) NOT NULL,
	[Password] VARBINARY(300) NOT NULL,
	[MobileNumber] VARCHAR(10),
	[IsActive] BIT DEFAULT 1 NOT NULL,
	[CreatedTimestamp] DATETIME2(7) DEFAULT GETDATE() NOT NULL,
	[ModifiedTimestamp] DATETIME2(7)
)
GO

CREATE TABLE MerchantServiceType
(
	[ServiceId] TINYINT CONSTRAINT pk_ServiceId PRIMARY KEY IDENTITY,
	[ServiceType] VARCHAR(50) UNIQUE NOT NULL
)
GO

CREATE TABLE MerchantServiceMapping
(
	[MerchantServiceMappingId] SMALLINT CONSTRAINT pk_MerchantServiceMappingId PRIMARY KEY NOT NULL IDENTITY,
	[EmailId] VARCHAR(255) CONSTRAINT fk_MerchantSM_EmailId REFERENCES Merchant(EmailId) NOT NULL,
	[ServiceId] TINYINT CONSTRAINT fk_ServiceId REFERENCES MerchantServiceType(ServiceId) NOT NULL,
	[DiscountPercent] DECIMAL(5,2) DEFAULT 0 NOT NULL,
	[IsActive] BIT DEFAULT 1 NOT NULL,
	[CreatedTimestamp] DATETIME2(7) DEFAULT GETDATE(),
	[ModifiedTimestamp] DATETIME2(7)
	CONSTRAINT unq_EmailId_ServiceId UNIQUE (EmailId, ServiceId)
)
GO

CREATE TABLE PaymentType
(
	[PaymentTypeId] TINYINT CONSTRAINT pk_PaymentTypeId PRIMARY KEY NOT NULL IDENTITY,
	[PaymentFrom] CHAR(1) CONSTRAINT chk_PaymentFrom CHECK(PaymentFrom IN ('B','M','W')) NOT NULL,
	[PaymentTo] CHAR(1) CONSTRAINT chk_PaymentTo CHECK(PaymentTo IN ('B','M','W')) NOT NULL,
	[PaymentType] BIT NOT NULL
)
GO

CREATE TABLE UserTransaction
(
	[UserTransactionId] BIGINT NOT NULL IDENTITY,
	[EmailId] VARCHAR(255) CONSTRAINT fk_UserTransaction_EmailId References [User](EmailId) NOT NULL,
	[Amount] MONEY CONSTRAINT chk_UserT_Amount CHECK(Amount!=0) NOT NULL,
	[TransactionDateTime] DATETIME DEFAULT GETDATE() NOT NULL,
	[PaymentTypeId] TINYINT CONSTRAINT fk_UserT_PaymentTypeId References [PaymentType](PaymentTypeId) NOT NULL,
	[Remarks] VARCHAR(50),
	[Info] VARCHAR(100) NOT NULL,
	[StatusId] TINYINT CONSTRAINT fk_UserT_StatusId References [Status](StatusId) NOT NULL,
	[PointsEarned] SMALLINT CONSTRAINT chk_PointsEarned CHECK(PointsEarned>=0) DEFAULT 0 NOT NULL,
	[IsRedeemed] BIT DEFAULT 0 NOT NULL
)
GO

CREATE TABLE MerchantTransactions
(
	[TransactionId] BIGINT CONSTRAINT pk_TransactionId PRIMARY KEY NOT NULL IDENTITY,
	[EmailId] VARCHAR(255) CONSTRAINT fk_MerchantT_EmailId References [Merchant](EmailId) NOT NULL,
	[Amount] MONEY CONSTRAINT chk_MerchantT_Amount CHECK(Amount>0) NOT NULL,
	[TransactionDateTime] DATETIME DEFAULT GETDATE() NOT NULL,
	[PaymentTypeId] TINYINT CONSTRAINT fk_MerchantT_PaymentTypeId References [PaymentType](PaymentTypeId) NOT NULL,
	[Remarks] VARCHAR(50),
	[Info] VARCHAR(100) NOT NULL,
	[StatusId] TINYINT CONSTRAINT fk_MerchantT_StatusId References [Status](StatusId) NOT NULL
)
GO

CREATE TABLE [OTPPurpose]
(
	[OTPPurposeId] TINYINT CONSTRAINT pk_OTPPurposeId PRIMARY KEY NOT NULL IDENTITY,
	[OTPPurpose] VARCHAR(30) CONSTRAINT uq_OTPPurpose UNIQUE NOT NULL
)
GO

CREATE TABLE [OTP]
(
	[OTPId] INT CONSTRAINT pk_OTPId PRIMARY KEY NOT NULL IDENTITY,
	[OTP] VARCHAR(6) DEFAULT DATEADD(mi, 5, GETDATE()) NOT NULL,
	[ReferenceId] VARCHAR(255) NOT NULL,
	[ExpiryDateTime] DATETIME NOT NULL,
	[OTPPurposeId] TINYINT CONSTRAINT fk_OTPPurposeId REFERENCES OTPPurpose(OTPPurposeId) NOT NULL,
	[IsValid] BIT DEFAULT 1 NOT NULL,
	CONSTRAINT uq_OTP_ExpiryDateTime UNIQUE ([OTP], [ReferenceId], [ExpiryDateTime])
)
GO