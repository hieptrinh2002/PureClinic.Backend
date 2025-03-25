using Microsoft.AspNetCore.Mvc;

namespace PureLifeClinic.API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorSpecializationController : ControllerBase
    {
        // GET: api/DoctorSpecialization
        // Retrieve a list of all doctor specializations
        // Response: 200 OK
        // [
        //     { "id": 1, "name": "Cardiology" },
        //     { "id": 2, "name": "Neurology" }
        // ]
        [HttpGet]
        public IActionResult GetAllSpecializations() { return Ok("not implement"); }

        // GET: api/DoctorSpecialization/{id}
        // Retrieve specialization details by ID
        // Response: 200 OK
        // { "id": 1, "name": "Cardiology" }
        // Response: 404 Not Found if not found
        [HttpGet("{id}")]
        public IActionResult GetSpecializationById(int id) { return Ok("not implement"); }

        // POST: api/DoctorSpecialization
        // Add a new specialization
        // Input:
        // {
        //     "name": "Orthopedics"
        // }
        // Response: 201 Created
        // { "id": 3, "name": "Orthopedics" }
        [HttpPost]
        public IActionResult CreateSpecialization([FromBody] SpecializationDto specialization) { return Ok("not implement");  }

        // PUT: api/DoctorSpecialization/{id}
        // Update specialization details by ID
        // Input:
        // {
        //     "name": "Updated Name"
        // }
        // Response: 200 OK
        // { "id": 1, "name": "Updated Name" }
        // Response: 404 Not Found if not found
        [HttpPut("{id}")]
        public IActionResult UpdateSpecialization(int id, [FromBody] SpecializationDto specialization) { return Ok("not implement");  }

        // DELETE: api/DoctorSpecialization/{id}
        // Delete specialization by ID
        // Response: 204 No Content if deleted successfully
        // Response: 404 Not Found if not found
        [HttpDelete("{id}")]
        public IActionResult DeleteSpecialization(int id) { return Ok("not implement"); }
    }
    public class SpecializationDto
    {
        public string Name { get; set; }
    }
}
