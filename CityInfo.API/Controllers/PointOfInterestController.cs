using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Repositories;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsOfInterest")]
    [ApiController]
    public class PointOfInterestController : ControllerBase
    {
        private readonly ILogger<PointOfInterestController> _logger;
        private readonly IMailService _mailService;

        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        private readonly CitiesDataStore _citiesDataStore;

        public PointOfInterestController(ILogger<PointOfInterestController> logger, IMailService mailService, CitiesDataStore citiesDataStore, ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _mailService = mailService;
            _citiesDataStore = citiesDataStore;
            _cityInfoRepository = cityInfoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointOfInterest(int cityId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($" City With {cityId} Not Found ...");
                return NotFound();
            }

            var pointOfInterestForCity = await _cityInfoRepository
                .GetPointOfInterestForCityAsync(cityId);
            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointOfInterestForCity));
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPoint(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($" City With {cityId} Not Found ...");
                return NotFound();
            }

            var pointOfInterestForCity = await _cityInfoRepository
                .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterestForCity == null)
            {
                _logger.LogInformation($" City With {cityId} And {pointOfInterestId} Not Found ...");
                return NotFound();
            }

            return Ok(

                _mapper.Map<PointOfInterestDto>(pointOfInterestForCity)

                );

        }

        #region Post

        [HttpPost()]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId
            , [FromBody] PointOfInterestForCreationDto interestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();

            }
            var finalPoint = _mapper.Map<PointOfInterest>(interestDto);
            await _cityInfoRepository.AddPointOfInterestDtoForCity(cityId, finalPoint);
            await _cityInfoRepository.SaveChangesAsync();
            var createdModel = _mapper.Map<PointOfInterestDto>(finalPoint);
            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityId,
                    pointOfInterestId = createdModel.Id
                },
                createdModel

                );
            //var city = _citiesDataStore.CityDtos.FirstOrDefault(x => x.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            //var maxOfPointInterestId = _citiesDataStore.CityDtos
            //    .SelectMany(x => x.PointOfInterestDtos)
            //    .Max(x => x.Id);

            //var createPoint = new PointOfInterestDto()
            //{
            //    Id = ++maxOfPointInterestId,
            //    Name = interestDto.Name,
            //    Description = interestDto.Description

            //};
            //city.PointOfInterestDtos.Add(createPoint);

            //return CreatedAtAction("GetPointOfInterest",
            //    new
            //    {

            //        cityId = cityId,
            //        pointOfInterestId = createPoint.Id
            //    },
            //    createPoint
            //);
        }

        #endregion

        #region Put

        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var point = _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (point == null)
            {
                return NotFound();
            }

            _mapper.Map(pointOfInterest, point);

            await _cityInfoRepository.SaveChangesAsync();
            return NoContent();
        }
        #endregion

        #region Patch

        [HttpPatch("{pointOfInterestid}")]
        public async Task<ActionResult>  PartiallyPointOfInterest(
            int cityId,
            int pointOfInterestid,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestid);

            if (pointEntity == null)
            {
                return NotFound();
            }

            var pointToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointEntity);

            patchDocument.ApplyTo(pointToPatch,ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointToPatch, pointEntity);
            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }


        #endregion

        #region Delete

        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult<PointOfInterestForDeleteDto>> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository
                .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

             _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
              await _cityInfoRepository.SaveChangesAsync();

            if (pointOfInterestEntity==null)
            {
                return NotFound();
            }


            _mailService.Send(
                "Point Of Interest Deleted",
                $" Point Of Interest {pointOfInterestEntity.Name} With {pointOfInterestEntity.Id}"
                );
            return NoContent();

        }

        #endregion




    }
}
