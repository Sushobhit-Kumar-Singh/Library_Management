CREATE DATABASE Library
Go

USE Library
Go

CREATE TABLE [dbo].[Book](
	[ISBN] [nvarchar](12) NOT NULL,
	[Title] [nvarchar](100) NULL,
	[Author] [nvarchar](80) NULL,
	[Genre] [nvarchar](50) NULL,
	[PublicationYear] [smallint] NULL,
	[CopiesAvailable] [int] NULL,
	[TotalCopies] [int] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ISBN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Book]  WITH CHECK ADD  CONSTRAINT [F_KEY] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Member] ([MemberId])
GO

ALTER TABLE [dbo].[Book] CHECK CONSTRAINT [F_KEY]
GO

ALTER TABLE [dbo].[Book]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Member] ([MemberId])
GO

ALTER TABLE [dbo].[Book]  WITH CHECK ADD CHECK  (([CopiesAvailable]>=(0)))
GO

ALTER TABLE [dbo].[Book]  WITH CHECK ADD CHECK  (([TotalCopies]>=(0)))
GO


CREATE TABLE [dbo].[Member](
	[MemberId] [int] IDENTITY(1,1) NOT NULL,
	[RoleType] [nvarchar](50) NULL,
	[Name] [nvarchar](80) NULL,
	[Address] [nvarchar](200) NULL,
	[PhoneNumber] [nvarchar](10) NULL,
	[EMail] [nvarchar](80) NULL,
	[Password] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[IsActive] [bit] NULL,
	[PasswordHash] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Member]  WITH CHECK ADD CHECK  (([Email] like '%_@__%.__%'))
GO

ALTER TABLE [dbo].[Member]  WITH CHECK ADD CHECK  (([PhoneNumber] like '[0-9]%[0-9]' AND len([PhoneNumber])=(10)))
GO

ALTER TABLE [dbo].[Member]  WITH CHECK ADD CHECK  (([RoleType]='Borrower' OR [RoleType]='Librarian'))
GO


CREATE TABLE [dbo].[Transaction](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[BookISBN] [nvarchar](12) NULL,
	[MemberId] [int] NULL,
	[IssueDate] [datetime] NULL,
	[DueDate] [datetime] NULL,
	[ReturnDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD FOREIGN KEY([BookISBN])
REFERENCES [dbo].[Book] ([ISBN])
GO

ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD FOREIGN KEY([MemberId])
REFERENCES [dbo].[Member] ([MemberId])
GO

ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Member] ([MemberId])
GO

ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [Fo_KEY] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Member] ([MemberId])
GO

ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [Fo_KEY]
GO





