using KKBundle.API.Controllers.PricingResources;
using KKBundle.API.Domains.Pricing;
using Microsoft.AspNetCore.Mvc;

namespace KKBundle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricingController(IPricingService pricingService) : ControllerBase
    {
        /// <summary>
        /// Generates a course bundle offer based on the teacher's request
        /// </summary>
        /// <param name="request"></param>
        /// <returns>An offer for the teacher</returns>
        /// <response code="201">Creates a bundle quote</response>
        /// <response code="400">If the request payload is invalid.</response>
        [ProducesResponseType(typeof(IEnumerable<Pricing>), 200)]
        [ProducesResponseType(400)]
        [HttpPost]
        public IActionResult CreateOffer([FromBody]TeacherRequest request)
        {
            if (request.Topics is null)
            {
                return BadRequest();
            }

            var pricingQuery = new PricingQuery(request.Topics);

            return CreatedAtAction(nameof(CreateOffer), pricingService.CalculateOffer(pricingQuery));
        }
    }
}
