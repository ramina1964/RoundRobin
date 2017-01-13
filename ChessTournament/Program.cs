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
				while (true)
				{
					Console.Write("Enter No. of Players:\t");
					var noOfPlayers = Convert.ToInt32(Console.ReadLine());
					var noOfRounds = noOfPlayers - 1;
					var problemDesc = new ProblemDesc(noOfPlayers, noOfRounds);

					// Simulate the round setup
					var admin = new Admin();
					admin.Simulate();

					// Present the results
					Console.WriteLine(admin.ScreenSummary);
				}
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
