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
    [Route("api/v{version:apiVersion}/nationalparks")]
    // When omitted, ApiVersion will be 1.0 as configured in Startup.cs
    [ApiController]
    // [ApiExplorerSettings(GroupName = "ParkyNationalParkOpenAPISpec")]
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;
        public NationalParksController(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of National Parks.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<NationalParkDto>))]
        public IActionResult GetNationalParks()
        {
            var modelList = _nationalParkRepository.GetNationalParks();
            var dtoList = _mapper.Map<List<NationalParkDto>>(modelList);

            return Ok(dtoList);
        }

        // Name is required for 
        [HttpGet("{id:int}", Name = nameof(GetNationalPark))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetNationalPark(int id)
        {
            var model = _nationalParkRepository.GetNationalPark(id);
            if (model == null) return NotFound();

            var dto = _mapper.Map<NationalParkDto>(model);

            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null) return BadRequest(ModelState);
            if (_nationalParkRepository.NationalParkExists(nationalParkDto.Name)) {
                ModelState.AddModelError("message", "National Park exists.");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var model = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_nationalParkRepository.CreateNationalPark(model))
            {
                ModelState.AddModelError("message", $"Something went wrong when saving the record {nationalParkDto.Name}.");
                return BadRequest(ModelState);
            }

            return CreatedAtRoute(
                nameof(GetNationalPark),
                new {
                    version = HttpContext.GetRequestedApiVersion().ToString(), // version is required because there are endpoints with the same name across versioned controllers
                    id = model.Id
                },
                model);
        }

        [HttpPatch("{id:int}", Name = nameof(UpdateNationalPark))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateNationalPark(int id, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || id != nationalParkDto.Id) return BadRequest(ModelState);

            var model = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_nationalParkRepository.UpdateNationalPark(model))
            {
                ModelState.AddModelError("message", $"Something went wrong when updating the record {nationalParkDto.Name}.");
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = nameof(DeleteNationalPark))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteNationalPark(int id)
        {
            if (!_nationalParkRepository.NationalParkExists(id)) return NotFound();

            var model = _nationalParkRepository.GetNationalPark(id);

            if (!_nationalParkRepository.DeleteNationalPark(model))
            {
                ModelState.AddModelError("message", $"Something went wrong when deleting the record {model.Name}.");
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
