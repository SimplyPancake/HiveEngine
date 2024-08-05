namespace Hive.Core.Attributes;

/// <summary>
/// Describes if a Bug can walk, and how many times.
/// Walk amount 0 for infinite walking.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class CanWalk : Attribute
{
	public int WalkAmount { get; set; }

	public List<Move> Moves()
	{
		return [];
	}
}
