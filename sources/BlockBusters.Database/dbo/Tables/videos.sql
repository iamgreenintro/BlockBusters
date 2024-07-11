CREATE TABLE [dbo].[videos] (
    [id]          INT            IDENTITY (1, 1) NOT NULL,
    [title]       NVARCHAR (50)  NOT NULL,
    [duration]    INT            NOT NULL,
    [image_url]   NVARCHAR (50)  NOT NULL,
    [description] NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_videos] PRIMARY KEY CLUSTERED ([id] ASC)
);



