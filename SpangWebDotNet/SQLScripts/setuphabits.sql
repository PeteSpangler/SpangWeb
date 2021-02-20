CREATE TABLE dbo.Habit(
  HabitId int IDENTITY(1,1) NOT NULL,
  DailyHabit nvarchar(100) NOT NULL,
  Intention nvarchar(max) NOT NULL,
  UserId nvarchar(150) NOT NULL,
  UserName nvarchar(150) NOT NULL,
  Created datetime2(7) NOT NULL,
 CONSTRAINT PK_Habit PRIMARY KEY CLUSTERED
(
  HabitId ASC
)
)
GO

CREATE TABLE dbo.Response(
  ResponseId int IDENTITY(1,1) NOT NULL,
  HabitId int NOT NULL,
  Feedback nvarchar(max) NOT NULL,
  UserId nvarchar(150) NOT NULL,
  UserName nvarchar(150) NOT NULL,
  Created datetime2(7) NOT NULL,
 CONSTRAINT PK_Response PRIMARY KEY CLUSTERED
(
  ResponseId ASC
)
)
GO

ALTER TABLE dbo.Response  WITH CHECK ADD  CONSTRAINT FK_Response_Habit FOREIGN KEY(HabitId)
REFERENCES dbo.Habit (HabitId)
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE dbo.Response CHECK CONSTRAINT FK_Response_Habit
GO


SET IDENTITY_INSERT dbo.Habit ON
GO
INSERT INTO dbo.Habit(HabitId, DailyHabit, Intention, UserId, UserName, Created)
VALUES(1, 'Putting my dishes directly into the dishwasher or the sink after eating.',
    'Doing so means that I will have cleared the dishes away and they will be ready to either be cleaned or run through the dishwasher',
    '1',
    'bob.test@test.com',
    '2021-01-18 14:32')

INSERT INTO dbo.Habit(HabitId, DailyHabit, Intention, UserId, UserName, Created)
VALUES(2, 'Taking at least 10 minutes each morning to meditate.',
    'If I can meditate each morning, I will be calmer and more collected to confront my day!',
    '2',
    'jane.test@test.com',
    '2021-01-18 14:48')
GO
SET IDENTITY_INSERT dbo.Habit OFF
GO

SET IDENTITY_INSERT dbo.Response ON
GO
INSERT INTO dbo.Response(ResponseId, HabitId, Feedback, UserId, UserName, Created)
VALUES(1, 1, 'This seems like a decent habit to have and could easily be stacked with another.', '2', 'jane.test@test.com', '2021-01-18 14:40')

INSERT INTO dbo.Response(ResponseId, HabitId, Feedback, UserId, UserName, Created)
VALUES(2, 1, 'This is kind of vague for a habit you intend to implement. It could be the second or third of a habit stack, but declared on its own it falls short.', '3', 'fred.test@test.com', '2021-01-18 16:18')
GO
SET IDENTITY_INSERT dbo.Response OFF
GO