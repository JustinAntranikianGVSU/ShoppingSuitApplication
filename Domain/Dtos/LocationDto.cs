
namespace Domain.Dtos
{
	public class LocationDto
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public LocationDto(int id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}
