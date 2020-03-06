
namespace Domain
{
	public class UserBasicDto
	{
		public int Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		/// <summary>
		/// First and Last names seperated by a single space.
		/// </summary>
		public string FullName { get => FirstName + " " + LastName; }

		public UserBasicDto(int id, string firstName, string lastName)
		{
			Id = id;
			FirstName = firstName;
			LastName = lastName;
		}
	}
}
