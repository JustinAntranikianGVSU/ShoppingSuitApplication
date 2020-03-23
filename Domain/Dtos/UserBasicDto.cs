
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

		public string Initals { get => $"{FirstName[0]}{LastName[0]}"; }

		public UserBasicDto(int id, string firstName, string lastName)
		{
			Id = id;
			FirstName = firstName;
			LastName = lastName;
		}
	}
}
