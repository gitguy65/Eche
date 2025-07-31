using System;
using System.Reflection;
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
    static readonly string homeScreen = @"

        ########   ######   ##   ##   ########
        ##         ##       ##   ##   ##
        ######     ##       #######   ########
        ######     ##       ##   ##   ########
        ##         ##       ##   ##   ##
        ########   ######   ##   ##   ########


        -- About --
        A board game consisting of 12 holes with 4 inital seeds each and played by two players. 
        It involves mathematical foresight.
        
        -- How to Play --
        Use your arrow keys to navigate and enter button to select or enter the hole number
        
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

	private static List<object> renderObject = []; 

    public static void Begin()
	{
        while (true)
        {
            switch (GamePlay.Status)
            {
                case GameStatus.INITIALIZED:
					renderObject.Clear();
					renderObject.AddRange(homeScreen, initMenu);
                    MenuAction(initMenu, ref selectedInit, InitMenuAction);
                    break;

                case GameStatus.PLAYER_SELECTION:
                    renderObject.Clear();
                    renderObject.AddRange(homeScreen, playerMenu);
                    MenuAction(playerMenu, ref selectedPlayer, PlayerMenuAction);
					break;

                case GameStatus.OPPONENT_SELECTION:
                    renderObject.Clear();
                    renderObject.AddRange(homeScreen, opponentMenu);
                    MenuAction(opponentMenu, ref selectedOpponent, OpponentMenuAction);
					break;

                case GameStatus.PLAY:
                    renderObject.Clear();
                    renderObject.AddRange("Starting");
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
			Render(renderObject);

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
		return;
    }

	private static void InitMenuAction(int selection)
	{
		if (selection == 0) 
		{
			GamePlay.Status = GameStatus.PLAYER_SELECTION;
			return;
		}

		Environment.Exit(0);
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

	// 

	private static void Render(List<object> renderObject)
	{
        int selection = 0;

        foreach (var item in renderObject) 
		{ 
			if (item is string str)
			{
				Console.WriteLine($@"{item}");
			}
			else if (item is string[] arr)
			{
				GameStatus status = GamePlay.Status;

				switch(status)
				{
					case GameStatus.INITIALIZED:
						selection = selectedInit;
						break;
					case GameStatus.PLAYER_SELECTION:
						selection = selectedPlayer;
						break;
					case GameStatus.OPPONENT_SELECTION:
						selection = selectedOpponent;
						break;
				}
				
				for (int i = 0; i < arr.Length; i++) 
				{	if(i == selection)
					{
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.WriteLine($@" {arr[i]}");
                        Console.ResetColor();
                    }
					else
					{
                        Console.WriteLine($@" {arr[i]}");
                    }
                }
			}
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

	public abstract void Play();
}

public class AI : Player
{
    public AI(int index) : base(index)
    {

    }

    public override void Play()
    {
        throw new NotImplementedException();
    }
}

public class Human : Player
{
	private int selectedHole;
    public Human(int index) : base(index)
    {
        
    }

    public override void Play()
    {
        string input = Console.ReadLine()!;
        bool _ = int.TryParse(input, out selectedHole);

        GamePlay.PlayGame(Index, selectedHole);
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
	public static string? ErrorMessage { get; private set; }


    public static void PlayGame(int playerIndex, int chosenHole)
    {
        CurrentHole = chosenHole;
        ErrorMessage = "";

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
				ErrorMessage = "Invalid move. Select a non empty hole from your side";
				Status = GameStatus.GAME_ERROR;
				return;
            }
        } 
    }

	private static void Move(int currentPlayerIndex, int numberOfPickedSeeds, int hole) 
	{
        NumberOfPickedSeeds = numberOfPickedSeeds;

		if (currentPlayerIndex == 0)
		{
			CurrentPlayer = PlayerOne;
		} else
		{
			CurrentPlayer = PlayerTwo;
		}

        while (NumberOfPickedSeeds > 0)
        {
            hole++;
            hole = hole == 12 ? 0 : hole;
            CurrentHole = hole;

            if (NumberOfPickedSeeds == 1 && Board.Holes[hole] == 3)
            {
                CurrentPlayer.Score++;
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
            Move(currentPlayerIndex, NumberOfPickedSeeds, hole);
        }

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
