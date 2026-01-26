using MedAppointment.DataTransferObjects.CredentialDtos;

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

        protected IActionResult SuccessAuthResult(Result<TokenDto> result)
        {
            if(result.IsSuccess())
            {
                this.HttpContext.Response.Cookies.Append("RefreshToken", result.Model!.RefreshToken);
                var jsonResult = new JsonResult(new
                {
                    result.Model.AccessToken
                });
                jsonResult.StatusCode = (int)result.HttpStatus;
                return jsonResult;
            }
            else
            {
                return CustomResult(result);
            }
            
        }
    }
}
