using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using Facet.Combinatorics;

namespace ChessTournament
{
	public class Utility
	{
		/********************************************** Class Interface **********************************************/
		internal const int IdStep = 1;

		internal const int StartPlayerId = 1;

		internal static Player FindPlayerById(IEnumerable<Player> players, int id) => players.FirstOrDefault(p => p.Id == id);

		internal static IEnumerable<Player> FindGroup(List<List<Match>> matches, List<Player> players)
		{
			var player = players.First();
			var results = new List<Player> { player };
			while (true)
			{
				var partner = FindPartnerFor(player.Id, matches, players);
				if (partner != null && !results.Contains(partner))
				{
					results.Add(partner);
					player = partner;
				}

				if (partner == null)
					return results;
			}
		}

		internal static List<Player> InitializePlayers()
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

		internal static List<List<Match>> InitializeAllMatches(List<Player> players)
		{
			var idList = GetAllPlayerIds(players);
			var result = new List<List<Match>>();

			// Establish a list containing all variations of length two in idList with repetion:
			// No. of elements in variations id N^2 for N elements in idList.
			var variations = new Variations<int>(idList, 2, GenerateOption.WithRepetition).ToList();

			var innerList = new List<Match>();
			foreach (var item in variations)
			{
				var p1 = FindPlayerById(players, item[0]);
				var p2 = FindPlayerById(players, item[1]);
				var match = new Match(p1, p2);
				if (p2.Id == StartPlayerId && innerList.Count == NoOfPlayers)
				{
					result.Add(innerList);
					innerList = new List<Match>() { match };
					continue;
				}

				innerList.Add(match);
			}

			result.Add(innerList);
			return result;
		}

		internal static IEnumerable<Match> FindMatchesFor(Player player, List<List<Match>> allMatches, List<Player> players)
			=> allMatches[players.IndexOf(player)];

		/*********************************************** Private Fields **********************************************/
		private static IList<int> GetAllPlayerIds(IEnumerable<Player> players) => players.Select(item => item.Id).ToList();

		private static Player FindPartnerFor(int id, List<List<Match>> allMatches, List<Player> players)
		{
			var player = FindPlayerById(players, id);
			var playerMatches = FindMatchesFor(player, allMatches, players);

			return (from match in playerMatches
					where match.FstPLayerId == id && !match.IsPlayed
					select match.SndPlayer).FirstOrDefault();
		}

		private static int NoOfPlayers => ProblemDesc.NoOfPlayers;
	}
}