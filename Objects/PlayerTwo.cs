using Godot;
using System;

public class PlayerTwo : KinematicBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	public KinematicBody2D _Ball;
	public int Speed = 300;
	private Vector2 _velocity = new Vector2();
	public GameController _GameController { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_Ball = GetNode<KinematicBody2D> ("../Ball");
		_GameController = GetNode<GameController> ("../GameController");
	}

	public void GetInput()
	{
		// Detect up/down/left/right keystate and only move when pressed
		_velocity = new Vector2();
		if (Math.Abs(this.Position.y - _Ball.Position.y) > 20)
		{
			if (_Ball.Position.y > this.Position.y)
			_velocity.y += Speed;
		else
				_velocity.y -= Speed;
		}
		else
		{
			_velocity.y = 0;
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		if (_GameController.CurrentState == GameStates.PLAYING)
		{
			GetInput();
			MoveAndCollide(_velocity * delta);
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
