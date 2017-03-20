
namespace ChessTournament
{
	public class Match
	{
		/************************************************ Constructor ************************************************/
		public Match(Player fstPlayer, Player sndPlayer)
		{
			FstPlayer = fstPlayer;
			SndPlayer = sndPlayer;

			FstPLayerId = FstPlayer.Id;
			SndPlayerId = SndPlayer.Id;
			FstPlayerRank = FstPlayer.Rank;
			SndPlayerRank = SndPlayer.Rank;

			IsPlayed = (FstPLayerId == SndPlayerId);
		}

		/********************************************** Class Interface **********************************************/
		internal Player FstPlayer { get; }

		internal Player SndPlayer { get; }

		internal int FstPLayerId { get; }

		internal int SndPlayerId { get; }

		internal int FstPlayerRank { get; }

		internal int SndPlayerRank { get; }

		internal bool IsPlayed { get; set; }

		public override string ToString() => $"({FstPLayerId,3}, {SndPlayerId,3})";
	}
}
