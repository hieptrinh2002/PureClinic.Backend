using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiVersion("1.0")] 
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AppointmentController : ControllerBase
    {
        // add new appointment

        // update apointment status 

        // 
    }
}
