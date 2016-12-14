using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static System.IO.File;

namespace ChessTournament
{
	internal class Admin
	{
		/************************************************ Constructors ***********************************************/
		internal Admin()
		{
			NoOfPlayers = ProblemDesc.NoOfPlayers;
			MaxNoOfRounds = ProblemDesc.MaxNoOfRounds;
			NoOfRoundsDesired = ProblemDesc.NoOfRoundsDesired;
			NoOfMatchesPerRound = ProblemDesc.NoOfMatchesPerRound;
			NoOfPossibleMatches = ProblemDesc.NoOfPossibleMatches;
			OutputFile = ProblemDesc.OutputFile;

			Players = Utility.InitializePlayers();
			AllMatches = Utility.InitializeAllMatches(Players);

			TriedRounds = EstimateElapsedTime();
			Rounds = GetCompletedRounds;
			IsDesiredNoOfRoundsMet = Rounds.Count == NoOfRoundsDesired;

			NoOfActualRounds = Rounds.Count;
			NoOfMatchesPlayed = Rounds.Sum(aRound => aRound.Count);

			ToFile();
			ScreenSummary = GetSummary(OutputMedium.Screen);
		}

		/********************************************** Class Interface **********************************************/
		internal int NoOfPlayers { get; }

		internal int NoOfRoundsDesired { get; }

		internal int MaxNoOfRounds { get; }

		internal List<Round> GetCompletedRounds => TriedRounds.Where(aRound => aRound.Count == NoOfMatchesPerRound).ToList();

		internal int NoOfMatchesPerRound { get; }

		internal int NoOfPossibleMatches { get; }

		internal string OutputFile { get; }

		internal string ScreenSummary { get; }

		internal List<Round> Rounds { get; }

		internal bool IsDesiredNoOfRoundsMet { get; }

		internal int NoOfActualRounds { get; }

		internal int NoOfMatchesPlayed { get; }

		internal int ElapsedSeconds { get; set; }

		internal string GetSummary(OutputMedium outputMedium)
		{
			var title = $"Summary of the Results\t\t\t@{DateTime.Now}";

			var fstLine = $"No. Of Players:\t\t{NoOfPlayers,4}\t\t" +
							$"Desired Rounds:\t\t{NoOfRoundsDesired,4}\t\tActual Rounds:\t\t{NoOfActualRounds,5}";

			var sndLine = $"Possible Matches:\t{NoOfPossibleMatches,4}\t\tActual Matches:\t\t" +
						  $"{NoOfMatchesPlayed,4}\t\tElapsed Time(s):\t{ElapsedSeconds,5}";

			var lastLine = $"Results are written to a File called \"{OutputFile}\".";
			var sb = new StringBuilder().AppendLine(title).AppendLine(fstLine).AppendLine(sndLine).AppendLine();

			if (outputMedium == OutputMedium.Screen)
				sb.AppendLine(lastLine);

			return sb.ToString();
		}

		public override string ToString()
		{
			return $"Desired No. of Rounds:\t{(IsDesiredNoOfRoundsMet ? "Met" : "Not Met")}\n" +
				   "Results are written into the output file.";
		}

		/*************************************************** Private Methds ****************************************************/
		/*************************************************** Private Methds ****************************************************/
		private string GetRemainingGroup()
		{
			return IsRoundSetUpPossible() ?
				"No Remaining Groups" :
				"There are Remaining Groups!";
		}

		private bool IsRoundSetUpPossible()
		{
			var playersNotMet = Players.Select(NotPlayedAgainst).ToList();
			var equalPlayerList = ExtractEqualPlayerLists(playersNotMet);
			return equalPlayerList.All(playerList => playerList.Count == 1 || playerList.Count % 2 == 0);
		}

		private IEnumerable<List<Player>> ExtractEqualPlayerLists(IReadOnlyList<List<Player>> playersNotMet)
		{
			var result = new List<List<Player>>() { playersNotMet[0] };
			for (var i = 0; i < result.Count - 1; i++)
			{
				var fstPlayerList = playersNotMet[i];
				var sndPlayerList = playersNotMet[i + 1];
				if (IsEqualList(fstPlayerList, sndPlayerList))
					result.Add(sndPlayerList);
			}

			return result;
		}

		private bool IsEqualList(List<Player> fstPlayerList, List<Player> sndPlayerList)
		{
			var fstListLength = fstPlayerList.Count;
			var sndListLength = sndPlayerList.Count;
			if (fstListLength != sndListLength)
				return false;

			for (var i = 0; i < sndListLength; i++)
			{
				if (fstPlayerList[i].Id != sndPlayerList[i].Id)
					return false;
			}

			return true;
		}

		private List<Player> NotPlayedAgainst(Player player)
		{
			var result = new List<Player>(NoOfPlayers);
			var potentialMatches = Utility.MatchesFor(player, AllMatches, Players);
			if (potentialMatches == null)
				return null;

			result.AddRange(
				from match in potentialMatches
				where !match.IsPlayed
				select match.SndPlayer);

			result.Add(player);
			return result;
		}

		private void ToFile()
		{
			var fileSummary = GetSummary(OutputMedium.File);
			var remainingGroup = GetRemainingGroup();
			var content = DisplayResults(fileSummary, remainingGroup);

			WriteAllText(OutputFile, content);
		}

		private string DisplayResults(string summary, string remainingGroup = "")
		{
			var result = new StringBuilder().AppendLine(summary);
			foreach (var round in Rounds)
			{
				var cost = round.Cost;
				var roundNo = Rounds.IndexOf(round) + 1;
				var content = $"R {roundNo,4}:\t{round,3} [Cost:{cost,4}]";
				result.AppendLine(content);
			}

			if (!IsDesiredNoOfRoundsMet)
				result.AppendLine().Append(remainingGroup).AppendLine();

			return result.ToString();
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
			for (var roundNo = 0; roundNo < NoOfRoundsDesired; roundNo++)
			{
				var aRound = new Round(AllMatches, Players);
				if (aRound.Count != NoOfMatchesPerRound)
					break;

				rounds.Add(aRound);
				ResetPlayers();
			}

			return rounds;
		}

		/*************************************************** Private Fields ****************************************************/
		private HashSet<HashSet<Match>> AllMatches { get; }

		private HashSet<Round> TriedRounds { get; }

		private List<Player> Players { get; }

		private void ResetPlayers() => Players.ForEach(p => p.IsBusy = false);
	}
}
