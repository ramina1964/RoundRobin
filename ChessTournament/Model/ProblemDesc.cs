using System;
using System.Collections.Generic;
using ChessTournament.Interfaces;

namespace ChessTournament.Model
{
	public class ProblemDesc : IProblemDesc
	{
		/************************************************* Constants *************************************************/
		// Min and Max values for NoOfPlayers
		internal const int MaxNoOfPlayers = 30;
		internal const int MinNoOfPlayers = 4;

		// Min and Max values for NoOfRounds
		internal const int MinNoOfRounds = 3;


		/************************************************ Constructor ************************************************/
		public ProblemDesc(int noOfPlayers, int noOfRounds)
		{
			NoOfPlayers = noOfPlayers;
			MaxNoOfRounds = NoOfPlayers - 1;

			NoOfRoundsDesired = noOfRounds;
			NoOfMatchesPerRound = NoOfPlayers / 2;

			// Calculate No. of Possible Matches
			NoOfPossibleMatches = NoOfPlayers * (NoOfPlayers - 1) / 2;
			OutputFile = $"Results - {NoOfPlayers} Players.txt";

			Players = Utility.InitializePlayers(NoOfPlayers) as HashSet<Player>;
			AllMatches = Utility.InitializeAllMatches(Players);
		}

		/********************************************** Methods and Properties **********************************************/
		// IProblemDesc Implementation
		public int NoOfPlayers
		{
			get { return _noOfPlayers; }
			set
			{
				if (MinNoOfPlayers > value || value > MaxNoOfPlayers || value % 2 != 0)
					throw new ArgumentOutOfRangeException(nameof(NoOfPlayers), value,
						$"NoOfPlayers must be an even int in the interval [{MinNoOfPlayers}, {MaxNoOfPlayers}]");

				_noOfPlayers = value;
			}
		}
		public int MaxNoOfRounds { get; set; }
		public int NoOfRoundsDesired
		{
			get { return _noOfRoundsDesired; }
			set
			{
				if (MinNoOfRounds > value || value > MaxNoOfRounds)
					throw new ArgumentOutOfRangeException(nameof(NoOfRoundsDesired), value,
						$"NoOfRoundsDesired must be an int in the interval [{MinNoOfRounds}, {MaxNoOfRounds}]");

				_noOfRoundsDesired = value;
			}
		}
		public int NoOfMatchesPerRound { get; set; }
		public string OutputFile { get; set; }
		public int NoOfPossibleMatches { get; set; }
		public HashSet<Player> Players { get; set; }
		public IEnumerable<HashSet<Match>> AllMatches { get; }

		/*************************************************** Private Fields ****************************************************/
		private int _noOfPlayers;
		private int _noOfRoundsDesired;
	}
}
