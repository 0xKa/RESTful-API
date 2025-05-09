using DAL.Data;
using DAL.DTO;
using System.Diagnostics.Contracts;

namespace BLL
{
    public class Student
    {
        public enum enMode { AddNew = 0, Update = 1 };
        enMode _Mode = enMode.AddNew;

        public StudentDTO SDTO 
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

        public required int ID { get; set; }
        public required string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public decimal? Grade { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }

        public Student(StudentDTO SDTO, enMode mode = enMode.AddNew)
        {
            this.ID = SDTO.ID;
            this.Name = SDTO.Name;
            this.BirthDate = SDTO.BirthDate;
            this.Grade = SDTO.Grade;
            this.Email = SDTO.Email;
            this.IsActive = SDTO.IsActive;
            
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

    }
}
