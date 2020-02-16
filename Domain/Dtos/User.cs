﻿using Domain.Security;
using System;
using System.Collections.Generic;

namespace Domain
{
	public class User
	{
		public int Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public List<Role> Roles { get; set; } = new List<Role>();

		public User() { }

		public User(string firstName, string lastName, string email)
		{
			FirstName = firstName;
			LastName = lastName;
			Email = email;
		}

		public User(int id, string firstName, string lastName, string email)
		{
			Id = id;
			FirstName = firstName;
			LastName = lastName;
			Email = email;
		}
	}
}
