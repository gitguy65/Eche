public class Program
{
    public static void Main()
	{
		Board.InitializeBoard();
		Player PlayerOne = new Player(0);
		Player PlayerTwo = new Player(1);
		
		GamePlay.PlayerOne = PlayerOne;
		GamePlay.PlayerTwo = PlayerTwo;
		
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

                    GamePlay.PlayGame(GamePlay.PlayerOne, startHole);
                }
            }
			else 
			{
				Console.WriteLine("Invalid move");
                GamePlay.PlayGame(GamePlay.PlayerOne, startHole);
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
                    GamePlay.PlayGame(GamePlay.PlayerTwo, startHole);
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

public static class GamePlay
{
	public static Player CurrentPlayer { get; set; }
	public static Player PlayerOne { get; set; }
	public static Player PlayerTwo { get; set; }
	public static int NumberOfPickedSeeds { get; private set; }
	public static int CurrentHole { get; set; }
	
	public static void PlayGame(Player currentPlayer, int hole)
	{
		CurrentHole = hole;
		CurrentPlayer = currentPlayer;

		NumberOfPickedSeeds = Board.Holes[hole];

		if(NumberOfPickedSeeds == 0)
		{
			Console.WriteLine("Invalid move. Hole is empty");
			// CurrentHole = null;
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
            hole = hole == 12 ? 0 : hole;
			CurrentHole = hole;

            if (NumberOfPickedSeeds == 1 && Board.Holes[hole] == 3)
            {
				currentPlayer.Score++; 
				Board.Holes[hole] = 0;
				NumberOfPickedSeeds = 0;

                GameState.CheckGame();
                GameState.DisplayBoard();
                break;
			}
			else if (NumberOfPickedSeeds == 1 && Board.Holes[hole] > 0)
			{
				NumberOfPickedSeeds = Board.Holes[hole] + 1;
				Board.Holes[hole] = 0; 
			}

			else
			{
                Board.Holes[hole]++;
                NumberOfPickedSeeds--;
            }

            GameState.CheckGame();
            GameState.DisplayBoard();
            Move(currentPlayer, NumberOfPickedSeeds, hole);
		}
		
		int newHole;
		
		if (currentPlayer.Index == PlayerOne.Index)
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
		int hole = GamePlay.CurrentHole == 0 ? 12 : GamePlay.CurrentHole;
		hole--;

        if (Board.Holes[hole] == 4)
		{
            if (hole >= 0 && hole < 6)
            {
                GamePlay.PlayerOne.Score++;
                Board.Holes[hole] = 0;
            }
            else
            {
                GamePlay.PlayerTwo.Score++;
                Board.Holes[hole] = 0;
            }
        }  
    }

    public static void DisplayBoard()
	{
        Console.WriteLine($@"
Current Player: {GamePlay.CurrentPlayer.Index}
Seeds: {GamePlay.NumberOfPickedSeeds}

Player 1 Score: {GamePlay.PlayerOne.Score}
Player 2 Score: {GamePlay.PlayerTwo.Score}

{Board.Holes[11]}	{Board.Holes[10]}	{Board.Holes[9]}	{Board.Holes[8]}	{Board.Holes[7]}	{Board.Holes[6]}
{Board.Holes[0]}	{Board.Holes[1]}	{Board.Holes[2]}	{Board.Holes[3]}	{Board.Holes[4]}	{Board.Holes[5]}

		");
    }
}

public class AI
{
	
}