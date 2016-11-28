
namespace ChessTournament
{
    public class Match
    {
        public Match(Player fstPlayer, Player sndPlayer)
        {
            FstPLayerIdx = fstPlayer.Index;
            SndPlayerIdx = sndPlayer.Index;
            FstPlayerRank = fstPlayer.Rank;
            SndPlayerRank = sndPlayer.Rank;
        }

        public int FstPLayerIdx { get; }

        public int SndPlayerIdx { get; }

        public int FstPlayerRank { get; }

        public int SndPlayerRank { get; }

        public override string ToString()
        {
            return $"({FstPLayerIdx, 2}, {SndPlayerIdx, 2})";
        }
    }
}
