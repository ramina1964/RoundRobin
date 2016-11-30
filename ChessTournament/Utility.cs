using System.Collections.Generic;
using System.Linq;

namespace ChessTournament
{
    public class Utility
    {
        private static readonly int NoOfPlayers = ProblemDesc.NoOfPlayers;

        internal static bool[,] InitializeRoundMatches(int noOfPlayers)
        {
            var isMatchPlayed = new bool[noOfPlayers, noOfPlayers];
            for (var i = 0; i < noOfPlayers; i++)
                isMatchPlayed[i, i] = true;

            return isMatchPlayed;
        }

        internal static void UpdatedMatches(IEnumerable<Match> playedMatches, bool[,] isMatchPlayed)
        {
            foreach (var match in playedMatches.ToList())
            {
                isMatchPlayed[match.FstPLayerId, match.SndPlayerId] = true;
                isMatchPlayed[match.SndPlayerId, match.FstPLayerId] = true;
            }
        }

        internal static Player FindPlayer(List<Player> players, int id) => players.FirstOrDefault(p => p.Id == id);

        internal static List<Player> FindPlayersToMeet(int id, HashSet<HashSet<Match>> matches)
        {
            var result = new List<Player>(NoOfPlayers - 1);
            result.AddRange( from match in matches.ElementAt(id) where !match.IsPlayed select match.SndPlayer );

            return result;
        }

        //internal static IEnumerable<int> FindGroup(bool[,] isPlayed)
        //{
        //    const int startId = 0;
        //    var idx = startId;

        //    var results = new List<int> { startId };
        //    while (true)
        //    {
        //        var partners = FindPlayersToMeet(idx, isPlayed);
        //        var found = false;
        //        foreach (var id in partners.Where(player => !results.Contains(player)))
        //        {
        //            results.Add(id);
        //            idx = id;
        //            found = true;
        //            break;
        //        }

        //        if (!found)
        //            return results;
        //    }
        //}

    }
}