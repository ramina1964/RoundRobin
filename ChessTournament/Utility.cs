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

        internal static Player FindPlayer(List<Player> players, int id) => players.FirstOrDefault(p => p.Id == id);

        internal static IEnumerable<Player> FindGroup(HashSet<HashSet<Match>> matches, List<Player> players)
        {
            var player = players[0];
            var results = new List<Player> { player };
            while (true)
            {
                var partner = FindPlayerToMeet(matches, players, player.Id);
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
            var noOfPlayers = ProblemDesc.NoOfPlayers;
            var result = new List<Player>(noOfPlayers);
            for (var i = 0; i < noOfPlayers; i++)
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
                if (p2.Id - p1.Id == IdStep)
                { result.Add(innerList); }
            }

            result.Add(innerList);
            return result;
        }

        /*********************************************** Private Fields **********************************************/
        private static IList<int> GetPlayerIds(IEnumerable<Player> players) => players.Select(item => item.Id).ToList();

        private static Player FindPlayerToMeet(HashSet<HashSet<Match>> allMatches, List<Player> players, int id)
        {
            var player = FindPlayer(players, id);
            var index = players.IndexOf(player);

            for (var j = 0; j < allMatches.ElementAt(index).Count; j++)
            {
                if (allMatches.ElementAt(index).ElementAt(j).FstPLayerId == id &&
                    !allMatches.ElementAt(index).ElementAt(j).IsPlayed)
                    return allMatches.ElementAt(index).ElementAt(j).SndPlayer;
            }

            return null;
        }
    }
}