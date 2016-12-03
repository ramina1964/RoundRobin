using System;

namespace ChessTournament
{
    public class ProblemDesc
    {
        /************************************************* Constants *************************************************/
        // Min and Max values for MaxNoOfPlayers
        public const int MaxNoOfPlayers = 26;
        public const int MinNoOfPlayers = 4;

        // Min and Max values for NoOfRounds
        public const int MinNoOfRounds = 3;
        public static int MaxNoOfRounds { get; set; }

        /************************************************ Constructor ************************************************/
        public ProblemDesc(int noOfPlayers, int noOfRounds)
        {
            NoOfPlayers = noOfPlayers;
            MaxNoOfRounds = NoOfPlayers - 1;

            NoOfRoundsDesired = noOfRounds;
            NoOfMatchesPerRound = NoOfPlayers / 2;

            // NoOfPossibleMatches = NoOfPlayers / ( 2! * (NoOfPlayers - 2)! )
            NoOfPossibleMatches = NoOfPlayers * (NoOfPlayers - 1) / 2;
            OutputFile = $"Results - {NoOfPlayers} Players.txt";
        }

        /********************************************** Class Interface **********************************************/
        internal static int NoOfPlayers
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

        internal static int NoOfRoundsDesired
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

        internal static int NoOfMatchesPerRound { get; set; }
        internal static string OutputFile { get; set; }
        internal static int NoOfPossibleMatches { get; set; }

        /*************************************************** Private Fields ****************************************************/
        private static int _noOfPlayers;
        private static int _noOfRoundsDesired;
    }
}
