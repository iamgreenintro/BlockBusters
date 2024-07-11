
SET NOCOUNT ON

SET IDENTITY_INSERT [video_genres] ON

DECLARE @mergeOutput TABLE ( [DMLAction] VARCHAR(6) );
MERGE INTO [video_genres] AS [Target]
USING (VALUES
  (1,1,1)
 ,(2,1,2)
) AS [Source] ([id],[video_id],[genre_id])
ON ([Target].[id] = [Source].[id])
WHEN MATCHED AND (
	NULLIF([Source].[video_id], [Target].[video_id]) IS NOT NULL OR NULLIF([Target].[video_id], [Source].[video_id]) IS NOT NULL OR 
	NULLIF([Source].[genre_id], [Target].[genre_id]) IS NOT NULL OR NULLIF([Target].[genre_id], [Source].[genre_id]) IS NOT NULL) THEN
 UPDATE SET
  [Target].[video_id] = [Source].[video_id], 
  [Target].[genre_id] = [Source].[genre_id]
WHEN NOT MATCHED BY TARGET THEN
 INSERT([id],[video_id],[genre_id])
 VALUES([Source].[id],[Source].[video_id],[Source].[genre_id])
WHEN NOT MATCHED BY SOURCE THEN 
 DELETE
OUTPUT $action INTO @mergeOutput;

DECLARE @mergeError int
 , @mergeCount int, @mergeCountIns int, @mergeCountUpd int, @mergeCountDel int
SELECT @mergeError = @@ERROR
SELECT @mergeCount = COUNT(1), @mergeCountIns = SUM(IIF([DMLAction] = 'INSERT', 1, 0)), @mergeCountUpd = SUM(IIF([DMLAction] = 'UPDATE', 1, 0)), @mergeCountDel = SUM (IIF([DMLAction] = 'DELETE', 1, 0)) FROM @mergeOutput
IF @mergeError != 0
 BEGIN
 PRINT 'ERROR OCCURRED IN MERGE FOR [video_genres]. Rows affected: ' + CAST(@mergeCount AS VARCHAR(100)); -- SQL should always return zero rows affected
 END
ELSE
 BEGIN
 PRINT '[video_genres] rows affected by MERGE: ' + CAST(COALESCE(@mergeCount,0) AS VARCHAR(100)) + ' (Inserted: ' + CAST(COALESCE(@mergeCountIns,0) AS VARCHAR(100)) + '; Updated: ' + CAST(COALESCE(@mergeCountUpd,0) AS VARCHAR(100)) + '; Deleted: ' + CAST(COALESCE(@mergeCountDel,0) AS VARCHAR(100)) + ')' ;
 END
GO



SET IDENTITY_INSERT [video_genres] OFF
SET NOCOUNT OFF
GO
