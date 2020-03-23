using System.Collections.Generic;
using System.Linq;

namespace Domain.Mappers
{
	public abstract class EntityToDtoMapperBase<Entity, Dto>
	{
		public abstract Dto Map(Entity entity);

		public List<Dto> Map(IEnumerable<Entity> entities) => entities.Select(Map).ToList();
	}
}
