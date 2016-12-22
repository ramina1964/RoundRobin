using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessTournament
{
	internal class Round
	{
		/************************************************ Constructor ************************************************/
		internal Round(HashSet<HashSet<Match>> allMatches, List<Player> players)
		{
			Players = players;
			NoOfPlayers = Players.Count;
			NoOfMatchesPerRound = ProblemDesc.NoOfMatchesPerRound;
			AllMatches = allMatches;

			RoundMatches = SetupRound();
			if (RoundMatches.Count == NoOfMatchesPerRound)
				Cost = RoundCost;
		}

		/********************************************** Class Interface **********************************************/

		internal int Cost { get; }

		public int NoOfPlayers { get; }

		public int NoOfMatchesPerRound { get; }

		internal int Count => RoundMatches.Count;

		internal bool IsEmpty => RoundMatches.Count == 0;

		public override string ToString() => Display();

		/*********************************************** Private Fields **********************************************/
		private List<Player> Players { get; }

		private int RoundCost => RoundMatches.Sum(item => Math.Abs(item.SndPlayerRank - item.FstPlayerRank));

		private HashSet<HashSet<Match>> AllMatches { get; }

		/*********************************************** Private Fields **********************************************/
		private List<Match> SetupRound()
		{
			var matches = new List<Match>();
			int? startSndId = null;
			while (matches.Count < NoOfMatchesPerRound)
			{
				var match = ChooseMatch(startSndId);
				if (match == null && matches.Count == 0)
					return matches;

				if (match == null)
				{
					var lastMatch = matches.Last();
					Utility.UpdateMatch(lastMatch, false);
					matches.Remove(lastMatch);
					startSndId = lastMatch.SndPlayerId + IdStep;
					continue;
				}

				Utility.UpdateMatch(match, true);
				matches.Add(match);
				startSndId = null;
			}

			UpdateDualMatches(matches);
			return matches;
		}

		private void UpdateDualMatches(List<Match> matches)
		{
			foreach (var match in matches)
			{
				var dualMatch = FindDualMatch(match);
				Utility.UpdateMatch(dualMatch, true);
			}
		}

		private Match FindDualMatch(Match match)
		{
			var matches = Utility.FindAllMatchesFor(match.SndPlayer, AllMatches, Players);
			return matches.FirstOrDefault(item => item.SndPlayerId == match.FstPLayerId);
		}

		private Match ChooseMatch(int? startSndId)
		{
			var fstPlayer = FindFreePlayer();
			var playerMatches = Utility.FindAllMatchesFor(fstPlayer, AllMatches, Players);

			if (fstPlayer == null)
				return null;

			if (startSndId == null)
				startSndId = fstPlayer.Id + IdStep;

			return playerMatches.FirstOrDefault(match =>
				!(match.SndPlayerId < startSndId) && !match.SndPlayer.IsBusy && !match.IsPlayed);
		}

		private List<Match> RoundMatches { get; }

		private string Display()
		{
			var sb = new StringBuilder();
			foreach (var aMatch in RoundMatches)
				sb.Append($"{aMatch}  ");

			return sb.ToString();
		}

		private Player FindFreePlayer() => Players.FirstOrDefault(player => !player.IsBusy);

		private const int IdStep = Utility.IdStep;
	}
}
