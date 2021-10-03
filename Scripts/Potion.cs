using Godot;
using System;

public class Potion : KinematicBody2D
{
	public bool isOnTable = true;
	public void init(Vector2 position, uint index)
	{
		this.CollisionLayer = 0;
		this.CollisionMask = 0;
		this.Position = position;
		this.GetNode<Sprite>("Sprite").RegionRect = new Rect2(index * 16f, 0, 16f, 16f);
	}

	public override void _Ready()
	{ }

	public override void _InputEvent(Godot.Object viewport, InputEvent inputEvent, int shapeIdx)
	{
		/*if (inputEvent is InputEventMouseButton buttonEvent && buttonEvent.ButtonIndex == (int)ButtonList.Left)
		{
				this.ApplyImpulse(Vector2.Zero, Vector2.Up * 800f);
		}*/
	}
}
