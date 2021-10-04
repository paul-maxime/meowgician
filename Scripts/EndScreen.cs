using Godot;

public class EndScreen : Node
{
	public bool IsDisplayed { get; private set; }

	private float holdToRetry = 0.0f;
	private ColorRect restartRect;

	public override void _Ready()
	{
		base._Ready();
		restartRect = GetNode<ColorRect>("CanvasLayer/RestartRect");
	}

	public override void _Process(float delta)
	{
		base._Process(delta);

		if (!IsDisplayed)
		{
			return;
		}

		if (Input.IsActionPressed("ui_accept"))
		{
			holdToRetry += delta;
		}
		else
		{
			holdToRetry = 0;
		}

		var restartRect = GetNode<ColorRect>("CanvasLayer/RestartRect");
		restartRect.Color = new Color(0, 0, 0, holdToRetry);
		if (holdToRetry >= 1.0f)
		{
			RestartGame();
		}
	}

	public void RestartGame()
	{
		holdToRetry = 0;
		GetTree().ReloadCurrentScene();
		GetTree().Paused = false;
	}

	public void Lose()
	{
		IsDisplayed = true;
		GetTree().Paused = true;
		FadeinLabel("GameOver", 3.0f);
		FadeoutScreen(new Color(1, 1, 1, 1), new Color(0.2f, 0, 0, 0.8f), 3.0f);
		restartRect.Visible = true;
		GetNode<AudioStreamPlayer>("AudioBoom").Play();
	}

	public void Win()
	{
		IsDisplayed = true;
		GetTree().Paused = true;
		FadeoutScreen(new Color(1, 1, 1, 0), new Color(0, 0.2f, 0, 0.5f), 2.0f);
		FadeinLabel("Win", 2.0f);
		restartRect.Visible = true;
	}

	private void FadeoutScreen(Color from, Color color, float delay)
	{
		var colorRect = GetNode<ColorRect>("CanvasLayer/ColorRect");
		var tween = GetNode<Tween>("CanvasLayer/ColorRect/Tween");
		colorRect.Color = from;
		colorRect.Visible = true;
		tween.InterpolateProperty(colorRect, new NodePath("color"), from, color, delay);
		tween.Start();
	}

	private void FadeinLabel(string labelName, float delay)
	{
		var label = GetNode<Label>($"CanvasLayer/Label{labelName}");
		var tween = GetNode<Tween>($"CanvasLayer/Label{labelName}/Tween");
		label.Visible = true;
		tween.InterpolateProperty(label, new NodePath("modulate"), new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), delay);
		tween.Start();
	}
}
