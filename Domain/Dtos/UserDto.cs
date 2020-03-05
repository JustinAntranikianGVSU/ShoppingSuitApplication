using CoreLibrary;
using System;
using System.Collections.Generic;

namespace Domain
{
	public class UserDto
	{
		public int Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		/// <summary>
		/// First and Last names seperated by a single space.
		/// </summary>
		public string FullName { get => FirstName + " " + LastName; }

		public string Email { get; set; }

		public List<Role> Roles { get; set; } = new List<Role>();

		public Guid? ClientIdentifier { get; set; }

		public UserDto() {}

		public UserDto(string firstName, string lastName) => (FirstName, LastName) = (firstName, lastName);

		public UserDto(string firstName, string lastName, string email) : this(firstName, lastName) => Email = email;
		
		public UserDto(int id, string firstName, string lastName) : this(firstName, lastName) => Id = id;
	}
}
