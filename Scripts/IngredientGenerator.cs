using Godot;

public class IngredientGenerator : KinematicBody2D
{
	private AnimationPlayer animationPlayer;
	private AnimationPlayer animationPlayerOutline;
	private Timer timer;

	public void Activate()
	{
		WaitAndAppear();
	}

	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayerOutline = GetNodeOrNull<AnimationPlayer>("AnimationPlayerOutline");
		timer = GetNode<Timer>("Timer");
	}

	private void WaitAndAppear()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Empty");
		GetNode<Timer>("Timer").WaitTime = GD.Randi() % 10 + 5;
		GetNode<Timer>("Timer").Start();
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
			if (animationPlayerOutline != null)
			{
				animationPlayerOutline.Stop();
			}
			if (animationPlayer.HasAnimation("Throwing"))
			{
				animationPlayer.Play("Throwing");
			}
			else
			{
				animationPlayer.Play("Leaving");
			}
		}
	}

	public void _on_AnimationPlayer_animation_finished(string animationName)
	{
		if (animationName == "Arriving")
		{
			AddToGroup("ingredientHandlers");
			animationPlayer.Play("Waiting");
			if (animationPlayerOutline != null)
			{
				animationPlayerOutline.Play("Waiting");
			}
			timer.WaitTime = 10;
			timer.Start();
		}
		else if (animationName == "Throwing")
		{
			animationPlayer.Play("Leaving");
		}
		else if (animationName == "Leaving")
		{
			WaitAndAppear();
		}
	}
}
