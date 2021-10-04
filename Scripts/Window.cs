using Godot;
using System;

public class Window : StaticBody2D
{
	private AnimationPlayer animationPlayer;
	private AnimationPlayer animationPlayerOutline;
	private Timer timer;

	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayerOutline = GetNode<AnimationPlayer>("AnimationPlayerOutline");
		animationPlayer.Play("Empty");
		timer = GetNode<Timer>("Timer");
		timer.Start();
	}

	public void _on_Timer_timeout()
	{
		if (animationPlayer.CurrentAnimation == "Empty")
		{
			animationPlayer.Play("Arriving");
		}
		else if (animationPlayer.CurrentAnimation == "Waiting")
		{
			RemoveFromGroup("ingredientHandlers");
			animationPlayerOutline.Stop();
			animationPlayer.Play("Throwing");
		}
	}

	public void _on_AnimationPlayer_animation_finished(string animationName)
	{
		if (animationName == "Arriving")
		{
			AddToGroup("ingredientHandlers");
			animationPlayer.Play("Waiting");
			animationPlayerOutline.Play("Waiting");
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
