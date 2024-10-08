namespace Hive.AI.Scorers;

public enum FeatureType
{
	// How many pieces are offboard, per piece type!
	NUM_OFFBOARD = 0,
	OPP_NUM_OFFBOARD = 1,

	// How many pieces are around the queen (0 if queen hasn't been placed)
	NUM_SURROUNDING_QUEEN = 2,
	OPP_NUM_SURROUNDING_QUEEN = 3,

	// How many pieces can move. Two numbers per insect: the first is considering any pieces,
	// the second discards the pieces that are surrounding the opponent's queen (and presumably
	// not to be moved)
	NUM_CAN_MOVE = 4,
	OPP_NUM_CAN_MOVE = 5,

	// Number of moves threatening to reach around opponents queen.
	// Two counts here: the first is the number of pieces that can
	// reach around the opponent's queen. The second is the number
	// of free positions around the opponent's queen that can be
	// reached.
	NUM_THREATENING_MOVES = 6,
	OPP_NUM_THREATENING_MOVES = 7,

	// Number of moves till a draw due to running out of moves. Max to 10.
	MOVES_TO_DRAW = 8,

	// Number of pieces that are "leaves" (only one neighbor)
	NUM_SINGLE = 9,
	OPP_NUM_SINGLE = 10,

	// Whether our queen is covered by a piece, second=1 if the opponent is covering our queen
	QUEEN_COVERED = 11,
	OPP_QUEEN_COVERED = 12,

	// Average manhattan distance to opposing queen for each of the piece types.
	AVERAGE_DISTANCE_TO_QUEEN = 13,
	OPP_AVERAGE_DISTANCE_TO_QUEEN = 14,

	// Last entry.
	NUM_FEATURES = 15
}
