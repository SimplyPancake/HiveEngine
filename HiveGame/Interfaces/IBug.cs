namespace HiveGame;

public interface IBug
{
	/// <summary>
	/// The name of the bug type
	/// </summary>
	string Name { get; }

	/// <summary>
	/// The description of the Bug type
	/// </summary>
	string Description { get; }

	/// <summary>
	/// The ID value that a bug has (now inherited from BugType)
	/// </summary>
	int BugTypeId { get; }

	/// <summary>
	/// The amount of pieces that a player should get of this class
	/// </summary>
	int GetAmount { get; }

	/// <summary>
	/// Short representation of a Bug, like 'Q' for the Queen.
	/// </summary>
	char ShortRepresentation { get; }

	/// <summary>
	/// If the normal move restrictions apply
	/// </summary>
	bool MoveRestrictionsApply { get; }
}
