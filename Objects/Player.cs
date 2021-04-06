using Godot;
using System;

public class Player : KinematicBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	public int Speed = 300;
	private Vector2 _velocity = new Vector2();
	public GameController _GameController { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_GameController = GetNode<GameController> ("../GameController");
	}

	public void GetInput()
	{
		// Detect up/down/left/right keystate and only move when pressed
		_velocity = new Vector2();

		if (Input.IsActionPressed("ui_down"))
			_velocity.y += Speed;

		if (Input.IsActionPressed("ui_up"))
			_velocity.y -= Speed;
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
