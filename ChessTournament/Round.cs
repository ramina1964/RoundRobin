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

        private int RoundCost()
        {
            return _matches.Sum(item => Math.Abs(item.SndPlayerRank - item.FstPlayerRank));
        }

        private static HashSet<Match> SetupRound(HashSet<HashSet<Match>> allMatches, bool[,] isPlayed)
        {
            var matches = new HashSet<Match>();
            var isBusy = new bool[ProblemDesc.NoOfPlayers];
            int? startSndIdx = null;
            while (matches.Count < ProblemDesc.NoOfMatchesPerRound)
            {
                var match = ChooseMatch(allMatches, startSndIdx, isBusy, isPlayed);
                if (match == null && matches.Count == 0)
                    return matches;

                if (match == null)
                {
                    var lastMatch = matches.ElementAt(matches.Count - 1);
                    isBusy[lastMatch.FstPLayerIdx] = false;
                    isBusy[lastMatch.SndPlayerIdx] = false;
                    matches.Remove(lastMatch);
                    startSndIdx = lastMatch.SndPlayerIdx + 1;
                    continue;
                }

                matches.Add(match);
                startSndIdx = null;
            }
            return matches;
        }

        internal static Match ChooseMatch(HashSet<HashSet<Match>> matches, int? startSndIdx, bool[] isBusy, bool[,] isPlayed)
        {
            var fstPlayerIdx = FindFirstPlayer(isBusy);
            if (fstPlayerIdx == null)
                return null;

            if (startSndIdx == null)
                startSndIdx = fstPlayerIdx + 1;

            var roundMatches = matches.ElementAt(fstPlayerIdx.Value);
            foreach (var match in roundMatches.Where(match => !(match.SndPlayerIdx < startSndIdx) &&
                                                         !isBusy[match.SndPlayerIdx] &&
                                                         !isPlayed[match.FstPLayerIdx, match.SndPlayerIdx]))
            {
                isBusy[match.FstPLayerIdx] = true;
                isBusy[match.SndPlayerIdx] = true;
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

        private static int? FindFirstPlayer(IReadOnlyList<bool> isPlayerBusy)
        {
            var noOfPlayers = isPlayerBusy.Count;
            for (var index = 0; index < noOfPlayers; index++)
            {
                if (isPlayerBusy[index])
                    continue;

                return index;
            }
            return null;
        }
    }
}