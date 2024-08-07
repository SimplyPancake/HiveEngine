namespace Hive.Core.Enums;

/// <summary>
/// Drescribes the attack pattern of a bug
/// </summary>
public enum MoveBehavior
{
	/// <summary>
	/// Means a Bug's moveset is compromised of ONLY moving
	/// </summary>
	MustMove,

	/// <summary>
	/// Means the Bug can also choose not to move on attacking
	/// e.g. the pillbug & the mosquito
	/// </summary>
	MayMove
}
