namespace Domain.Dtos
{
	public class AccessListBasicDto
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public AccessListBasicDto(int id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}
