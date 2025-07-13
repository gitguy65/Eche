public class Program
{
    public static void Main()
	{
		Board.InitializeBoard();
		Player PlayerOne = new Player(0);
		Player PlayerTwo = new Player(1);
		
		Game.PlayerOne = PlayerOne;
		Game.PlayerTwo = PlayerTwo;
		
		StartGame.Begin();
	}
}
	
public static class StartGame
{	
	public static void Begin()
	{
	
		Console.WriteLine(@"
Éché
An exciting board game
		
4	4	4	4	4	4
4	4	4	4	4	4
		
11	10	9	8	7	6
0	1	2	3	4	5
		
Pick a side of the board and capture the most seeds.
		
_How to play_
Choose a side of the game board and pick any hole on your side with at least one seed to get started.
		
Enter '1' to select the top row or '2' to select the bottom row
");
		
		string playerInput = Console.ReadLine()!;
		
		if (playerInput == "1") 
		{
			Console.WriteLine("You are player one");
			Console.WriteLine("Pick any hole from 0 - 5");
			string startHoleInput = Console.ReadLine()!;

			if(int.TryParse(startHoleInput, out int startHole))
			{
				if(startHole >= 0 && startHole < 6)
				{
                    Game.PlayGame(Game.PlayerOne, startHole);
                }
            }
			else 
			{
				Console.WriteLine("Invalid move");
                Game.PlayGame(Game.PlayerOne, startHole);
            }
		}
		
		else if (playerInput == "2")
		{
			Console.WriteLine("You are player two");
			Console.WriteLine("Pick any hole from 6 - 11");
			string startHoleInput = Console.ReadLine()!;
			
			if(int.TryParse(startHoleInput, out int startHole))
			{
				if(startHole >= 6 && startHole < 12)
				{
                    Game.PlayGame(Game.PlayerTwo, startHole);
                }
            }
        }

        else
		{
			Console.WriteLine("Invalid player");
            throw new Exception();
        }
    }
}

public static class Board
{
	public static int[] Holes = new int[12];
	
	public static void InitializeBoard()
	{
		for(int i = 0; i < Holes.Length; i++)
		{
			Holes[i] = 4;
		}
	}
}

public class Player
{
	public int Index { get; set; }
	public int Score { get; set; }
	
	public Player(int index)
	{
		Index = index;
		Score = 0;
	}
}

public static class Game
{
	public static Player CurrentPlayer { get; set; }
	public static Player PlayerOne { get; set; }
	public static Player PlayerTwo { get; set; }
	public static int NumberOfPickedSeeds { get; private set; }
	
	public static void PlayGame(Player currentPlayer, int hole)
	{
		CurrentPlayer = currentPlayer;

		NumberOfPickedSeeds = Board.Holes[hole];

		if(NumberOfPickedSeeds == 0)
		{
			Console.WriteLine("Invalid move. Hole is empty");
			return;
		}

		Board.Holes[hole] = 0;
		Console.WriteLine($"You picked {NumberOfPickedSeeds} seeds from hole {hole}");

		if (currentPlayer.Index == 0 && hole >= 0 && hole < 6)
		{
			Move(currentPlayer, NumberOfPickedSeeds, hole++);
		}
		else if (currentPlayer.Index == 1 && hole > 5 && hole <= 11)
		{
			Move(currentPlayer, NumberOfPickedSeeds, hole++);
		}
		else
		{
			Console.WriteLine("Invalid move. Select from your side");
		}
		
		return;
	}
	
	private static void Move(Player currentPlayer, int numberOfPickedSeeds, int hole)
	{
		NumberOfPickedSeeds = numberOfPickedSeeds;
		while (NumberOfPickedSeeds > 0) 
		{
			hole++;

			if (NumberOfPickedSeeds == 1 && Board.Holes[hole] == 3)
			{
				NumberOfPickedSeeds = 0;
				Board.Holes[hole] = 0;
				currentPlayer.Score++;
				break;
			}
			
			if(NumberOfPickedSeeds == 1 && Board.Holes[hole] > 0)
			{
				NumberOfPickedSeeds = Board.Holes[hole] + 1;
				Board.Holes[hole] = 0;
			}
		
			if(hole == 11)
				hole = 0;
				
			Board.Holes[hole]++;
			NumberOfPickedSeeds--;
			GameState.CheckGame();
			Move(currentPlayer, NumberOfPickedSeeds, hole);
		}
		
		int newHole;
		
		if (currentPlayer.Index == PlayerTwo.Index)
		{
			CurrentPlayer = PlayerTwo;
			Console.WriteLine("PlayerTwo's turn, select a seeded hole from the top row");		
		}
		else
		{
			CurrentPlayer = PlayerOne;
			Console.WriteLine("PlayerOne's turn, select a seeded hole from the bottom row"); 
		}

		string newHoleInput = Console.ReadLine()!;
		bool _ = int.TryParse(newHoleInput, out newHole);
		PlayGame(CurrentPlayer, newHole);
	}
}

public static class GameState
{
	public static void CheckGame()
	{
		for(int i = 0; i < Board.Holes.Length; i++)
		{
			if(Board.Holes[i] == 4)
			{
				if(i >= 0 && i < 6)
					Game.PlayerTwo.Score++;
					
				if(i >= 5 && i < 12)
					Game.PlayerOne.Score++;
			} 
		}
        ReprintBoardWithStats();
    }

    public static void ReprintBoardWithStats()
	{
        Console.WriteLine($"Current Player {Game.CurrentPlayer.Index}");
        Console.WriteLine($"SeedCount {Game.NumberOfPickedSeeds}");
    }
}

public class AI
{
	
}