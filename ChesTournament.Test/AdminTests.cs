using System.Collections.Generic;
using System.Linq;
using ChessTournament.Model;
using NUnit.Framework;

namespace ChesTournament.Test
{
	[TestFixture]
	public class AdminTests
	{
		[TestCase(4), TestCase(6), TestCase(8), TestCase(10), TestCase(12), TestCase(14)]
		[TestCase(16), TestCase(18), TestCase(20), TestCase(22), TestCase(24)]
		public void ShouldGenerateCorrectNoOfActualRounds(int noOfPlayers)
		{
			// Arrange
			var noOfDesiredRounds = noOfPlayers - 1;
			var problemDesc = new ProblemDesc(noOfPlayers, noOfDesiredRounds);
			var expected = noOfDesiredRounds;
			var sut = new Admin(problemDesc);

			// Act
			sut.Simulate();
			var actual = sut.NoOfActualRounds;

			// Assert
			Assert.That(actual, Is.EqualTo(expected));
		}

        [TestCase(4)]
        public void Should_generate_correct_rounds(int noOfPlayers)
        {
            // Arrange
            var noOfDesiredRounds = noOfPlayers - 1;
            var problemDesc = new ProblemDesc(noOfPlayers, noOfDesiredRounds);
            var sut = new Admin(problemDesc);
            var expected = GetExpectedSolutions(noOfPlayers, sut);

            // Act
            sut.Simulate();
            var actual = sut.Rounds;

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        private List<Round> GetExpectedSolutions(int noOfPlayers, Admin admin)
		{
			var result = new List<Round>();
			var allMatches = admin.AllMatches.ToList();

			switch (noOfPlayers)
			{
                case 4:
					//var aRound = new List<Match>() { new Match(new Player(1, 1), new Player(2, 2)), new Match(new Player(1, 1), new Player(2, 2)) };
					var aRound = new List<Match>() { allMatches[0].ToList()[1], allMatches[0].ToArray()[2], allMatches[0].ToArray()[3]};
                    break;
			}

			return result;
		}
	}
}
