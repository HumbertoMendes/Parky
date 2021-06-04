using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.Interfaces;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : Controller
    {
        private INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;
        public NationalParksController(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var modelList = _nationalParkRepository.GetNationalParks();
            var dtoList = _mapper.Map<List<NationalParkDto>>(modelList);

            return Ok(dtoList);
        }

        // Name is required for 
        [HttpGet("{id:int}", Name = nameof(GetNationalPark))]
        public IActionResult GetNationalPark(int id)
        {
            var model = _nationalParkRepository.GetNationalPark(id);
            if (model == null) return NotFound();

            var dto = _mapper.Map<NationalParkDto>(model);

            return Ok(dto);
        }

        [HttpPost]
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

            return CreatedAtRoute(nameof(GetNationalPark), new { id = model.Id }, model);
        }

        [HttpPatch("{id:int}", Name = nameof(UpdateNationalPark))]
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
