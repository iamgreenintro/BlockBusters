CREATE TABLE [dbo].[video_genres] (
    [id]       INT IDENTITY (1, 1) NOT NULL,
    [video_id] INT NOT NULL,
    [genre_id] INT NOT NULL,
    CONSTRAINT [PK_video_genre] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_video_genre_genres] FOREIGN KEY ([genre_id]) REFERENCES [dbo].[genres] ([id]),
    CONSTRAINT [FK_video_genre_videos] FOREIGN KEY ([video_id]) REFERENCES [dbo].[videos] ([id])
);

