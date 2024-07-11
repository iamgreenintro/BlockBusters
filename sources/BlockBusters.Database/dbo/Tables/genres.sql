CREATE TABLE [dbo].[genres] (
    [id]    INT           IDENTITY (1, 1) NOT NULL,
    [genre] NVARCHAR (50) NOT NULL,
    CONSTRAINT [UQ_GenreName] UNIQUE ([genre]),
    CONSTRAINT [PK_genres] PRIMARY KEY CLUSTERED ([id] ASC)
);

