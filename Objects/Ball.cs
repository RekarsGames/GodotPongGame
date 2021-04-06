using Godot;
using System;

public class Ball : KinematicBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	public Action<string> _UpdateScore;

	private Random rand = new Random();

	public int Speed = 500;
	private Vector2 _velocity = new Vector2();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	public void ResetBall(Vector2 pos, float dir)
	{
		Speed = 500;
		RotationDegrees = dir;
		Position = pos;
		_velocity = new Vector2(0, 0).Rotated(Rotation);
	}

	public void StartBall(Vector2 pos, float dir)
	{
		Speed = 500;
		RotationDegrees = dir;
		Position = pos;
		_velocity = new Vector2(Speed, 0).Rotated(Rotation);
	}

	private int RandomizeBounce()
	{
		return rand.Next(-10,10);
	}

	public override void _PhysicsProcess(float delta)
	{
		var collision = MoveAndCollide(_velocity * delta);
		if (collision != null)
		{
			int _newBounceDelta = 0;

			if (collision.Collider is Player p1)
			{
				Speed += 35;
				_newBounceDelta += RandomizeBounce();
				_velocity = _velocity.Bounce(collision.Normal).Rotated(_newBounceDelta);
			}
			else if (collision.Collider is PlayerTwo p2)
			{
				Speed += 35;
				_newBounceDelta += RandomizeBounce();
				_velocity = _velocity.Bounce(collision.Normal).Rotated(_newBounceDelta);
			}
			else if (collision.Collider is SideWall wall)
			{
				if (wall.Name == "SideWall")
				{
					_UpdateScore.Invoke("P1");
				}
				else
				{
					_UpdateScore.Invoke("P2");
				}
				
			}
			else
			{
				_velocity = _velocity.Bounce(collision.Normal);
			}

			if (_velocity.x < 150 && _velocity.x > 0)
			{
				_velocity.x += 100;
			}
			else if (_velocity.x > -150 && _velocity.x < 0)
			{
				_velocity.x -= 100;
			}

			if (collision.Collider.HasMethod("Hit"))
			{
				collision.Collider.Call("Hit");
			}
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
