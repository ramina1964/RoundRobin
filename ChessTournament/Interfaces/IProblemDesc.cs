using System.Collections.Generic;
using ChessTournament.Model;

namespace ChessTournament.Interfaces
{
	public interface IProblemDesc
	{
		int NoOfPlayers { get; set; }
		int MaxNoOfRounds { get; set; }
		int NoOfRoundsDesired { get; set; }
		int NoOfMatchesPerRound { get; set; }
		int NoOfPossibleMatches { get; set; }
		string OutputFile { get; set; }

		HashSet<Player> Players { get; set; }
		IEnumerable<HashSet<Match>> AllMatches { get; }
	}
}
