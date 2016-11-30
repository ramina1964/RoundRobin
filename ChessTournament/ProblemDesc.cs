using System;
using System.Collections.Generic;
using System.Linq;
using Facet.Combinatorics;

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

            // NoOfPossibleMatches = NoOfPlayers / ( 2! * (NoOfPlayers - 2)! )
            NoOfPossibleMatches = NoOfPlayers * (NoOfPlayers - 1) / 2;
            OutputFile = $"Results - {NoOfPlayers} Players.txt";
        }

        public static List<Player> InitializePlayers()
        {
            var result = new List<Player>(NoOfPlayers);
            for (var i = 0; i < NoOfPlayers; i++)
            {
                var p = new Player(i, i + 1);
                result.Insert(i, p);
            }

            return result;
        }

        public static HashSet<HashSet<Match>> InitializeAllMatches(List<Player> players)
        {
            var idList = GetPlayerIds(players);
            var result = new HashSet<HashSet<Match>>();

            var comb = new Combinations<int>(idList, 2).ToList();

            var innerList = new HashSet<Match>();
            foreach (var item in comb)
            {
                var p1 = Utility.FindPlayer(players, item[0]);
                var p2 = Utility.FindPlayer(players, item[1]);
                var match = new Match(p1, p2);
                if (p2.Id - p1.Id == 1)
                {
                    if (innerList.Count > 0)
                    { result.Add(innerList); }
                    innerList = new HashSet<Match>() {match};
                    continue;
                }

                innerList.Add(match);
                if (p2.Id - p1.Id == 1)
                { result.Add(innerList); }
            }

            result.Add(innerList);
            return result;
        }

        private static IList<int> GetPlayerIds(IEnumerable<Player> players)
        {
            return players.Select(item => item.Id).ToList();
        }

        private static int _noOfPlayers;
        private static int _noOfRoundsDesired;
    }
}
