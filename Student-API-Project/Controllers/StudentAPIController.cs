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

        [HttpGet("AllStudents")]
        public ActionResult<IEnumerable<Student>> GetALlStudents()
        {
            return Ok(Database.Students);
        }



    }
}
