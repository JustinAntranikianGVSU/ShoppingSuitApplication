﻿using System;

namespace CoreLibrary
{
	public class Client
	{
		public Guid Identifier { get; set; }

		public string Name { get; set; }

		public Client(Guid identifier, string name)
		{
			Identifier = identifier;
			Name = name;
		}
	}
}
