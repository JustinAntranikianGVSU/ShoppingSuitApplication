using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Clients
{
	public static class ClientLookup
	{
		public static Guid WellFargoClientGuid = new Guid("1560b0b4-3711-4162-9a0e-05caf36ef55a");

		public static Guid BankOfAmericaClientGuid = new Guid("40321985-2a90-41af-958d-606e59bcb19b");

		public static List<Client> Clients;

		static ClientLookup()
		{
			Clients = new List<Client>()
			{
				new Client(WellFargoClientGuid, "Wells Fargo Inc"),
				new Client(BankOfAmericaClientGuid, "Bank Of America Global")
			};
		}

		public static Client GetClient(Guid identifier)
		{
			var client = Clients.SingleOrDefault(oo => oo.Identifier == identifier);

			if (client is null)
			{
				throw new Exception($"Could not find {nameof(Client)} with identifier {identifier}. Please fix whatever data is refering to a client that doesn't exist.");
			}

			return client;
		}
	}
}
