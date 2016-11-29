using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessTournament
{
    public class Round
    {
        private readonly HashSet<Match> _matches;

        public Round(HashSet<HashSet<Match>> matches, bool[,] isPlayed)
        {
            _matches = SetupRound(matches, isPlayed);

            if (_matches.Count == ProblemDesc.NoOfMatchesPerRound)
                Cost = RoundCost();
        }

        internal int Cost { get; }
        internal List<Match> GetMatches => _matches.ToList();
        internal int Count => _matches.Count;
        internal bool IsEmpty => _matches.Count == 0;

        private int RoundCost() => _matches.Sum(item => Math.Abs(item.SndPlayerRank - item.FstPlayerRank));

        private static HashSet<Match> SetupRound(HashSet<HashSet<Match>> allMatches, bool[,] isPlayed)
        {
            var matches = new HashSet<Match>();
            var isBusy = new bool[ProblemDesc.NoOfPlayers];
            int? startSndId = null;
            while (matches.Count < ProblemDesc.NoOfMatchesPerRound)
            {
                var match = ChooseMatch(allMatches, startSndId, isBusy, isPlayed);
                if (match == null && matches.Count == 0)
                    return matches;

                if (match == null)
                {
                    var lastMatch = matches.ElementAt(matches.Count - 1);
                    isBusy[lastMatch.FstPLayerId] = false;
                    isBusy[lastMatch.SndPlayerId] = false;
                    matches.Remove(lastMatch);
                    startSndId = lastMatch.SndPlayerId + 1;
                    continue;
                }

                matches.Add(match);
                startSndId = null;
            }
            return matches;
        }

        internal static Match ChooseMatch(HashSet<HashSet<Match>> matches, int? startSndId, bool[] isBusy, bool[,] isPlayed)
        {
            var fstPlayerId = FindFreePlayerId(isBusy);
            if (fstPlayerId == null)
                return null;

            if (startSndId == null)
                startSndId = fstPlayerId + 1;

            var roundMatches = matches.ElementAt(fstPlayerId.Value);
            foreach (var match in roundMatches.Where(match => !(match.SndPlayerId < startSndId) &&
                                                         !isBusy[match.SndPlayerId] &&
                                                         !isPlayed[match.FstPLayerId, match.SndPlayerId]))
            {
                isBusy[match.FstPLayerId] = true;
                isBusy[match.SndPlayerId] = true;
                return match;
            }

            return null;
        }

        private string Display()
        {
            var sb = new StringBuilder();
            foreach (var aMatch in _matches)
                sb.Append($"{aMatch}  ");

            return sb.ToString();
        }

        public override string ToString()
        {
            return Display();
        }

        private static int? FindFreePlayerId(IReadOnlyList<bool> isPlayerBusy)
        {
            var noOfPlayers = isPlayerBusy.Count;
            for (var id = 0; id < noOfPlayers; id++)
            {
                if (isPlayerBusy[id])
                    continue;

                return id;
            }
            return null;
        }
    }
}