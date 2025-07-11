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
	public static Player PlayerOne;
	public static Player PlayerTwo;
	
	public static void Begin()
	{
	
		Console.WriteLine(@"
		
		Éché
		An exciting board game
		
		{4}  {4}  {4}  {4}  {4}  {4}
		{4}  {4}  {4}  {4}  {4}  {4}
		
		 11  10    9	 8    7    6
		 0    1    2    3    4    5
		
		Pick a side of the board and capture the most seeds.
		
		How to:
		Choose a side of the game board and pick any hole on your side with at least one seed to get started.
		
		Enter '1' to select the top row or '2' to select the bottom row
		");
		
		string playerInput = Console.ReadLine()!;
		
		if (playerInput == "1") 
		{
			Console.WriteLine("You are player one");
			Console.WriteLine("Pick any hole from 6 - 11");
			string startHoleInput = Console.ReadLine()!;
			
			if(int.TryParse(startHoleInput, out int startHole))
			{
				if(startHole >= 7 && startHole <= 11)
					Game.PlayGame(PlayerOne, startHole);
			}
			else 
			{
				Console.WriteLine("Invalid move");
			}
			
		}
		
		else if (playerInput == "2")
		{
			Console.WriteLine("You are player one");
			Console.WriteLine("Pick any hole from 6 - 11");
			string startHoleInput = Console.ReadLine()!;
			
			if(int.TryParse(startHoleInput, out int startHole))
			{
				if(startHole >= 0 && startHole <= 6)
					Game.PlayGame(PlayerTwo, startHole);
			}
			
		}

		else
		{
			Console.WriteLine("Invalid player");
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
	public static int NumberOfPickedSeeds { get; set; }
	
	public static void PlayGame(Player currentPlayer, int hole)
	{
		NumberOfPickedSeeds = Board.Holes[hole];
		Board.Holes[hole] = 0;
		
		if(NumberOfPickedSeeds == 0)
		{
			Console.WriteLine("Invalid move. Hole is empty");
			return;
		}
			
		if(CurrentPlayer.Index == 0 && hole > 7) 
		{	
			Move(currentPlayer, NumberOfPickedSeeds, hole++);
		} 
		else if(CurrentPlayer.Index == 1 && hole < 7)
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
		while (numberOfPickedSeeds > 0) 
		{
			hole++; 
			
			if(numberOfPickedSeeds == 1 && Board.Holes[hole] == 3)
			{
				numberOfPickedSeeds = 0;
				Board.Holes[hole] = 0;
				currentPlayer.Score++;
			}
			
			if(numberOfPickedSeeds == 1 && Board.Holes[hole] > 0)
			{
				numberOfPickedSeeds = Board.Holes[hole]++;
				Board.Holes[hole] = 0;
			}
		
			if(hole == 11)
				 hole = 0;
				
			Board.Holes[hole] = Board.Holes[hole]++;
			numberOfPickedSeeds--;
			GameState.CheckGame(CurrentPlayer, PlayerOne, PlayerTwo);
			Move(currentPlayer, numberOfPickedSeeds, hole);
		}
		
		string newHoleInput;
    int newHole;
		
		if (currentPlayer.Index == PlayerTwo.Index)
    {
      currentPlayer = PlayerTwo;
      Console.WriteLine("PlayerTwo's turn, select a seeded hole from the top row");
      newHoleInput = Console.ReadLine();
      int.TryParse(newHoleInput, out newHole);
    }
    else
    {
      currentPlayer = PlayerOne;
      Console.WriteLine("PlayerOne's turn, select a seeded hole from the bottom row");
      newHoleInput = Console.ReadLine();
      int.TryParse(newHoleInput, out newHole);
    }
		Game.PlayGame(currentPlayer, newHole);
	}
}

public static class GameState
{
	public static void CheckGame(Player currentPlayer, Player PlayerOne, Player PlayerTwo)
	{
		for(int i = 0; i < Board.Holes.Length; i++)
		{
			if(Board.Holes[i] == 4)
			{
				if(i >= 0 && i < 6)
					PlayerTwo.Score++;
					
				if(i >= 5 && i < 12)
					PlayerOne.Score++;
			} 
			else 
			{
				ReprintBoardWithStats(currentPlayer);
			}
		}
	}
	
	public static void ReprintBoardWithStats(Player currentPlayer)
	{
		Console.WriteLine($"Current Player {0}", currentPlayer.Index);
		Console.WriteLine($"SeedCount {0}", Game.NumberOfPickedSeeds);
	}
}

public class AI
{
	
}