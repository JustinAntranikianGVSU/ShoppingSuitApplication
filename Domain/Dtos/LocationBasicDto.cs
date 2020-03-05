
namespace Domain.Dtos
{
	public class LocationBasicDto
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public LocationBasicDto(int id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}
