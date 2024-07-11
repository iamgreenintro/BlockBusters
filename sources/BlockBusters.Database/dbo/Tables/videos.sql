CREATE TABLE [dbo].[videos] (
    [id]          INT            IDENTITY (1, 1) NOT NULL,
    [title]       NVARCHAR (50)  NOT NULL,
    [duration]    INT            NOT NULL,
    [image_url]   NVARCHAR (50)  NOT NULL,
    [description] NVARCHAR (500) NOT NULL,
    CONSTRAINT [UQ_VideoTitle] UNIQUE ([title]),
    CONSTRAINT [PK_videos] PRIMARY KEY CLUSTERED ([id] ASC)
);



