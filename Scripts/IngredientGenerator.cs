using Godot;

public class IngredientGenerator : KinematicBody2D
{
	private AnimationPlayer animationPlayer;
	private AnimationPlayer animationPlayerOutline;
	private Timer timer;
	private bool activated = false;

	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayerOutline = GetNodeOrNull<AnimationPlayer>("AnimationPlayerOutline");
		timer = GetNode<Timer>("Timer");
		wait();
	}

	private void wait()
	{
		animationPlayer.Play("Empty");
		timer.WaitTime = GD.Randi() % 10 + 5;
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
			if (animationPlayerOutline != null)
			{
				animationPlayerOutline.Stop();
			}
			animationPlayer.Play("Leaving");
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
		else if (animationName == "Leaving")
		{
			wait();
		}
	}
}
