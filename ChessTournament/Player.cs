using System.Collections.Generic;

namespace ChessTournament
{
	public class Player : IComparer<Player>
	{
		public Player(int index, int rank)
		{
			// Add exception handling for index and rank.
			// Index must be greater or equal than zero.
			// Rank must be greater than zero.

			Index = index;
			Rank = rank;
		}

		internal int Index { get; }
		internal int Rank { get; }

		public int Compare(Player x, Player y) => x.Index > y.Index ? 1 : 0;
	}
}