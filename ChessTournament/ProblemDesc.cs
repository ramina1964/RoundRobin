using System;
using System.Collections.Generic;

namespace ChessTournament
{
    internal class ProblemDesc
    {
        /************************************************* Constants *************************************************/
        // Min and Max values for MaxNoOfPlayers
        internal const int MaxNoOfPlayers = 30;
        internal const int MinNoOfPlayers = 4;

        // Min and Max values for NoOfRounds
        internal const int MinNoOfRounds = 3;
        internal int MaxNoOfRounds { get; set; }

        /************************************************ Constructor ************************************************/
        internal ProblemDesc(int noOfPlayers, int noOfRounds)
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

		/********************************************** Class Interface **********************************************/
		internal IEnumerable<HashSet<Match>> AllMatches { get; }

		internal HashSet<Player> Players { get; set; }

		internal int NoOfPlayers
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

        internal int NoOfRoundsDesired
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

        internal int NoOfMatchesPerRound { get; set; }
        internal string OutputFile { get; set; }
        internal int NoOfPossibleMatches { get; set; }

        /*************************************************** Private Fields ****************************************************/
        private int _noOfPlayers;
        private int _noOfRoundsDesired;
	}
}
