using Godot;
using System;

public class Potion : RigidBody2D
{
	private static Color[] colors = {
		new Color(.8f, .8f, 1f),
		new Color(1f, .8f, .8f),
		new Color(.8f, 1f, 1f),
		new Color(.8f, 1f, .8f),
	};

	public override void _Ready()
	{
		float x = (float)Godot.GD.RandRange(100, 1024 - 100);
		float y = (float)Godot.GD.RandRange(100, 400);
		this.Position = new Vector2(x, y);

		uint index = Godot.GD.Randi() % 4;

		float textureX = index * 16f;
		float textureY = 0;

		this.GetNode<Sprite>("Sprite").RegionRect = new Rect2(textureX, textureY, 16f, 16f);
		this.GetNode<Light2D>("Light2D").Color = colors[index];
	}

	public override void _InputEvent(Godot.Object viewport, InputEvent inputEvent, int shapeIdx)
	{
		if (inputEvent is InputEventMouseButton buttonEvent && buttonEvent.ButtonIndex == (int)ButtonList.Left)
		{
			this.ApplyImpulse(Vector2.Zero, Vector2.Up * 800f);
		}
	}
}
