using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Student_API_Project.Model;
using Student_API_Project.DatabaseSimulation;

namespace Student_API_Project.Controllers
{
    [Route("api/Students")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {

        [HttpGet("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Student>> GetAllStudents()
        {
            if (Database.Students.Count == 0)
                return NotFound("No Students Found");

            return Ok(Database.Students);
        }

        [HttpGet("Passed", Name = "GetPassedStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Student>> GetPassedStudents()
        {
            var passedStudents = Database.Students.Where(s => s.Grade >= 50).ToList();

            if (passedStudents.Count == 0)
                return NotFound("No Passed Students Found");

            return Ok(passedStudents);
        }

        [HttpGet("Failed", Name = "GetFailedStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Student>> GetFailedStudents()
        {
            var failedStudents = Database.Students.Where(s => s.Grade < 50).ToList();

            if (failedStudents.Count == 0)
                return NotFound("No Failed Students Found");

            return Ok(failedStudents);
        }

        [HttpGet("Average", Name = "GetAverageGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<double> GetAverageGrade()
        {
            if (Database.Students.Count == 0)
                return NotFound("No Students Found");

            return Ok(Database.Students.Average(s => s.Grade));
        }


        [HttpGet("{ID}", Name = "GetStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Student> GetStudentByID(int ID)
        {
            if (ID <= 0)
                return BadRequest("error: ID must be greater than 0");

            Student? student = Database.Students.FirstOrDefault(s => s.ID == ID);

            if (student == null)
                return NotFound($"error: No Student Found with ID = {ID}");

            return Ok(student);
        }


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



    }
}
