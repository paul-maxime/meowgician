using Godot;
using System;

public class Potion : KinematicBody2D
{
	public bool isOnTable = true;
	public uint color;
	public void init(Vector2 position, uint color)
	{
		this.CollisionLayer = 0;
		this.CollisionMask = 0;
		this.color = color;
		this.Position = position;
		this.GetNode<Sprite>("Sprite").RegionRect = new Rect2(color * 16f, 0, 16f, 16f);
	}
}
