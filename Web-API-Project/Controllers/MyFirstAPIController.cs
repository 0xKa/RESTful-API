using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web_API_Project.Controllers
{
    //[Route("api/[controller]")] //dynamic route 
    [Route("api/MyFirstAPI")]    //hardcoded route

    [ApiController]
    public class MyFirstAPIController : ControllerBase
    {
        [HttpGet("MyName", Name = "MyName")]
        public string GetMyName()
        {
            return "Reda Hilal";
        }

        [HttpGet("Text", Name = "Text" )]
        public string GetText()
        {
            return "Sample Text...";
        }

        [HttpGet("Sum/{num1}/{num2}")]
        public double Sum2Num(double num1, double num2)
        {
            return num1 + num2;
        }
        
        [HttpGet("Multi/{num1}/{num2}")]
        public double Multi2Num(double num1, double num2)
        {
            return num1 * num2;
        }

        [HttpGet("Divide/{a}/{b}")]
        //note: we can use ActionResult<double> instead of double to return Either a double or a custom status code + message
        public ActionResult<double> Divide(double a, double b)
        {
            if (b == 0)
                return BadRequest("Cannot divide by zero"); // returns 400 Bad Request

            return Ok(a / b); // returns 200 OK with result
        }




    }
}
