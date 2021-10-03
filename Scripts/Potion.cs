using Godot;
using System;

public class Potion : KinematicBody2D
{
	public bool isOnTable = true;
	private Eye leftEye;
	private Eye rightEye;
	private Earthquake earthquake;
	public uint color;

	public void init(Vector2 position, uint color)
	{
		this.CollisionLayer = 0;
		this.CollisionMask = 0;
		this.color = color;
		this.Position = position;
		this.GetNode<Sprite>("Sprite").RegionRect = new Rect2(color * 16f, 0, 16f, 16f);
	}

	public override void _Ready()
	{
		earthquake = GetNode<Earthquake>("/root/Root/Earthquake");
		leftEye = GetNode<Eye>("Sprite/Eyes/IrisLeft");
		rightEye = GetNode<Eye>("Sprite/Eyes/IrisRight");
	}

	public void UpdateEyes(Vector2 direction, float delta)
	{
		if (earthquake.IsShaking)
		{
			leftEye.WoobleRandomly(delta * earthquake.Intensity);
			rightEye.WoobleRandomly(delta * earthquake.Intensity);
		}
		else
		{
			leftEye.LookAtDirection(direction);
			rightEye.LookAtDirection(direction);
		}
	}
}
