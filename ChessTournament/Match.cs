
namespace ChessTournament
{
    public class Match
    {
        public Match(Player fstPlayer, Player sndPlayer)
        {
            FstPLayerId = fstPlayer.Id;
            SndPlayerId = sndPlayer.Id;
            FstPlayerRank = fstPlayer.Rank;
            SndPlayerRank = sndPlayer.Rank;
            IsPlayed = false;
        }

        public int FstPLayerId { get; }

        public int SndPlayerId { get; }

        public int FstPlayerRank { get; }

        public int SndPlayerRank { get; }

        public bool IsPlayed { get; set; }

        public override string ToString()
        {
            return $"({FstPLayerId, 2}, {SndPlayerId, 2})";
        }
    }
}
