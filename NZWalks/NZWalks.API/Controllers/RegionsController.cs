using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }


        // ========= GET BEGIN ==================================================
        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            // Pass details to Repository
            var regions =  await regionRepository.GetAllAsync();

            // If null then Not Found
            if (regions == null) { return NotFound(); }

            // Convert data back to DTO
            /*var regionsDTO = new List<Models.DTO.Region>();
            regions.ToList().ForEach(region =>
            {
                var regionDTO = new Models.DTO.Region()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    Area = region.Area,
                    Lat = region.Lat,
                    Long = region.Long,
                    Population = region.Population,
                };
                regionsDTO.Add(regionDTO);
            });*/

            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

            // Return Ok response
            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            // Pass details to Repository
            var region = await regionRepository.GetAsync(id);

            // If null then Not Found
            if (region == null) { return NotFound(); }

            // Convert data back to DTO
            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            // Return Ok response
            return Ok(regionDTO);
        }


        // ========= POST BEGIN ==================================================
        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            // Convert DTO to Domain Model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population
            };

            // Pass details to Repository
            region = await regionRepository.AddAsync(region);

            // Convert data back to DTO
            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            // Return Created Action
            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
        }


        // ========= UPDATE BEGIN ==================================================
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            // Convert DTO to Domain Model
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population
            };

            // Update Region using repository
            region = await regionRepository.UpdateAsync(id, region);

            // If null then Not Found
            if (region == null) { return NotFound(); }

            // Convert Domain back to DTO
            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            // Return Ok response
            return Ok();
        }


        // ========= DELETE BEGIN ==================================================
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            // Get region from database
            var region = await regionRepository.DeleteAsync(id);

            // If null NotFound
            if (region == null) { return NotFound(); }

            // Convert response back to DTO
            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            // Return Ok response
            return Ok(regionDTO);
        }
    }
}
