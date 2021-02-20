CREATE PROC dbo.Response_Delete
	(
	@ResponseId int
)
AS
BEGIN
	SET NOCOUNT ON

	DELETE
	FROM dbo.Response
	WHERE ResponseId = @ResponseId
END
GO

CREATE PROC dbo.Response_Get_ByHabitId
	(
	@HabitId int
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT ResponseId, HabitId, Feedback, Username, Created
	FROM dbo.Response
	WHERE HabitId = @HabitId
END
GO

CREATE PROC dbo.Response_Post
	(
	@HabitId int,
	@Feedback nvarchar(max),
	@UserId nvarchar(150),
	@UserName nvarchar(150),
	@Created datetime2
)
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO dbo.Response
		(HabitId, Feedback, UserId, UserName, Created)
	SELECT @HabitId, @Feedback, @UserId, @UserName, @Created

	SELECT ResponseId, Feedback, UserName, UserId, Created
	FROM dbo.Response
	WHERE ResponseId = SCOPE_IDENTITY()
END
GO

CREATE PROC dbo.Response_Put
	(
	@ResponseId int,
	@Feedback nvarchar(max)
)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE dbo.Response
	SET Feedback = @Feedback
	WHERE ResponseId = @ResponseId

	SELECT a.ResponseId, a.HabitId, a.Feedback, u.UserName, a.Created
	FROM dbo.Response a
		LEFT JOIN AspNetUsers u ON a.UserId = u.Id
	WHERE ResponseId = @ResponseId
END
GO


CREATE PROC dbo.Habit_AddForLoadTest
AS
BEGIN
	DECLARE @i int = 1

	WHILE @i < 10000
	BEGIN
		INSERT INTO dbo.Habit
			(DailyHabit, Intention, UserId, UserName, Created)
		VALUES('Question ' + CAST(@i AS nvarchar(5)), 'Intention ' + CAST(@i AS nvarchar(5)), 'User1', 'User1', GETUTCDATE())
		SET @i = @i + 1
	END
END
GO

CREATE PROC dbo.Habit_Delete
	(
	@HabitId int
)
AS
BEGIN
	SET NOCOUNT ON

	DELETE
	FROM dbo.Habit
	WHERE HabitId = @HabitId
END
GO

CREATE PROC dbo.Habit_Exists
	(
	@HabitId int
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT CASE WHEN EXISTS (SELECT HabitId
		FROM dbo.Habit
		WHERE HabitId = @HabitId)
        THEN CAST (1 AS BIT)
        ELSE CAST (0 AS BIT) END AS Result

END
GO

CREATE PROC dbo.Habit_GetMany
AS
BEGIN
	SET NOCOUNT ON

	SELECT HabitId, DailyHabit, Intention, UserId, UserName, Created
	FROM dbo.Habit
END
GO

CREATE PROC dbo.Habit_GetMany_BySearch
	(
	@Search nvarchar(100)
)
AS
BEGIN
	SET NOCOUNT ON

		SELECT HabitId, DailyHabit, Intention, UserId, UserName, Created
		FROM dbo.Habit
		WHERE DailyHabit LIKE '%' + @Search + '%'

	UNION

		SELECT HabitId, DailyHabit, Intention, UserId, UserName, Created
		FROM dbo.Habit
		WHERE Intention LIKE '%' + @Search + '%'
END
GO

CREATE PROC dbo.Habit_GetMany_BySearch_WithPaging
	(
	@Search nvarchar(100),
	@PageNumber int,
	@PageSize int
)
AS
BEGIN
	SELECT HabitId, DailyHabit, Intention, UserId, UserName, Created
	FROM
		(	SELECT HabitId, DailyHabit, Intention, UserId, UserName, Created
			FROM dbo.Habit
			WHERE DailyHabit LIKE '%' + @Search + '%'

		UNION

			SELECT HabitId, DailyHabit, Intention, UserId, UserName, Created
			FROM dbo.Habit
			WHERE Intention LIKE '%' + @Search + '%') Sub
	ORDER BY HabitId
	OFFSET @PageSize * (@PageNumber - 1) ROWS
    FETCH NEXT @PageSize ROWS ONLY
END
GO

CREATE PROC dbo.Habit_GetMany_WithResponses
AS
BEGIN
	SET NOCOUNT ON

	SELECT q.HabitId, q.DailyHabit, q.Intention, q.UserName, q.Created,
		a.HabitId, a.ResponseId, a.Feedback, a.Username, a.Created
	FROM dbo.Habit q
		LEFT JOIN dbo.Response a ON q.HabitId = a.HabitId
END
GO

CREATE PROC dbo.Habit_GetSingle
	(
	@HabitId int
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT HabitId, DailyHabit, Intention, UserId, Username, Created
	FROM dbo.Habit
	WHERE HabitId = @HabitId
END
GO

CREATE PROC dbo.Habit_GetNoResponses
AS
BEGIN
	SET NOCOUNT ON

	SELECT HabitId, DailyHabit, Intention, UserId, UserName, Created
	FROM dbo.Habit q
	WHERE NOT EXISTS (SELECT *
	FROM dbo.Response a
	WHERE a.HabitId = q.HabitId)
END
GO

CREATE PROC dbo.Habit_Post
	(
	@DailyHabit nvarchar(100),
	@Intention nvarchar(max),
	@UserId nvarchar(150),
	@UserName nvarchar(150),
	@Created datetime2
)
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO dbo.Habit
		(DailyHabit, Intention, UserId, UserName, Created)
	VALUES(@DailyHabit, @Intention, @UserId, @UserName, @Created)

	SELECT SCOPE_IDENTITY() AS HabitId
END
GO

CREATE PROC dbo.Habit_Put
	(
	@HabitId int,
	@DailyHabit nvarchar(100),
	@Intention nvarchar(max)
)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE dbo.Habit
	SET DailyHabit = @DailyHabit, Intention = @Intention
	WHERE HabitId = @HabitId
END
GO

CREATE PROC dbo.Response_Get_ByResponseId
	(
	@ResponseId int
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT ResponseId, Feedback, Username, Created
	FROM dbo.Response
	WHERE ResponseId = @ResponseId
END
GO