using CityInfo.API.Models;
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
        private readonly CitiesDataStore _citiesDataStore;

        public PointOfInterestController(ILogger<PointOfInterestController> logger, IMailService mailService,CitiesDataStore citiesDataStore)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _mailService = mailService;
            _citiesDataStore = citiesDataStore;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointOfInterest (int cityId)
        {
            var city = _citiesDataStore.CityDtos.FirstOrDefault(x=>x.Id==cityId);
          
            try
            {
                
                if (city == null)
                { 
                    _logger.LogInformation($"city with {cityId} not  found");
                    return NotFound();

                }
                return Ok(city.PointOfInterestDtos);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Exception Occured",ex);
                return StatusCode(500, "A Problem Happen While... ");
                throw;
            }
        }

        [HttpGet("{pointOfInterestId}",Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPoint(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.CityDtos.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
               
                return NotFound();

            }

            var point = city.PointOfInterestDtos.FirstOrDefault(x => x.Id == pointOfInterestId);
            if (point == null)
            {
                return NotFound();
            }

            return Ok(point);
        }

        #region Post

        [HttpPost()]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId
            , [FromBody] PointOfInterestForCreationDto interestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var city = _citiesDataStore.CityDtos.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var maxOfPointInterestId = _citiesDataStore.CityDtos
                .SelectMany(x => x.PointOfInterestDtos)
                .Max(x => x.Id);

            var createPoint = new PointOfInterestDto()
            {
                Id = ++maxOfPointInterestId,
                Name = interestDto.Name,
                Description = interestDto.Description

            };
            city.PointOfInterestDtos.Add(createPoint);

            return CreatedAtAction("GetPointOfInterest",
                new
                {

                    cityId = cityId,
                    pointOfInterestId = createPoint.Id
                },
                createPoint
            );
        }

        #endregion

        #region Put

        [HttpPut("{pointOfInterestId}")]
        public ActionResult UpdatePointOfInterest(int cityId,int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
        {
            var city = _citiesDataStore.CityDtos.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
                return NotFound();
            var point = city.PointOfInterestDtos.FirstOrDefault(x=>x.Id==pointOfInterestId);
            if (point == null)
                return NotFound();
            point.Name = pointOfInterest.Name;
            point.Description=pointOfInterest.Description;
            return NoContent();
        }
        #endregion

        #region Patch

        [HttpPatch("{pointOfInterestid}")]
        public ActionResult PartiallyPointOfInterest(
            int cityId,
            int pointOfInterestid,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument )
        {
            var city = _citiesDataStore.CityDtos.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
                return NotFound();

            var pointOfInterestForStore = city.PointOfInterestDtos.FirstOrDefault(x => x.Id == pointOfInterestid);
            if (pointOfInterestForStore == null)
                return NotFound();

            var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
            {
                Name = pointOfInterestForStore.Name,
                Description = pointOfInterestForStore.Description
            };

            patchDocument.ApplyTo(pointOfInterestToPatch,ModelState);
            if (!ModelState.IsValid)
                return BadRequest();

            if (!TryValidateModel(pointOfInterestToPatch))
                return BadRequest(modelState:ModelState);

            pointOfInterestForStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestForStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }


        #endregion

        #region Delete

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult<PointOfInterestForDeleteDto> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.CityDtos.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
                return NotFound();

            var pointOfInterest = city.PointOfInterestDtos.FirstOrDefault(x => x.Id == pointOfInterestId);
            if (pointOfInterest == null)
                return NotFound();

            city.PointOfInterestDtos.Remove(pointOfInterest);

            _mailService.Send(
                "Point Of Interest Deleted",
                $" Point Of Interest {pointOfInterest.Name} With {pointOfInterest.Id}"
                );
            return NoContent();

        } 

        #endregion




    }
}
