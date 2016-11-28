using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessTournament
{
    public class ProblemDesc
    {
        // Min and Max values for MaxNoOfPlayers
        public const int MaxNoOfPlayers = 26;
        public const int MinNoOfPlayers = 4;

        // Min and Max values for NoOfRounds
        public const int MinNoOfRounds = 3;
        public static int MaxNoOfRounds { get; set; }

        public static int NoOfPlayers
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

        public static int NoOfRoundsDesired
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

        public static int NoOfMatchesPerRound { get; set; }
        public static string OutputFile { get; set; }
        public static int NoOfPossibleMatches { get; set; }

        public ProblemDesc(int noOfPlayers, int noOfRounds)
        {
            NoOfPlayers = noOfPlayers;
            MaxNoOfRounds = NoOfPlayers - 1;

            NoOfRoundsDesired = noOfRounds;
            NoOfMatchesPerRound = NoOfPlayers / 2;

            NoOfPossibleMatches = NoOfPlayers * (NoOfPlayers + 1) / 2;
            OutputFile = $"Results - {NoOfPlayers} Players.txt";
        }

        public static List<Player> InitializePlayers()
        {
            var result = new List<Player>(NoOfPlayers);
            for (var i = 0; i < NoOfPlayers; i++)
            {
                var p = new Player(i, i);
                result.Insert(i, p);
            }

            return result;
        }

        public static HashSet<HashSet<Match>> InitializeAllMatches(IEnumerable<Player> list)
        {
            var result = new HashSet<HashSet<Match>>();
            var enumerable = list as Player[] ?? list.ToArray();
            var count = enumerable.Count();

            const int seqLength = 2;
            for (var i = 0; i < count - seqLength + 1; i++)
            {
                var innerList = new HashSet<Match>();
                for (var j = i + 1; j <= count - 1; j++)
                {
                    var match = new Match(enumerable.ElementAt(i), enumerable.ElementAt(j));
                    innerList.Add(match);
                }

                result.Add(innerList);
            }

            return result;
        }

        private static int _noOfPlayers;
        private static int _noOfRoundsDesired;
    }
}
