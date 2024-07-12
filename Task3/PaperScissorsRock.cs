using ConsoleTables;
using Task3.Exceptions;
namespace Task3;

public class PaperScissorsRock
{
	public static void StartGame(string[] moves)
	{
		try
		{
			IsValidMoves(moves);

			byte[] key = SHA3_256.GenerateRandomKey(32);
			(string pcMove, int indexOfMove) = ChoosingMove(moves);

			string hmac = SHA3_256.ComputeHmacSha3_256(pcMove, key);

			Console.WriteLine($"HMAC: {hmac}");

			int usersChoice = AskingToChooseTheMove(moves);

			if (SHA3_256.ComputeHmacSha3_256(pcMove, key) != hmac)
			{
				Console.WriteLine("Computer is playing dishonestly. Game over.");
				Environment.Exit(0);
			}

			Console.WriteLine($"Your move : {moves[usersChoice - 1]}");
			Console.WriteLine($"Computer's move : {pcMove}");

			GameResult gameResult = DeterminingResult(usersChoice - 1, indexOfMove, moves.Length);

			if (gameResult == GameResult.Draw)
			{
				Console.WriteLine("Draw.");
			}
			else if (gameResult == GameResult.UserWon)
			{
				Console.WriteLine("You win!");
			}
			else
			{
				Console.WriteLine("Computer win!");
			}

			Console.WriteLine($"HMAC key : {SHA3_256.ComputeHmacSha3_256(key)}");
		}
		catch (InvalidLengthException exception)
		{
			Console.WriteLine(exception.Message);
		}
		catch (ArgumentRepeatedException exception)
		{
			Console.WriteLine(exception.Message);
		}
		catch (Exception exception)
		{
			Console.WriteLine(exception.Message); 
		}
	}

	private static bool IsValidMoves(string[] moves)
	{
		if (moves.Length < 3)
		{
			throw new InvalidLengthException("Given less moves for playing game. It should not be less than 3 moves.");
		}

		if (moves.Length % 2 == 0)
		{
			throw new InvalidLengthException("Number of moves is even. Should be given odd moves as rule of the game.");
		}

		if (moves.ToHashSet().Count != moves.Length)
		{
			throw new ArgumentRepeatedException("Given repeated moves. Moves should be single.");
		}

		return true;
	}

	private static (string move, int moveIndex) ChoosingMove(string[] moves)
	{
		int indexOfMove = new Random().Next(0, moves.Length);

		return (moves[indexOfMove], indexOfMove);
	}

	private static int AskingToChooseTheMove(string[] moves)
	{
		Console.WriteLine("Available moves : ");

		for (int i = 0; i < moves.Length; i++)
		{
			Console.WriteLine($"{i + 1} - {moves[i]}");
		}

		Console.WriteLine("0 - Exit");
		Console.WriteLine("? - Help");

		Console.Write("Enter your move : ");
		string? choice = Console.ReadLine();

		if (!int.TryParse(choice, out int intChoice) && choice is not "?")
		{
			Console.WriteLine("Please, select the correct option.");
			return AskingToChooseTheMove(moves);
		}

		if (intChoice < 0 || intChoice > moves.Length)
		{
			Console.WriteLine("Please, select the correct option.");
			return AskingToChooseTheMove(moves);
		}
		else if (choice is "0")
		{
			Console.WriteLine("Are you sure to exit from game?");
			Console.WriteLine("1 - Yes");
			Console.WriteLine("Any key for \"No\"");

			ConsoleKey answer = Console.ReadKey().Key;

			if (answer == ConsoleKey.NumPad1 || answer == ConsoleKey.D1)
			{
				Environment.Exit(0);
			}
			
			Console.WriteLine();

			return AskingToChooseTheMove(moves);
		}
		else if (choice is "?")
		{
			DrawingTable(moves);

			Console.WriteLine("Press any key to return to the menu...");
			Console.ReadKey();

			Console.WriteLine();
			return AskingToChooseTheMove(moves);
		}

		return intChoice;
	}

	private static GameResult DeterminingResult(int usersMove, int pcMove, int length)
	{
		if (usersMove == pcMove)
		{
			return GameResult.Draw;
		}

		int half = length / 2;

		while (half-- > 0)
		{
			usersMove++;

			if(usersMove == length)
			{
				usersMove = 0;
			}

			if(usersMove == pcMove)
			{
				return GameResult.PCWon;
			}
		}

		return GameResult.UserWon;
	}

	private static void DrawingTable(string[] moves)
	{
		List<string> columns = ["v PC\\User >"];
		columns.AddRange(moves);

		var option = new ConsoleTableOptions()
		{
			EnableCount = false,
			Columns = columns,
		};

		var table = new ConsoleTable(option);

		string[,] values = new string[moves.Length, moves.Length + 1];

		for (int i = 0; i < moves.Length; i++)
		{
			int winCounter = moves.Length / 2;
			values[i, 0] = moves[i];

			int j = i + 1;

			values[i, j++] = "Draw";

			while (winCounter-- > 0 && j <= moves.Length)
			{
				values[i, j] = "Win";
				values[j - 1, i + 1] = "Lose";

				j++;
			}

			while (j <= moves.Length)
			{
				values[i, j] = "Lose";
				values[j - 1, i + 1] = "Win";

				j++;
			}
		}

		for (int i = 0; i < moves.Length; i++)
		{
			string[] row = new string[moves.Length + 1];

			for (int j = 0; j <= moves.Length; j++)
			{
				row[j] = values[i, j];
			}

			table.AddRow(row);
		}

		table.Write();
	}
}
