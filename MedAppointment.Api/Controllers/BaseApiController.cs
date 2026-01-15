namespace MedAppointment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult CustomResult(Result result)
        {
            var jsonResult = new JsonResult(result);
            jsonResult.StatusCode = (int)result.HttpStatus;
            return jsonResult;
        }
    }
}
