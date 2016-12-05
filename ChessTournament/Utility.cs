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

        internal static Player FindPlayer(IEnumerable<Player> players, int id) => players.FirstOrDefault(p => p.Id == id);

        internal static IEnumerable<Player> FindGroup(HashSet<HashSet<Match>> matches, List<Player> players)
        {
            var player = players.ElementAt(0);
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

        internal static HashSet<HashSet<Match>> InitializeAllMatches(List<Player> players)
        {
            var idList = GetPlayerIds(players);
            var result = new HashSet<HashSet<Match>>();

            var comb = new Combinations<int>(idList, 2).ToList();

            var innerList = new HashSet<Match>();
            foreach (var item in comb)
            {
                var p1 = FindPlayer(players, item[0]);
                var p2 = FindPlayer(players, item[1]);
                var match = new Match(p1, p2);
                if (p2.Id - p1.Id == IdStep)
                {
                    if (innerList.Count > 0)
                    { result.Add(innerList); }
                    innerList = new HashSet<Match>() { match };
                    continue;
                }

                innerList.Add(match);
            }

            result.Add(innerList);
            return result;
        }

        internal static IEnumerable<Match> MatchesFor(Player player, IEnumerable<HashSet<Match>> allMatches, List<Player> players)
        {
            var roundIndex = players.IndexOf(player);
            return allMatches.ElementAt(roundIndex);
        }

        /*********************************************** Private Fields **********************************************/
        private static IList<int> GetPlayerIds(IEnumerable<Player> players) => players.Select(item => item.Id).ToList();

        private static Player FindPartnerFor(int id, IEnumerable<HashSet<Match>> allMatches, List<Player> players)
        {
            var player = FindPlayer(players, id);
            var playerMatches = MatchesFor(player, allMatches, players);

            return (from match in playerMatches
                    where match.FstPLayerId == id && !match.IsPlayed
                    select match.SndPlayer).FirstOrDefault();
        }

        private static int NoOfPlayers => ProblemDesc.NoOfPlayers;
    }
}