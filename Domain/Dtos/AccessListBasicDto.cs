namespace Domain.Dtos
{
	public class AccessListBasicDto
	{
		public int AccessListId { get; set; }

		public string AccessListName { get; set; }

		public AccessListBasicDto(int accessListId, string accessListName)
		{
			AccessListId = accessListId;
			AccessListName = accessListName;
		}
	}
}
