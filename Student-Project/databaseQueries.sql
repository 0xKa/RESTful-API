use StudentDB;
GO

--student table
CREATE TABLE Student (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    BirthDate DATE,
    Grade DECIMAL(4,2),
    Email NVARCHAR(100),
    IsActive BIT DEFAULT 1
);
GO

-- procedure 1
CREATE PROCEDURE GetAllStudents
AS
BEGIN
    SET NOCOUNT ON; --prevent rowsAffected message

    SELECT ID, Name, BirthDate, Grade, Email, IsActive
    FROM Student;
END;
GO

-- procedure 2
CREATE PROCEDURE GetPassedFailedStudents
    @IsPassed BIT -- 1 for passed students, 0 for failed
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ID, Name, BirthDate, Grade, Email, IsActive
    FROM Student
    WHERE 
        Grade IS NOT NULL AND
        (
            (@IsPassed = 1 AND Grade >= 50.0) OR
            (@IsPassed = 0 AND Grade < 50.0)
        );
END;
GO


-- procedure 3
CREATE PROCEDURE GetAverageGrade
AS
BEGIN
    SET NOCOUNT ON;

    SELECT AVG(Grade) AS AverageGrade
    FROM Student
    WHERE Grade IS NOT NULL;
END;
GO

-- procedure 4
CREATE PROCEDURE GetStudentCount
AS
BEGIN
    SET NOCOUNT ON;

    SELECT AVG(Grade) AS AverageGrade
    FROM Student
    WHERE Grade IS NOT NULL;
END;
GO
-- procedure 5
GO
-- procedure 6
GO
-- procedure 7
GO
-- procedure 8
GO
