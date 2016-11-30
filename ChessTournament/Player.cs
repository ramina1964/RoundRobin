using System;
using System.Collections.Generic;

namespace ChessTournament
{
	public class Player : IComparer<Player>
	{
		public Player(int id, int rank)
		{
			Id = id;
			Rank = rank;
		}

		internal int Id
		{
			get { return _id; }

			set
			{
				if(_id < 0 || _id > 1000000 )
					throw new ArgumentOutOfRangeException(nameof(Id), value, "Value of Player ID is out of range!");
			}
		}

		internal int Rank
		{
			get { return _rank; }

			set
			{
				if (_rank < 0 || _rank > 3000)
					throw new ArgumentOutOfRangeException(nameof(Rank), value, "Value of Rank is out of range!");
			}
		}

		public int Compare(Player x, Player y) => x.Id > y.Id ? 1 : 0;


		/*********************************************** Private Fields **********************************************/
		private readonly int _id;
		private readonly int _rank;
	}
}