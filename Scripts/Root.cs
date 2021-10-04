using Godot;
using System;

public class Game : Node2D
{
	public bool IsGameOver { get; private set; }

	public override void _EnterTree()
	{
		base._EnterTree();
		GD.Randomize();
	}

	public void GameOver()
	{
		IsGameOver = true;

		var colorRect = GetNode<ColorRect>("CanvasLayer/ColorRect");
		var tween = GetNode<Tween>("CanvasLayer/ColorRect/Tween");
		colorRect.Visible = true;
		// colorRect.Color = new Color(1, 1, 1);

		tween.InterpolateProperty(colorRect, new NodePath("color"), new Color(1, 1, 1, 1), new Color(0, 0, 0, 0), 3.0f);
		tween.Start();
	}
}
