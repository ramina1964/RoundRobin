using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static System.IO.File;
using static System.String;

namespace ChessTournament
{
    public class Admin
    {
        private readonly HashSet<HashSet<Match>> _allMatches;
        private readonly bool[,] _isMatchPlayed;
        private readonly HashSet<Round> _rounds;

        public Admin()
        {
            _allMatches = ProblemDesc.InitializeAllMatches();

            _isMatchPlayed = Utility.InitializeRoundMatches(ProblemDesc.NoOfPlayers);
            _rounds = EstimateElapsedTime();
            Rounds = GetRounds;
            IsDesiredNoOfRoundsMet = Rounds.Count == ProblemDesc.NoOfRoundsDesired;

            NoOfActualRounds = Rounds.Count;
            NoOfMatchesPlayed = GetNoOfPlayedMatches();

            Summary = GetSummary();
            RemainingGroup = GetRemainingGroup();
        }

        internal List<Round> GetRounds
            => _rounds.Where(aRound => aRound.Count == ProblemDesc.NoOfMatchesPerRound).ToList();

        internal string Summary { get; }
        internal List<Round> Rounds { get; }
        internal bool IsDesiredNoOfRoundsMet { get; }
        internal int NoOfActualRounds { get; }
        internal int NoOfMatchesPlayed { get; }
        internal int ElapsedSeconds { get; set; }
        internal string RemainingGroup { get; }

        internal string GetRemainingGroup()
        {
            if (NoOfActualRounds == ProblemDesc.MaxNoOfRounds)
                return Empty;

            var sb = new StringBuilder().Append("Remaining Group:").AppendLine();
            var group = Utility.FindGroup(_isMatchPlayed);
            foreach (var item in group)
                sb.Append($"{item, 2} -> ");

            return sb.AppendLine().ToString();
        }

        internal int GetNoOfPlayedMatches()
        {
            return Rounds.Sum(aRound => aRound.Count);
        }

        private HashSet<Round> EstimateElapsedTime()
        {
            var watch = Stopwatch.StartNew();
            var rounds = SetupRounds();
            watch.Stop();
            ElapsedSeconds = (int)watch.ElapsedMilliseconds / 1000;
            return rounds;
        }

        private HashSet<Round> SetupRounds()
        {
            var rounds = new HashSet<Round>();
            for (var roundNo = 0; roundNo < ProblemDesc.NoOfRoundsDesired; roundNo++)
            {
                var aRound = new Round(_allMatches, _isMatchPlayed);
                if (aRound.Count != ProblemDesc.NoOfMatchesPerRound)
                    break;

                Utility.UpdatedMatches(aRound.GetMatches, _isMatchPlayed);
                rounds.Add(aRound);
            }

            return rounds;
        }

        internal string DisplayAllMathes()
        {
            var sb = new StringBuilder();
            foreach (var match in _allMatches)
                sb.AppendLine(match.ToString());

            return sb.ToString();
        }

        internal void ToFile()
        {
            var content = DisplayRounds(Rounds, Summary, RemainingGroup);
            WriteAllText(ProblemDesc.OutputFile, content);
        }

        private string DisplayRounds(IReadOnlyCollection<Round> rounds, string summary, string remainingGroup)
        {
            var result = new StringBuilder().AppendLine(summary);
            for (var roundNo = 0; roundNo < rounds.Count; roundNo++)
            {
                var aRound = rounds.ElementAt(roundNo);
                var cost = aRound.Cost;
                var content = $"R {roundNo + 1, 4}:\t{aRound, 2} [Cost:{cost, 4}]";

                result.AppendLine(content);
            }

            if (!IsDesiredNoOfRoundsMet)
                result.AppendLine().Append(remainingGroup).AppendLine();

            return result.ToString();
        }

        internal string GetSummary()
        {
            var sb = new StringBuilder().AppendLine($"\nSummary of the Results\t\t\t@{DateTime.Now}");
            sb.AppendLine($"No. Of Players:\t\t{ProblemDesc.NoOfPlayers,4}\t\t" +
                          $"Desired Rounds:\t\t{ProblemDesc.NoOfRoundsDesired,4}\t\tActual Rounds:\t\t{NoOfActualRounds,5}");

            var lastLine = $"Possible Matches:\t{ProblemDesc.NoOfPossibleMatches,4}\t\tActual Matches:\t\t" +
                           $"{NoOfMatchesPlayed,4}\t\tElapsed Time(s):\t{ElapsedSeconds,5}";
            return sb.AppendLine(lastLine).ToString();
        }

        public override string ToString()
        {
            return $"Desired No. of Rounds:\t{(IsDesiredNoOfRoundsMet ? "Met" : "Not Met")}\n" +
                   "Results are written into the output file.";
        }
    }
}
