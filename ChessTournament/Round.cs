using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessTournament
{
	internal class Round
	{
		/************************************************ Constructor ************************************************/
		internal Round(List<List<Match>> matches, List<Player> players)
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

		internal int Count => RoundMatches.Count;

		internal bool IsEmpty => RoundMatches.Count == 0;

		public override string ToString() => Display();

		/*********************************************** Private Fields **********************************************/
		private List<Player> Players { get; }

		private int RoundCost => RoundMatches.Sum(item => Math.Abs(item.SndPlayerRank - item.FstPlayerRank));

		private List<List<Match>> AllMatches { get; }

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
					UpdateMatch(lastMatch, false);
					matches.Remove(lastMatch);
					startSndId = lastMatch.SndPlayerId + IdStep;
					continue;
				}

				UpdateMatch(match, true);
				matches.Add(match);
				startSndId = null;
			}

			UpdateDualMatches(matches);
			return matches;
		}

		private void UpdateMatch(Match match, bool isPlayed)
		{
			if (isPlayed)
			{
				match.FstPlayer.IsBusy = true;
				match.SndPlayer.IsBusy = true;
				match.IsPlayed = true;
				return;
			}

			match.FstPlayer.IsBusy = false;
			match.SndPlayer.IsBusy = false;
			match.IsPlayed = false;
		}

		private void UpdateDualMatches(List<Match> matches)
		{
			foreach (var match in matches)
			{
				var dualMatch = FindDualMatch(Players, match);
				UpdateMatch(dualMatch, true);
			}
		}

		private Match FindDualMatch(List<Player> players, Match match)
		{
			var matches = Utility.FindMatchesFor(match.SndPlayer, AllMatches, players);
			return matches.FirstOrDefault(item => item.SndPlayerId == match.FstPLayerId);
		}


		private Match ChooseMatch(int? startSndId)
		{
			var fstPlayer = FindFreePlayer();
			var playerMatches = Utility.FindMatchesFor(fstPlayer, AllMatches, Players);

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
