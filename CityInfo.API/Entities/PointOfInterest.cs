namespace CityInfo.API.Entities
{
    public class PointOfInterest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }=string.Empty;
        public City City { get; set; }
        public int CityId { get; set; }

        public PointOfInterest(string name)
        {
            this.Name = name;
        }

    }
}
