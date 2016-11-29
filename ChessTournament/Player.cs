using System.Collections.Generic;

namespace ChessTournament
{
	public class Player : IComparer<Player>
	{
		public Player(int id, int rank)
		{
			// Add exception handling for id and rank.
			// Id must be greater or equal than zero.
			// Rank must be greater than zero.

			Id = id;
			Rank = rank;
		}

		internal int Id { get; }
		internal int Rank { get; }

		public int Compare(Player x, Player y) => x.Id > y.Id ? 1 : 0;
	}
}