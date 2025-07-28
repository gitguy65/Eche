using System;
using System.Security.Cryptography;

public class Program
{
    public static void Main()
	{
		Board.InitializeBoard();
		
		
		StartGame.Begin();
	}
}
	
public static class StartGame
{
    /*
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
		
Enter '1' to be playerOne or '2' to be PlayerTwo
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

	*/

    static readonly string homeScreen = @"

		+------+        +----+	  +--+    +--+    +-------+
		|  ____+	  /  ____+	  |  |    |  |    |  _____+
		| |		     /  /		  |  |    |  |    |  |
		|  ----+	/  /		  |  |____|  |    |  |----+
		|  ____+    |  |		  |  ______  |    |  _____+
		| |	      	|  |		  |  |	  |  |    |  | 
		| |____+	\  \_____+	  |  |	  |  |    |  |____+
		| 	   |  	 \		 |	  |  |    |  |    |		  |	   
		+------+      +------+	  +--+    +--+    +-------+


	-- About --
a board game consisting of 12 holes with 4 inital seeds each and played by two players. it involves mathematical foresight.

	-- How to Play --
use your arrow keys to navigate and enter button to select or enter the hole number

	-- Holes --
4	4	4	4	4	4
4	4	4	4	4	4
		
	-- Hole Selection --
11	10	9	8	7	6
0	1	2	3	4	5

";

    static readonly string startButton = @"
	+------------+
	| Start Game |
	+------------+";

    static readonly string exitMenu = @"
	+-----------+
	| Exit Game |
	+-----------+";

    static readonly string playerOneButton = @"
	+------------+
	| Player One |
	+------------+";

    static readonly string playerTwoButton = @"
	+------------+
	| Player Two |
	+------------+";

    static readonly string HumanButton = @"
	+-------+
	| Human |
	+-------+";

    static readonly string AIButton = @"
	+----+
	| AI |
	+----+";

    static readonly string backButton = @"
	+---------+
	| Go Back |
	+---------+";

    private static int selectedInit = 0;
    private static int selectedPlayer = 0;
    private static int selectedOpponent = 0;

    private static string[] initMenu = { startButton, exitMenu };
    private static string[] playerMenu = { playerOneButton, playerTwoButton, backButton };
	private static string[] opponentMenu = { HumanButton, AIButton, backButton };

    public static void Begin()
	{
        while (true)
        {
            switch (GamePlay.Status)
            {
                case GameStatus.INITIALIZED:
                    MenuAction(initMenu, ref selectedInit, InitMenuAction);
                    break;

                case GameStatus.PLAYER_SELECTION:
                    MenuAction(playerMenu, ref selectedPlayer, PlayerMenuAction);
					break;

                case GameStatus.OPPONENT_SELECTION: 
                    MenuAction(opponentMenu, ref selectedOpponent, OpponentMenuAction);
					break;

                case GameStatus.PLAY:

					break;

                case GameStatus.PLAYING:

                    break;

                case GameStatus.PLAY_COMPLETE:

                    break;

                case GameStatus.GAME_OVER:

                    break;

                case GameStatus.GAME_ERROR:

                    break;
            }
        }
    }

    private static void MenuAction(string[] Menu, ref int selection, Action<int> action)
    { 
        ConsoleKeyInfo key;

		do
		{ 
            Console.Clear();
            Console.WriteLine($@"{homeScreen}");

            for (int i = 0; i <= Menu.Length; i++)
			{
				if (i == selection)
				{
					Console.ForegroundColor = ConsoleColor.White;
					Console.BackgroundColor = ConsoleColor.Black;
					Console.WriteLine($@"> {Menu[i]}");
					Console.ResetColor();
				}
				else
				{
					Console.WriteLine($@". {Menu[i]}");
				}
			}

			key = Console.ReadKey(true);

            switch (key.Key)
			{
				case ConsoleKey.UpArrow:
					selection = selection <= 0 ? 0 : --selection;
					break;

				case ConsoleKey.DownArrow:
					selection = selection >= Menu.Length ? Menu.Length : ++selection;
					break;

				case ConsoleKey.Enter:
					action(selection);
					return;

			}
		}
		while (key.Key != ConsoleKey.Escape);

    }

	private static void InitMenuAction(int selection)
	{
		if (selection == 0) 
		{
			GamePlay.Status = GameStatus.PLAYER_SELECTION;
			return;
		}
		else
		{
			Environment.Exit(0);		
		}
	}

