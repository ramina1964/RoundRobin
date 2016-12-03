using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessTournament
{
    public class Round
    {
        /************************************************ Constructor ************************************************/
        public Round(HashSet<HashSet<Match>> matches, List<Player> players)
        {
            Players = players;
            NoOfPlayers = Players.Count;
            NoOfMatchesPerRound = ProblemDesc.NoOfMatchesPerRound;
            AllMatches = matches;

            RoundMatches = SetupRound();
            if (RoundMatches.Count == NoOfMatchesPerRound)
                Cost = RoundCost;
        }


        /********************************************** Class Interface **********************************************/
        internal int Cost { get; }

        public int NoOfPlayers { get; }

        public int NoOfMatchesPerRound { get; }

        //internal List<Match> GetMatches => RoundMatches.ToList();

        internal int Count => RoundMatches.Count;

        internal bool IsEmpty => RoundMatches.Count == 0;

        public override string ToString() => Display();

        /*********************************************** Private Fields **********************************************/
        private List<Player> Players { get; }

        private int RoundCost => RoundMatches.Sum(item => Math.Abs(item.SndPlayerRank - item.FstPlayerRank));

        private HashSet<HashSet<Match>> AllMatches { get; }


        /*********************************************** Private Fields **********************************************/
        private Match ChooseMatch(int? startSndId)
        {
            var fstPlayer = FindFreePlayer(Players);

            if (fstPlayer == null)
                return null;

            if (startSndId == null)
                startSndId = fstPlayer.Id + 1;

            var roundMatches = AllMatches.ElementAt(fstPlayer.Id);
            foreach (var match in roundMatches)
            {
                if ( match.SndPlayerId < startSndId || match.SndPlayer.IsBusy || match.IsPlayed)
                    continue;

                match.FstPlayer.IsBusy = true;
                match.SndPlayer.IsBusy = true;
                match.IsPlayed = true;
                return match;
            }

            return null;
        }

        private HashSet<Match> SetupRound()
        {
            var matches = new HashSet<Match>();
            int? startSndId = null;
            while (matches.Count < NoOfMatchesPerRound)
            {
                var match = ChooseMatch(startSndId);
                if (match == null && matches.Count == 0)
                    return matches;

                if (match == null)
                {
                    var lastMatch = matches.ElementAt(matches.Count - 1);
                    lastMatch.FstPlayer.IsBusy = false;
                    lastMatch.SndPlayer.IsBusy = false;
                    lastMatch.IsPlayed = false;
                    matches.Remove(lastMatch);
                    startSndId = lastMatch.SndPlayerId + 1;
                    continue;
                }

                match.IsPlayed = true;
                matches.Add(match);
                startSndId = null;
            }
            return matches;
        }

        private HashSet<Match> RoundMatches { get; }

        private string Display()
        {
            var sb = new StringBuilder();
            foreach (var aMatch in RoundMatches)
                sb.Append($"{aMatch}  ");

            return sb.ToString();
        }

        private static Player FindFreePlayer(IEnumerable<Player> players) => players.FirstOrDefault(player => !player.IsBusy);
    }
}
 