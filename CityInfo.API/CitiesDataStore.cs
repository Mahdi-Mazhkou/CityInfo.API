using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public List<CityDto> CityDtos { get; set; }

        public CitiesDataStore()
        {
            CityDtos = new List<CityDto>()

            {
                new CityDto()
                {
                    Id = 1 , Name = "Tehran" , Description = "This Is A Tehran",
                    PointOfInterestDtos = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Didani1",
                            Description = "Jaye Didani1"
                            
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Didani2",
                            Description = "Jaye Didani2"

                        },

                    }
                },
                new CityDto()
                {
                    Id = 2 , Name = "Shiraz" , Description = "This Is A Shiraz",
                    PointOfInterestDtos = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 3,
                            Name = "Didani3",
                            Description = "Jahaye Didani3"

                        },
                        new PointOfInterestDto()
                        {
                            Id = 4,
                            Name = "4",
                            Description = "Jahaye Didani4"

                        }
                    }
                },
                
            };
        }
    }
}
