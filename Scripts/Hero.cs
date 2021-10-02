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

		if (velocity != new Vector2())
		{
			GetNode<AnimationPlayer>("AnimationPlayer").Play("Walk");
		}
		else
		{
			GetNode<AnimationPlayer>("AnimationPlayer").Play("Idle");
		}

		velocity = velocity.Normalized() * speed;
	}

	public override void _PhysicsProcess(float delta)
	{
		GetInput();
		velocity = MoveAndSlide(velocity);
	}
}
