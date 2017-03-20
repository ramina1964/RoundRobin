using System;

namespace ChessTournament
{
	public class Program
	{
		private static void Main()
		{
			try
			{
				Console.WriteLine("Solution of Round Robin Setup\n");
				while (true)
				{
					Console.Write("Enter No. of Players:\t");
					var isValidInt = int.TryParse(Console.ReadLine(), out var noOfPlayers);
					if (!isValidInt)
						return;

					var noOfRounds = noOfPlayers - 1;
					var problemDesc = new ProblemDesc(noOfPlayers, noOfRounds);

					// Simulate the round setup
					var admin = new Admin(problemDesc);
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
