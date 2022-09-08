using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantSimulation.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class ApiController : ControllerBase
    {
    }
}
