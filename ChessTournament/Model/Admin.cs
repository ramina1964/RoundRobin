using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ChessTournament.Enums;

namespace ChessTournament.Model
{
	public class Admin
	{
		/************************************************ Constructors ***********************************************/
		public Admin(Interfaces.IProblemDesc problemDesc)
		{
			_problemDesc = problemDesc;
			NoOfPlayers = problemDesc.NoOfPlayers;
			MaxNoOfRounds = problemDesc.MaxNoOfRounds;
			NoOfRoundsDesired = problemDesc.NoOfRoundsDesired;
			NoOfMatchesPerRound = problemDesc.NoOfMatchesPerRound;
			NoOfPossibleMatches = problemDesc.NoOfPossibleMatches;
			OutputFile = problemDesc.OutputFile;

			Players = problemDesc.Players;
			AllMatches = problemDesc.AllMatches;
		}

		/********************************************** Class Interface **********************************************/
		internal int NoOfPlayers { get; }

		internal int NoOfRoundsDesired { get; }

		internal int MaxNoOfRounds { get; }

		public int NoOfMatchesPerRound { get; }

		public int NoOfPossibleMatches { get; }

		internal string OutputFile { get; }

		internal string ScreenSummary { get; set; }

		public List<Round> Rounds { get; set; }

		public bool IsDesiredNoOfRoundsMet { get; set; }

		public int NoOfActualRounds { get; set; }

		public int NoOfMatchesPlayed { get; set; }

		public int ElapsedTimeInSec { get; set; }

		public void Simulate()
		{
			TriedRounds = EstimateElapsedTime().ToList();
			Rounds = GetCompletedRounds.ToList();
			IsDesiredNoOfRoundsMet = Rounds.Count == NoOfRoundsDesired;

			NoOfActualRounds = Rounds.Count;
			NoOfMatchesPlayed = Rounds.Sum(aRound => aRound.Count);

			ToFile();
			ScreenSummary = GetSummary(OutputMedium.Screen);
		}

		public override string ToString()
		{
			return $"Desired No. of Rounds:\t{(IsDesiredNoOfRoundsMet ? "Met" : "Not Met")}\n" +
				   "Results are written into the output file.";
		}

		/*************************************************** Private Methds ****************************************************/
		private IEnumerable<Round> GetCompletedRounds => TriedRounds.Where(aRound => aRound.Count == NoOfMatchesPerRound);

		private string GetSummary(OutputMedium outputMedium)
		{
			var title = $"\nSummary of the Results\t\t\t@{DateTime.Now}";

			var fstLine = $"No. Of Players:\t\t{NoOfPlayers,4}\t\t" +
							$"Desired Rounds:\t\t{NoOfRoundsDesired,4}\t\tActual Rounds:\t\t{NoOfActualRounds,5}";

			var sndLine = $"Possible Matches:\t{NoOfPossibleMatches,4}\t\tActual Matches:\t\t" +
						  $"{NoOfMatchesPlayed,4}\t\tElapsed Time(s):\t{ElapsedTimeInSec,5}";

			var lastLine = $"Results are written to a File called \"{OutputFile}\".";
			var sb = new StringBuilder().AppendLine(title).AppendLine(fstLine).AppendLine(sndLine);

			if (outputMedium == OutputMedium.Screen)
				sb.AppendLine(lastLine);

			return sb.AppendLine().ToString();
		}

		private string GetRemainingGroup()
		{
			if (IsDesiredNoOfRoundsMet)
				return string.Empty;

			var potentialPartners = Players.Select(PotentialPartners);
			var equalLists = Utility.ExtractEqualPlayerLists(potentialPartners).Where(plist => plist.Count % 2 != 0);
			return Utility.DisplayRemainigLists(equalLists);
		}

		private HashSet<Player> PotentialPartners(Player player)
		{
			var result = new HashSet<Player>();
			var playerMatches = Utility.FindAllMatchesFor(player, AllMatches, Players).ToList();
			if (playerMatches.Count == 0)
				return null;

			foreach (var match in playerMatches.Where(match => !match.IsPlayed))
			{ result.Add(match.SndPlayer); }

			result.Add(player);
			return result;
		}

		private void ToFile()
		{
			var fileSummary = GetSummary(OutputMedium.File);
			var remainingGroup = GetRemainingGroup();
			var content = DisplayResults(fileSummary, remainingGroup);

			File.WriteAllText(OutputFile, content);
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

		private IEnumerable<Round> EstimateElapsedTime()
		{
			var watch = Stopwatch.StartNew();
			var rounds = SetupRounds();
			watch.Stop();
			ElapsedTimeInSec = (int)watch.ElapsedMilliseconds / 1000;
			return rounds;
		}

		private IEnumerable<Round> SetupRounds()
		{
			var rounds = new List<Round>();
			for (var roundNo = 0; roundNo < NoOfRoundsDesired; roundNo++)
			{
				var aRound = new Round(_problemDesc);
				aRound.Setup();
				if (aRound.Count != NoOfMatchesPerRound)
					break;

				rounds.Add(aRound);
				ResetPlayers();
			}

			return rounds;
		}

		/*************************************************** Private Fields ****************************************************/
		private readonly Interfaces.IProblemDesc _problemDesc;

		public IEnumerable<HashSet<Match>> AllMatches { get; }

		private List<Round> TriedRounds { get; set; }

		private HashSet<Player> Players { get; }

		private void ResetPlayers() => Players.ToList().ForEach(p => p.IsBusy = false);
	}
}
