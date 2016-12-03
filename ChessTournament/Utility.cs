using System.Collections.Generic;
using System.Linq;

namespace ChessTournament
{
    public class Utility
    {
        /********************************************** Class Interface **********************************************/

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

        /*********************************************** Private Fields **********************************************/

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