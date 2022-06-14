using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
        }

        // ========= GET BEGIN ==================================================
        [HttpGet]
        [Authorize(Roles = "reader")]
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
        [Authorize(Roles = "reader")]
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
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            // Validate incoming reqest
            if(!await ValidateAddWalkAsync(addWalkRequest))
            {
                return BadRequest(ModelState);
            }

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
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            // Validate the incoming request
            if(!await ValidateUpdateWalkAsync(updateWalkRequest))
            {
                return BadRequest(ModelState);
            }

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
        [Authorize(Roles = "writer")]
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

        #region Private Methods

        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            /*if (addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest),
                    $"{nameof(addWalkRequest)} cannot be empty.");
                return false;
            }*/

            /*if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name),
                    $"{nameof(addWalkRequest.Name)} is required.");
            }*/

            /*if (addWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length),
                    $"{nameof(addWalkRequest.Length)} should be greater than zero.");
            }*/

            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
            if(region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId),
                    $"{nameof(addWalkRequest.RegionId)} is invalid.");
            }

            var walkDepository = await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if (walkDepository == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId),
                    $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid.");
            }
            
            if(ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }


        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            /*if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest),
                    $"{nameof(updateWalkRequest)} cannot be empty.");
                return false;
            }*/

            /*if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name),
                    $"{nameof(updateWalkRequest.Name)} is required.");
            }*/

            /*if (updateWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length),
                    $"{nameof(updateWalkRequest.Length)} should be greater than zero.");
            }*/

            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId),
                    $"{nameof(updateWalkRequest.RegionId)} is invalid.");
            }

            var walkDepository = await walkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkDepository == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId),
                    $"{nameof(updateWalkRequest.WalkDifficultyId)} is invalid.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        #endregion

    }
}
