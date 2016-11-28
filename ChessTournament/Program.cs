using System;
using System.Collections.Generic;

namespace ChessTournament
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                Console.WriteLine("Solution of Round Robin Setup\n");

                Console.Write("Enter No. of Players:\t");
                var noOfPlayers = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter No. of Rounds:\t");
                var noOfRounbds = Convert.ToInt32(Console.ReadLine());

                var problemDesc = new ProblemDesc(noOfPlayers, noOfRounbds);

                var admin = new Admin();
                admin.ToFile();
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
