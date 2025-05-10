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

        [HttpPost("AddNewStudent", Name = "AddStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentDTO> AddNewStudent(StudentDTO newDTO)
        {
            if (newDTO == null)
                return BadRequest("Invalid student data.");

            Student student = new Student(newDTO);
            student.Save();

            newDTO.ID = student.ID; //new ID will be generated after saving to db

            return CreatedAtRoute("GetStudentByID", new { id = newDTO.ID }, newDTO); // Returns the created resource
        }


        [HttpPut("Update/{ID}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> UpdateStudent(int ID, StudentDTO updatedStudent)
        {
            if (updatedStudent.ID != ID)
                return BadRequest("ID in body does not match ID in route.");

            if (ID < 1 || updatedStudent == null)
                return BadRequest("Invalid student data.");

            Student? student = Student.Find(ID);
            if (student == null)
            {
                return NotFound($"Student with ID {ID} not found.");
            }

            student.Name = updatedStudent.Name;
            student.BirthDate = updatedStudent.BirthDate;
            student.Email = updatedStudent.Email;
            student.Grade = updatedStudent.Grade;
            student.IsActive = updatedStudent.IsActive;

            if (student.Save())
                return Ok(student.DTO);
            else
                return StatusCode(500, new { message = "Failed to update student. Please try again." });

        }

        [HttpDelete("Delete/{ID}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Student> DeleteStudent(int ID)
        {
            if (ID <= 0)
                return BadRequest("error: ID must be greater than 0");

            if (Student.DeleteStudent(ID))
                return Ok($"Student with ID = {ID} Removed.");
            else
                return NotFound($"error: Student with ID = {ID} Not Found.");
        }


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



        // File Upload to Server //

        //file dir
        public static string filesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

        [HttpPost("UploadFile", Name = "UploadFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            
            // Ensure the directory exists
            Directory.CreateDirectory(filesDirectory);

            // Generate unique filename
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(filesDirectory, fileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new
            {
                FileName = fileName,
                OriginalName = file.FileName,
                SizeInBytes = file.Length,
                ContentType = file.ContentType,
                AccessUrl = Url.RouteUrl("GetFile", new { FileName = fileName }, Request.Scheme)
            });
        }


        [HttpGet("GetFile/{FileName}", Name = "GetFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetFile(string FileName)
        {
            string filePath = Path.Combine(filesDirectory, FileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound("Image Not Found on the Server.");

            var file = System.IO.File.OpenRead(filePath);
            var mimeType = _GetMimeType(filePath);

            return File(file, mimeType);
        }
        private string _GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".doc" or ".docx" => "document/doc",
                ".xls" or ".xlsx" => "excelFile/xls",
                ".png" => "image/png",
                _ => "application/octet-stream" // standard fallback for unknown file types

            };

        }

    }
}

