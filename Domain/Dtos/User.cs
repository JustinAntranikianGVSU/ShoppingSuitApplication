using Domain.Security;
using System;
using System.Collections.Generic;

namespace Domain
{
	public class User
	{
		public int Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		/// <summary>
		/// First and Last names seperated by a single space.
		/// </summary>
		public string FullName
		{
			get => FirstName + " " + LastName;
		}

		public string Email { get; set; }

		public List<Role> Roles { get; set; } = new List<Role>();

		public Guid? ClientIdentifier { get; set; }

		public User() { }

		public User(string firstName, string lastName, string email)
		{
			FirstName = firstName;
			LastName = lastName;
			Email = email;
		}

		public User(int id, string firstName, string lastName, string email) : this(firstName, lastName, email)
		{
			Id = id;
		}
	}
}
