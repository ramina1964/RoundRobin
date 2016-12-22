using System.Collections.Generic;
using System.Linq;
using System.Text;

using Facet.Combinatorics;

namespace ChessTournament
{
	public class Utility
	{
		/********************************************** Class Interface **********************************************/
		internal const int IdStep = 1;

		internal const int StartPlayerId = 1;

		internal static Player FindPlayerById(int id, IEnumerable<Player> players) => players.FirstOrDefault(p => p.Id == id);

		internal static IEnumerable<Player> InitializePlayers
		{
			get
			{
				var result = new List<Player>(NoOfPlayers);
				for (var i = 0; i < NoOfPlayers; i++)
				{
					var index = StartPlayerId + i * IdStep;
					var p = new Player(index, i + 1);
					result.Insert(i, p);
				}

				return result;
			}
		}

		internal static IEnumerable<HashSet<Match>> InitializeAllMatches(IEnumerable<Player> players)
		{
			var pList = players.ToList();
			var idList = GetAllPlayerIds(pList);
			var result = new HashSet<HashSet<Match>>();

			// Establish a list containing all variations of length two in idList with repetion:
			// No. of elements in variations id N^2 for N elements in idList.
			var variations = new Variations<int>(idList, 2, GenerateOption.WithRepetition).ToList();

			var innerList = new HashSet<Match>();
			foreach (var item in variations)
			{
				var p1 = FindPlayerById(item[0], pList);
				var p2 = FindPlayerById(item[1], pList);
				var match = new Match(p1, p2);
				if (p2.Id == StartPlayerId && innerList.Count == NoOfPlayers)
				{
					result.Add(innerList);
					innerList = new HashSet<Match>() { match };
					continue;
				}

				innerList.Add(match);
			}

			result.Add(innerList);
			return result;
		}

		internal static void UpdateMatch(Match match, bool isPlayed)
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

		internal static IEnumerable<List<Player>> ExtractEqualPlayerLists(IEnumerable<List<Player>> partnerList)
		{
			var partners = partnerList.ToList();
			var count = partners.Count;
			var result = new List<List<Player>>(NoOfPlayers);
			for (var i = 0; i < count - 1; i++)
			{
				var fstPlayerList = partners[i];
				var localList = new List<List<Player>> { fstPlayerList };
				var isFound = false;
				for (var j = i + 1; j < count; j++)
				{
					var sndPlayerList = partners[j];
					if (!AreListsEqual(fstPlayerList, sndPlayerList))
					{ continue; }

					if (!localList.Contains(sndPlayerList))
					{ localList.Add(sndPlayerList); }

					isFound = true;
				}

				if (!isFound)
					continue;

				MergeFstToSndList(localList, result);
			}

			return result;
		}

		private static void MergeFstToSndList(IEnumerable<List<Player>> localList, ICollection<List<Player>> result)
		{
			foreach (var group in localList)
			{
				if (!result.Contains(group))
					result.Add(group);
			}
		}

		internal static string DisplayRemainigLists(IEnumerable<List<Player>> equalLists)
		{
			// var enumerables = equalLists as IList<List<Player>> ?? equalLists.ToList();
			var equals = equalLists.ToList();
			var lastIndex = equals[0].Count - 1;
			var sb = new StringBuilder("Remaining Groups:").AppendLine();
			foreach (var item in equals)
			{
				sb.Append($"Player No. {item[lastIndex].Id,3}: ");
				foreach (var player in item)
				{ sb.Append($"{player.Id,3} -> "); }
				sb.AppendLine();
			}

			return sb.ToString();
		}

		internal static IEnumerable<Match> FindAllMatchesFor(Player player, IEnumerable<HashSet<Match>> allMatches, IEnumerable<Player> players)
		{
			var pList = players.ToList();
			return allMatches.ElementAt(pList.IndexOf(player));
		}

		/*********************************************** Private Fields **********************************************/
		private static bool AreListsEqual(IEnumerable<Player> fstList, IEnumerable<Player> sndList)
		{ return fstList.All(fstItem => sndList.Any(sndItem => sndItem.Id == fstItem.Id)); }

		private static IList<int> GetAllPlayerIds(IEnumerable<Player> players) => players.Select(item => item.Id).ToList();

		private static int NoOfPlayers => ProblemDesc.NoOfPlayers;
	}
}