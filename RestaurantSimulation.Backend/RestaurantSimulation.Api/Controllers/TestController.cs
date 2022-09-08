using Microsoft.AspNetCore.Mvc;

namespace RestaurantSimulation.Api.Controllers
{
    [Route("api/test")]
    public class TestController : ApiController
    {
        [HttpGet("list")]
        public IActionResult GetRandomList()
        {
            return Ok(new List<string> { "Robert", "Mihai"});
        }
    }
}
