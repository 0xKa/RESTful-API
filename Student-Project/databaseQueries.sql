--student table
CREATE TABLE Student (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    BirthDate DATE,
    Grade DECIMAL(4,2),
    Email NVARCHAR(100),
    IsActive BIT DEFAULT 1
);

-- procedure 1
CREATE PROCEDURE GetAllStudents
AS
BEGIN
    SET NOCOUNT ON; --prevent rowsAffected message

    SELECT ID, Name, BirthDate, Grade, Email, IsActive
    FROM Student;
END;
