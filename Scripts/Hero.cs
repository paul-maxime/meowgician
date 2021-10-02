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
			if (velocity.x > 0)
			{
				animationPlayer.Play("Walk right");
			}
			else if (velocity.x < 0)
			{
				animationPlayer.Play("Walk left");
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
			else if (animationPlayer.CurrentAnimation == "Walk left")
			{
				animationPlayer.Play("Idle left");
			}
			else if (animationPlayer.CurrentAnimation == "Walk right")
			{
				animationPlayer.Play("Idle right");
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
