using DAL.Data;
using DAL.DTO;
using System.Diagnostics.Contracts;

namespace BLL
{
    public class Student
    {
        public enum enMode { AddNew = 0, Update = 1 };
        enMode _Mode = enMode.AddNew;

        //objects can have more than one DTO to trasfer the needed properties only.
        public StudentDTO DTO 
        {
            get {
                return (new StudentDTO()
                {
                    ID = this.ID,
                    Name = this.Name,
                    BirthDate = this.BirthDate,
                    Grade = this.Grade,
                    Email = this.Email,
                    IsActive = this.IsActive
                });
            }
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public decimal? Grade { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }

        public Student(StudentDTO studentDto, enMode mode = enMode.AddNew)
        {
            this.ID = studentDto.ID;
            this.Name = studentDto.Name;
            this.BirthDate = studentDto.BirthDate;
            this.Grade = studentDto.Grade;
            this.Email = studentDto.Email;
            this.IsActive = studentDto.IsActive;
            
            _Mode = mode;
        }


        public static List<StudentDTO> GetAllStudents() =>
            StudentData.GetAllStudents();

        public static List<StudentDTO> GetPassedStudents() => 
            StudentData.GetPassedFailedStudents(true);
        public static List<StudentDTO> GetFailedStudents() => 
            StudentData.GetPassedFailedStudents(false);

        public static int GetStudentCount() => 
            StudentData.GetStudentCount();

        public static double GetAverageGrade() => 
            StudentData.GetAverageGrade();

        public static Student? Find(int ID)
        {
            if (ID < 1) return null;

            StudentDTO? dto = StudentData.GetStudentByID(ID);

            if (dto == null)
                return null;
            else
                return new Student(dto, enMode.Update);
        }

    }
}
