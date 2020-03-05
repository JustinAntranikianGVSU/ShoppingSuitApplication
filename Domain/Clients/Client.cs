using System;

namespace Domain.Clients
{
	public class Client
	{
		public Guid Identifier { get; set; }

		public string ClientName { get; set; }

		public Client(Guid identifier, string name)
		{
			Identifier = identifier;
			ClientName = name;
		}
	}
}
