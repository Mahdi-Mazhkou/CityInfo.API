using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
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
            var cities=await _cityInfoRepository.GetCitiesAsync();
            
            return Ok(
                    _mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(cities)
                );
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
        //    var cities = _citiesDataStore.CityDtos.FirstOrDefault(x => x.Id == id);
        //    if (cities == null)
        //    {
        //        return NotFound();
        //    }

            return Ok();
        }

    }
}

