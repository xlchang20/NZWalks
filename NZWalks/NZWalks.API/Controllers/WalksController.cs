using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {

        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        // ========= GET BEGIN ==================================================
        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            // Fetch data from database - Domain.Walks
            var walksDomain = await walkRepository.GetAllAsync();

            // Convert Domain Walks to DTO Walks
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);

            // Return Response
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetWalkASync(Guid id)
        {
            // Get Walk Domain object from database
            var walk = await walkRepository.GetAsync(id);

            // If null then Not Found
            if(walk == null) { return NotFound(); }

            // Convert data back to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            // Return Response
            return Ok(walkDTO);
        }


        // ========= POST BEGIN ==================================================
        [HttpPost]
        public async Task<IActionResult> AddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            // Convert DTO to Domain Model
            var walkDomain = new Models.Domain.Walk()
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };

            // Pass details to Repository
            walkDomain = await walkRepository.AddAsync(walkDomain);

            // Convert data back to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            // Return Created Action
            return CreatedAtAction(nameof(GetWalkASync), new { id = walkDTO.Id }, walkDTO);
        }


        // ========= UPDATE BEGIN ==================================================
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            // Convert DTO to Domain Model
            var walkDomain = new Models.Domain.Walk()
            {
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };

            // Update Region using repository
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            // If null then Not Found
            if (walkDomain == null) { return NotFound(); }

            // Convert Domain back to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            // Return Ok response
            return Ok();
        }


        // ========= DELETE BEGIN ==================================================
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            // Get walk from database
            var walk = await walkRepository.DeleteAsync(id);

            // If null NotFound
            if (walk == null) { return NotFound(); }

            // Convert response back to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            // Return Ok response
            return Ok(walkDTO);
        }

    }
}
