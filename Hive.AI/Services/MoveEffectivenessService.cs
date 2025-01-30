using Hive.Core;
using Hive.Core.Models;

namespace Hive.AI.Services
{
	public class MoveEffectivenessService(Match state, Color color)
	{
		// THIS IS THE ALREADY SIMULATED MATCH. DO NOT CHANGE THE STATE OF THE MATCH
		private readonly Match _state = state;
		private readonly Color _color = color;

		/// <summary>
		/// Returns if the move provided is effective
		/// </summary>
		/// <param name="move">The last made move</param>
		/// <returns>If the move is effective</returns>
		public bool IsEffectiveMove(Move move)
		{
			return PinsOpponentPiece(move) ||
				   SurroundsOpponentQueen(move) ||
				   FreesPinnedPiece(move) ||
				   ThreatensOpponentPiece(move) ||
				   ImprovesMobility(move) ||
				   BlocksOpponentMove(move) ||
				   AdvancesStrategicPosition(move) ||
				   CreatesNewThreat(move);
		}

		private bool PinsOpponentPiece(Move move)
		{
			return false;
		}

		private bool SurroundsOpponentQueen(Move move)
		{
			return false;
		}

		private bool FreesPinnedPiece(Move move)
		{
			return false;
		}

		private bool ThreatensOpponentPiece(Move move)
		{
			return false;
		}

		private bool ImprovesMobility(Move move)
		{
			return false;
		}

		private bool BlocksOpponentMove(Move move)
		{
			return false;
		}

		private bool AdvancesStrategicPosition(Move move)
		{
			return false;
		}

		private bool CreatesNewThreat(Move move)
		{
			return false;
		}
	}
}