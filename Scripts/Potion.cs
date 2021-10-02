using Godot;
using System;

public class Potion : RigidBody2D
{
	private static Random Rand = new Random();

	public override void _Ready()
	{
		float x = (float)Godot.GD.RandRange(100, 1024 - 100);
		float y = (float)Godot.GD.RandRange(-512 + 100, 0);
		this.Position = new Vector2(x, y);

		float textureX = (Godot.GD.Randi() % 5) * 16f;
		float textureY = (Godot.GD.Randi() % 5) * 16f;

		this.GetNode<Sprite>("Sprite").RegionRect = new Rect2(textureX, textureY, 16f, 16f);
	}

	public override void _InputEvent(Godot.Object viewport, InputEvent inputEvent, int shapeIdx)
	{
		if (inputEvent is InputEventMouseButton buttonEvent && buttonEvent.ButtonIndex == (int)ButtonList.Left)
		{
			this.ApplyImpulse(Vector2.Zero, Vector2.Up * 800f);
		}
	}
}