	private static void PlayerMenuAction(int selection)
	{
		switch(selection)
		{
			case 0:
                GamePlay.PlayerOne = new Human(0); 
				return;

			case 1:
                GamePlay.PlayerTwo = new Human(1);
                return;
			default:
				GamePlay.Status = GameStatus.INITIALIZED;
				return;
        } 
    }

	private static void OpponentMenuAction(int selection)
	{
		switch (selection)
		{
			case 0:
				if(GamePlay.PlayerOne != null)
				{
					GamePlay.PlayerTwo = new Human(1);
				}
				else
				{
					GamePlay.PlayerOne = new Human(0);
				}
				GamePlay.Status = GameStatus.PLAY;
				return;

			case 1:
                if (GamePlay.PlayerOne != null)
                {
                    GamePlay.PlayerTwo = new AI(1);
                }
                else
                {
                    GamePlay.PlayerOne = new AI(0);
                }
                GamePlay.Status = GameStatus.PLAY;
                return;

			default:
				GamePlay.Status = GameStatus.PLAYER_SELECTION;
				return;
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

public abstract class Player
{
	public int Index { get; set; }
	public int Score { get; set; }
	
	public Player(int index)
	{
		Index = index;
		Score = 0;
	}
}

public class AI : Player
{
    public AI(int index) : base(index)
    {

    }
}

public class Human : Player
{
	private int newHole;
    public Human(int index) : base(index)
    {
        string newHoleInput = Console.ReadLine()!;
        bool _ = int.TryParse(newHoleInput, out newHole);

        GamePlay.PlayGame(index, newHole);
    }
}

public static class GamePlay
{
	public static Player CurrentPlayer { get; set; }
	public static Player PlayerOne { get; set; }
	public static Player PlayerTwo { get; set; }
	public static int NumberOfPickedSeeds { get; private set; }
	public static int CurrentHole { get; set; }
    public static GameStatus Status { get; set; }

    public static void PlayGame(int playerIndex, int chosenHole)
    {
        CurrentHole = chosenHole;

        NumberOfPickedSeeds = Board.Holes[chosenHole];

        if (NumberOfPickedSeeds == 0)
        {
            Console.WriteLine("Invalid move. Hole is empty");
            return;
        }

        Board.Holes[chosenHole] = 0;

        if (Status == GameStatus.PLAY)
        {
            Console.WriteLine($"You picked {NumberOfPickedSeeds} seeds from hole {chosenHole}");

            if (chosenHole >= 0 && chosenHole < 6)
            {
                Move(playerIndex, NumberOfPickedSeeds, ++chosenHole);
            }
        }
		else
		{
            if (playerIndex == 0 && chosenHole >= 0 && chosenHole < 6)
            {
                Move(playerIndex, NumberOfPickedSeeds, ++chosenHole);
            }
            else if (playerIndex == 1 && chosenHole > 5 && chosenHole <= 11)
            {
                Move(playerIndex, NumberOfPickedSeeds, ++chosenHole);
            }
            else
            {
				// manage state
                Console.WriteLine("Invalid move. Select from your side");
            }
        } 
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

	}

	private static void Move(int currentPlayerIndex, int numberOfPickedSeeds, int hole) 
	{

        Status = GameStatus.PLAY_COMPLETE;
        return;
    }
}

public static class GameState
{
	public static void CheckGame()
	{
		CheckGameStatus();
        CheckGameScore();
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

	private static void CheckGameStatus()
	{

	}

	private static void CheckGameScore()
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

	public static void ChangePlayer()
	{
        if (GamePlay.CurrentPlayer.Index == GamePlay.PlayerOne.Index)
        {
            GamePlay.CurrentPlayer = GamePlay.PlayerTwo;
            Console.WriteLine("PlayerTwo's turn, select a seeded hole from the top row");
        }
        else
        {
            GamePlay.CurrentPlayer = GamePlay.PlayerOne;
            Console.WriteLine("PlayerOne's turn, select a seeded hole from the bottom row");
        }
    }
}



public enum GameStatus 
{ 
	INITIALIZED,
	PLAYER_SELECTION,
	OPPONENT_SELECTION,
	PLAY,
	PLAY_COMPLETE,
	PLAYING,
	GAME_OVER,
	GAME_ERROR
}
