using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using DAL.DTO;
using BLL;

namespace Student_API_Project.Controllers
{
    [Route("api/Students")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {

        [HttpGet("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetAllStudents()
        {
            List<StudentDTO> students = BLL.Student.GetAllStudents();
            if (students == null || students.Count == 0)
                return NotFound("No Students Found");

            return Ok(students);

        }


        [HttpGet("Passed", Name = "GetPassedStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetPassedStudents()
        {
            var passedStudents = BLL.Student.GetPassedStudents();

            if (passedStudents.Count == 0)
                return NotFound("No Students Passed.");

            return Ok(passedStudents);


        }


        [HttpGet("Failed", Name = "GetFailedStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetFailedStudents()
        {
            var failedStudents = BLL.Student.GetFailedStudents();

            if (failedStudents.Count == 0)
                return NotFound("No Students Failed.");

            return Ok(failedStudents);
        }


        [HttpGet("Average", Name = "GetAverageGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<double> GetAverageGrade()
        {
            if (BLL.Student.GetStudentCount() == 0)
                return NotFound("No Students Found");

            return Ok(BLL.Student.GetAverageGrade());
        }


        [HttpGet("{ID}", Name = "GetStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentDTO> GetStudentByID(int ID)
        {
            if (ID < 1)
                return BadRequest("error: ID must be greater than zero.");

            Student? student = BLL.Student.Find(ID);

            if (student == null)
                return NotFound($"error: Student with ID = {ID} Not Found.");


            return Ok(student.DTO); //returns the dto not the BL object
        }


    }
}

        /*

        


        

        [HttpPost("AddNewStudent", Name = "Add Student")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Student> AddNewStudent(Student newStudent)
        {
            if (newStudent == null)
                return BadRequest("Invalid student data.");

            newStudent.ID = Database.Students.Count > 0 ? Database.Students.Max(s => s.ID) + 1 : 1; //sitmulate database auto increament
            Database.Students.Add(newStudent);

            return CreatedAtRoute("GetStudentByID", new { id = newStudent.ID }, newStudent); // Returns the created resource
        }


        [HttpDelete("Delete/{ID}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Student> DeleteStudent(int ID)
        {
            if (ID <= 0)
                return BadRequest("error: ID must be greater than 0");

            Student? student = Database.Students.FirstOrDefault(s => s.ID == ID);

            if (student == null)
                return NotFound($"error: No Student Found with ID = {ID}");

            Database.Students.Remove(student);
            return Ok($"Student with ID = {ID} Removed.");
        }

        [HttpPut("Update/{ID}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Student> UpdateStudent(int id, Student updatedStudent)
        {
            if (id < 1 || updatedStudent == null)
            {
                return BadRequest("Invalid student data.");
            }

            var student = Database.Students.FirstOrDefault(s => s.ID == id);
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            student.Name = updatedStudent.Name;
            student.BirthDate = updatedStudent.BirthDate;
            student.Grade = updatedStudent.Grade;

            return Ok(student);
        }



        */