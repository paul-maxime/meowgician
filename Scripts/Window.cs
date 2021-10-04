using Godot;
using System;

public class Window : StaticBody2D
{
	private AnimationPlayer animationPlayer;
	private Timer timer;

	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.Play("Empty");
		timer = GetNode<Timer>("Timer");
		timer.Start();
	}

	public void _on_Timer_timeout()
	{
		GD.Print(animationPlayer.CurrentAnimation);
		GD.Print(animationPlayer.Name);
		if (animationPlayer.CurrentAnimation == "Empty")
		{
			animationPlayer.Play("Arriving");
		}
		else if (animationPlayer.CurrentAnimation == "Waiting")
		{
			animationPlayer.Play("Throwing");
		}
	}

	public void _on_AnimationPlayer_animation_finished(string animationName)
	{
		if (animationName == "Arriving")
		{
			animationPlayer.Play("Waiting");
			timer.WaitTime = 10;
			timer.Start();
		}
		else if (animationName == "Throwing")
		{
			animationPlayer.Play("Leaving");
		}
		else if (animationName == "Leaving")
		{
			animationPlayer.Play("Empty");
			timer.WaitTime = 5;
			timer.Start();
		}
	}
}
