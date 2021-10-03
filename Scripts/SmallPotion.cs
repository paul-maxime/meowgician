using Godot;
using System;

public class SmallPotion : StaticBody2D
{
	private uint index;

	public void init(float x, float y, uint index)
	{
		this.Position = new Vector2(x, y);
		this.index = index;
	}

	public override void _Ready()
	{
		float textureX = index * 16f;
		float textureY = 0;

		this.GetNode<Sprite>("Sprite").RegionRect = new Rect2(textureX, textureY, 16f, 16f);
	}

	public override void _InputEvent(Godot.Object viewport, InputEvent inputEvent, int shapeIdx)
	{
		/*if (inputEvent is InputEventMouseButton buttonEvent && buttonEvent.ButtonIndex == (int)ButtonList.Left)
		{
				this.ApplyImpulse(Vector2.Zero, Vector2.Up * 800f);
		}*/
	}
}
