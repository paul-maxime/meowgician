using Godot;
using System;

public class Hero : KinematicBody2D
{
	[Export] public int speed = 200;

	public Vector2 velocity = new Vector2();

	public override void _Ready()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Idle");
	}

	public void GetInput()
	{
		velocity = new Vector2();

		if (Input.IsActionPressed("ui_right"))
			velocity.x += 1;

		if (Input.IsActionPressed("ui_left"))
			velocity.x -= 1;

		if (Input.IsActionPressed("ui_down"))
			velocity.y += 1;

		if (Input.IsActionPressed("ui_up"))
			velocity.y -= 1;

		var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		if (velocity != new Vector2())
		{
			if (velocity.y >= 0)
			{
				animationPlayer.Play("Walk");
			}
			else
			{
				animationPlayer.Play("Walk back");
			}
		}
		else
		{
			if (animationPlayer.CurrentAnimation == "Walk")
			{
				animationPlayer.Play("Idle");
			}
			else if (animationPlayer.CurrentAnimation == "Walk back")
			{
				animationPlayer.Play("Idle back");
			}
		}

		velocity = velocity.Normalized() * speed;
	}

	public override void _PhysicsProcess(float delta)
	{
		GetInput();
		velocity = MoveAndSlide(velocity);
	}
}
