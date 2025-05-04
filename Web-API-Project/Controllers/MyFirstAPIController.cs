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

        [HttpGet("Sum/{num1},{num2}")]
        public double Sum2Num(double num1, double num2)
        {
            return num1 + num2;
        }
        
        [HttpGet("Multi/{num1},{num2}")]
        public double Multi2Num(double num1, double num2)
        {
            return num1 * num2;
        }

    }
}
