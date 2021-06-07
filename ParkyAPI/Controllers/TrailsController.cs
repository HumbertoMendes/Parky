using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.Interfaces;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/trails")]
    [ApiController]
    // [ApiExplorerSettings(GroupName = "ParkyTrailsOpenAPISpec")]
    public class TrailsController : Controller
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;
        public TrailsController(ITrailRepository trailRepository, IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of Trails.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TrailDto>))]
        public IActionResult GetTrails()
        {
            var modelList = _trailRepository.GetTrails();
            var dtoList = _mapper.Map<List<TrailDto>>(modelList);

            return Ok(dtoList);
        }

        /// <summary>
        /// Get list of Trails in National Park.
        /// </summary>
        /// <returns></returns>
        //[HttpGet("{nationalParkId:int}", Name = nameof(GetTrailsInNationalPark))]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TrailDto>))]
        //public IActionResult GetTrailsInNationalPark(int nationalParkId)
        //{
        //    var modelList = _trailRepository.GetTrailsInNationalPark(nationalParkId);
        //    var dtoList = _mapper.Map<List<TrailDto>>(modelList);

        //    return Ok(dtoList);
        //}

        // Name is required for 
        [HttpGet("{id:int}", Name = nameof(GetTrail))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTrail(int id)
        {
            var model = _trailRepository.GetTrail(id);
            if (model == null) return NotFound();

            var dto = _mapper.Map<TrailDto>(model);

            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            if (trailDto == null) return BadRequest(ModelState);
            if (_trailRepository.TrailExists(trailDto.Name)) {
                ModelState.AddModelError("message", "Trail exists.");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var model = _mapper.Map<Trail>(trailDto);

            if (!_trailRepository.CreateTrail(model))
            {
                ModelState.AddModelError("message", $"Something went wrong when saving the record {trailDto.Name}.");
                return BadRequest(ModelState);
            }

            return CreatedAtRoute(nameof(GetTrail), new { id = model.Id }, model);
        }

        [HttpPatch("{id:int}", Name = nameof(UpdateTrail))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateTrail(int id, [FromBody] TrailUpdateDto trailDto)
        {
            if (trailDto == null || id != trailDto.Id) return BadRequest(ModelState);

            var model = _mapper.Map<Trail>(trailDto);

            if (!_trailRepository.UpdateTrail(model))
            {
                ModelState.AddModelError("message", $"Something went wrong when updating the record {trailDto.Name}.");
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = nameof(DeleteTrail))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteTrail(int id)
        {
            if (!_trailRepository.TrailExists(id)) return NotFound();

            var model = _trailRepository.GetTrail(id);

            if (!_trailRepository.DeleteTrail(model))
            {
                ModelState.AddModelError("message", $"Something went wrong when deleting the record {model.Name}.");
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
