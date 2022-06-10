using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        // ========= GET BEGIN ==================================================
        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var walkDifficultyDomain = await walkDifficultyRepository.GetAllAsync();
            if (walkDifficultyDomain == null) { return NotFound(); }
            var walkDifficultyDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficultyDomain);
            return Ok(walkDifficultyDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        //[ActionName("GetWalkDifficultyAsync")]
        public async Task<IActionResult> GetWalkDifficultyById(Guid id)
        {
            // Pass details to Repository
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);

            // If null then Not Found
            if (walkDifficulty == null) { return NotFound(); }

            // Convert data back to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            // Return Ok response
            return Ok(walkDifficultyDTO);
        }

        // ========= POST BEGIN ==================================================
        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            // Convert DTO to Domain Model
            var walkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = addWalkDifficultyRequest.Code
            };

            // Pass details to Repository
            walkDifficulty = await walkDifficultyRepository.AddAsync(walkDifficulty);

            // Convert data back to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            // Return Created Action
            return CreatedAtAction(nameof(GetWalkDifficultyById), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }

        // ========= UPDATE BEGIN ==================================================
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            // Convert DTO to Domain Model
            var existWalkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDifficultyRequest.Code
            };

            // Update WalkDifficulty using repository
            existWalkDifficulty = await walkDifficultyRepository.UpdateAsync(id, existWalkDifficulty);

            // If null then Not Found
            if (existWalkDifficulty == null) { return NotFound(); }

            // Convert Domain back to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(existWalkDifficulty);

            // Return Ok response
            return Ok(walkDifficultyDTO);
        }

        // ========= DELETE BEGIN ==================================================
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            // Get WalkDifficulty from database
            var existWalkDifficulty = await walkDifficultyRepository.DeleteAsync(id);

            // If null NotFound
            if (existWalkDifficulty == null) { return NotFound(); }

            // Convert response back to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(existWalkDifficulty);

            // Return Ok response
            return Ok(walkDifficultyDTO);
        }
    }
}
