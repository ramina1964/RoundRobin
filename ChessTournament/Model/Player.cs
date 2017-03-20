using System;
using System.Collections.Generic;

namespace ChessTournament.Model
{
	public class Player : IComparer<Player>
	{
		/************************************************ Constructor ************************************************/
		public Player(int id, int rank)
		{
			Id = id;
			Rank = rank;
			IsBusy = false;
		}

		/********************************************** Class Interface **********************************************/
		internal int Id
		{
			get { return _id; }

			set
			{
				if (0 > value || value > 1000000)
					throw new ArgumentOutOfRangeException(nameof(Id), value, "Value of Player ID is out of range!");

				_id = value;
			}
		}

		internal int Rank
		{
			get { return _rank; }

			set
			{
				if (0 > value || value > 3000)
					throw new ArgumentOutOfRangeException(nameof(Rank), value, "Value of Rank is out of range!");

				_rank = value;
			}
		}

		internal bool IsBusy { get; set; }

		public int Compare(Player x, Player y) => x.Id > y.Id ? 1 : 0;

		/*********************************************** Private Fields **********************************************/
		private int _id;
		private int _rank;
	}
}