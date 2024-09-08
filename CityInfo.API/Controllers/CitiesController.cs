using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/Cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ??
                                  throw new ArgumentException(nameof(cityInfoRepository));
            _mapper = mapper ??
                      throw new ArgumentException(nameof(mapper)); ;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestDto>>> GetCities()
        {
            var cities = await _cityInfoRepository.GetCitiesAsync();

            return Ok(
                    _mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(cities)
                );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id, bool includePointOfInterest = false)
        {
            var city = _cityInfoRepository.GetCityAsync(id, includePointOfInterest);

            if (includePointOfInterest)
                return Ok(_mapper.Map<CityDto>(city));


            return Ok(_mapper.Map<CityWithoutPointOfInterestDto>(city));
        }

    }
}

