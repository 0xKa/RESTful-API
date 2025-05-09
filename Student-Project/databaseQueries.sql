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
CREATE PROCEDURE GetStudentByID
    @ID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ID, Name, BirthDate, Grade, Email, IsActive
    FROM Student
    WHERE ID = @ID;
END;
GO

-- procedure 5
CREATE PROCEDURE AddNewStudent
    @Name NVARCHAR(100),
    @BirthDate DATE = NULL,
    @Grade DECIMAL(4,2) = NULL,
    @Email NVARCHAR(100) = NULL,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Student (Name, BirthDate, Grade, Email, IsActive)
    VALUES (@Name, @BirthDate, @Grade, @Email, @IsActive);

    SELECT SCOPE_IDENTITY() AS NewStudentID;
END;
GO

-- procedure 6
CREATE PROCEDURE UpdateStudent
    @ID INT,
    @Name NVARCHAR(100),
    @BirthDate DATE = NULL,
    @Grade DECIMAL(4,2) = NULL,
    @Email NVARCHAR(100) = NULL,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Student
    SET Name = @Name,
        BirthDate = @BirthDate,
        Grade = @Grade,
        Email = @Email,
        IsActive = @IsActive
    WHERE ID = @ID;
END;
GO

-- procedure 7
CREATE PROCEDURE DeleteStudent
    @ID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Student
    WHERE ID = @ID;
END;
GO

-- procedure 8
GO
