using System;

namespace ChessTournament
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                Console.WriteLine("Solution of Round Robin Setup\n");
                //Console.Write("Enter No. of Players:\t");
                //var noOfPlayers = Convert.ToInt32(Console.ReadLine());

                //Console.Write("Enter No. of Rounds:\t");
                //var noOfRounbds = Convert.ToInt32(Console.ReadLine());

                const int noOfPlayers = 24;
                const int noOfRounds = noOfPlayers - 1;

                var problemDesc = new ProblemDesc(noOfPlayers, noOfRounds);
                var admin = new Admin();

                Console.WriteLine(admin.ScreenSummary);
                Console.ReadLine();
            }

            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"\n{ex.Message}");
                Console.ReadLine();
            }

            catch (Exception ex)
            {
                Console.WriteLine($"\n{ex.Message}");
                Console.ReadLine();
            }
        }
    }
}
