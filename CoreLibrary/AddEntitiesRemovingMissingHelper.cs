using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreLibrary
{
	/// <summary>
	/// Common pattern in the app is to send a list of ids to update a data object. We want to remove the existing records that don't appear in the new list,
	/// and add then new ones. This class requires a bit of set up, but you can rinse and repeat for all instances of this use case.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="Identifier"></typeparam>
	public class AddEntitiesRemovingMissingHelper<T, Identifier> where T : class where Identifier : struct
	{
		private readonly Func<T, bool> _toKeepSelector;
		private readonly Func<T, Identifier> _idSelector;
		private readonly Func<Identifier, T> _entityCreator;

		public AddEntitiesRemovingMissingHelper(Func<T, bool> toKeepSelector, Func<T, Identifier> idSelector, Func<Identifier, T> entityCreator)
		{
			_toKeepSelector = toKeepSelector;
			_idSelector = idSelector;
			_entityCreator = entityCreator;
		}

		/// <summary>
		/// Super generic function that takes in an IEnumerable of previous entites. Then it removes those objects that are not in the list of ids.
		/// The next step is to create the new entites that are not in the existing collection.
		/// The final step is to combine the existing entites to keep and the new entities into a new list.
		/// </summary>
		/// <param name="entities"></param>
		/// <param name="ids"></param>
		/// <returns></returns>
		public List<T> GetEntites(IEnumerable<T> entities, List<Identifier> ids)
		{
			var entitesToKeep = entities.Where(_toKeepSelector);
			var existingIds = entities.Select(_idSelector);
			var entitesToAdd = ids.Except(existingIds).Select(_entityCreator);
			return entitesToKeep.Concat(entitesToAdd).ToList();
		}
	}
}
