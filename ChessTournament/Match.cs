﻿
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
            IsPlayed = false;
        }

        /********************************************** Class Interface **********************************************/
        public Player FstPlayer { get; set; }

        public Player SndPlayer { get; set; }

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
