using Godot;
using System;

public class GameController : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	public const string StartString = "TO START GAME \n HIT SPACE KEY";
	public const string RoundString = "TO BEGIN NEXT ROUND \n HIT SPACE KEY";
	public const string P1WinString = "VICTORY. HIT SPACE KEY \n TO START NEW GAME";
	public const string P2WinString = "DEFEAT. HIT SPACE KEY \n TO START NEW GAME";
	
	public KinematicBody2D _Player;

	
	public KinematicBody2D _PlayerTwo;

	
	public KinematicBody2D _Ball;

	public Label GameLabel;

	public int P1Score { get; set; }

	public int P2Score { get; set; }

	public GameStates CurrentState { get; set; } = GameStates.START;

	public System.Timers.Timer _flashTimer { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_flashTimer = new System.Timers.Timer();
		_flashTimer.Interval = 1000;
		_flashTimer.Elapsed += OnFlashElapsed;
		_flashTimer.AutoReset = true;

		_Player = GetNode<KinematicBody2D> ("../Player");
		
		_PlayerTwo = GetNode<KinematicBody2D> ("../PlayerTwo");
		_Ball = GetNode<KinematicBody2D> ("../Ball");
		GameLabel = GetNode<Label> ("../StartGameLabel");

		var _ball = _Ball as Ball;
		_ball._UpdateScore = OnUpdateScore;
		_flashTimer.Start();
	}

	private int GetRandomDir()
	{
		Random r = new Random();
		int _rn = r.Next(0, 360);
		while ((_rn < 30 || (_rn > 70 && _rn < 120) || (_rn > 160 && _rn < 200) || _rn > 325))
		{
			_rn = r.Next(0, 360);
		}
		return _rn;
	}

	private void OnFlashElapsed(object sender, System.Timers.ElapsedEventArgs e)
	{
		GameLabel.Visible = !GameLabel.Visible;
	}

	//Start the game for the first time, or after a game over.
	private void StartGame()
	{
		P1Score = 0;
		P2Score = 0;

		Label _l = GetNode<Label> ("../Score Board/Board/P1Area/P1Score");
			_l.Text = P1Score.ToString();
		Label _l2 = GetNode<Label> ("../Score Board/Board/P2Area/P2Score");
			_l2.Text = P2Score.ToString();

		_flashTimer.Stop();
		GameLabel.Visible = false;
		CurrentState = GameStates.PLAYING;

		var _ball = _Ball as Ball;
		ResetPlayers();
		_ball.StartBall(new Vector2(512, 300), GetRandomDir());
	}

	//Start a new round
	private void ResetMatch()
	{
		_flashTimer.Stop();
		GameLabel.Visible = false;
		var _ball = _Ball as Ball;
		ResetPlayers();
		CurrentState = GameStates.PLAYING;
		_ball.StartBall(new Vector2(512, 300), GetRandomDir());
	}

	private void OnUpdateScore(string Name)
	{
		if (Name == "P2")
		{
			P1Score++;
			Label _l = GetNode<Label> ("../Score Board/Board/P1Area/P1Score");
			_l.Text = P1Score.ToString();
		}
		else
		{
			P2Score++;
			Label _l = GetNode<Label> ("../Score Board/Board/P2Area/P2Score");
			_l.Text = P2Score.ToString();
		}

		if (P1Score >= 10)
		{
			CurrentState = GameStates.END;
			_flashTimer.Start();
			GameLabel.Text = P1WinString;
		}
		else if (P2Score >= 10)
		{
			CurrentState = GameStates.END;
			_flashTimer.Start();
			GameLabel.Text = P2WinString;
		}
		else
		{
			CurrentState = GameStates.ROUND;
			_flashTimer.Start();
			GameLabel.Text = RoundString;
		}

		ResetPlayers();
		var _ball = _Ball as Ball;
		_ball.ResetBall(new Vector2(0, 0), GetRandomDir());
	}

	private void ResetPlayers()
	{
		_Player.Position = new Vector2(50,300);
		_PlayerTwo.Position = new Vector2(974,300);
	}

 // Called every frame. 'delta' is the elapsed time since the previous frame.
 public override void _Process(float delta)
 {
	 if (CurrentState == GameStates.START)
	 {
		 if (Input.IsActionPressed("ui_accept"))
		 {
			 StartGame();
		 }
	 }
	 else if (CurrentState == GameStates.END)
	 {
		 if (Input.IsActionPressed("ui_accept"))
		 {
			 StartGame();
		 }
	 }
	 else if (CurrentState == GameStates.ROUND)
	 {
		 if (Input.IsActionPressed("ui_accept"))
		 {
			 ResetMatch();
		 }
	 }
 }
}
