using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessTournament
{
    public class Round
    {
        /************************************************ Constructor ************************************************/
        public Round(HashSet<HashSet<Match>> matches)
        {
            Matches = SetupRound(matches);

            if (Matches.Count == ProblemDesc.NoOfMatchesPerRound)
                Cost = RoundCost();
        }

        /********************************************** Class Interface **********************************************/
        internal int Cost { get; }
        internal List<Match> GetMatches => Matches.ToList();
        internal int Count => Matches.Count;
        internal bool IsEmpty => Matches.Count == 0;

        public override string ToString() => Display();


        /*********************************************** Private Fields **********************************************/
        private int RoundCost() => Matches.Sum(item => Math.Abs(item.SndPlayerRank - item.FstPlayerRank));

        private static Match ChooseMatch(IEnumerable<HashSet<Match>> matches, int? startSndId, bool[] isBusy)
        {
            var fstPlayerId = FindFreePlayerId(isBusy);
            if (fstPlayerId == null)
                return null;

            if (startSndId == null)
                startSndId = fstPlayerId + 1;

            var roundMatches = matches.ElementAt(fstPlayerId.Value);
            foreach (var match in roundMatches)
            {
                if ( match.SndPlayerId < startSndId || isBusy[match.SndPlayerId] || match.IsPlayed)
                    continue;

                isBusy[match.FstPLayerId] = true;
                isBusy[match.SndPlayerId] = true;
                match.IsPlayed = true;
                return match;
            }

            return null;
        }

        private static HashSet<Match> SetupRound(HashSet<HashSet<Match>> allMatches)
        {
            var matches = new HashSet<Match>();
            var isBusy = new bool[ProblemDesc.NoOfPlayers];
            int? startSndId = null;
            while (matches.Count < ProblemDesc.NoOfMatchesPerRound)
            {
                var match = ChooseMatch(allMatches, startSndId, isBusy);
                if (match == null && matches.Count == 0)
                    return matches;

                if (match == null)
                {
                    var lastMatch = matches.ElementAt(matches.Count - 1);
                    isBusy[lastMatch.FstPLayerId] = false;
                    isBusy[lastMatch.SndPlayerId] = false;
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


        private HashSet<Match> Matches { get; }

        private string Display()
        {
            var sb = new StringBuilder();
            foreach (var aMatch in Matches)
                sb.Append($"{aMatch}  ");

            return sb.ToString();
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
 